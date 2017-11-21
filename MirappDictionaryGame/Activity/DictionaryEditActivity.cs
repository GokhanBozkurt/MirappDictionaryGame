using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;


namespace MirappDictionaryGame
{
    [Activity(Label = "Edit", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class DictionaryEditActivity : Activity
    {
        private EditText _dictonaryEditWordText;
        private TextView _dictonaryEditLangugage;
        private Button _dictonaryEditUpdateButton;
        private Button _dictonaryEditCancelButton;
        private EditText _translatedWordText;
        private MyDictonaryWord _item = new MyDictonaryWord();
        private RepositoryMyDictonaryWord<MyDictonaryWord> _repository;
        private Button _dictonaryEditDeleteButton;
        private bool favorites;
        private FavoriteWord favoriteItem;
        private EditText snonymWord;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DictonaryEdit);

            FindViews();

            HandleEvents();

            SetRepository();

            SetWord();
        }

        private void SetWord()
        {
            if (Intent.Extras.GetInt("wordId") != 0)
            {
                 favorites = Intent.Extras.GetBoolean("favorites");
                
                if (favorites)
                {
                    favoriteItem = new FavoriteWord() { Id = Intent.Extras.GetInt("wordId") };
                    favoriteItem = ManagerRepository.Instance.FavoriteWord.GetRecord(favoriteItem);
                    _dictonaryEditWordText.Text = favoriteItem.Word;
                    _translatedWordText.Text = favoriteItem.TranslatedWord;
                    _dictonaryEditLangugage.Text = favoriteItem.Language;
                    snonymWord.Text = favoriteItem.SnonymWord;
                }
                else
                {
                    _item = new MyDictonaryWord() { Id = Intent.Extras.GetInt("wordId") };
                    _item = _repository.GetRecord(_item);
                    _dictonaryEditWordText.Text = _item.Word;
                    _translatedWordText.Text = _item.TranslatedWord;
                    _dictonaryEditLangugage.Text = _item.Language;
                    snonymWord.Text = _item.SnonymWord;
                }


            }
        }

        private void SetRepository()
        {
            _repository = new RepositoryMyDictonaryWord<MyDictonaryWord>();
            _repository.Open();
            _repository.CreateTable();
        }


        protected void FindViews()
        {
            _dictonaryEditWordText = FindViewById<EditText>(Resource.Id.DictonaryEditWordText);
            _translatedWordText = FindViewById<EditText>(Resource.Id.DictonaryEditTranslatedWordText);
            snonymWord = FindViewById<EditText>(Resource.Id.DictonaryEditSnonymWord);
            _dictonaryEditLangugage = FindViewById<TextView>(Resource.Id.DictonaryEditLangugage);
            _dictonaryEditUpdateButton = FindViewById<Button>(Resource.Id.DictonaryEditUpdateButton);
            _dictonaryEditDeleteButton = FindViewById<Button>(Resource.Id.DictonaryEditDeleteButton);
            _dictonaryEditCancelButton = FindViewById<Button>(Resource.Id.DictonaryEditCancelButton);
            
        }

        protected void HandleEvents()
        {
            _dictonaryEditUpdateButton.Click += DictonaryEditUpdateButton_Click;
            _dictonaryEditDeleteButton.Click += DictonaryEditDeleteButton_Click;
            _dictonaryEditCancelButton.Click += DictonaryEditCancelButton_Click;
        }

        private void DictonaryEditCancelButton_Click(object sender, EventArgs e)
        {
            LoadMain();
        }

        private void DictonaryEditDeleteButton_Click(object sender, EventArgs e)
        {
            if (favorites)
            {
                if (_repository.Delete(_item))
                {
                    ManagerDictionary.DictonaryUpdated();
                    LoadMain();
                }
                else
                {
                    ShowErrorMessage();
                }
            }
            else
            {
                if (ManagerRepository.Instance.FavoriteWord.Delete(favoriteItem))
                {
                    ManagerDictionary.DictonaryUpdated();
                    LoadMain();
                }
            }
            
        }

        private void ShowErrorMessage()
        {
            var toast1 = Toast.MakeText(this, String.Format("Hata :{0}  ", _repository.RepositoryException.Message), ToastLength.Short);
            toast1.Show();
        }

        private void LoadMain()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(AppStartActivity));
            intent.PutExtra("goToList", true);
            StartActivityForResult(intent, 100);
        }

        private void DictonaryEditUpdateButton_Click(object sender, EventArgs e)
        {
            if (favorites)
            {
                favoriteItem.Word = _dictonaryEditWordText.Text;
                favoriteItem.TranslatedWord = _translatedWordText.Text;
                favoriteItem.SnonymWord = snonymWord.Text;
                if (ManagerRepository.Instance.FavoriteWord.Update(favoriteItem))
                {
                    LoadMain();
                }
                else
                {
                    ShowErrorMessage();
                }
            }
            else
            {
                _item.Word = _dictonaryEditWordText.Text;
                _item.TranslatedWord = _translatedWordText.Text;
                _item.SnonymWord = snonymWord.Text;
                if (_repository.Update(_item))
                {
                    ManagerDictionary.DictonaryUpdated();
                    LoadMain();
                }
                else
                {
                    ShowErrorMessage();
                }
            }
            
        }
    }
}