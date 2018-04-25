using System;
using Xamarin.Forms;

namespace BotFramework.UI
{
    public class ReceiptCardView : CardView
    {
        public ReceiptCardView()
        {
        }

        protected override void SetupView()
        {
            var attachment = (BindingContext as Attachment);
            var reciept = attachment?.Content as ReceiptCard;
            AddImages(attachment);
            AddFacts(reciept?.Facts);
            AddRecieptLines(reciept?.Items);
            AddButtons(attachment);
        }

        protected virtual void AddFooter(ReceiptCard reciept)
        {
            if (!string.IsNullOrWhiteSpace(reciept.Tax))
                Children.Add(new Label
                {
                    HorizontalTextAlignment = TextAlignment.End,
                    Text = $"Tax :{reciept.Tax}",
                });

            if (!string.IsNullOrWhiteSpace(reciept.Vat))
                Children.Add(new Label
                {
                    HorizontalTextAlignment = TextAlignment.End,
                    Text = $"Vat :{reciept.Vat}",
                });

            if (!string.IsNullOrWhiteSpace(reciept.Total))
                Children.Add(new Label
                {
                    HorizontalTextAlignment = TextAlignment.End,
                    Text = $"Total :{reciept.Total}",
                });
        }

        protected virtual void AddRecieptLines(ReceiptItem[] items)
        {
            if (items == null)
                return;
            foreach (var item in items)
            {
                AddRecieptLineItem(item);
            }
        }

        protected virtual void AddRecieptLineItem(ReceiptItem item)
        {
            Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BindingContext = item,
                Children = {
                    new Label().SetBindings(Label.TextProperty,"Title"),
                    new Label{ HorizontalTextAlignment = TextAlignment.End, VerticalOptions = LayoutOptions.EndAndExpand }.SetBindings(Label.TextProperty,"Price"),
                }
            });

        }

        protected virtual void AddFacts(Fact[] facts)
        {
            if (facts == null)
                return;
            foreach (var fact in facts)
                AddFact(fact);
        }

        protected virtual void AddFact(Fact fact)
        {
            Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BindingContext = fact,
                Children = {
                    new Label().SetBindings(Label.TextProperty,"Key"),
                    new Label{ HorizontalTextAlignment = TextAlignment.End, VerticalOptions = LayoutOptions.EndAndExpand }.SetBindings(Label.TextProperty,"Value"),
                }
            });
        }
    }
}
