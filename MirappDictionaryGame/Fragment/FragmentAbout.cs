using Android.App;
using Android.OS;
using Android.Views;

namespace MirappDictionaryGame 
{
    public class FragmentAbout :Fragment
    {

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.AppAbout, container, false);
            return view;
        }

    }


}