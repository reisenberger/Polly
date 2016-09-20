using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using Polly.Events;

namespace Polly
{
    public partial class Policy
    {
        /// <summary>
        /// A <see cref="Subject{PolicyEvent}"/>  representing events emitted during executions through this <see cref="Policy"/>.
        /// </summary>
        protected readonly ISubject<PolicyEvent> _eventSubject = new Subject<PolicyEvent>();

        /// <summary>
        /// Subscribes the subscriber to events emitted by this policy.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public Policy WithEventSubscriber(IObserver<PolicyEvent> subscriber)
        {
            _eventSubject.Subscribe(subscriber);
            return this;
        }
    }

    public partial class Policy<TResult>
    {
        /// <summary>
        /// A <see cref="Subject{PolicyEvent}"/>  representing events emitted during executions through this <see cref="Policy{TResult}"/>.
        /// </summary>
        protected readonly ISubject<PolicyEvent> _eventSubject = new Subject<PolicyEvent>();

        /// <summary>
        /// Subscribes the subscriber to events emitted by this policy.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public Policy<TResult> WithEventSubscriber(IObserver<PolicyEvent> subscriber)
        {
            _eventSubject.Subscribe(subscriber);
            return this;
        }

    }
}
