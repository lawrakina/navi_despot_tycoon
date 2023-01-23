# JustTrack SDK Changelog (Unity)

## Version 4.2.3 (24th August 2021)

### Changed

- The SDK now automatically handles a network reconnect when fetching an attribution.
- Updated Android SDK to 4.2.3.
- Updated iOS SDK to 4.2.3.
- Refactored the settings of the SDK. They are now stored as a resource, so you can instantiate the SDK at runtime.
- There is now only one API token per platform instead of one for development and release.

## Version 4.2.2 (29th July 2021)

### Added

- The SDK now adds the JustTrack backend as a receiver for SDKAdNetwork attribution postbacks (for iOS 15).

### Changed

- Updated Android SDK to 4.2.2.
- Updated iOS SDK to 4.2.2.

## Version 4.2.1 (1st July 2021)

### Bug-Fixes

- Fixed a missing parameter when constructing standard events with a unit for Android.

## Version 4.2.0 (25th June 2021)

### Changed

- Events tracking the progress of a user for a level or quest have been extended to automatically track the progress of the user and provide the duration the app was active during that time upon completion.
- The SDK no longer requests the `READ_PHONE_STATE` and `ACCESS_WIFI_STATE` permissions on Android.
- The session id is now included in all events sent to the backend.
- Setting a tracking id now also requires you to name the provider of that tracking id.

### Removed

- Removed `setRealTime` from all interfaces again as well as realTime event functionality.
- It is no longer possible to set the duration for progression events by hand.

### Bug-Fixes

- The SDK will now suggest you add some small generated code to work around limitations of the IL2CPP compiler to integrate with IronSource.

## Version 4.1.2 (27th May 2021)

### Removed

- Removed the external dependency manager from Google as dependency as Google removed their scoped registry. You have to provide a version of it now yourself.

## Version 4.1.1 (21st May 2021)

### Changed

- Fixed bad version constraint for dependency.

## Version 4.1.0 (19th May 2021)

### Changed

- Updated Android SDK to 4.1.1.
- Updated iOS SDK to 4.1.0.

## Version 4.0.1 (30th April 2021)

### Changed

- Updated Android SDK to 4.0.1.

### Bug-Fixes

- Fixed predefined events to publish their custom parameters with the correct dimensions.

## Version 4.0.0 (28th April 2021)

### Added

- Added `AttributionCampaign`, `AttributionChannel`, `AttributionNetwork` classes and changed the representation of an attribution slightly.
- Added `CustomUserEvent`, `Dimension`, `EventDetails`, `StandardUserEvent`, and `UserEvent` classes, enums, and interfaces.
- Added support for retargeting attributions. You can now get a new attribution if a user clicks on a retargeting campaign while your game is already running.

### Changed

- Updated Android SDK to 4.0.0.
- `AttributionResponse` and `AttributionRecruiter` now carry additional fields.
- Renamed `Units` to `Unit`.
- `PublishEvent` now accepts a string, an instance of `EventDetails`, or a complete `UserEvent`.
- Predefined events are now represented by classes instead of constants.

### Removed

- Removed AttributionNamed class.

## Version 2.4.5 (14th March 2021)

### Changed

- Added support to provide the SDK as a Unity package.

## Version 2.4.4 (3rd March 2021)

### Bug-Fixes

- Initialize the correct SDK agent when running in the editor.

## Version 2.4.3 (20th January 2021)

### Bug-Fixes

- Avoid initializing the SDK multiple times at the same time.

## Version 2.4.2 (12th January 2021)

### Added

- Added Appsflyer integration. You can now select a tracking provider per platform and the SDK will take care of the rest.
- Added field to query whether IronSource was already initialized.

### Changed

- Merged both prefabs into single prefab and extended logic to take care of all integration logic automatically.
- Updated Android and iOS SDKs to 2.4.2.

### Bug-Fixes

- Correctly forward IronSource ad impressions on Android.

---
