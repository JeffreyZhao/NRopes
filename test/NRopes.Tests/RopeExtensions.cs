namespace NRopes.Tests {
    using System;

    internal static class RopeExtensions {
        public static Action Do(this Rope rope, Action<Rope> action) {
            return () => action(rope);
        }
    }
}