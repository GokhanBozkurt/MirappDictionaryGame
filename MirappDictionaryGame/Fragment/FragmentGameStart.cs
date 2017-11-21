using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using Exception = System.Exception;
using widget = Android.Support.Design.Widget;

namespace MirappDictionaryGame
{
    public class FragmentGameStart : Fragment
    {
        //private Spinner _dictionaryGameSpinner;
        private bool _fabStatus;
        private ImageView _gameStartImageView;
        private ImageView _dictonaryGameStartEasyImageView;
        private ImageView _dictonaryGameStartMediumImageView;
        private ImageView _dictonaryGameStartHardImageView;
        private widget.FloatingActionButton _gameStartFab;
        private double _metric;
        private TextView gameStartGameLevel;
        private TextView gameStartGameMaxScore;
        private Switch playWithFavorites;
        private ImageView playLanguage;
        private TextView playLanguageFrom;
        private TextView playLanguageTo;

        public string Language { get { return playLanguageFrom.Text == "English" ? "Türkçe":"English"; } }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();

            LoadViews();

            SetViewMetrics();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.GameStart, container, false);
            return view;
        }

        public override void OnPause()
        {
            base.OnPause();
            HideFab();
        }

        public override void OnResume()
        {
            base.OnResume();
            HideFab();
        }
        public override void OnStart()
        {
            base.OnStart();
            HideFab();
        }
        public override void OnStop()
        {
            base.OnStop();
            HideFab();
        }

        protected void FindViews()
        {
            //_dictionaryGameSpinner = View.FindViewById<Spinner>(Resource.Id.DictionaryGameSpinner);
            _gameStartImageView = View.FindViewById<ImageView>(Resource.Id.GameStartImageView);
            _dictonaryGameStartEasyImageView = View.FindViewById<ImageView>(Resource.Id.DictonaryGameStartEasyImageView);
            _dictonaryGameStartMediumImageView = View.FindViewById<ImageView>(Resource.Id.DictonaryGameStartMediumImageView);
            _dictonaryGameStartHardImageView = View.FindViewById<ImageView>(Resource.Id.DictonaryGameStartHardImageView);
            gameStartGameLevel = View.FindViewById<TextView>(Resource.Id.GameStartGameLevel);
            gameStartGameMaxScore = View.FindViewById<TextView>(Resource.Id.GameStartGameMaxScore);
            playWithFavorites = View.FindViewById<Switch>(Resource.Id.PlayWithFavorites);
            playLanguage = View.FindViewById<ImageView>(Resource.Id.PlayLanguage);
            _gameStartFab = View.FindViewById<widget.FloatingActionButton>(Resource.Id.GameStartAddFab);
            playLanguageFrom = View.FindViewById<TextView>(Resource.Id.PlayLanguageFrom);
            playLanguageTo = View.FindViewById<TextView>(Resource.Id.PlayLanguageTo);
            


        }
        public int ConvertToDp(float px)
        {

            var density = Resources.DisplayMetrics.Density;
            var dps = (int)((px / density) + 0.5f);
            return dps;
        }
        private void SetViewMetrics()
        {
            _gameStartImageView.LayoutParameters.Height = GetWidth(1.5);
            _gameStartImageView.LayoutParameters.Width = GetWidth(1.5);
            _metric = 1.5;
            _dictonaryGameStartEasyImageView.LayoutParameters.Height = GetWidth(_metric);
            _dictonaryGameStartEasyImageView.LayoutParameters.Width = GetWidth(_metric);

            _dictonaryGameStartMediumImageView.LayoutParameters.Height = GetWidth(_metric);
            _dictonaryGameStartMediumImageView.LayoutParameters.Width = GetWidth(_metric);

            _dictonaryGameStartHardImageView.LayoutParameters.Height = GetWidth(_metric);
            _dictonaryGameStartHardImageView.LayoutParameters.Width = GetWidth(_metric);

        }



        private float ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        private int GetWidth(double metric)
        {
            var metrics = Resources.DisplayMetrics;
            return Convert.ToInt32(ConvertPixelsToDp(metrics.WidthPixels) * metric);
        }
        protected void HandleEvents()
        {
            _dictonaryGameStartEasyImageView.Click += DictonaryGameStartEasyImageViewClick;
            _dictonaryGameStartMediumImageView.Click += DictonaryGameStartMediumImageViewClick;
            _dictonaryGameStartHardImageView.Click += DictonaryGameStartHardImageViewClick;
            _gameStartImageView.Click += GameStartImageViewClick;
            _gameStartFab.Click += GameStartFab_Click;
            playLanguage.Click += PlayLanguage_Click;

        }

        private void PlayLanguage_Click(object sender, EventArgs e)
        {
            if (playLanguageFrom.Text=="Türkish")
            {
                playLanguageFrom.Text = "English";
                playLanguageTo.Text = "Türkish";
            }
            else
            {
                playLanguageFrom.Text = "Türkish";
                playLanguageTo.Text = "English";
            }
        }

        private void GameStartFab_Click(object sender, EventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            var myFragmentContainer = new MyFragmentContainer
            {
                MyFragment = new FragmentAdd(),
                Name = Resource.String.Add
            };
            ft.Replace(Resource.Id.HomeFrameLayout, myFragmentContainer.MyFragment, "Add");
            //SupportActionBar.SetTitle(myFragmentContainer.Name);
            ft.Commit();
        }

        private void DictonaryGameStartHardImageViewClick(object sender, EventArgs e)
        {
            StartGame(GamePlayLevels.Hard, (View)sender);
        }

        private void DictonaryGameStartEasyImageViewClick(object sender, EventArgs e)
        {
            StartGame(GamePlayLevels.Easy, (View)sender);
        }

        private void DictonaryGameStartMediumImageViewClick(object sender, EventArgs e)
        {
            StartGame(GamePlayLevels.Medium, (View)sender);
        }


        private void GameStartImageViewClick(object sender, EventArgs e)
        {

            if (_fabStatus == false)
            {
                ExpandFab();
                _fabStatus = true;
            }
            else
            {
                HideFab();
            }
        }

        private void HideFab()
        {
            try
            {
                _dictonaryGameStartEasyImageView.Animate().TranslationY(0).SetDuration(1000);
                _dictonaryGameStartMediumImageView.Animate().TranslationY(0).TranslationX(0).SetDuration(1000);
                _dictonaryGameStartHardImageView.Animate().TranslationY(0).TranslationX(0).SetDuration(1000);
                _dictonaryGameStartEasyImageView.Clickable = false;
                _dictonaryGameStartMediumImageView.Clickable = false;
                _dictonaryGameStartHardImageView.Clickable = false;
                _fabStatus = false;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void ExpandFab()
        {
            //Sneack();
            _dictonaryGameStartEasyImageView.Animate().TranslationY(-600).SetDuration(1000);
            _dictonaryGameStartEasyImageView.Clickable = true;
            _dictonaryGameStartMediumImageView.Animate().TranslationY(600).TranslationX(500).SetDuration(1000);
            _dictonaryGameStartMediumImageView.Clickable = true;
            _dictonaryGameStartHardImageView.Animate().TranslationY(600).TranslationX(-500).SetDuration(1000);
            _dictonaryGameStartHardImageView.Clickable = true;
        }

        private void StartGame(GamePlayLevels gameLevels, View buton)
        {
            try
            {
                ManagerSound.Instance.Start(Activity);
                var intent = new Intent();
                intent.SetClass(Activity, typeof(GamePlayActivity));
                intent.PutExtra("GameLevel", gameLevels.ToString());
                //intent.PutExtra("Language", _dictionaryGameSpinner.SelectedItem.ToString());
                intent.PutExtra("Language", Language);
                intent.PutExtra("PlayWithFavorites", playWithFavorites.Checked);
                buton?.Animate()
                      .SetDuration(1000)
                      //.ScaleX(1)
                      //.ScaleY(1)
                      .Rotation(360)
                      .Alpha(0)
                      .SetInterpolator(new AccelerateInterpolator())
                      .WithEndAction(new Runnable(() =>
                                                        {
                                                            StartActivityForResult(intent, 100);
                                                            Activity.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
                                                        }
                      ))
                    ;

            }
            catch (Exception ex)
            {
                var toast = Toast.MakeText(this.Activity, ex.Message, ToastLength.Short);
                toast.Show();
            }
        }

        private void LoadViews()
        {
            //var adapter = ArrayAdapter.CreateFromResource(this.Activity, Resource.Array.Languages, Android.Resource.Layout.SimpleSpinnerItem);
            //adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //_dictionaryGameSpinner.Adapter = adapter;

            gameStartGameLevel.Text = $" Level " + ManagerGamePlay.GetCurrentLevel().LevelNumber.ToString();
            gameStartGameMaxScore.Text = $"Max Score {ManagerGameOver.Instance.MaxScore}";
        }

        private void Sneack()
        {
            Snackbar
              .Make(this.View, "Message sent", Snackbar.LengthLong)
              .SetAction("Undo", (view) => { /*Undo message sending here.*/ })
              .Show(); // Don’t forget to show!
        }
    }
}