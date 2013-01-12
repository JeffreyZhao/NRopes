namespace NRopes.Tests {
    using System.Collections.Generic;

    public class RopeTests : RopeTestsBase {
        protected override IEnumerable<Rope> TargetRopes {
            get { yield return Rope.Empty; }
        }
    }
}