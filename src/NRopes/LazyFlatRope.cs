namespace NRopes {
    using System;
    using System.Collections.Generic;

    public sealed class LazyFlatRope : Rope {
        private abstract class ImplementationDelegate {
            public abstract string UnderlyString { get; }
            public abstract int StartIndex { get; }
            public abstract int Length { get; }
            public abstract bool IsEvaluated { get; }
            public abstract ImplementationDelegate Evaluate();

            public LazyFlatRope Substring(int startIndex, int length) {
                return new LazyFlatRope(CreateDelegate(UnderlyString, StartIndex + startIndex, length));
            }

            public IEnumerator<char> GetEnumerator() {
                for (var i = 0; i < Length; i++) {
                    yield return UnderlyString[StartIndex + i];
                }
            }
        }

        private sealed class UnevaluatedDelegate : ImplementationDelegate {
            private readonly string _str;
            private readonly int _startIndex;
            private readonly int _length;

            public UnevaluatedDelegate(string str, int startIndex, int length) {
                _str = str;
                _startIndex = startIndex;
                _length = length;
            }

            public override string UnderlyString {
                get { return _str; }
            }

            public override int StartIndex {
                get { return _startIndex; }
            }

            public override int Length {
                get { return _length; }
            }

            public override bool IsEvaluated {
                get { return false; }
            }

            public override ImplementationDelegate Evaluate() {
                return new EvaluatedDelegate(UnderlyString.Substring(StartIndex, Length));
            }
        }

        private sealed class EvaluatedDelegate : ImplementationDelegate {
            private readonly string _str;

            public EvaluatedDelegate(string str) {
                _str = str;
            }

            public override string UnderlyString {
                get { return _str; }
            }

            public override int StartIndex {
                get { return 0; }
            }

            public override int Length {
                get { return _str.Length; }
            }

            public override bool IsEvaluated {
                get { return true; }
            }

            public override ImplementationDelegate Evaluate() {
                return this;
            }
        }

        private static ImplementationDelegate CreateDelegate(string str, int startIndex, int length) {
            if (startIndex == 0 && str.Length == length)
                return new EvaluatedDelegate(str);

            return new UnevaluatedDelegate(str, startIndex, length);
        }

        private volatile ImplementationDelegate _delegate;

        public LazyFlatRope(string str) {
            if (str == null) throw new ArgumentNullException("str");

            _delegate = new EvaluatedDelegate(str);
        }

        public LazyFlatRope(string str, int startIndex, int length) {
            if (str == null) throw new ArgumentNullException("str");
            ValidateHelper.ValidateSubstring(str.Length, startIndex, length);

            _delegate = CreateDelegate(str, startIndex, length);
        }

        private LazyFlatRope(ImplementationDelegate d) {
            _delegate = d;
        }

        public string UnderlyString {
            get { return _delegate.UnderlyString; }
        }

        public int StartIndex {
            get { return _delegate.StartIndex; }
        }

        public override int Length {
            get { return _delegate.Length; }
        }

        public bool IsEvaluated {
            get { return _delegate.IsEvaluated; }
        }

        public void Evaluate() {
            _delegate = _delegate.Evaluate();
        }

        public override int Depth {
            get { return 0; }
        }

        public override Rope Substring(int startIndex, int length) {
            return base.Substring(startIndex, length) ?? _delegate.Substring(startIndex, length);
        }

        public override IEnumerator<char> GetEnumerator() {
            return _delegate.GetEnumerator();
        }
    }
}