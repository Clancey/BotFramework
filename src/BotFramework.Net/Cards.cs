﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BotFramework
{
    /// <summary>
    /// A Hero card (card with a single, large image)
    /// </summary>
    public class HeroCard : Card
    {
        public const string ContentType = "application/vnd.microsoft.card.hero";
    }

    public class SigninCard : Card
    {
        public const string ContentType = "application/vnd.microsoft.card.signin";
    }

    public class ThumbnailCard : Card
    {
        public const string ContentType = "application/vnd.microsoft.card.thumbnail";
    }

    public class AudioCard : MediaCard
    {
        public const string ContentType = "application/vnd.microsoft.card.audio";
    }
    public class AnimationCard : MediaCard
    {
        public const string ContentType = "application/vnd.microsoft.card.animation";
    }

    public class VideoCard : MediaCard
    {
        public const string ContentType = "application/vnd.microsoft.card.video";
    }

    public class Card
    {
        /// <summary>
        /// Gets or sets title of the card
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "title")]
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets subtitle of the card
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "subtitle")]
        public virtual string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets text for the card
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "text")]
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets array of images for the card
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "images")]
        public virtual CardImage[] Images { get; set; }

        /// <summary>
        /// Gets or sets set of actions applicable to the current card
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "buttons")]
        public virtual CardAction[] Buttons { get; set; }

        /// <summary>
        /// Gets or sets this action will be activated when user taps on the
        /// card itself
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "tap")]
        public virtual CardAction Tap { get; set; }
    }



    /// <summary>
    /// Object describing a media thumbnail
    /// </summary>
    public partial class ThumbnailUrl
    {
        /// <summary>
        /// Gets or sets url pointing to an thumbnail to use for media content
        /// </summary>
        [Newtonsoft.Json.JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets alt text to display for screen readers on the
        /// thumbnail image
        /// </summary>
        [Newtonsoft.Json.JsonProperty("alt")]
        public string Alt { get; set; }
    }

    /// <summary>
    /// MediaUrl data
    /// </summary>
    public partial class MediaUrl
    {
        /// <summary>
        /// Gets or sets url for the media
        /// </summary>
        [Newtonsoft.Json.JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets optional profile hint to the client to differentiate
        /// multiple MediaUrl objects from each other
        /// </summary>
        [Newtonsoft.Json.JsonProperty("profile")]
        public string Profile { get; set; }
    }

    public class MediaCard : Card
    {
        /// <summary>
        /// Gets or sets aspect ratio (16:9)(4:3)
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "aspect")]
        public string Aspect { get; set; }

        /// <summary>
        /// Gets or sets thumbnail placeholder
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "image")]
        public ThumbnailUrl Image { get; set; }

        /// <summary>
        /// Gets or sets array of media Url objects
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "media")]
        public MediaUrl[] Media { get; set; }



        /// <summary>
        /// Gets or sets is it OK for this content to be shareable with others
        /// (default:true)
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "shareable")]
        public bool? Shareable { get; set; }

        /// <summary>
        /// Gets or sets should the client loop playback at end of content
        /// (default:true)
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "autoloop")]
        public bool? Autoloop { get; set; }

        /// <summary>
        /// Gets or sets should the client automatically start playback of
        /// video in this card (default:true)
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "autostart")]
        public bool? Autostart { get; set; }
    }

    public class ReceiptCard : Card
    {
        public const string ContentType = "application/vnd.microsoft.card.receipt";

        /// <summary>
        /// Gets or sets array of Receipt Items
        /// </summary>
        public ReceiptItem[] Items { get; set; }

        /// <summary>
        /// Gets or sets array of Fact Objects   Array of key-value pairs.
        /// </summary>
        public Fact[] Facts { get; set; }

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
        [Newtonsoft.Json.JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value for this Fact
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "value")]
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
        [Newtonsoft.Json.JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets subtitle appears just below Title field, differs from
        /// Title in font styling only
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets text field appears just below subtitle, differs from
        /// Subtitle in font styling only
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets image
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "image")]
        public CardImage Image { get; set; }

        /// <summary>
        /// Gets or sets amount with currency
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

        /// <summary>
        /// Gets or sets number of items of given kind
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "quantity")]
        public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets this action will be activated when user taps on the
        /// Item bubble.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "tap")]
        public CardAction Tap { get; set; }
    }

    public class AdaptiveCard : Card
    {

        public const string ContentType = "application/vnd.microsoft.card.adaptive";
        public AdaptiveCards.AdaptiveCard Card { get; set; } = new AdaptiveCards.AdaptiveCard();
        public AdaptiveCard()
        {
        }

        /// <summary>
        ///     Title for the card (used when displayed in a dialog)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public override string Title { get => Card.Title; set => Card.Title = value; }

        public string Type { get => Card.Type; set => Card.Type = value; }


        public List<AdaptiveCards.AdaptiveElement> Body { get => Card.Body; set => Card.Body = value; }


        /// <summary>
        ///     Actions for this container
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AdaptiveCards.AdaptiveAction> Actions { get => Card.Actions; set => Card.Actions = value; }



        /// <summary>
        ///     Speak annotation for the card
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Speak { get => Card.Speak; set => Card.Speak = value; }

        /// <summary>
        ///     Background image for card
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Uri BackgroundImage { get => Card.BackgroundImage; set => Card.BackgroundImage = value; }

        /// <summary>
        ///     version of schema that this card was authored
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AdaptiveCards.AdaptiveSchemaVersion Version { get => Card.Version; set => Card.Version = value; }

        /// <summary>
        ///     if a client doesn't support the minVersion the card should be rejected.  If it does, then the elements that are not
        ///     supported are safe to ignore
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AdaptiveCards.AdaptiveSchemaVersion MinVersion { get => Card.MinVersion; set => Card.MinVersion = value; }

        /// <summary>
        ///     if a client is not able to show the card, show fallbackText to the user. This can be in markdown format.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FallbackText { get => Card.FallbackText; set => Card.FallbackText = value; }

        public static implicit operator AdaptiveCards.AdaptiveCard(AdaptiveCard card) => card.Card;
        public string ToJson() => Card?.ToJson();
        public string Json { get; set; }

    }
}