using System;
using System.Linq;
using Polly.CircuitBreaker;
using Polly.Utilities;

namespace Polly
{
    /// <summary>
    /// Fluent API for defining a Circuit Breaker <see cref="Policy"/>. 
    /// </summary>
    public static class CircuitBreakerSyntax
    {
        /// <summary>
        /// <para> Builds a <see cref="Policy"/> that will function like a Circuit Breaker.</para>
        /// <para>The circuit will break if <paramref name="exceptionsAllowedBeforeBreaking"/>
        /// exceptions that are handled by this policy are raised consecutively. </para>
        /// <para>The circuit will stay broken for the <paramref name="durationOfBreak"/>. Any attempt to execute this policy
        /// while the circuit is broken, will immediately throw a <see cref="BrokenCircuitException"/> containing the exception 
        /// that broke the circuit.
        /// </para>
        /// <para>If the first action after the break duration period results in a handled exception, the circuit will break
        /// again for another <paramref name="durationOfBreak"/>; if no exception is thrown, the circuit will reset.
        /// </para>
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="exceptionsAllowedBeforeBreaking">The number of exceptions that are allowed before opening the circuit.</param>
        /// <param name="durationOfBreak">The duration the circuit will stay open before resetting.</param>
        /// <returns>The policy instance.</returns>
        /// <remarks>(see "Release It!" by Michael T. Nygard fi)</remarks>
        /// <exception cref="System.ArgumentOutOfRangeException">exceptionsAllowedBeforeBreaking;Value must be greater than zero.</exception>
        /// <exception cref="ArgumentNullException">onBreak</exception>
        /// <exception cref="ArgumentNullException">onReset</exception>
        public static CircuitBreakerPolicy CircuitBreaker(this PolicyBuilder policyBuilder, int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak)
        {
            Action<Exception, TimeSpan> doNothingOnBreak = (_, __) => { };
            Action doNothingOnReset = () => { };

            return policyBuilder.CircuitBreaker
                (exceptionsAllowedBeforeBreaking, 
                durationOfBreak, 
                doNothingOnBreak,
                doNothingOnReset
                );
        }

        /// <summary>
        /// <para> Builds a <see cref="Policy"/> that will function like a Circuit Breaker.</para>
        /// <para>The circuit will break if <paramref name="exceptionsAllowedBeforeBreaking"/>
        /// exceptions that are handled by this policy are raised consecutively. </para>
        /// <para>The circuit will stay broken for the <paramref name="durationOfBreak"/>. Any attempt to execute this policy
        /// while the circuit is broken, will immediately throw a <see cref="BrokenCircuitException"/> containing the exception 
        /// that broke the circuit.
        /// </para>
        /// <para>If the first action after the break duration period results in a handled exception, the circuit will break
        /// again for another <paramref name="durationOfBreak"/>; if no exception is thrown, the circuit will reset.
        /// </para>
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="exceptionsAllowedBeforeBreaking">The number of exceptions that are allowed before opening the circuit.</param>
        /// <param name="durationOfBreak">The duration the circuit will stay open before resetting.</param>
        /// <param name="onBreak">The action to call when the circuit transitions to an <see cref="CircuitState.Open"/> state.</param>
        /// <param name="onReset">The action to call when the circuit resets to a <see cref="CircuitState.Closed"/> state.</param>
        /// <returns>The policy instance.</returns>
        /// <remarks>(see "Release It!" by Michael T. Nygard fi)</remarks>
        /// <exception cref="System.ArgumentOutOfRangeException">exceptionsAllowedBeforeBreaking;Value must be greater than zero.</exception>
        /// <exception cref="ArgumentNullException">onBreak</exception>
        /// <exception cref="ArgumentNullException">onReset</exception>
        public static CircuitBreakerPolicy CircuitBreaker(this PolicyBuilder policyBuilder, int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak, Action<Exception, TimeSpan> onBreak, Action onReset)
        {
            return policyBuilder.CircuitBreaker(
                exceptionsAllowedBeforeBreaking, 
                durationOfBreak, 
                (exception, timespan, context) => onBreak(exception, timespan), 
                context => onReset()
                );
        }

