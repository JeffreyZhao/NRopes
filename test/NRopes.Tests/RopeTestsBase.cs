namespace NRopes.Tests {
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public abstract class RopeTestsBase {
        protected abstract IEnumerable<Rope> TargetRopes { get; }

        [Fact]
        public void SubstringShouldThrowExceptionsForInvalidParameters() {
            foreach (var rope in TargetRopes) {
                rope.Invoking(r => r.Substring(-1))
                    .ShouldThrow<ArgumentOutOfRangeException>().And
                    .ParamName.Should().Be("startIndex");

                rope.Invoking(r => r.Substring(r.Length + 1))
                    .ShouldThrow<ArgumentOutOfRangeException>().And
                    .ParamName.Should().Be("startIndex");

                rope.Invoking(r => r.Substring(0, -1))
                    .ShouldThrow<ArgumentOutOfRangeException>().And
                    .ParamName.Should().Be("length");

                rope.Invoking(r => r.Substring(0, r.Length + 1))
                    .ShouldThrow<ArgumentOutOfRangeException>().And
                    .ParamName.Should().Be("length");
            }
        }

        [Fact]
        public void SubstringShouldReturnSelfForZeroStartIndexAndFullLength() {
            foreach (var rope in TargetRopes) {
                if (rope.Length > 0) {
                    rope.Substring(0, rope.Length).Should().BeSameAs(rope);
                }
            }
        }

        [Fact]
        public void SubstringShouldReturnEmptyForZeroLength() {
            foreach (var rope in TargetRopes) {
                rope.Substring(rope.Length, 0).Should().BeSameAs(Rope.Empty);
            }
        }
    }
}