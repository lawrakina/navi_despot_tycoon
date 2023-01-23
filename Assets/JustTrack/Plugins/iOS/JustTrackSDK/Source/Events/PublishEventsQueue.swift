import Foundation

class PublishEventsQueue {
    fileprivate static let MAX_BATCH_SIZE: Int = 100
    private static let WAIT_TIME: TimeInterval = 5

    private var done: Bool
    private weak var sdk: JustTrackSDKImpl?
    private let eventStore: EventStore
    private var retryQueue: [PublishingEvent]
    private var workQueue: [PublishingEvent]
    private var currentBatch: Batch?
    private var reconnectSubscription: Subscription?

    init(_ sdk: JustTrackSDKImpl, _ connectivityManager: ConnectivityManager?) {
        self.done = false
        self.sdk = sdk
        self.retryQueue = []
        self.workQueue = []
        self.currentBatch = nil

        var eventStore = EventStore()
        while let storedEvent = eventStore.readStoredEvent() {
            workQueue.append(PublishingEvent(event: storedEvent))
        }

        self.eventStore = eventStore
        self.reconnectSubscription = connectivityManager?.registerOnReconnect(self.onReachable)

        if !workQueue.isEmpty {
            DispatchQueue.main.async { self.handle() }
        }
    }

    func publishEvent(event: PublishableUserEvent) -> Future<Void> {
        objc_sync_enter(self)
        defer { objc_sync_exit(self) }

        let id = eventStore.getNextId();
        let publishingEvent = PublishingEvent(event: StorableEvent(id: id, event: event))
        eventStore.storeEvent(event: publishingEvent.event)
        workQueue.append(publishingEvent)
        DispatchQueue.main.async { self.handle() }

        return publishingEvent.promise.toFuture()
    }

    func shutdown() {
        objc_sync_enter(self)
        defer { objc_sync_exit(self) }

        self.done = false
        // we are no longer interested in any updates, so stop this
        self.reconnectSubscription?.unsubscribe()
        self.reconnectSubscription = nil
    }

    private func handle() {
        objc_sync_enter(self)
        defer { objc_sync_exit(self) }

        if self.done {
            return
        }

        while let nextEvent = workQueue.popLast() {
            if currentBatch == nil {
                currentBatch = Batch()
            }
            if let batch = currentBatch {
                batch.batch.append(nextEvent)

                if batch.batch.count >= PublishEventsQueue.MAX_BATCH_SIZE || nextEvent.event.event.name == UserEvent.SESSION_TRACKING_END {
                    publishBatch(batch: batch.batch)
                    currentBatch = nil
                }
            }
        }

        if let batch = currentBatch {
            // we have a batch to publish, start the countdown
            DispatchQueue.main.asyncAfter(deadline: .now() + PublishEventsQueue.WAIT_TIME, execute: {
                objc_sync_enter(self)
                defer { objc_sync_exit(self) }

                if let newBatch = self.currentBatch {
                    if newBatch.id == batch.id {
                        self.publishBatch(batch: newBatch.batch)
                        self.currentBatch = nil
                    }
                }
            })
        }
    }

    internal func onReachable() {
        objc_sync_enter(self)
        defer { objc_sync_exit(self) }

        // all events we want to retry are now new work, enqueue them
        while let nextEvent = retryQueue.popLast() {
            workQueue.append(nextEvent)
        }

        // and then schedule to be called again (easier with the locks that way)
        DispatchQueue.main.async { self.handle() }
    }

    private func publishBatch(batch: [PublishingEvent]) {
        sdk?.publishEventBatch(batch: batch).observe(using: { result in
            switch result {
            case .success(_):
                for event in batch {
                    self.eventStore.removeEvent(event: event.event)
                    _ = event.promise.resolve(Void())
                }
            case .failure(_):
                for event in batch {
                    self.retryQueue.append(event)
                }

            }
        })
    }
}

internal struct PublishingEvent {
    fileprivate let event: StorableEvent
    fileprivate let promise: FutureImpl<Void>

    init(event: StorableEvent) {
        self.event = event
        self.promise = FutureImpl()
    }

    func eventId() -> UUID {
        return event.eventId
    }

    func baseEvent() -> PublishableUserEvent {
        return event.event
    }

    func happenedAt() -> Date {
        return event.happenedAt
    }
}

// needs to be a class - if you change this to a struct, we will append new events to a
// COPY of the current batch, causing us to never fill the batch!
fileprivate class Batch {
    fileprivate let id: UUID
    fileprivate var batch: [PublishingEvent]

    fileprivate init() {
        id = UUID()
        batch = []
    }
}
