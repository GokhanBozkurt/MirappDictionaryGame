
using Android.Support.V4.App;
using Android.Widget;

namespace MirappDictionaryGame
{
    public class BaseFragment : Fragment
    {
        protected ListView ListView;

        protected virtual void HandleEvents()
        {
            if (ListView!=null)
            {
            }
            
        }

        protected virtual  void FindViews()
        {
            //ListView = this.View.FindViewById<ListView>(Resource.Id.Listview1);
            ListView.ChoiceMode = ChoiceMode.Single;


        }
    }
}