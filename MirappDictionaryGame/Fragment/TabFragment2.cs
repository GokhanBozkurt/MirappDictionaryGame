using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MirappDictionaryGame 
{
    public class TabFragment2 : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Tab, container, false);
            var sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
            sampleTextView.Text = "sample fragment text 2";

            return view;
        }
    }
}