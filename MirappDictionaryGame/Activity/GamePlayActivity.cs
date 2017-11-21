using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;
using Java.Lang;
using Exception = System.Exception;
using String = System.String;
using System.Linq;

namespace MirappDictionaryGame
{
    [Activity(
        Label = "Game",
        Icon = "@drawable/icon",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class GamePlayActivity : Activity
    {
        #region Properties
        private Repository<MyDictonaryWord> _repository;
        private readonly DictonaryWordContainer _wordContainer = new DictonaryWordContainer();

        private Button _dictionaryGameFromButton;
        private Button _dictionaryGameToButton1;
        private Button _dictionaryGameToButton2;
        private Button _dictionaryGameToButton3;
        private Button _dictionaryGameToButton4;
        private Button _dictionaryGameToButton5;
        private Button _dictionaryGameToButton6;
        private Button dictionaryGametoMain;
        private Button addToFavorite;


        private Button DictionaryGameSnonymWordButton;

        private Button _dictionaryGameResult;
        private ImageView _dictionaryGameLife1;
        private ImageView _dictionaryGameLife2;
        private ImageView _dictionaryGameLife3;
        private ImageView _dictionaryGameLife4;
        private ImageView _dictionaryGameLife5;
        private ImageView _dictionaryGameLife6;
        private ImageView _dictionaryGameLife7;
        private TextView gameStartGameLevel;
        private TextView gamePlayGameWordCount;
        private TextView gamePlayGameScore;

        private ImageView myAnimation;
        private Android.Graphics.Drawables.AnimationDrawable myAnimationDrawable;
        private MyGameTimer _myGameTimer;
        private List<int> _listButton;
        private List<MyDictonaryWord> _listDictonaryWords;
        private readonly List<MyDictonaryWord> _listWrongyWords = new List<MyDictonaryWord>();
        public GamePlayLevelContainer gamePlayLevelContainer;
        private GameProperty _gameProperty;
        private GameLevel currentGameLevel;

        private readonly Random _random = new Random();
        private int _randomNumber;
        private int _trueRandomNumber;
        public string Language;
        private bool isGameSucessfullyFinished => _listDictonaryWords.Count == 0;
        long TotalScore = 0;
        private bool NewLevel;
        public bool playWithFavorites = false;
        #endregion

        #region Overrides

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.GamePlay);

            FindViews();

            HandleEvents();

            try
            {
                ActivityStartUp();

                GameStartup();
            }
            catch (Exception ex)
            {
                ManagerAlert.ShowAlert(this, String.Format("Hata :{0}  StackTrace:{1}", ex.Message, ex.StackTrace));
                //var toast1 = Toast.MakeText(this, String.Format("Hata :{0}  StackTrace:{1}", ex.Message,ex.StackTrace), ToastLength.Long);
                //toast1.Show();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            _myGameTimer.Start();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _myGameTimer.Reset();
        }

        protected override void OnStop()
        {
            base.OnStop();
            _myGameTimer.Reset();
        }


        #endregion
        private void ActivityStartUp()
        {
            ReadIntentExtras();
            currentGameLevel = ManagerGamePlay.GetCurrentLevel();
            SetGameLevel();
            SetRepository();
            ManagerGameOver.Instance.Reset();
            _listDictonaryWords = null;
            SetGameLife();
            SetTimer();
            ManagerGameOver.Instance.ClearWordContainer();
            addToFavorite.Alpha = 0;
        }

        private void SetGameLevel()
        {
            if (playWithFavorites)
            {
                gameStartGameLevel.Text = "Favorites";
            }
            else
            {
                gameStartGameLevel.Text = currentGameLevel.ToString();
            }
        }

        private void GameStartup()
        {
            try
            {
                InvisibleForm();
                StartGame();
                gamePlayGameWordCount.Text = $"" + _listDictonaryWords.Count + " Word ";
                _dictionaryGameResult.Alpha = 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //myAnimation.Visibility = ViewStates.Invisible;
        }

        private void ReadIntentExtras()
        {
            gamePlayLevelContainer = ManagerGamePlay.GetGameLevelContainer(Intent.Extras.GetString("GameLevel"));
            playWithFavorites = Intent.Extras.GetBoolean("PlayWithFavorites");
            Language = Intent.Extras.GetString("Language");
            _gameProperty = ManagerGamePlay.Get(gamePlayLevelContainer.GameLevel);
            ManagerGameOver.Instance.GameProperty = _gameProperty;
            ManagerGameOver.Instance.GamePlayLevel = gamePlayLevelContainer.GameLevel;
        }

        private void SetGameLife()
        {
            _dictionaryGameLife1.Visibility = ViewStates.Visible;
            _dictionaryGameLife2.Visibility = ViewStates.Visible;
            _dictionaryGameLife3.Visibility = ViewStates.Visible;
            _dictionaryGameLife4.Visibility = ViewStates.Visible;
            _dictionaryGameLife5.Visibility = ViewStates.Visible;
            _dictionaryGameLife6.Visibility = ViewStates.Visible;
            _dictionaryGameLife7.Visibility = ViewStates.Visible;

            switch (gamePlayLevelContainer.GameLevel)
            {
                case GamePlayLevels.Medium:
                    _dictionaryGameLife7.Visibility = ViewStates.Invisible;
                    break;
                case GamePlayLevels.Hard:
                    _dictionaryGameLife6.Visibility = ViewStates.Invisible;
                    _dictionaryGameLife7.Visibility = ViewStates.Invisible;
                    break;
            }
        }

        private void SetRepository()
        {
            _repository = new Repository<MyDictonaryWord>();
            _repository.Open();
            _repository.CreateTable();
        }

        private void FindViews()
        {
            _dictionaryGameFromButton = FindViewById<Button>(Resource.Id.DictionaryGameFROMButton);
            _dictionaryGameToButton1 = FindViewById<Button>(Resource.Id.DictionaryGameToButton1);
            _dictionaryGameToButton2 = FindViewById<Button>(Resource.Id.DictionaryGameToButton2);
            _dictionaryGameToButton3 = FindViewById<Button>(Resource.Id.DictionaryGameToButton3);
            _dictionaryGameToButton4 = FindViewById<Button>(Resource.Id.DictionaryGameToButton4);
            _dictionaryGameToButton5 = FindViewById<Button>(Resource.Id.DictionaryGameToButton5);
            _dictionaryGameToButton6 = FindViewById<Button>(Resource.Id.DictionaryGameToButton6);
            dictionaryGametoMain = FindViewById<Button>(Resource.Id.DictionaryGametoMain);
            _dictionaryGameLife1 = FindViewById<ImageView>(Resource.Id.DictionaryGameLife1);
            _dictionaryGameLife2 = FindViewById<ImageView>(Resource.Id.DictionaryGameLife2);
            _dictionaryGameLife3 = FindViewById<ImageView>(Resource.Id.DictionaryGameLife3);
            _dictionaryGameLife4 = FindViewById<ImageView>(Resource.Id.DictionaryGameLife4);
            _dictionaryGameLife5 = FindViewById<ImageView>(Resource.Id.DictionaryGameLife5);
            _dictionaryGameLife6 = FindViewById<ImageView>(Resource.Id.DictionaryGameLife6);
            _dictionaryGameLife7 = FindViewById<ImageView>(Resource.Id.DictionaryGameLife7);
            _dictionaryGameResult = FindViewById<Button>(Resource.Id.DictionaryGameResult);
            gameStartGameLevel = FindViewById<TextView>(Resource.Id.GamePlayGameLevel);
            gamePlayGameWordCount = FindViewById<TextView>(Resource.Id.GamePlayGameWordCount);
            gamePlayGameScore = FindViewById<TextView>(Resource.Id.GamePlayGameScore);
            myAnimation = (ImageView)FindViewById(Resource.Id.GameNewLevelAnimation);
            myAnimationDrawable = (Android.Graphics.Drawables.AnimationDrawable)myAnimation.Drawable;
            DictionaryGameSnonymWordButton = FindViewById<Button>(Resource.Id.DictionaryGameSnonymWordButton);
            addToFavorite = FindViewById<Button>(Resource.Id.AddToFavorite);

        }

        private void HandleEvents()
        {
            _dictionaryGameFromButton.LongClick += _dictionaryGameFromButton_LongClick;
            _dictionaryGameToButton1.Click += DictionaryGameToButton1_Click;
            _dictionaryGameToButton2.Click += DictionaryGameToButton2_Click;
            _dictionaryGameToButton3.Click += DictionaryGameToButton3_Click;
            _dictionaryGameToButton4.Click += DictionaryGameToButton4_Click;
            _dictionaryGameToButton5.Click += DictionaryGameToButton5_Click;
            _dictionaryGameToButton6.Click += DictionaryGameToButton6_Click;
            dictionaryGametoMain.Click += dictionaryGametoMain_Click;
            addToFavorite.Click += AddToFavorite_Click;
        }

        private void _dictionaryGameFromButton_LongClick(object sender, View.LongClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(_wordContainer.CurrentWord.SnonymWord))
            {
                DictionaryGameSnonymWordButton.Visibility = ViewStates.Visible;
                DictionaryGameSnonymWordButton.Alpha = 1;
                DictionaryGameSnonymWordButton.Text = _wordContainer.CurrentWord.SnonymWord;
                DictionaryGameSnonymWordButton.Animate().SetDuration(4000).Alpha(0);
            }               
        }

        private void AddToFavorite_Click(object sender, EventArgs e)
        {
            try
            {
                var FavoriteWord = new FavoriteWord
                {
                    Id= _wordContainer.LastWord.Id,
                    Language = _wordContainer.LastWord.Language,
                    TranslatedWord = _wordContainer.LastWord.TranslatedWord,
                    Word = _wordContainer.LastWord.Word
                };

                if (playWithFavorites)
                {
                    var deleted = ManagerRepository.Instance.FavoriteWord.Delete(FavoriteWord);
                    if (deleted)
                    {
                        addToFavorite.Text = "Removed From Favorites";
                        addToFavorite.Animate()
                        .SetDuration(1000)
                        .Alpha(0);
                    }
                    else
                    {
                        ManagerAlert.ShowToast(this, $"{FavoriteWord.ToString()} Can not remove from favorites", ToastLength.Long);
                    }
                }
                else
                {
                   

                    //ManagerAlert.ShowAlert(this, FavoriteWord.ToString());
                    if (ManagerRepository.Instance.FavoriteWord.Add(FavoriteWord))
                    {
                        addToFavorite.Text = "ADDED";

                        addToFavorite.Animate()
                        .SetDuration(1000)
                        .Alpha(0);
                    }
                }
            }
            catch (Exception ex)
            {
                ManagerAlert.ShowToast(this, ex.Message, ToastLength.Long);

            }
        }

        #region Event Methods
        private void MyTimer_OnTickEvent(Activity myActivity)
        {

        }

        private void dictionaryGametoMain_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(AppStartActivity));
            StartActivityForResult(intent, 100);
            OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }

