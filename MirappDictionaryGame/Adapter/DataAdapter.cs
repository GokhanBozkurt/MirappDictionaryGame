using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace MirappDictionaryGame
{

    public class DataAdapter : RecyclerView.Adapter
    {
        private readonly Activity _context;
        private readonly List<MyDictonaryWord> _items;
        private bool favorites;

        public delegate bool DeleteDelegate(MyDictonaryWord myDictonaryWord, bool favorites);
        public event DeleteDelegate DeleteEvent;
        public event DeleteDelegate AddEvent;
        public DataAdapter(Activity context, List<MyDictonaryWord> items,bool favorites)
        {
            this._context = context;
            this._items = items;
            this.favorites = favorites;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ((ViewHolder)holder).Word.Text = _items[position].Word;
            ((ViewHolder)holder).TransltedWord.Text = _items[position].TranslatedWord;
            ((ViewHolder)holder).SnonymWord.Text = _items[position].SnonymWord; 
            ((ViewHolder)holder).Id.Text = _items[position].Id.ToString(); 
        }
        public override void RegisterAdapterDataObserver(RecyclerView.AdapterDataObserver observer)
        {
            base.RegisterAdapterDataObserver(observer); 
        }

        public override void UnregisterAdapterDataObserver(RecyclerView.AdapterDataObserver observer)
        {
            base.UnregisterAdapterDataObserver(observer);
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row_layout, parent, false);
            return new ViewHolder(view);
        }

        public override int ItemCount
        {
            get
            {
                return _items.Count;
            }
        }

        public void addItem(MyDictonaryWord myDictonaryWord)
        {
            if (AddEvent(myDictonaryWord, favorites))
            {
                _items.Add(myDictonaryWord);
                NotifyItemInserted(_items.Count);
            }
            else
            {
                NotifyDataSetChanged();
            }

        }

        public void removeItem(int position, MyDictonaryWord myDictonaryWord)
        {
            if (DeleteEvent(myDictonaryWord, favorites))
            {
                _items.Remove(myDictonaryWord);
                NotifyDataSetChanged();
                //NotifyItemRemoved(position);
                //NotifyItemRangeChanged(position, _items.Count);
            }
            else
            {
                NotifyDataSetChanged();
            }

        }

        public MyDictonaryWord getItem(int position)
        {
            return _items[position];
        }


        public class ViewHolder : RecyclerView.ViewHolder
        {
           public  TextView Id;
           public  TextView Word;
            public  TextView TransltedWord;
            public TextView SnonymWord;
            public ViewHolder(View view) : base(view)
            {
                Id = (TextView)view.FindViewById(Resource.Id.listwordItemWordId);
                Word = (TextView)view.FindViewById(Resource.Id.listwordItemWord);
                TransltedWord = (TextView)view.FindViewById(Resource.Id.listwordItemTranslatedWord);
                SnonymWord = (TextView)view.FindViewById(Resource.Id.listwordItemSnonymWord);
            }
        }

    }

    
}