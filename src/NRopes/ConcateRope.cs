namespace NRopes {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public sealed class ConcateRope : Rope, IStringBuilderPreferable {
        private const int MaxDepth = 31;

        public int FlattenRightChildLengthLimit = 10;

        /// <summary>
        /// Start from the 3nd fib numbers.
        /// </summary>
        private static readonly int[] Fibs;

        static ConcateRope() {
            Fibs = new int[MaxDepth];

            Fibs[0] = 2; // fib(3)
            Fibs[1] = 3; // fib(4)

            for (var i = 2; i < MaxDepth; i++) {
                Fibs[i] = Fibs[i - 2] + Fibs[i - 1];
            }
        }

        private static int GetMinLength(int depth) {
            return Fibs[depth - 1];
        }

        private static int GetDepth(Rope rope) {
            var concateRope = rope as ConcateRope;
            return concateRope == null ? 0 : concateRope.Depth;
        }

        private readonly Rope _left;
        private readonly Rope _right;
        private readonly int _length;
        private readonly int _depth;

        public ConcateRope(Rope left, Rope right) {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");

            _depth = Math.Max(GetDepth(left), GetDepth(right)) + 1;
            if (_depth > MaxDepth) {
                throw new ArgumentException(
                    String.Format("The result tree of rope cannot be over {0} in depth.", MaxDepth));
            }

            _left = left;
            _right = right;
            _length = left.Length + right.Length;
        }

        public int Depth {
            get { return _depth; }
        }

        public Rope Left {
            get { return _left; }
        }

        public Rope Right {
            get { return _right; }
        }

        public override int Length {
            get { return _length; }
        }

        public override Rope ConcateWith(Rope rope) {
            if (rope == null) throw new ArgumentNullException("rope");

            var limit = FlattenRightChildLengthLimit;

            var flatRope = rope as FlatRope;
            if (flatRope == null || flatRope.Length > limit) {
                return base.ConcateWith(rope);
            }

            var rightRope = _right as FlatRope;
            if (rightRope == null || rightRope.Length > limit) {
                return base.ConcateWith(rope);
            }

            if (flatRope.Length + rightRope.Length > limit) {
                return base.ConcateWith(rope);
            }

            var left = Left.ToString();
            var right = Right.ToString();

            return Left.ConcateWith(new FlatRope(left + right));
        }

        public override Rope Substring(int startIndex, int length) {
            return base.Substring(startIndex, length) ?? SubstringInternal(startIndex, length);
        }

        private Rope SubstringInternal(int startIndex, int length) {
            if (startIndex >= Left.Length) {
                return Right.Substring(startIndex - Left.Length, length);
            }

            if (startIndex + length < Left.Length) {
                return Left.Substring(startIndex, length);
            }

            var left = Left.Substring(startIndex);
            var right = Right.Substring(0, length - Left.Length);

            return left.ConcateWith(right);
        }

        public override IEnumerator<char> GetEnumerator() {
            return Left.Concat(Right).GetEnumerator();
        }

        public bool IsBalanced {
            get { return Length >= GetMinLength(_depth); }
        }

        public Rope Rebalance() {
            throw new NotImplementedException();
        }

        private void ToString(StringBuilder sb, IStringBuilderPreferable left, IStringBuilderPreferable right) {
            if (left == null) {
                sb.Append(Left);
                right.ToString(sb);
            }
            else {
                left.ToString(sb);

                if (right == null) {
                    sb.Append(Right);
                }
                else {
                    right.ToString(sb);
                }
            }
        }

        public void ToString(StringBuilder sb) {
            var left = Left as IStringBuilderPreferable;
            var right = Right as IStringBuilderPreferable;

            if (left == null && right == null) {
                sb.Append(Left).Append(Right);
            }
            else {
                ToString(sb, left, right);
            }
        }

        public override string ToString() {
            var left = Left as IStringBuilderPreferable;
            var right = Right as IStringBuilderPreferable;

            if (left == null && right == null) {
                return Left.ToString() + Right;
            }

            var sb = new StringBuilder(Length);
            ToString(sb, left, right);

            return sb.ToString();
        }
    }
}