using System;
using System.Collections;
using System.Collections.Generic;

// DO NOT EDIT - AUTOMATICALLY GENERATED

namespace JustTrack {
    public enum Unit {
        /**
         * We want to count how many times something happened in total.
         */
        Count,
        /**
         * We want to measure how long something takes with millisecond precision.
         */
        Milliseconds,
        /**
         * We want to measure how long something takes with second precision.
         */
        Seconds,
    }

    /**
     * A time-based unit grouping the milliseconds,seconds units.
     */
    public enum TimeUnitGroup {
        /**
         * See Unit.Milliseconds
         */
        Milliseconds,
        /**
         * See Unit.Seconds
         */
        Seconds,
    }

    internal static class TimeUnitGroupConversions {
        internal static Unit ToUnit(TimeUnitGroup unit) {
            switch (unit) {
                case TimeUnitGroup.Milliseconds: return Unit.Milliseconds;
                case TimeUnitGroup.Seconds: return Unit.Seconds;
                default: throw new Exception($"Unexpected enum variant: {unit}");
            }
        }
    }
}
