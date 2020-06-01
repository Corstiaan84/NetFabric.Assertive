using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class AsyncEnumerableValueTypeAssertions<TActual, TActualItem> 
        : ValueTypeAssertionsBase<TActual>
        where TActual : struct
    {
        internal AsyncEnumerableValueTypeAssertions(TActual Actual, AsyncEnumerableInfo enumerableInfo)
            : base(Actual) 
            => EnumerableInfo = enumerableInfo;

        public AsyncEnumerableInfo EnumerableInfo { get; }

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> EvaluateTrue(Func<TActual, bool> func)
            => EvaluateTrue<AsyncEnumerableValueTypeAssertions<TActual, TActualItem>>(this, func);

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> EvaluateFalse(Func<TActual, bool> func)
            => EvaluateFalse<AsyncEnumerableValueTypeAssertions<TActual, TActualItem>>(this, func);

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeEmpty(bool warnRefStructs = true, bool warnRefReturns = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), warnRefStructs, warnRefReturns);

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected, bool warnRefStructs = true, bool warnRefReturns = true)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), warnRefStructs, warnRefReturns);

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, bool warnRefStructs = true, bool warnRefReturns = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

            AssertAsyncEnumerableEquality(Actual, EnumerableInfo, expected, comparer, warnRefStructs, warnRefReturns);

            return this;
        }
    }
}