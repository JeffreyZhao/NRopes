namespace NRopes {
    using System;

    internal static class ValidateHelper {
        public static void ValidateSubstring(int strLength, int startIndex, int length) {
            if (startIndex < 0) {
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be less than length of string.");
            }

            if (startIndex > strLength) {
                throw new ArgumentOutOfRangeException("startIndex", "startIndex cannot be larger than length of string.");
            }

            if (length < 0) {
                throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            }

            if (startIndex > strLength - length) {
                throw new ArgumentOutOfRangeException("length", "startIndex and length must refer to a location within the string.");
            }
        }
    }
}