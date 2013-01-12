namespace NRopes {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class FlatRope : Rope {
        private readonly string _str;

        public FlatRope(string str) {
            if (str == null) throw new ArgumentNullException("str");

            _str = str;
        }

        public override int Length {
            get { return _str.Length; }
        }

        public override int Depth {
            get { return 0; }
        }

        public override Rope Substring(int startIndex, int length) {
            return base.Substring(startIndex, length) ?? new FlatRope(_str.Substring(startIndex, length));
        }

        public override IEnumerator<char> GetEnumerator() {
            return _str.Cast<char>().GetEnumerator();
        }

        public override string ToString() {
            return _str;
        }
    }
}