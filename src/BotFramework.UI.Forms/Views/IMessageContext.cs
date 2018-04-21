using System;
using Xamarin.Forms;

namespace BotFramework.UI
{
	public interface IMessageContext
	{
		bool IsFromMe { get; set; }
        ViewCell HostingCell { get; set; }
	}
}