        private void DictionaryGameToButton6_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }

        private void DictionaryGameToButton5_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }

        private void DictionaryGameToButton4_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }

        private void DictionaryGameToButton3_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }

        private void DictionaryGameToButton2_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }

        private void DictionaryGameToButton1_Click(object sender, EventArgs e)
        {
            CheckWord((Button)(sender));
        }

        #endregion

        private void SetTimer()
        {
            ManagerGameOver.Instance.MillisInFuture = _gameProperty.MillisInFuture;
            _myGameTimer = new MyGameTimer(_gameProperty.MillisInFuture, 1000);
            _myGameTimer.Set(this, "", Resource.Id.DictionaryGameTimeButton, gamePlayLevelContainer, Language,playWithFavorites);
            _myGameTimer.OnTickEvent += MyTimer_OnTickEvent;
            _myGameTimer.Reset();
            _myGameTimer.Start();
        }

        public virtual List<MyDictonaryWord> LoadDictonary(MyDictonaryWord dictonaryWords)
        {
            if (playWithFavorites)
            {
                return ManagerDictionary.GetGameWordList(dictonaryWords, true);
            }
            else
            {
                return ManagerDictionary.GetGameWordList(dictonaryWords, false);
            }
        }

        private void StartGame()
        {

            try
            {
                var dictonaryWords = new MyDictonaryWord()
                {
                    Language = Language
                };

                if (_listDictonaryWords == null)
                {
                    _listDictonaryWords = LoadDictonary(dictonaryWords);
                    ManagerGameOver.Instance.ElapsedStropWatch.Start();
                }
                else if (NewLevel)
                {
                    NewLevel = false;
                    _listDictonaryWords.AddRange(ManagerDictionary.GetNewLevelGameWordList(dictonaryWords));

                }
                VisibleClearForm(gamePlayLevelContainer.GameLevel);

                var randomWords = ManagerDictionary.PrepareWordList(dictonaryWords);

                switch (gamePlayLevelContainer.GameLevel)
                {
                    case GamePlayLevels.Easy:
                        _listButton = GenerateRandom(4);
                        break;
                    case GamePlayLevels.Medium:
                        _listButton = GenerateRandom(5);
                        break;
                    case GamePlayLevels.Hard:
                        _listButton = GenerateRandom(6);
                        break;
                }

                SetGuessWordButtons(randomWords);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetGuessWordButtons(List<MyDictonaryWord> randomWords)
        {
            _trueRandomNumber = _random.Next(_listDictonaryWords.Count);
            _wordContainer.CurrentWord = _listDictonaryWords[_trueRandomNumber];
            _dictionaryGameFromButton.Text = _wordContainer.CurrentWord.Word;

            Button randomButton = GetRandomButton(_listButton[0]);
            randomButton.Text = _wordContainer.CurrentWord.TranslatedWord;
            var trueWordindx = GetTrueWordindxIndx(randomWords);
            randomWords.RemoveAt(trueWordindx);

            _randomNumber = _random.Next(randomWords.Count);
            randomButton = GetRandomButton(_listButton[1]);
            randomButton.Text = randomWords[_randomNumber].TranslatedWord;
            randomWords.RemoveAt(_randomNumber);

            _randomNumber = _random.Next(randomWords.Count);
            randomButton = GetRandomButton(_listButton[2]);
            randomButton.Text = randomWords[_randomNumber].TranslatedWord;
            randomWords.RemoveAt(_randomNumber);

            _randomNumber = _random.Next(randomWords.Count);
            randomButton = GetRandomButton(_listButton[3]);
            randomButton.Text = randomWords[_randomNumber].TranslatedWord;
            randomWords.RemoveAt(_randomNumber);

            if ((gamePlayLevelContainer.GameLevel == GamePlayLevels.Medium) || (gamePlayLevelContainer.GameLevel == GamePlayLevels.Hard))
            {
                _randomNumber = _random.Next(randomWords.Count);
                randomButton = GetRandomButton(_listButton[4]);
                randomButton.Text = randomWords[_randomNumber].TranslatedWord;
                randomWords.RemoveAt(_randomNumber);
            }

            if (gamePlayLevelContainer.GameLevel == GamePlayLevels.Hard)
            {
                _randomNumber = _random.Next(randomWords.Count);
                randomButton = GetRandomButton(_listButton[5]);
                randomButton.Text = randomWords[_randomNumber].TranslatedWord;
                randomWords.RemoveAt(_randomNumber);
            }
        }

        private void CheckWord(Button button)
        {
            ManagerGameOver.Instance.TryCount++;
            _dictionaryGameResult.Alpha = 0;
            //right word
            if (button.Text == _wordContainer.CurrentWord.TranslatedWord)
            {
                SetDictionaryGameResult(true, $"{_wordContainer.CurrentWord.Word} is {_wordContainer.CurrentWord.TranslatedWord}", 1500);
                addToFavorite.Alpha = 1;
                SuccesAction();
                if (isGameSucessfullyFinished)
                {
                    GameOverAction();
                }
                else
                {
                    StartGame();
                }
            }
            else
            {
                addToFavorite.Alpha = 0;
                ManagerSound.Instance.Wrong(this);
                button.Animate()
                    .SetDuration(500)
                    .Alpha(0);
                CheckTryCount();

                AddWrongWord(_wordContainer.CurrentWord);
                MyDictonaryWord wrongWord = ManagerDictionary.GetWord(button.Text);
                if (_gameProperty.TryCount > 0)
                {
                    if (wrongWord!=null)
                    {
                        SetDictionaryGameResult(false, $" {button.Text} is {wrongWord.Word}", 2500);
                    }
                }
            }
            SetGameScore();
            //dictionaryGameDummyButton.Visibility = ViewStates.Visible;
            //dictionaryGameDummyButton.Text = ManagerGameOver.Instance.ToString();
        }

        private void SetGameScore()
        {
            //if (playWithFavorites)
            //{
            //    gamePlayGameScore.Text = "";
            //}
            //else
            {
                gamePlayGameScore.Text = $"Score { ManagerGameOver.Instance.CalculateTotalScore}";
            }
        }

        //Write to bottom wright/wrong word
        private void SetDictionaryGameResult(bool success, string message, long duration)
        {
            if (success)
            {
                _dictionaryGameResult.Visibility = ViewStates.Visible;
                _dictionaryGameResult.Text = message;
                _dictionaryGameResult.SetBackgroundColor(ManagerColor.Green);
                //_dictionaryGameResult.SetTextColor(Color.ParseColor("#FFECB3"));

                _dictionaryGameResult.Animate()
                    .SetDuration(duration)
                    .Alpha(1)
                    .WithEndAction(new Runnable(() =>
                    {
                        _dictionaryGameResult.Alpha = 1;
                    }
                    )
                )
                ;
            }
            else
            {
                _dictionaryGameResult.Visibility = ViewStates.Visible;
                _dictionaryGameResult.Text = message;
                _dictionaryGameResult.SetBackgroundColor(ManagerColor.Red);
                _dictionaryGameResult.SetTextColor(ManagerColor.White);
                _dictionaryGameResult.Animate()
                          .SetDuration(duration)
                          .Alpha(1)
                          .WithEndAction(new Runnable(() =>
                          {
                              _dictionaryGameResult.Alpha = 0;
                              if (!String.IsNullOrEmpty(_wordContainer.LastWord.Word))
                              {
                                  SetDictionaryGameResult(true, $"{_wordContainer.LastWord.Word} = {_wordContainer.LastWord.TranslatedWord}", 1000);
                              }
                              else
                              {
                                  _dictionaryGameResult.Text = "";
                              }

                          }));
            }

        }

        private void CheckTryCount()
        {
            //_dictionaryGameTryCount.Text = $"{_gameProperty.TryCount}";
            var gameLifeButton = GameLifeButton;
            _gameProperty.TryCount--;
            if (_gameProperty.TryCount == 0)
            {
                _myGameTimer.Reset();
                GameOverAction();
                //gameLifeButton?.Animate()
                //    .SetDuration(1500)
                //    .ScaleX(0)
                //    .ScaleY(0)
                //     .Alpha(0)
                //    .SetInterpolator(new AccelerateInterpolator(1))
                //    .WithEndAction(new Runnable(GameOverAction)
                //    );
            }
            else
            {
                gameLifeButton?.Animate()
                   .SetDuration(1500)
                   .ScaleX(0)
                   .ScaleY(0)
                   .Alpha(0)
                   .SetInterpolator(new AccelerateInterpolator());

                //gameLifeButton.StartAnimation(ManagerAnimation.Scale1To0Animation(this));
            }
        }

        private ImageView GameLifeButton
        {
            get
            {
                switch (_gameProperty.TryCount)
                {
                    case 1:
                        return _dictionaryGameLife1;
                    case 2:
                        return _dictionaryGameLife2;
                    case 3:
                        return _dictionaryGameLife3;
                    case 4:
                        return _dictionaryGameLife4;
                    case 5:
                        return _dictionaryGameLife5;
                    case 6:
                        return _dictionaryGameLife6;
                    case 7:
                        return _dictionaryGameLife7;
                    default:
                        return null;
                }
            }
        }

        private void SuccesAction()
        {
            ManagerGameOver.Instance.SuccessCount++;
            _wordContainer.PassedMillis = _myGameTimer.PassedMillis;
            ManagerGameOver.Instance.AddWordContainer(_wordContainer);
            _listDictonaryWords.RemoveAt(_trueRandomNumber);
            ManagerSound.Instance.Sucess(this);
            gamePlayGameWordCount.Text = $"" + _listDictonaryWords.Count + " Word ";
            InvisibleForm();
            _myGameTimer.Reset();
            _myGameTimer.Start();
        }

        private void GameOverAction()
        {
            //ManagerGameOver.Instance.SuccessCount++;
            //_wordContainer.PassedMillis = _myGameTimer.PassedMillis;
            //ManagerGameOver.Instance.AddWordContainer(_wordContainer);
            ManagerGameOver.Instance.GamePlayLevel = gamePlayLevelContainer.GameLevel;
            TotalScore = ManagerGameOver.Instance.CalculateTotalScore;
            gamePlayGameScore.Text = $"Score {TotalScore}";

            if (isGameSucessfullyFinished)
            {
                ActionUpdateLevel();
                NewLevelAction();
            }
            else
            {
                _myGameTimer.Reset();
                ManagerSound.Instance.GameOver(this);
                var intent = new Intent();
                intent.SetClass(this, typeof(GameOverActivity));
                intent.PutExtra("GameLevel", gamePlayLevelContainer.GameLevel.ToString());
                intent.PutExtra("TotalScore", TotalScore);
                intent.PutExtra("Language", Language);
                intent.PutExtra("PlayWithFavorites", playWithFavorites);
                StartActivityForResult(intent, 100);
                OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
            }
        }

        private void ActionUpdateLevel()
        {
            if (!playWithFavorites)
            {
                ManagerGamePlay.UpdateLevel(currentGameLevel);
            }
        }

        private void NewLevelAction()
        {
            _myGameTimer.Reset();
            ManagerSound.Instance.NewLevel(this);
            var intent = new Intent();
            intent.SetClass(this, typeof(GameOverActivity));
            intent.PutExtra("GameLevel", gamePlayLevelContainer.GameLevel.ToString());
            intent.PutExtra("TotalScore", TotalScore);
            intent.PutExtra("LevelUp", true);
            intent.PutExtra("Language", Language);
            intent.PutExtra("PlayWithFavorites", playWithFavorites);
            StartActivityForResult(intent, 100);
            OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
            return;
#pragma warning disable CS0162 // Unreachable code detected
            gameStartGameLevel.Text = ManagerGamePlay.GetCurrentLevel().ToString();
#pragma warning restore CS0162 // Unreachable code detected
            myAnimation.Visibility = ViewStates.Visible;

            myAnimation.Post(
                new Runnable(() =>
                {
                    myAnimationDrawable.Start();
                    NewLevel = true;
                    GameStartup();
                })
            );

        }

        private void AddWrongWord(MyDictonaryWord translatedWord)
        {
            if (!_listWrongyWords.Contains(translatedWord))
            {
                _listWrongyWords.Add(translatedWord);
            }
        }

        private void InvisibleForm()
        {
            _dictionaryGameFromButton.Visibility = ViewStates.Invisible;
            _dictionaryGameToButton1.Visibility = ViewStates.Invisible;
            _dictionaryGameToButton2.Visibility = ViewStates.Invisible;
            _dictionaryGameToButton3.Visibility = ViewStates.Invisible;
            _dictionaryGameToButton4.Visibility = ViewStates.Invisible;
            _dictionaryGameToButton5.Visibility = ViewStates.Invisible;
            _dictionaryGameToButton6.Visibility = ViewStates.Invisible;
            //addToFavorite.Alpha = 0;
            //if (playWithFavorites)
            //{
            //    addToFavorite.Alpha = 0;
            //}
            //else
            //{
            //    addToFavorite.Alpha = 1;
            //}
        }

        private void VisibleClearForm(GamePlayLevels gameLevel)
        {
            _dictionaryGameFromButton.Alpha = 1;
            _dictionaryGameToButton1.Alpha = 1;
            _dictionaryGameToButton2.Alpha = 1;
            _dictionaryGameToButton3.Alpha = 1;
            _dictionaryGameToButton4.Alpha = 1;
            _dictionaryGameToButton5.Alpha = 1;
            _dictionaryGameToButton6.Alpha = 1;
            _dictionaryGameFromButton.Visibility = ViewStates.Visible;
            _dictionaryGameToButton1.Visibility = ViewStates.Visible;
            _dictionaryGameToButton2.Visibility = ViewStates.Visible;
            _dictionaryGameToButton3.Visibility = ViewStates.Visible;
            _dictionaryGameToButton4.Visibility = ViewStates.Visible;
            if (gameLevel == GamePlayLevels.Easy)
            {
                _dictionaryGameToButton5.Visibility = ViewStates.Invisible;
                _dictionaryGameToButton6.Visibility = ViewStates.Invisible;
            }
            if (gameLevel == GamePlayLevels.Medium)
            {
                _dictionaryGameToButton5.Visibility = ViewStates.Visible;
                _dictionaryGameToButton6.Visibility = ViewStates.Invisible;
            }
            if (gameLevel == GamePlayLevels.Hard)
            {
                _dictionaryGameToButton5.Visibility = ViewStates.Visible;
                _dictionaryGameToButton6.Visibility = ViewStates.Visible;
            }

            _dictionaryGameFromButton.Text = "";
            _dictionaryGameToButton1.Text = "";
            _dictionaryGameToButton2.Text = "";
            _dictionaryGameToButton3.Text = "";
            _dictionaryGameToButton4.Text = "";
            _dictionaryGameToButton5.Text = "";
            _dictionaryGameToButton6.Text = "";
            ActionAddToFavoriteSetValue();
        }

        private void ActionAddToFavoriteSetValue()
        {
            if (!playWithFavorites)
            {
                //addToFavorite.Visibility = ViewStates.Invisible;
                addToFavorite.Text = "Add To Favorite";
                //addToFavorite.Alpha = 1;
            }
            else
            {
                addToFavorite.Text = "Remove from Favorite";
                //addToFavorite.Alpha = 1;
            }
        }

        public List<int> GenerateRandom(int count)
        {
            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();
            while (candidates.Count < count)
            {
                // May strike a duplicate.
                candidates.Add(_random.Next(count));
            }

            // load them in to a list.
            List<int> result = new List<int>();
            result.AddRange(candidates);

            // shuffle the results:
            int i = result.Count;
            while (i > 1)
            {
                i--;
                int k = _random.Next(i + 1);
                int value = result[k];
                result[k] = result[i];
                result[i] = value;
            }
            return result;
        }

        private Button GetRandomButton(int next)
        {
            switch (next)
            {
                case 0:
                    return _dictionaryGameToButton1;
                case 1:
                    return _dictionaryGameToButton2;
                case 2:
                    return _dictionaryGameToButton3;
                case 3:
                    return _dictionaryGameToButton4;
                case 4:
                    return _dictionaryGameToButton5;
                case 5:
                    return _dictionaryGameToButton6;
                default:
                    return _dictionaryGameToButton1;
            }
        }

        private int GetTrueWordindxIndx(IEnumerable<MyDictonaryWord> randomWordses)
        {
            int indx = 0;
            int trueWordindx = 0;
            foreach (MyDictonaryWord dictonaryWordse in randomWordses)
            {
                if ((dictonaryWordse.TranslatedWord == _wordContainer.CurrentWord.TranslatedWord) && (dictonaryWordse.Word == _wordContainer.CurrentWord.Word) && (dictonaryWordse.Language == _wordContainer.CurrentWord.Language))
                {
                    trueWordindx = indx;
                }
                indx++;
            }
            return trueWordindx;
        }

        private List<MyDictonaryWord> CopyList(List<MyDictonaryWord> list)
        {
            List<MyDictonaryWord> returnListlis = new List<MyDictonaryWord>();
            foreach (var variable in list)
            {
                returnListlis.Add(variable);
            }
            return returnListlis;
        }
    }



    public class MyGameTimer : CountDownTimer
    {
        private Activity _myActivity;
        private int _timeControl;
        private GamePlayLevelContainer _gameLevelContainer;
        private string _language;
        private bool playWithFavorites;

        private Button TimeControl => _myActivity.FindViewById<Button>(_timeControl);
        public long CountDownInterval { get; set; }

        public long MillisInFuture { get; set; }
        public long PassedMillis { get; set; }
        public delegate void OnTickHandler(Activity myActivity);
        public event OnTickHandler OnTickEvent;

        public void Set(Activity myActivity, string messageText, int timeControl, GamePlayLevelContainer gameLevelContainer, string language,bool playWithFavorites)
        {
            _myActivity = myActivity;
            _timeControl = timeControl;
            _gameLevelContainer = gameLevelContainer;
            _language = language;
            this.playWithFavorites = playWithFavorites;
        }

        public MyGameTimer(long millisInFuture, long countDownInterval) : base(millisInFuture, countDownInterval)
        {
            MillisInFuture = millisInFuture;
            CountDownInterval = countDownInterval;
        }



        private void GameOverAction()
        {
            long TotalScore = 0;
            ManagerSound.Instance.GameOver(_myActivity);
            var intent = new Intent();
            TotalScore = ManagerGameOver.Instance.CalculateTotalScore;
            intent.SetClass(_myActivity, typeof(GameOverActivity));
            intent.PutExtra("GameLevel", _gameLevelContainer.GameLevel.ToString());
            intent.PutExtra("PlayWithFavorites", playWithFavorites);
            intent.PutExtra("Language", _language);
            intent.PutExtra("TotalScore", TotalScore);
            _myActivity.StartActivityForResult(intent, 100);
            _myActivity.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }

        public override void OnFinish()
        {
            TimeControl.Text = $"{0}";
            TimeControl.Animate()
                   .SetDuration(100)
                   .ScaleX(0)
                   .ScaleY(0)
                   .SetInterpolator(new LinearInterpolator())
                   .WithEndAction(new Runnable(GameOverAction)
                   );
        }

        public override void OnTick(long millisUntilFinished)
        {
            OnTickEvent?.Invoke(_myActivity);
            PassedMillis = MillisInFuture - millisUntilFinished;
            TimeControl.Text = $"{millisUntilFinished / 1000}";
            Animation anim = _gameLevelContainer.GetAnimation(_myActivity, millisUntilFinished);
            if (anim != null)
            {
                TimeControl.SetBackgroundResource(Resource.Drawable.GameTimeButtonRedBackRound);
                TimeControl.StartAnimation(anim);
            }
            else
            {
                TimeControl.SetBackgroundResource(Resource.Drawable.GameTimeButtonRound);
            }

        }

        public void Reset()
        {
            base.Cancel();
            PassedMillis = 0;
        }
    }


}