using System;

namespace Emdat.InVision
{
	/// <summary>
	/// Subscription with limited number of properties to save into cookie
	/// </summary>
	public class CookieSubscription
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CookieSubscription"/> class.
		/// </summary>
		public CookieSubscription(Subscription subscription)
		{
			this.FormatId = subscription.FormatId;
			this.NotifyOnScreen = subscription.NotifyOnScreen;
			this.NotifyEmail = subscription.NotifyEmail;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CookieSubscription"/> class, parameterless constructor.
		/// </summary>
		public CookieSubscription()
		{
			this.FormatId = "7"; //default to PDF;
			this.NotifyOnScreen = true; // default to on screen notification
			this.NotifyEmail = false;
		}

		#region properties

		public string FormatId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether user should be notified on screen when report is ready.
		/// </summary>
		/// <value><c>true</c> if user wantes on screen notification; otherwise, <c>false</c>.</value>
		public bool NotifyOnScreen { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether user should be notified over email when report is ready.
		/// </summary>
		/// <value><c>true</c> if user wantes email notification; otherwise, <c>false</c>.</value>
		public bool NotifyEmail { get; set; }

		#endregion
	}
}
