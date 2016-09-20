using System;
using Polly.Utilities;

namespace Polly.Wrap
{
    public partial class PolicyWrap
    {
        private String _policyWrapKey;

        /// <summary>
        /// A key intended to be unique to each <see cref="PolicyWrap"/> instance, which is passed with executions as the <see cref="M:Context.PolicyWrapKey"/> property.
        /// </summary>
        public String PolicyWrapKey => _policyWrapKey ?? (_policyWrapKey = GetType().Name + KeyHelper.GuidPart());

        /// <summary>
        /// Sets the PolicyWrapKey for this <see cref="PolicyWrap"/> instance.
        /// <remarks>Must be called before the policy is first used.</remarks>
        /// </summary>
        /// <param name="policyWrapKey">The unique, used-definable key to assign to this <see cref="PolicyWrap"/> instance.</param>
        public PolicyWrap WithPolicyWrapKey(String policyWrapKey)
        {
            if (_policyWrapKey != null) throw new ArgumentException("PolicyWrapKey has already been set for this policy instance.", nameof(policyWrapKey));
            _policyWrapKey = policyWrapKey;
            return this;
        }
    }

    public partial class PolicyWrap<TResult>
    {
        private String _policyWrapKey;

        /// <summary>
        /// A key intended to be unique to each <see cref="PolicyWrap"/> instance, which is passed with executions as the <see cref="M:Context.PolicyWrapKey"/> property.
        /// </summary>
        public String PolicyWrapKey => _policyWrapKey ?? (_policyWrapKey = GetType().Name + KeyHelper.GuidPart());

        /// <summary>
        /// Sets the PolicyWrapKey for this <see cref="PolicyWrap"/> instance.
        /// <remarks>Must be called before the policy is first used.</remarks>
        /// </summary>
        /// <param name="policyWrapKey">The unique, used-definable key to assign to this <see cref="PolicyWrap"/> instance.</param>
        public PolicyWrap<TResult> WithPolicyWrapKey(String policyWrapKey)
        {
            if (_policyWrapKey != null) throw new ArgumentException("PolicyWrapKey has already been set for this policy instance.", nameof(policyWrapKey));
            _policyWrapKey = policyWrapKey;
            return this;
        }
    }
}
