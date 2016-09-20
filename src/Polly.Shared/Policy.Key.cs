using System;
using Polly.Utilities;

namespace Polly
{
    public partial class Policy
    {
        private String _policyKey;

        /// <summary>
        /// A key intended to be unique to each <see cref="Policy"/> instance, which is passed with executions as the <see cref="M:Context.PolicyKey"/> property.
        /// </summary>
        public String PolicyKey => _policyKey ?? (_policyKey = GetType().Name + KeyHelper.GuidPart());

        /// <summary>
        /// Sets the PolicyKey for this <see cref="Policy"/> instance.
        /// <remarks>Must be called before the policy is first used.</remarks>
        /// </summary>
        /// <param name="policyKey">The unique, used-definable key to assign to this <see cref="Policy"/> instance.</param>
        public Policy WithPolicyKey(String policyKey)
        {
            if (_policyKey != null) throw new ArgumentException("PolicyKey has already been set for this policy instance.", nameof(policyKey));
            _policyKey = policyKey;
            return this;
        }
    }

    public partial class Policy<TResult>
    {
        private String _policyKey;

        /// <summary>
        /// A key intended to be unique to each <see cref="Policy"/> instance, which is passed with executions as the <see cref="M:Context.PolicyKey"/> property.
        /// </summary>
        public String PolicyKey => _policyKey ?? (_policyKey = GetType().Name + KeyHelper.GuidPart());

        /// <summary>
        /// Sets the PolicyKey for this <see cref="Policy"/> instance.
        /// <remarks>Must be called before the policy is first used.</remarks>
        /// </summary>
        /// <param name="policyKey">The unique, used-definable key to assign to this <see cref="Policy"/> instance.</param>
        public Policy<TResult> WithPolicyKey(String policyKey)
        {
            if (_policyKey != null) throw new ArgumentException("PolicyKey has already been set for this policy instance.", nameof(policyKey));
            _policyKey = policyKey;
            return this;
        }
    }
}
