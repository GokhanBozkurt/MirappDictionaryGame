using Android.Content;
using Android.Widget;

namespace MirappDictionaryGame
{
    public class ManagerAdapter
    {
        //private static ManagerAdapter instance;

        //public ManagerAdapter Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance=new ManagerAdapter();
        //        }
        //        return instance;
        //    }
        //}

        //public void Initilaze()
        //{

        //}  

        public static ISpinnerAdapter GetLanguageAdapter(Context context)
        {
            var adapter = ArrayAdapter.CreateFromResource(context, Resource.Array.Languages,Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            return adapter;
        }
    }
}