        /// <summary>
        /// <para> Builds a <see cref="Policy"/> that will function like a Circuit Breaker.</para>
        /// <para>The circuit will break if <paramref name="exceptionsAllowedBeforeBreaking"/>
        /// exceptions that are handled by this policy are raised consecutively. </para>
        /// <para>The circuit will stay broken for the <paramref name="durationOfBreak"/>. Any attempt to execute this policy
        /// while the circuit is broken, will immediately throw a <see cref="BrokenCircuitException"/> containing the exception 
        /// that broke the circuit.
        /// </para>
        /// <para>If the first action after the break duration period results in a handled exception, the circuit will break
        /// again for another <paramref name="durationOfBreak"/>; if no exception is thrown, the circuit will reset.
        /// </para>
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="exceptionsAllowedBeforeBreaking">The number of exceptions that are allowed before opening the circuit.</param>
        /// <param name="durationOfBreak">The duration the circuit will stay open before resetting.</param>
        /// <param name="onBreak">The action to call when the circuit transitions to an <see cref="CircuitState.Open"/> state.</param>
        /// <param name="onReset">The action to call when the circuit resets to a <see cref="CircuitState.Closed"/> state.</param>
        /// <returns>The policy instance.</returns>
        /// <remarks>(see "Release It!" by Michael T. Nygard fi)</remarks>
        /// <exception cref="System.ArgumentOutOfRangeException">exceptionsAllowedBeforeBreaking;Value must be greater than zero.</exception>
        /// <exception cref="ArgumentNullException">onBreak</exception>
        /// <exception cref="ArgumentNullException">onReset</exception>
        public static CircuitBreakerPolicy CircuitBreaker(this PolicyBuilder policyBuilder, int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak, Action<Exception, TimeSpan, Context> onBreak, Action<Context> onReset)
        {
            Action doNothingOnHalfOpen = () => { };
            return policyBuilder.CircuitBreaker(exceptionsAllowedBeforeBreaking, 
                durationOfBreak, 
                onBreak, 
                onReset,
                doNothingOnHalfOpen
                );
        }

        /// <summary>
        /// <para> Builds a <see cref="Policy"/> that will function like a Circuit Breaker.</para>
        /// <para>The circuit will break if <paramref name="exceptionsAllowedBeforeBreaking"/>
        /// exceptions that are handled by this policy are raised consecutively. </para>
        /// <para>The circuit will stay broken for the <paramref name="durationOfBreak"/>. Any attempt to execute this policy
        /// while the circuit is broken, will immediately throw a <see cref="BrokenCircuitException"/> containing the exception 
        /// that broke the circuit.
        /// </para>
        /// <para>If the first action after the break duration period results in a handled exception, the circuit will break
        /// again for another <paramref name="durationOfBreak"/>; if no exception is thrown, the circuit will reset.
        /// </para>
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="exceptionsAllowedBeforeBreaking">The number of exceptions that are allowed before opening the circuit.</param>
        /// <param name="durationOfBreak">The duration the circuit will stay open before resetting.</param>
        /// <param name="onBreak">The action to call when the circuit transitions to an <see cref="CircuitState.Open"/> state.</param>
        /// <param name="onReset">The action to call when the circuit resets to a <see cref="CircuitState.Closed"/> state.</param>
        /// <param name="onHalfOpen">The action to call when the circuit transitions to <see cref="CircuitState.HalfOpen"/> state, ready to try action executions again. </param>
        /// <returns>The policy instance.</returns>
        /// <remarks>(see "Release It!" by Michael T. Nygard fi)</remarks>
        /// <exception cref="System.ArgumentOutOfRangeException">exceptionsAllowedBeforeBreaking;Value must be greater than zero.</exception>
        /// <exception cref="ArgumentNullException">onBreak</exception>
        /// <exception cref="ArgumentNullException">onReset</exception>
        public static CircuitBreakerPolicy CircuitBreaker(this PolicyBuilder policyBuilder, int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak, Action<Exception, TimeSpan> onBreak, Action onReset, Action onHalfOpen)
        {
            return policyBuilder.CircuitBreaker(
                exceptionsAllowedBeforeBreaking,
                durationOfBreak,
                (exception, timespan, context) => onBreak(exception, timespan),
                context => onReset(),
                onHalfOpen
                );
        }

