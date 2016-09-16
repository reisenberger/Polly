using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polly.Utilities
{
    internal class PredicateHelper
    {
        public static IEnumerable<ExceptionPredicate> EmptyExceptionPredicates = Enumerable.Empty<ExceptionPredicate>();
    }

    internal class PredicateHelper<TResult>
    {
        public static IEnumerable<ResultPredicate<TResult>> EmptyResultPredicates = Enumerable.Empty<ResultPredicate<TResult>>();
    }
}
