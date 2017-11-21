using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Linq;
using System.Text;
using System.IO;

namespace MirappDictionaryGame
{
    public class FragmentSetting : Fragment
    {
        private Button dictionaryDelete;
        private Button dictionaryLoad;
        private Button deleteFavorites;
        private Switch _switchSoundEffects;
        private ProgressBar _loadingProgressBar;
        private ProgressDialog _progress;
        private TextView _listCount;
        private Button scoreDelete;
        private Button levelDelete;
        private Button sendFavoriteWords;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();

            SetValues();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Setting, container, false);
            return view;
        }
        private void SetValues()
        {
            _listCount.Text = $"{ManagerDictionary.WordList.Count} Word exists";
            if (ManagerGameSetting.Instance.Sound)
            {
                _switchSoundEffects.Checked = true;
            }
            else
            {
                _switchSoundEffects.Checked = false;
            }

        }

        public void FindViews()
        {
            dictionaryDelete = View.FindViewById<Button>(Resource.Id.DictionaryDelete);
            dictionaryLoad = View.FindViewById<Button>(Resource.Id.DictionaryLoad);
            deleteFavorites = View.FindViewById<Button>(Resource.Id.DeleteFavorites);
            _switchSoundEffects = View.FindViewById<Switch>(Resource.Id.SwitchSoundEffects);
            _loadingProgressBar = View.FindViewById<ProgressBar>(Resource.Id.LoadingProgressBar);
            _listCount = View.FindViewById<TextView>(Resource.Id.SettingCount);
            scoreDelete = View.FindViewById<Button>(Resource.Id.ScoreDelete);
            levelDelete = View.FindViewById<Button>(Resource.Id.LevelDelete);
            sendFavoriteWords = View.FindViewById<Button>(Resource.Id.SendFavoriteWords);

            
            _loadingProgressBar.Visibility = ViewStates.Invisible;
            _loadingProgressBar.Max = 100;
            _loadingProgressBar.Progress = 0;
        }

        public void HandleEvents()
        {
            dictionaryDelete.Click += DictionaryDelete_Click;
            dictionaryLoad.Click += DictionaryLoad_Click;
            deleteFavorites.Click += DeleteFavorites_Click;
            scoreDelete.Click += ScoreDelete_Click;
            levelDelete.Click += LevelDelete_Click;
            _switchSoundEffects.Click += SwitchSoundEffectsClick;
            sendFavoriteWords.Click += SendFavoriteWords_Click;
        }

        private void DeleteFavorites_Click(object sender, EventArgs e)
        {
            ManagerRepository.Instance.FavoriteWord.DeleteAll();
            ManagerAlert.ShowToast(this.Activity, "Levels Deleted", ToastLength.Long);
        }

        private void SwitchSoundEffectsClick(object sender, EventArgs e)
        {
            try
            {

                var records = ManagerRepository.Instance.GameSetting.GetRecords();
                var count = records.Count;
                if (count > 1)
                {
                    ManagerRepository.Instance.GameSetting.DeleteAll();
                    count = ManagerRepository.Instance.GameSetting.GetRecords().Count;
                }

                if (count == 0)
                {
                    var gameSetting = new GameSetting();
                    gameSetting.SoundActive = _switchSoundEffects.Checked;

                    if (ManagerRepository.Instance.GameSetting.Insert(gameSetting))
                    {
                        var toast = Toast.MakeText(this.Activity, "Sucessfull saved ...  ", ToastLength.Short);
                        toast.Show();
                    }
                    else
                    {
                        var toast = Toast.MakeText(this.Activity, "Error.Please try again ...  ", ToastLength.Short);
                        toast.Show();
                    }
                }
                else
                {

                    var gameSetting = records[0];
                    gameSetting.SoundActive = _switchSoundEffects.Checked;

                    if (ManagerRepository.Instance.GameSetting.Update(gameSetting))
                    {
                        var toast = Toast.MakeText(this.Activity, "Sucessfull saved ...  ", ToastLength.Short);
                        toast.Show();
                    }
                    else
                    {
                        var toast = Toast.MakeText(this.Activity, "Error.Please try again ...  ", ToastLength.Short);
                        toast.Show();
                    }

                }


                //ManagerAlert.ShowToast(this.Activity, $" Sound:{ManagerGameSetting.Instance.Sound.ToString()}", ToastLength.Long);
            }
            catch (Exception ex)
            {
                ManagerAlert.ShowAlert(this.Activity, ex.Message);
                ManagerAlert.ShowAlert(this.Activity, ex.StackTrace);
            }
        }

        private void DictionaryLoad_Click(object sender, EventArgs e)
        {
            Task task = new Task(() =>
            {
                ManagerDictionary.Delete();
                _progress.Hide();
                SetValues();
            });

            _progress = new ProgressDialog(this.Activity) { Indeterminate = true };
            _progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progress.SetMessage("Loading... Please wait...");
            _progress.SetCancelable(false);
            _progress.Show();
            task.Start();
        }

        private void DictionaryDelete_Click(object sender, EventArgs e)
        {
            Task task = new Task(() =>
            {
                ManagerDictionary.LoadDictionary(this.Activity, true);
                _progress.Hide();
                SetValues();
            });

            _progress = new ProgressDialog(this.Activity) { Indeterminate = true };
            _progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progress.SetMessage("Loading... Please wait...");
            _progress.SetCancelable(false);
            _progress.Show();
            task.Start();

        }

        private void ScoreDelete_Click(object sender, EventArgs e)
        {
            ManagerRepository.Instance.GameScore.DeleteAll();
            ManagerAlert.ShowToast(this.Activity, "Scores Deleted", ToastLength.Long);
        }
        private void LevelDelete_Click(object sender, EventArgs e)
        {
            ManagerRepository.Instance.GameLevel.DeleteAll();
            ManagerAlert.ShowToast(this.Activity, "Levels Deleted", ToastLength.Long);
        }

        private void SendFavoriteWords_Click(object sender, EventArgs e)
        {

            try
            {
                var count = ManagerRepository.Instance.FavoriteWord.GetRecords().Count;
                ManagerAlert.ShowToast(this.Activity, count.ToString() + " words", ToastLength.Long);

                //var email = new Android.Content.Intent(Android.Content.Intent.ActionSend);
                ////email.PutExtra(Android.Content.Intent.ExtraEmail, new string[] { "gokhan.bozkurt@softtech.com.tr" });
                //email.PutExtra(Android.Content.Intent.ExtraSubject, "Favorite Words");
                //email.PutExtra(Android.Content.Intent.ExtraText, GetFavoritiesListasHtml());
                //email.SetType("message/rfc822");
                //StartActivity(email);

                

                string path = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "Mirapp");
                Java.IO.File imageFile = new Java.IO.File(path, "MirappFavoriteWords.txt");
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                    dir.CreateSubdirectory(path);

                imageFile.CreateNewFile();

                System.IO.File.WriteAllText(path, GetFavoritiesListasHtml());

                //var byttte = Encoding.ASCII.GetBytes(GetFavoritiesListasHtml());
                //Java.IO.FileOutputStream fo = new Java.IO.FileOutputStream(imageFile);

                //fo.Write(byttte);
                //fo.Close();


            }
            catch (Exception ex)
            {
                ManagerAlert.ShowToast(this.Activity, ex.Message, ToastLength.Long);
                ManagerAlert.ShowToast(this.Activity, ex.StackTrace, ToastLength.Long);

            }
        }


        public static string GetFavoritiesListasHtml()
        {

            StringBuilder sb = new StringBuilder();
            //sb.Append("<TABLE>\n");
            foreach (var item in ManagerRepository.Instance.FavoriteWord.GetRecords().Take(10))
            {
                //sb.Append("<TR>\n");
                //sb.Append("<TD>");
                sb.Append(item.ToString());
                //sb.Append("</TD>");
                //sb.Append("</TR>\n");
            }

            return sb.ToString();
        }

    }
}