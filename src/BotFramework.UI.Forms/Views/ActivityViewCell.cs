using Xamarin.Forms;

namespace BotFramework.UI
{
    public class ActivityViewCell : ViewCell
    {
        public ActivityViewCell()
        {
            View = new ActivityView { ParentCell = this };
        }
    }
}
