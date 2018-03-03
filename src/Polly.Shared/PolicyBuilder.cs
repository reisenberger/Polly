﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Polly
{
    /// <summary>
    /// Builder class that holds the list of current exception predicates.
    /// </summary>
    public sealed partial class PolicyBuilder
    {
        internal static PolicyBuilder Empty = new PolicyBuilder(); 

        private readonly IList<ExceptionPredicate> _exceptionPredicates = new List<ExceptionPredicate>();

        internal PolicyBuilder() { }

        internal PolicyBuilder(ExceptionPredicate exceptionPredicate) : this()
        {
            _exceptionPredicates.Add(exceptionPredicate);
        }

        internal PolicyBuilder(IEnumerable<ExceptionPredicate> exceptionPredicates) : this()
        {
            _exceptionPredicates = new List<ExceptionPredicate>(exceptionPredicates);
        }

        internal IList<ExceptionPredicate> ExceptionPredicates
        {
            get { return _exceptionPredicates; }
        }

        #region Hide object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type" /> of the current instance.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Type" /> instance that represents the exact runtime type of the current instance.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }

        #endregion
    }

    /// <summary>
    /// Builder class that holds the list of current execution predicates filtering TResult result values.
    /// </summary>
    public sealed partial class PolicyBuilder<TResult>
    {
        internal static PolicyBuilder<TResult> Empty = new PolicyBuilder<TResult>();

        private readonly IList<ExceptionPredicate> _exceptionPredicates = new List<ExceptionPredicate>();
        private readonly IList<ResultPredicate<TResult>> _resultPredicates = new List<ResultPredicate<TResult>>();

        internal PolicyBuilder() { }

        internal PolicyBuilder(Func<TResult, bool> resultPredicate) : this()
        {
            this.OrResult(resultPredicate);
        }

        internal PolicyBuilder(ExceptionPredicate predicate) : this()
        {
            _exceptionPredicates.Add(predicate);
        }

        internal PolicyBuilder(IEnumerable<ExceptionPredicate> exceptionPredicates)
            : this()
        {
            _exceptionPredicates = new List<ExceptionPredicate>(exceptionPredicates);
        }

        internal PolicyBuilder(IEnumerable<ExceptionPredicate> exceptionPredicates, IEnumerable<ResultPredicate<TResult>> resultPredicates)
            : this()
        {
            _exceptionPredicates = new List<ExceptionPredicate>(exceptionPredicates);
            _resultPredicates = new List<ResultPredicate<TResult>>(resultPredicates);
        }

        internal IList<ExceptionPredicate> ExceptionPredicates
        {
            get { return _exceptionPredicates; }
        }

        internal IList<ResultPredicate<TResult>> ResultPredicates
        {
            get { return _resultPredicates; }
        }

        #region Hide object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type" /> of the current instance.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Type" /> instance that represents the exact runtime type of the current instance.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }

        #endregion
    }
}