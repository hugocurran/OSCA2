using System;
using System.Text;


namespace OSCA.Policy
{
    /// <summary>
    /// Class describing an OSCA policy exception
    /// </summary>
    public class PolicyEnforcementException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyEnforcementException"/> class.
        /// </summary>
        public PolicyEnforcementException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyEnforcementException"/> class.
        /// </summary>
        /// <param name="Message">The message.</param>
        public PolicyEnforcementException(string Message) : base(Message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyEnforcementException"/> class.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <param name="e">The e.</param>
        public PolicyEnforcementException(string Message, Exception e) : base(Message, e) { }

    }
}
