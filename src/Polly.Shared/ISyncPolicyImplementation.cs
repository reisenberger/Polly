using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;

namespace Polly
{
    /// <summary>
    /// Defines methods which must be provided by a synchronous implementation of a policy, for delegates returning a result of type <typeparamref name="TResult"/>
    /// </summary>
    /// <typeparam name="TResult">The return type of the delegates executed through the policy implementation.</typeparam>
    public interface ISyncPolicyImplementation<TResult>
    {
        /// <summary>
        /// Executes the action synchronously through the policy implementation.
        /// </summary>
        /// <typeparam name="TExecutable">The type of the <see cref="ISyncPollyExecutable{TResult}"/> being executed.
        /// <remarks>By making this a generic type parameter constrained to <see cref="ISyncPollyExecutable{TResult}"/> on a generic method - rather than having the method parameter be of type <see cref="ISyncPollyExecutable{TResult}"/> directly - 
        /// we avoid boxing and thus a heap allocation, where the implementation of <see cref="ISyncPollyExecutable{TResult}"/> is a struct.</remarks>
        /// </typeparam>
        /// <param name="action">The synchronous action which will be executed.</param>
        /// <param name="context">Context data that is passed to the execution.</param>
        /// <param name="cancellationToken">The cancellation token which will govern the execution.</param>
        /// <returns>A result of type <typeparamref name="TResult"/></returns>
        TResult Execute<TExecutable>(in TExecutable action, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>;
    }
}
