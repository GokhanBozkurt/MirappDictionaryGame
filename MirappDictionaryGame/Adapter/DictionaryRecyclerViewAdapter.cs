using System.Collections.Generic;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;

namespace MirappDictionaryGame
{
    public class DictionaryRecyclerViewAdapter : RecyclerView.Adapter
    {
        private readonly Activity _context;
        private readonly IList<MyDictonaryWord> _items;
        private bool HideTranslation = false;
        private bool ShowSnonym=false;

        // Event handler for item clicks:
        public event EventHandler<string> ItemClick;
        public event EventHandler<RelativeLayout> ItemDeleteClick;
        public event EventHandler<RelativeLayout> ItemLongClick;
        public event EventHandler<RelativeLayout> ItemEditClick;
        public DictionaryRecyclerViewAdapter(Activity context, IList<MyDictonaryWord> items)
        {
            this._context = context;
            this._items = items;
        }

        public DictionaryRecyclerViewAdapter(Activity context, IList<MyDictonaryWord> items, bool hideTranslation, bool showSnonym) : this(context, items)
        {
            this.HideTranslation = hideTranslation;
            this.ShowSnonym = showSnonym;
        }

        internal void removeItem(int position)
        {
            _items.RemoveAt(position);
            NotifyDataSetChanged();
        }

        public class ViewHolder : RecyclerView.ViewHolder
        {
            public TextView DictonaryRowWordId { get; set; }
            public TextView DictonaryRowWord { get; set; }
            public TextView DictonaryRowToWord { get; set; }
            public TextView SnonymWord { get; set; }
            public Button DictionaryListRowDelete { get; set; }
            public Button DictionaryListRowEdit { get; set; }
            
            public ViewHolder(RelativeLayout view, bool hideTransaltion,bool showSnonym, Action<string> listener,Action<RelativeLayout> listener_delete, Action<RelativeLayout> listener_long_click,Action<RelativeLayout> listener_edit) : base(view)
            {
                
                DictonaryRowWordId = view.FindViewById<TextView>(Resource.Id.DictonaryRowWordID);
                DictonaryRowWord = view.FindViewById<TextView>(Resource.Id.DictonaryRowWord);                
                DictonaryRowToWord = view.FindViewById<TextView>(Resource.Id.DictonaryRowToWord);
                SnonymWord = view.FindViewById<TextView>(Resource.Id.SnonymWord);
                
                DictonaryRowToWord.Visibility = hideTransaltion ? ViewStates.Invisible : ViewStates.Visible;
                SnonymWord.Visibility = showSnonym ? ViewStates.Visible : ViewStates.Invisible;
                DictionaryListRowDelete = view.FindViewById<Button>(Resource.Id.DictionaryListRowDelete);
                DictionaryListRowEdit = view.FindViewById<Button>(Resource.Id.DictionaryListRowEdit);
                /*DictionaryListRowDelete.Click += (sender, e) =>
                {
                    var id = DictonaryRowWordId.Text;
                    if (favorite)
                    {
                        ManagerRepository.Instance.FavoriteWord.Delete(new FavoriteWord { Id = int.Parse(id), Word = DictonaryRowWord.Text, TranslatedWord = DictonaryRowToWord.Text });
                    }
                    else
                    {
                        var word = new MyDictonaryWord { Id = int.Parse(id), Word = DictonaryRowWord.Text, TranslatedWord = DictonaryRowToWord.Text };
                        Toast.MakeText(view.Context, "This is id " + id + " " + word.ToString(), ToastLength.Short).Show();
                        ManagerRepository.Instance.MyDictonaryWord.Delete(word);
                        ManagerDictionary.DictonaryUpdated();
                    }
                };
                int iddd=0;
                try
                {
                    iddd = int.Parse(DictonaryRowWordId.Text);
                }
                catch (Exception)
                {

                }
                var worda = new MyDictonaryWord { Id = iddd, Word = DictonaryRowWord.Text, TranslatedWord = DictonaryRowToWord.Text };
               */
                DictionaryListRowDelete.Click += (sender, e) => listener_delete(view);
                DictionaryListRowEdit.Click += (sender, e) => listener_edit(view);


                view.Click += (sender, e) => listener(DictonaryRowWordId.Text);
                view.LongClick += (sender, e) => listener_long_click(view);
            }
        }

        public override int ItemCount => _items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var litem = _items[position];
            ((ViewHolder)holder).DictonaryRowWordId.Text = litem.Id.ToString();
            ((ViewHolder)holder).DictonaryRowWord.Text = litem.Word;
            ((ViewHolder)holder).DictonaryRowToWord.Text = litem.TranslatedWord;
            ((ViewHolder)holder).SnonymWord.Text = litem.SnonymWord;
            //((ViewHolder)holder).TextView.SetCompoundDrawablesWithIntrinsicBounds(flagResId, 0, 0, 0);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var relativeLayout = (RelativeLayout)LayoutInflater.From(parent.Context).Inflate(Resource.Layout.DictionaryListRow, parent, false);
            return new ViewHolder(relativeLayout, HideTranslation, ShowSnonym,OnClick, OnDeleteClick, OnLongClick,OnEditClick);
        }
        void OnClick(string position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
        void OnDeleteClick(RelativeLayout view)
        {
            if (ItemDeleteClick != null)
                ItemDeleteClick(this, view);
        }
        void OnLongClick(RelativeLayout view)
        {
            if (ItemLongClick != null)
                ItemLongClick(this, view);
        }
        void OnEditClick(RelativeLayout view)
        {
            if (ItemEditClick != null)
                ItemEditClick(this, view);
        }

    }
}