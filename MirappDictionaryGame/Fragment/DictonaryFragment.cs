using System;
using System.Linq;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MirappDictionaryGame 
{
    public class DictonaryFragment : BaseFragment
    {
        private AutoCompleteTextView _wordText;
        private EditText _translatedWordText;
        private Repository<MyDictonaryWord> _repository;
        private Spinner _spinner;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();

            SetRepository();

            LoadViews();
            
        }

        private void SetRepository()
        {
            _repository = new Repository<MyDictonaryWord>();
            _repository.Open();
            _repository.CreateTable();
        }

        protected override void FindViews()
        {
            _translatedWordText = View.FindViewById<EditText>(Resource.Id.TranslatedWordText);
            _spinner = View.FindViewById<Spinner>(Resource.Id.spinner);
            _wordText = View.FindViewById<AutoCompleteTextView>(Resource.Id.WordTextAutoComplete);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Add, container, false);
            return view;
        }

        public override void OnStop()
        {
            _repository.Close();
            base.OnStop();
        }

        protected override void HandleEvents()
        {
            _wordText.ItemClick += WordTextAutoComplete_ItemClick;
        }

        private void WordTextAutoComplete_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var count = ManagerDictionary.WordList.Where(a => a.Word == _wordText.Text).Select(b => b.TranslatedWord).Count();
            if (count>0)
            {
                _translatedWordText.Text = ManagerDictionary.WordList.Where(a => a.Word == _wordText.Text).Select(b => b.TranslatedWord).First();
            }
        }

        private bool CheckForm()
        {
            if (_wordText.Text=="" || _translatedWordText.Text=="")
            {
                var toast = Toast.MakeText(this.Activity, "Please fill the form", ToastLength.Short);
                toast.Show();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            var toast = Toast.MakeText(this.Activity, "Word added", ToastLength.Short);
            toast.Show();

            _wordText.Text = "";
            _translatedWordText.Text = "";
            _wordText.RequestFocus();
        }

        private void LoadViews()
        {
            var adapter = ArrayAdapter.CreateFromResource(this.Activity, Resource.Array.Languages, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _spinner.Adapter = adapter;

            adapter = new ArrayAdapter<String>(this.Activity, Resource.Layout.WordListItem, ManagerDictionary.WordList.Select(a=>a.Word).ToList());
            _wordText.Adapter = adapter;
        }

    }
}