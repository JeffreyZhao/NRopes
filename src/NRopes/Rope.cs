namespace NRopes {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Rope : IEnumerable<char> {
        private sealed class EmptyRope : Rope {
            public override int Length {
                get { return 0; }
            }

            public override IEnumerator<char> GetEnumerator() {
                return Enumerable.Empty<char>().GetEnumerator();
            }

            public override string ToString() {
                return "";
            }
        }

        public static readonly Rope Empty = new EmptyRope();

        public static bool IsNullOrEmpty(Rope rope) {
            return rope == null || rope.Length == 0;
        }

        public static Rope Concate(Rope left, Rope right) {
            if (left == null) return right ?? Empty;
            if (right == null) return left;

            return left.ConcateWith(right);
        }

        public abstract int Length { get; }

        public virtual Rope ConcateWith(Rope rope) {
            if (rope == null) throw new ArgumentNullException("rope");

            if (Length == 0) return rope;
            if (rope.Length == 0) return this;

            var newRope = new ConcateRope(this, rope);
            return newRope.IsBalanced ? newRope : newRope.Rebalance();
        }

        public Rope Substring(int startIndex) {
            return Substring(startIndex, Length - startIndex);
        }

        public virtual Rope Substring(int startIndex, int length) {
            var strLength = Length;

            ValidateHelper.ValidateSubstring(strLength, startIndex, length);

            if (length == 0) return Empty;
            if (startIndex == 0 && length == strLength) return this;

            return null;
        }

        public abstract IEnumerator<char> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override string ToString() {
            throw new NotImplementedException("Please override ToString() method in derived classes of Rope.");
        }
    }
}