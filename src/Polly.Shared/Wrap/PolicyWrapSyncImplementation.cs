using System;
using System.Threading;
using Polly.Execution;

namespace Polly.Wrap
{

    internal class PolicyWrapSyncImplementationNonGenericNonGeneric : ISyncPolicyImplementation<Object>
    {
        private readonly Policy _outerPolicy;
        private readonly Policy _innerPolicy;

        internal PolicyWrapSyncImplementationNonGenericNonGeneric(IsPolicy policy, Policy outer, ISyncPolicy inner)
        {
            // PolicyWrap nonGenericNonGenericWrap = policy as PolicyWrap ?? throw new ArgumentNullException(nameof(policy)); // Will be used once we emit events.
            _outerPolicy = outer ?? throw new ArgumentNullException(nameof(outer));
            _innerPolicy = inner as Policy ?? throw new ArgumentNullException(nameof(inner));
        }

        public Object Execute<TExecutable>(TExecutable func, Context context, CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<Object>
        {
            SyncPollyExecutableAction<Policy, TExecutable> innerExecutionAsExecutable = new SyncPollyExecutableAction<Policy, TExecutable>(
                (ctx, ct, policy, f) => policy.ExecuteSyncExecutableThroughPolicy<TExecutable>(f, ctx, ct),
                _innerPolicy,
                func);
            _outerPolicy.ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableAction<Policy, TExecutable>>(
                innerExecutionAsExecutable,
                context,
                cancellationToken);
            return null;

            // Equivalent to this, avoiding all closures: return _outerPolicy.Execute((ctx, ct) => _innerPolicy.Execute(func, ctx, ct), context, cancellationToken);
        }
    }

    internal class PolicyWrapSyncImplementationNonGenericNonGeneric<TResult> : ISyncPolicyImplementation<TResult>
    {
        private readonly Policy _outerPolicy;
        private readonly Policy _innerPolicy;

        internal PolicyWrapSyncImplementationNonGenericNonGeneric(IsPolicy policy, IsPolicy outer, ISyncPolicy inner)
        {
            // PolicyWrap<TResult> nonGenericNonGenericWrap = policy ?? throw new ArgumentNullException(nameof(policy)); // Will be used once we emit events.
            _outerPolicy = outer as Policy ?? throw new ArgumentNullException(nameof(outer));
            _innerPolicy = inner as Policy ?? throw new ArgumentNullException(nameof(inner));
        }

        public TResult Execute<TExecutable>(TExecutable func, Context context, CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            SyncPollyExecutableFunc<Policy, TExecutable, TResult> innerExecutionAsExecutable = new SyncPollyExecutableFunc<Policy, TExecutable, TResult>(
                (ctx, ct, policy, f) => policy.ExecuteSyncExecutableThroughPolicy<TExecutable, TResult>(f, ctx, ct),
                _innerPolicy,
                func);
            return _outerPolicy.ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFunc<Policy, TExecutable, TResult>, TResult>(
                innerExecutionAsExecutable,
                context,
                cancellationToken);

            // Equivalent to this, avoiding all closures: return _outerPolicy.Execute<TResult>((ctx, ct) => _innerPolicy.Execute<TResult>(func, ctx, ct), context, cancellationToken);
        }
    }

    internal class PolicyWrapSyncImplementationGenericGeneric<TResult> : ISyncPolicyImplementation<TResult>
    {
        private readonly Policy<TResult> _outerPolicy;
        private readonly Policy<TResult> _innerPolicy;

        internal PolicyWrapSyncImplementationGenericGeneric(IsPolicy policy, Policy<TResult> outer, ISyncPolicy<TResult> inner)
        {
            // PolicyWrap<TResult> genericGenericWrap = policy as PolicyWrap<TResult> ?? throw new ArgumentNullException(nameof(policy)); // Will be used once we emit events.
            _outerPolicy = outer ?? throw new ArgumentNullException(nameof(outer));
            _innerPolicy = inner as Policy<TResult> ?? throw new ArgumentNullException(nameof(inner));
        }

