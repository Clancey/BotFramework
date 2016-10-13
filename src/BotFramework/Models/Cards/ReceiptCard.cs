using System;
namespace BotFramework
{
	public class ReceiptCard : Card
	{
		public const string ContentType = "application/vnd.microsoft.card.receipt";

		public ReceiptItem [] Items { get; set; }

		public Fact [] Facts { get; set; }

		public string Total { get; set; }

		public string Tax { get; set; }

		public string Vat { get; set; }
	}

	public class Fact
	{
		public string Key { get; set; }

		public string Value { get; set; }

	}

	public class ReceiptItem
	{
		public string Title { get; set; }

		public string Subtitle { get; set; }

		public string Text { get; set; }

		public CardImage Image { get; set; }

		public string Price { get; set; }

		public string Quantity { get; set; }

		public CardAction Tap { get; set; }
	}
}
