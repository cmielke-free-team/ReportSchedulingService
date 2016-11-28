using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdat.InVision
{
    /// <summary>
    /// 
    /// </summary>
    public class SubscriptionAlreadyExistsException : Exception
    {
        /// <summary>
        /// Gets or sets the subscription.
        /// </summary>
        /// <value>The subscription.</value>
        public Subscription Subscription { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        public SubscriptionAlreadyExistsException(Subscription subscription)
            : this(subscription, string.Format("A subscription named '{0}' already exists.", subscription.Description))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="message">The message.</param>
        public SubscriptionAlreadyExistsException(Subscription subscription, string message)
            : base(message)
        {
            this.Subscription = subscription;
        }
    }
}