        public TResult Execute<TExecutable>(TExecutable func, Context context, CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            SyncPollyExecutableFunc<Policy<TResult>, TExecutable, TResult> innerExecutionAsExecutable = new SyncPollyExecutableFunc<Policy<TResult>, TExecutable, TResult>(
                (ctx, ct, policy, f) => policy.ExecuteSyncExecutableThroughPolicy(f, ctx, ct),
                _innerPolicy,
                func);
            return _outerPolicy.ExecuteSyncExecutableThroughPolicy(
                innerExecutionAsExecutable,
                context,
                cancellationToken);

            // Equivalent to this, avoiding all closures: return _outerPolicy.Execute<TResult>((ctx, ct) => _innerPolicy.Execute<TResult>(func, ctx, ct), context, cancellationToken);
        }
    }

    internal class PolicyWrapSyncImplementationGenericNonGeneric<TResult> : ISyncPolicyImplementation<TResult>
    {
        private readonly Policy<TResult> _outerPolicy;
        private readonly Policy _innerPolicy;

        internal PolicyWrapSyncImplementationGenericNonGeneric(IsPolicy policy, Policy<TResult> outer, ISyncPolicy inner)
        {
            // PolicyWrap<TResult> genericNonGenericWrap = policy as PolicyWrap<TResult> ?? throw new ArgumentNullException(nameof(policy)); // Will be used once we emit events.
            _outerPolicy = outer ?? throw new ArgumentNullException(nameof(outer));
            _innerPolicy = inner as Policy ?? throw new ArgumentNullException(nameof(inner));
        }

        public TResult Execute<TExecutable>(TExecutable func, Context context, CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            SyncPollyExecutableFunc<Policy, TExecutable, TResult> innerExecutionAsExecutable = new SyncPollyExecutableFunc<Policy, TExecutable, TResult>(
                (ctx, ct, policy, f) => policy.ExecuteSyncExecutableThroughPolicy<TExecutable, TResult>(f, ctx, ct),
                _innerPolicy,
                func);
            return _outerPolicy.ExecuteSyncExecutableThroughPolicy(
                innerExecutionAsExecutable,
                context,
                cancellationToken);

            // Equivalent to this, avoiding all closures: return _outerPolicy.Execute<TResult>((ctx, ct) => _innerPolicy.Execute<TResult>(func, ctx, ct), context, cancellationToken);
        }
    }

    internal class PolicyWrapSyncImplementationNonGenericGeneric<TResult> : ISyncPolicyImplementation<TResult>
    {
        private readonly Policy _outerPolicy;
        private readonly Policy<TResult> _innerPolicy;

        internal PolicyWrapSyncImplementationNonGenericGeneric(IsPolicy policy, Policy outer, ISyncPolicy<TResult> inner)
        {
            // PolicyWrap<TResult> nonGenericGenericWrap = policy as PolicyWrap<TResult> ?? throw new ArgumentNullException(nameof(policy)); // Will be used once we emit events.
            _outerPolicy = outer ?? throw new ArgumentNullException(nameof(outer));
            _innerPolicy = inner as Policy<TResult> ?? throw new ArgumentNullException(nameof(inner));
        }

        public TResult Execute<TExecutable>(TExecutable func, Context context, CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            SyncPollyExecutableFunc<Policy<TResult>, TExecutable, TResult> innerExecutionAsExecutable = new SyncPollyExecutableFunc<Policy<TResult>, TExecutable, TResult>(
                (ctx, ct, policy, f) => policy.ExecuteSyncExecutableThroughPolicy(f, ctx, ct),
                _innerPolicy,
                func);

            return _outerPolicy.ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFunc<Policy<TResult>, TExecutable, TResult>, TResult>(
                innerExecutionAsExecutable,
                context,
                cancellationToken);

            // Equivalent to this, avoiding all closures: return _outerPolicy.Execute<TResult>((ctx, ct) => _innerPolicy.Execute<TResult>(func, ctx, ct), context, cancellationToken);
        }
    }

}