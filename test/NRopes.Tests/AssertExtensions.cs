namespace NRopes.Tests {
    using System;
    using FluentAssertions.Primitives;
    using Xunit;

    public static class AssertExtensions {
        public static void BeSameAs<TSubject, TAssertions>(
            this ReferenceTypeAssertions<TSubject, TAssertions> assertions, TSubject obj)
            where TAssertions : ReferenceTypeAssertions<TSubject, TAssertions> {

            Assert.True(
                Object.ReferenceEquals(assertions.Subject, obj),
                String.Format("Object not the same.\nExpected: {0}\nActual: {1}", assertions.Subject, obj));
        }
    }
}