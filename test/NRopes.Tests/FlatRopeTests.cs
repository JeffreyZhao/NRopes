namespace NRopes.Tests {
    using System.Collections.Generic;

    public class FlatRopeTests : RopeTestsBase {
        protected override IEnumerable<Rope> TargetRopes {
            get {
                yield return new FlatRope("");
                yield return new FlatRope("Hello World");
            }
        }
    }
}