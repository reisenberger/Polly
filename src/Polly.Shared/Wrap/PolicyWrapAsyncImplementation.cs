using System;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;

namespace Polly.Wrap
{

    internal class PolicyWrapAsyncImplementationNonGenericNonGeneric : IAsyncPolicyImplementation<Object>
    {
        private readonly Policy _outerPolicy;
        private readonly Policy _innerPolicy;

        internal PolicyWrapAsyncImplementationNonGenericNonGeneric(IsPolicy policy, Policy outer, IAsyncPolicy inner)
        {
            // PolicyWrap nonGenericNonGenericWrap = policy as PolicyWrap ?? throw new ArgumentNullException(nameof(policy)); // Will be used once we emit events.
            _outerPolicy = outer ?? throw new ArgumentNullException(nameof(outer));
            _innerPolicy = inner as Policy ?? throw new ArgumentNullException(nameof(inner));
        }

        public async Task<Object> ExecuteAsync<TExecutable>(TExecutable func, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<Object>
        {
            AsyncPollyExecutableAction<Policy, TExecutable> innerExecutionAsExecutable = new AsyncPollyExecutableAction<Policy, TExecutable>(
                (ctx, ct, capture, policy, f) => policy.ExecuteAsyncExecutableThroughPolicy<TExecutable>(f, ctx, ct, capture),
                _innerPolicy,
                func);
            await _outerPolicy.ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableAction<Policy, TExecutable>>(
                innerExecutionAsExecutable,
                context,
                cancellationToken, continueOnCapturedContext);
            return null;

            // Equivalent to this, avoiding all closures: return _outerPolicy.Execute((ctx, ct) => _innerPolicy.Execute(func, ctx, ct), context, cancellationToken);
        }
    }

    internal class PolicyWrapAsyncImplementationNonGenericNonGeneric<TResult> : IAsyncPolicyImplementation<TResult>
    {
        private readonly Policy _outerPolicy;
        private readonly Policy _innerPolicy;

        internal PolicyWrapAsyncImplementationNonGenericNonGeneric(IsPolicy policy, IsPolicy outer, IAsyncPolicy inner)
        {
            // PolicyWrap<TResult> nonGenericNonGenericWrap = policy ?? throw new ArgumentNullException(nameof(policy)); // Will be used once we emit events.
            _outerPolicy = outer as Policy ?? throw new ArgumentNullException(nameof(outer));
            _innerPolicy = inner as Policy ?? throw new ArgumentNullException(nameof(inner));
        }

        public Task<TResult> ExecuteAsync<TExecutable>(TExecutable func, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<TResult>
        {
            AsyncPollyExecutableFunc<Policy, TExecutable, TResult> innerExecutionAsExecutable = new AsyncPollyExecutableFunc<Policy, TExecutable, TResult>(
                (ctx, ct, capture, policy, f) => policy.ExecuteAsyncExecutableThroughPolicy<TExecutable, TResult>(f, ctx, ct, capture),
                _innerPolicy,
                func);
            return _outerPolicy.ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFunc<Policy, TExecutable, TResult>, TResult>(
                innerExecutionAsExecutable,
                context,
                cancellationToken, continueOnCapturedContext);

            // Equivalent to this, avoiding all closures: return _outerPolicy.ExecuteAsync<TResult>((ctx, ct) => _innerPolicy.ExecuteAsync<TResult>(func, ctx, ct), context, cancellationToken);
        }
    }

    internal class PolicyWrapAsyncImplementationGenericGeneric<TResult> : IAsyncPolicyImplementation<TResult>
    {
        private readonly Policy<TResult> _outerPolicy;
        private readonly Policy<TResult> _innerPolicy;

        internal PolicyWrapAsyncImplementationGenericGeneric(IsPolicy policy, Policy<TResult> outer, IAsyncPolicy<TResult> inner)
        {
            // PolicyWrap<TResult> genericGenericWrap = policy as PolicyWrap<TResult> ?? throw new ArgumentNullException(nameof(policy)); // Will be used once we emit events.
            _outerPolicy = outer ?? throw new ArgumentNullException(nameof(outer));
            _innerPolicy = inner as Policy<TResult> ?? throw new ArgumentNullException(nameof(inner));
        }

