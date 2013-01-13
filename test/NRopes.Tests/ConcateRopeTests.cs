namespace NRopes.Tests {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using FluentAssertions;
    using Xunit;

    public class ConcateRopeTests : RopeTestsBase {
        private static int Fib(int n) {
            if (n <= 2) return 1;

            return Fib(n - 1) + Fib(n - 2);
        }

        private static ConcateRope CreateRope(int depth, int length) {
            Debug.Assert(depth >= 1);

            var numOfParts = depth + 1;
            var lengthOfParts = length / numOfParts;

            var primitiveRope = new FlatRope(new String(' ', lengthOfParts));

            Rope rope = primitiveRope;
            for (var i = 1; i < numOfParts; i++) {
                if (i < numOfParts - 1) {
                    rope = new ConcateRope(rope, primitiveRope);
                }
                else {
                    var lastRope = new FlatRope(new String(' ', length - rope.Length));
                    rope = new ConcateRope(rope, lastRope);
                }
            }

            var concateRope = rope.As<ConcateRope>();
            Debug.Assert(concateRope.Depth == depth);
            Debug.Assert(concateRope.Length == length);

            return concateRope;
        }

        protected override IEnumerable<Rope> TargetRopes {
            get { yield return new ConcateRope(new FlatRope("A"), new FlatRope("B")); }
        }

        [Fact]
        public void ConstructorShouldThrowIfEitherChildIsNull() {
            new Action(() => new ConcateRope(null, new FlatRope("A")))
                .ShouldThrow<ArgumentNullException>().And
                .ParamName.Should().Be("left");

            new Action(() => new ConcateRope(new FlatRope("B"), null))
                .ShouldThrow<ArgumentNullException>().And
                .ParamName.Should().Be("right");
        }

        [Fact]
        public void SimplePropertiesShouldBeRight() {
            var hwRope = new ConcateRope(new FlatRope("Hello "), new FlatRope("World"));
            hwRope.Depth.Should().Be(1);

            var rope = new ConcateRope(new FlatRope("Say "), hwRope);
            rope.Depth.Should().Be(2);
            rope.Length.Should().Be("Say Hello World".Length);
            rope.ToString().Should().Be("Say Hello World");
        }

        [Fact]
        public void IsBalancedShouldBeFalseForLengthLowerThanFibDepthPlusTwo() {
            CreateRope(1, Fib(3) - 1).IsBalanced.Should().BeFalse();
            CreateRope(2, Fib(4) - 1).IsBalanced.Should().BeFalse();
            CreateRope(5, Fib(7) - 1).IsBalanced.Should().BeFalse();
        }

        [Fact]
        public void IsBalancedShouldBeTrueForLengthGreaterThanOrEqualToFibDepthPlusTwo() {
            CreateRope(1, Fib(3)).IsBalanced.Should().BeTrue();
            CreateRope(2, Fib(4)).IsBalanced.Should().BeTrue();
            CreateRope(5, Fib(7)).IsBalanced.Should().BeTrue();
        }
    }
}