using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;

namespace Polly
{
    /// <summary>
    /// Defines methods which must be provided by an asynchronous implementation of a policy, for delegates returning a promise of a result of type <typeparamref name="TResult"/>
    /// </summary>
    /// <typeparam name="TResult">The return type of the delegates executed through the policy implementation.</typeparam>
    public interface IAsyncPolicyImplementation<TResult>
    {
        /// <summary>
        /// Executes the action asynchronously through the policy implementation.
        /// </summary>
        /// <typeparam name="TExecutableAsync">The type of the <see cref="IAsyncPollyExecutable{TResult}"/> being executed.
        /// <remarks>By making this a generic type parameter on a generic method - constrained to <see cref="IAsyncPollyExecutable{TResult}"/> - rather than having the method parameter be of type <see cref="IAsyncPollyExecutable{TResult}"/> directly - 
        /// we avoid boxing and thus a heap allocation, where the implementation of <see cref="IAsyncPollyExecutable{TResult}"/> is a struct.</remarks>
        /// </typeparam>
        /// <param name="action">The asynchronous action which will be executed.</param>
        /// <param name="context">Context data that is passed to the execution.</param>
        /// <param name="cancellationToken">The cancellation token which will govern the execution.</param>
        /// <param name="continueOnCapturedContext">Whether internal execution should continue on a captured context, after an await.</param>
        /// <returns>A promise of a result of type <typeparamref name="TResult"/></returns>        
        Task<TResult> ExecuteAsync<TExecutableAsync>(TExecutableAsync action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutableAsync : IAsyncPollyExecutable<TResult>;
    }
}