        public Task<TResult> ExecuteAsync<TExecutable>(TExecutable func, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<TResult>
        {
            AsyncPollyExecutableFunc<Policy<TResult>, TExecutable, TResult> innerExecutionAsExecutable = new AsyncPollyExecutableFunc<Policy<TResult>, TExecutable, TResult>(
                (ctx, ct, capture, policy, f) => policy.ExecuteAsyncExecutableThroughPolicy(f, ctx, ct, capture),
                _innerPolicy,
                func);
            return _outerPolicy.ExecuteAsyncExecutableThroughPolicy(
                innerExecutionAsExecutable,
                context,
                cancellationToken, continueOnCapturedContext);

            // Equivalent to this, avoiding all closures: return _outerPolicy.ExecuteAsync<TResult>((ctx, ct) => _innerPolicy.ExecuteAsync<TResult>(func, ctx, ct), context, cancellationToken);
        }
    }

    internal class PolicyWrapAsyncImplementationGenericNonGeneric<TResult> : IAsyncPolicyImplementation<TResult>
    {
        private readonly Policy<TResult> _outerPolicy;
        private readonly Policy _innerPolicy;

        internal PolicyWrapAsyncImplementationGenericNonGeneric(IsPolicy policy, Policy<TResult> outer, IAsyncPolicy inner)
        {
            // PolicyWrap<TResult> genericNonGenericWrap = policy as PolicyWrap<TResult> ?? throw new ArgumentNullException(nameof(policy)); // Will be used once we emit events.
            _outerPolicy = outer ?? throw new ArgumentNullException(nameof(outer));
            _innerPolicy = inner as Policy ?? throw new ArgumentNullException(nameof(inner));
        }

        public Task<TResult> ExecuteAsync<TExecutable>(TExecutable func, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<TResult>
        {
            AsyncPollyExecutableFunc<Policy, TExecutable, TResult> innerExecutionAsExecutable = new AsyncPollyExecutableFunc<Policy, TExecutable, TResult>(
                (ctx, ct, capture, policy, f) => policy.ExecuteAsyncExecutableThroughPolicy<TExecutable, TResult>(f, ctx, ct, capture),
                _innerPolicy,
                func);
            return _outerPolicy.ExecuteAsyncExecutableThroughPolicy(
                innerExecutionAsExecutable,
                context,
                cancellationToken, continueOnCapturedContext);

            // Equivalent to this, avoiding all closures: return _outerPolicy.ExecuteAsync<TResult>((ctx, ct) => _innerPolicy.ExecuteAsync<TResult>(func, ctx, ct), context, cancellationToken);
        }
    }

    internal class PolicyWrapAsyncImplementationNonGenericGeneric<TResult> : IAsyncPolicyImplementation<TResult>
    {
        private readonly Policy _outerPolicy;
        private readonly Policy<TResult> _innerPolicy;

        internal PolicyWrapAsyncImplementationNonGenericGeneric(IsPolicy policy, Policy outer, IAsyncPolicy<TResult> inner)
        {
            // PolicyWrap<TResult> nonGenericGenericWrap = policy as PolicyWrap<TResult> ?? throw new ArgumentNullException(nameof(policy)); // Will be used once we emit events.
            _outerPolicy = outer ?? throw new ArgumentNullException(nameof(outer));
            _innerPolicy = inner as Policy<TResult> ?? throw new ArgumentNullException(nameof(inner));
        }

        public Task<TResult> ExecuteAsync<TExecutable>(TExecutable func, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<TResult>
        {
            AsyncPollyExecutableFunc<Policy<TResult>, TExecutable, TResult> innerExecutionAsExecutable = new AsyncPollyExecutableFunc<Policy<TResult>, TExecutable, TResult>(
                (ctx, ct, capture, policy, f) => policy.ExecuteAsyncExecutableThroughPolicy(f, ctx, ct, capture),
                _innerPolicy,
                func);

            return _outerPolicy.ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFunc<Policy<TResult>, TExecutable, TResult>, TResult>(
                innerExecutionAsExecutable,
                context,
                cancellationToken, continueOnCapturedContext);

            // Equivalent to this, avoiding all closures: return _outerPolicy.ExecuteAsync<TResult>((ctx, ct) => _innerPolicy.ExecuteAsync<TResult>(func, ctx, ct), context, cancellationToken);
        }
    }

}