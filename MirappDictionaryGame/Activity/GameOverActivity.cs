using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System.Timers;
using Android.Graphics;
using Android.Views;
using Java.IO;
using System.IO;

namespace MirappDictionaryGame
{
    [Activity(Label = "Game Over", NoHistory = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class GameOverActivity : Activity
    {
        private Button _dictionaryGameOverResultSuccessPercentage;
        private TextView gameOverGameLevel;
        private TextView maxScore;

        private Button _dictionaryGameStartAgainButton;
        private Button _dictionaryMainButton;

        private ImageView myAnimation;
        private Android.Graphics.Drawables.AnimationDrawable myAnimationDrawable;

        long TotalScore = 0;

        public string Language { get; set; }

        public GamePlayLevelContainer gamePlayLevelContainer { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.GameOver);

            FindViews();

            HandleEvents();

            ReadIntentExtras();
        }
        long _countSeconds;
        long countTick;
        private Timer _timer;
        private bool LevelUp;
        private bool playWithFavorites = false;
        private Button shareScore;

        private void ReadIntentExtras()
        {
            gamePlayLevelContainer = ManagerGamePlay.GetGameLevelContainer(Intent.Extras.GetString("GameLevel"));
            Language = Intent.Extras.GetString("Language");
            LevelUp = Intent.Extras.GetBoolean("LevelUp");
            TotalScore = Intent.Extras.GetLong("TotalScore");
            playWithFavorites = Intent.Extras.GetBoolean("PlayWithFavorites");

            if (playWithFavorites)
            {
                gameOverGameLevel.Text = "Favorites";
                maxScore.Text = "Score : " + TotalScore;
            }
            else
            {
                maxScore.Text = "Max Score : " + ManagerGameOver.Instance.MaxScore.ToString();
                if (LevelUp)
                {
                    _dictionaryGameStartAgainButton.Text = $"Go To New Level ";
                }
                if (TotalScore == 0)
                {
                    _dictionaryGameOverResultSuccessPercentage.Text = $"Score {TotalScore}";
                }
                else
                {
                    if (!playWithFavorites)
                    {
                        ManagerGameOver.Instance.InsertScoreToDb(TotalScore);
                    }                  
                }
                gameOverGameLevel.Text = ManagerGamePlay.GetCurrentLevel().ToString();
            }

            _timer = new Timer();
            _countSeconds = 0;
            if (TotalScore < 10)
            {
                countTick = 5;
            }
            else
            {
                countTick = TotalScore / 100;

            }
            _timer.Enabled = true;
            _timer.Interval = 10;
            _timer.Elapsed += OnTimeEvent;
        }

        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
            _countSeconds += countTick;

            RunOnUiThread(() =>
            {
                if (_countSeconds > TotalScore)
                {
                    _dictionaryGameOverResultSuccessPercentage.Text = $" Score { TotalScore}";
                }
                else
                {
                    _dictionaryGameOverResultSuccessPercentage.Text = $" Score { _countSeconds}";
                }
            });

            if (_countSeconds > TotalScore)
            {
                _timer.Stop();
                RunOnUiThread(() =>
                {
                    //_dictionaryGameOverResultSuccessPercentage.Text = $" Score { TotalScore}";
                    string level = "";
                    if (LevelUp && !playWithFavorites)
                    {
                        level = "Level Up  !!!";
                    }
                    var score = "Score";
                    if (TotalScore == ManagerGameOver.Instance.MaxScore && ManagerGameOver.Instance.MaxScore > 0 && !playWithFavorites)
                    {
                        score = "NEW Score  !!! ";
                    }
                    _dictionaryGameOverResultSuccessPercentage.Text = $"{score} {TotalScore} { level } ";
                    shareScore.Visibility = ViewStates.Visible;
                });

            }
        }

        private void HandleEvents()
        {
            _dictionaryGameStartAgainButton.Click += DictionaryGameStartAgainButton_Click;
            _dictionaryMainButton.Click += DictionaryMainButton_Click;
            shareScore.Click += shareScore_Click;
        }

        private void DictionaryMainButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(AppStartActivity));
            StartActivityForResult(intent, 100);
            OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }
        private void shareScore_Click(object sender, EventArgs e)
        {
            try
            {
                var count = ManagerRepository.Instance.FavoriteWord.GetRecords().Count;
                var shr = new Intent(Intent.ActionSend);
                shr.PutExtra(Intent.ExtraSubject, "Score");
                Android.Net.Uri uri = Android.Net.Uri.FromFile(takeScreenShot2());
                shr.SetDataAndType(uri, "image/*");
                shr.PutExtra(Intent.ExtraText, $"My Mira App Dict Game Score is {TotalScore} ");
                shr.PutExtra(Intent.ExtraStream, uri);
                StartActivity(shr);
            }
            catch (Exception ex)
            {

                ManagerAlert.ShowToast(this, ex.Message, ToastLength.Long);
            }
        }


        public Java.IO.File takeScreenShot2()
        {
            //
            string path = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "Mirapp");
            View v1 = Window.DecorView.RootView;
            v1.DrawingCacheEnabled = true;
            Bitmap bitmap = Bitmap.CreateBitmap(v1.GetDrawingCache(true));
            Java.IO.File imageFile = new Java.IO.File(path, System.Environment.TickCount + ".jpg");
            MemoryStream bytes = new MemoryStream();
            int quality = 100;
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
                dir.CreateSubdirectory(path);

            FileOutputStream fo = new Java.IO.FileOutputStream(imageFile);
            imageFile.CreateNewFile();

            bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, quality, bytes);
            fo.Write(bytes.ToArray());
            fo.Close();

            return imageFile;

            // a litle work for you :)
            // openScreenshot(imageFile);
        }


        private void DictionaryGameStartAgainButton_Click(object sender, EventArgs e)
        {
            ManagerSound.Instance.Start(this);
            var intent = new Intent();
            intent.SetClass(this, typeof(GamePlayActivity));
            intent.PutExtra("GameLevel", gamePlayLevelContainer.GameLevel.ToString());
            intent.PutExtra("Language", Language);
            intent.PutExtra("PlayWithFavorites", playWithFavorites);

            StartActivityForResult(intent, 100);
            OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }

        private void FindViews()
        {
            _dictionaryGameOverResultSuccessPercentage = FindViewById<Button>(Resource.Id.DictionaryGameOverResultSuccessPercentage);
            maxScore = FindViewById<TextView>(Resource.Id.DictionaryGameOverMaxScore);
            //_dictionaryGameOverResultTotalSecond = FindViewById<TextView>(Resource.Id.DictionaryGameOverResultTotalSecond);
            _dictionaryGameStartAgainButton = FindViewById<Button>(Resource.Id.DictionaryGameStartAgainButton);
            _dictionaryMainButton = FindViewById<Button>(Resource.Id.DictionaryMainButton);
            gameOverGameLevel = FindViewById<TextView>(Resource.Id.GameOverGameLevel);
            myAnimation = (ImageView)FindViewById(Resource.Id.GameOverNewLevelAnimation);
            myAnimationDrawable = (Android.Graphics.Drawables.AnimationDrawable)myAnimation.Drawable;
            shareScore = FindViewById<Button>(Resource.Id.ShareScore);
            shareScore.Visibility = ViewStates.Invisible;

        }
    }
}