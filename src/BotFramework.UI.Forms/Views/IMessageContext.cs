using System;
namespace BotFramework.UI
{
	public interface IMessageContext
	{
		bool IsFromMe { get; set; }
	}
}