        /// <summary>
        /// <para> Builds a <see cref="Policy"/> that will function like a Circuit Breaker.</para>
        /// <para>The circuit will break if <paramref name="exceptionsAllowedBeforeBreaking"/>
        /// exceptions that are handled by this policy are raised consecutively. </para>
        /// <para>The circuit will stay broken for the <paramref name="durationOfBreak"/>. Any attempt to execute this policy
        /// while the circuit is broken, will immediately throw a <see cref="BrokenCircuitException"/> containing the exception 
        /// that broke the circuit.
        /// </para>
        /// <para>If the first action after the break duration period results in a handled exception, the circuit will break
        /// again for another <paramref name="durationOfBreak"/>; if no exception is thrown, the circuit will reset.
        /// </para>
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="exceptionsAllowedBeforeBreaking">The number of exceptions that are allowed before opening the circuit.</param>
        /// <param name="durationOfBreak">The duration the circuit will stay open before resetting.</param>
        /// <param name="onBreak">The action to call when the circuit transitions to an <see cref="CircuitState.Open"/> state.</param>
        /// <param name="onReset">The action to call when the circuit resets to a <see cref="CircuitState.Closed"/> state.</param>
        /// <param name="onHalfOpen">The action to call when the circuit transitions to <see cref="CircuitState.HalfOpen"/> state, ready to try action executions again. </param>
        /// <returns>The policy instance.</returns>
        /// <remarks>(see "Release It!" by Michael T. Nygard fi)</remarks>
        /// <exception cref="System.ArgumentOutOfRangeException">exceptionsAllowedBeforeBreaking;Value must be greater than zero.</exception>
        /// <exception cref="ArgumentNullException">onBreak</exception>
        /// <exception cref="ArgumentNullException">onReset</exception>
        /// <exception cref="ArgumentNullException">onHalfOpen</exception>
        public static CircuitBreakerPolicy CircuitBreaker(this PolicyBuilder policyBuilder, int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak, Action<Exception, TimeSpan, Context> onBreak, Action<Context> onReset, Action onHalfOpen)
        {
            IConsecutiveCountCircuitBreakerConfiguration configuration = new ConsecutiveCountCircuitBreakerConfiguration(exceptionsAllowedBeforeBreaking, durationOfBreak);

            return policyBuilder.CircuitBreaker(configuration, onBreak, onReset, onHalfOpen);
        }

        /// <summary>
        /// <para> Builds a <see cref="Policy"/> that will function like a Circuit Breaker.</para>
        /// <para>The circuit will break according to the configuration provided in the <see cref="IConsecutiveCountCircuitBreakerConfiguration"/>
        /// </para>
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="configuration">The configuration parameters for a <see cref="CircuitBreakerPolicy"/>.</param>
        /// <param name="onBreak">The action to call when the circuit transitions to an <see cref="CircuitState.Open"/> state.</param>
        /// <param name="onReset">The action to call when the circuit resets to a <see cref="CircuitState.Closed"/> state.</param>
        /// <param name="onHalfOpen">The action to call when the circuit transitions to <see cref="CircuitState.HalfOpen"/> state, ready to try action executions again. </param>
        /// <returns>The policy instance.</returns>
        /// <remarks>(see "Release It!" by Michael T. Nygard fi)</remarks>
        /// <exception cref="System.ArgumentOutOfRangeException">exceptionsAllowedBeforeBreaking;Value must be greater than zero.</exception>
        /// <exception cref="ArgumentNullException">onBreak</exception>
        /// <exception cref="ArgumentNullException">onReset</exception>
        /// <exception cref="ArgumentNullException">onHalfOpen</exception>
        public static CircuitBreakerPolicy CircuitBreaker(this PolicyBuilder policyBuilder, IConsecutiveCountCircuitBreakerConfiguration configuration, Action<Exception, TimeSpan, Context> onBreak, Action<Context> onReset, Action onHalfOpen)
        {
            if (onBreak == null) throw new ArgumentNullException("onBreak");
            if (onReset == null) throw new ArgumentNullException("onReset");
            if (onHalfOpen == null) throw new ArgumentNullException("onHalfOpen");

            var breakerController = new ConsecutiveCountCircuitController<EmptyStruct>(
                configuration,
                (outcome, timespan, context) => onBreak(outcome.Exception, timespan, context),
                onReset,
                onHalfOpen);
            return new CircuitBreakerPolicy(
                (action, context, cancellationToken) => CircuitBreakerEngine.Implementation(
                    (ctx, ct) => { action(ctx, ct); return EmptyStruct.Instance; },
                    context,
                    cancellationToken,
                    policyBuilder.ExceptionPredicates,
                    PredicateHelper<EmptyStruct>.EmptyResultPredicates,
                    breakerController),
                policyBuilder.ExceptionPredicates,
                breakerController
            );
        }
    }
}