using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;


namespace MirappDictionaryGame
{
    public class FragmentScores : Fragment 
    {
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            var intent = new Intent();
            intent.SetClass(Activity, typeof(MyListActivity));
            StartActivityForResult(intent, 100);

            return;
#pragma warning disable CS0162 // Unreachable code detected
            LoadList();
#pragma warning restore CS0162 // Unreachable code detected
            var DeleteScores = View.FindViewById<Button>(Resource.Id.DeleteScores);
            DeleteScores.Click += DeleteScores_Click;
        }

        private void DeleteScores_Click(object sender, EventArgs e)
        {
            ManagerRepository.Instance.GameScore.DeleteAll();
            LoadList();
        }

        private void LoadList()
        {
            var list = View.FindViewById<ListView>(Resource.Id.ScoresList);
            var scoreList = ManagerRepository.Instance.GameScore.GetRecords();
            var scoresListAdapter = new ScoresListAdapter(Activity, scoreList);
            list.Adapter = scoresListAdapter;

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Scores, container, false);
            return view;
        }
 
    }

    class ScoresListAdapter : BaseAdapter<GameScore>
    {
        private readonly IList<GameScore> _items;
        private readonly Activity _context;

        public ScoresListAdapter(Activity context, IList<GameScore> items)
        {
            this._context = context;
            this._items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override GameScore this[int position] => _items[position];

        public override int Count => _items.Count;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.ScoreRowItem, null);
            view.FindViewById<TextView>(Resource.Id.ScoreRowItem).Text = _items[position].Score.ToString();
            view.FindViewById<TextView>(Resource.Id.ScoreDateRowItem).Text = _items[position].ScoreDate.ToString();

            return view;
        }

        public void Remove(long id)
        {
        }
    }
}