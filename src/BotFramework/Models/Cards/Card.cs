using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BotFramework
{
	public class Card
	{
		[JsonProperty ("title")]
		public string Title { get; set; }

		[JsonProperty ("subtitle")]
		public string Subtitle { get; set; }

		[JsonProperty ("text")]
		public string Text { get; set; }

		[JsonProperty ("images")]
		public IList<CardImage> Images { get; set; }

		[JsonProperty ("buttons")]
		public IList<CardAction> Buttons { get; set; }

		[JsonProperty ("tap")]
		public CardAction Tap { get; set; }
	}
}
