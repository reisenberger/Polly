﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Fallback
{
    public partial class FallbackPolicy : IFallbackPolicy
    {
        internal FallbackPolicy(PolicyBuilder builder, FallbackSyncImplementationFactory factory)
            : base(builder, factory)
        { }

        internal override Task<TMethodGeneric> ExecuteInternalAsync<TExecutable, TMethodGeneric>(TExecutable func, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            throw GenericExecuteMethodsAreAnInvalidOperationOnNonGenericFallbackPolicy<TMethodGeneric>();
        }
        
    }

    public partial class FallbackPolicy<TResult> : IFallbackPolicy<TResult>
    {
        internal FallbackPolicy(PolicyBuilder<TResult> builder, FallbackAsyncImplementationFactory<TResult> factory)
       : base(builder, factory)
        { }
    }
}