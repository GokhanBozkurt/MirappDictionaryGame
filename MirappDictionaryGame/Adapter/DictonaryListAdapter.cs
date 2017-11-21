using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
namespace MirappDictionaryGame 
{
    class DictonaryListAdapter : BaseAdapter<MyDictonaryWord>
    {
        private readonly IList<MyDictonaryWord> _items;
        private readonly Activity _context;

        public DictonaryListAdapter(Activity context, IList<MyDictonaryWord> items)
        {
            this._context = context;
            this._items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override MyDictonaryWord this[int position] => _items[position];

        public override int Count => _items.Count;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.DictionaryListRow, null); 
            view.FindViewById<TextView>(Resource.Id.DictonaryRowWordID).Text = _items[position].Id.ToString();
            view.FindViewById<TextView>(Resource.Id.DictonaryRowWord).Text = _items[position].Word.ToUpper();
            view.FindViewById<TextView>(Resource.Id.DictonaryRowToWord).Text = _items[position].TranslatedWord.ToUpper();
            
            return view;
        }

        public void Remove(long id)
        {
        }
    }
}