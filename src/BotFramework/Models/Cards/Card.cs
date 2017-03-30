using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BotFramework
{
	public class Card
	{
		/// <summary>
		/// Gets or sets title of the card
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "title")]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets subtitle of the card
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "subtitle")]
		public string Subtitle { get; set; }

		/// <summary>
		/// Gets or sets text for the card
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "text")]
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets array of images for the card
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "images")]
		public CardImage [] Images { get; set; }

		/// <summary>
		/// Gets or sets set of actions applicable to the current card
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "buttons")]
		public CardAction [] Buttons { get; set; }

		/// <summary>
		/// Gets or sets this action will be activated when user taps on the
		/// card itself
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "tap")]
		public CardAction Tap { get; set; }
	}
}
