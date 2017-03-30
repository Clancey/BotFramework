using System;
namespace BotFramework
{
	public class ReceiptCard : Card
	{
		public const string ContentType = "application/vnd.microsoft.card.receipt";

		/// <summary>
		/// Gets or sets array of Receipt Items
		/// </summary>
		public ReceiptItem [] Items { get; set; }

		/// <summary>
		/// Gets or sets array of Fact Objects   Array of key-value pairs.
		/// </summary>
		public Fact [] Facts { get; set; }

		/// <summary>
		/// Gets or sets total amount of money paid (or should be paid)
		/// </summary>
		public string Total { get; set; }

		/// <summary>
		/// Gets or sets total amount of TAX paid(or should be paid)
		/// </summary>
		public string Tax { get; set; }

		/// <summary>
		/// Gets or sets total amount of VAT paid(or should be paid)
		/// </summary>
		public string Vat { get; set; }
	}

	/// <summary>
	/// Set of key-value pairs. Advantage of this section is that key and value
	/// properties will be
	/// rendered with default style information with some delimiter between
	/// them. So there is no need for developer to specify style information.
	/// </summary>
	public class Fact
	{
		/// <summary>
		/// Gets or sets the key for this Fact
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "key")]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the value for this Fact
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "value")]
		public string Value { get; set; }

	}

	/// <summary>
	/// An item on a receipt card
	/// </summary>
	public class ReceiptItem
	{
		/// <summary>
		/// Gets or sets title of the Card
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "title")]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets subtitle appears just below Title field, differs from
		/// Title in font styling only
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "subtitle")]
		public string Subtitle { get; set; }

		/// <summary>
		/// Gets or sets text field appears just below subtitle, differs from
		/// Subtitle in font styling only
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "text")]
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets image
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "image")]
		public CardImage Image { get; set; }

		/// <summary>
		/// Gets or sets amount with currency
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "price")]
		public string Price { get; set; }

		/// <summary>
		/// Gets or sets number of items of given kind
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "quantity")]
		public string Quantity { get; set; }

		/// <summary>
		/// Gets or sets this action will be activated when user taps on the
		/// Item bubble.
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "tap")]
		public CardAction Tap { get; set; }
	}
}
