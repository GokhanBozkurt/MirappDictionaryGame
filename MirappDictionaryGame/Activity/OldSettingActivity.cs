using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Toolbars = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Android.Widget;

namespace MirappDictionaryGame
{
    [Activity(
        MainLauncher = false,
        Theme = "@style/AppThemeRedNoActionBar",
        Label = "Add Word",
        Icon = "@drawable/icon",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class OldSettingActivity : AppCompatActivityBase
    {
        private Button _dictionaryDelete;
        private Button _dictionaryLoad;
        private Button _saveSetting;
        private Switch _switchSoundEffects;
        private ProgressBar _loadingProgressBar;
        private ProgressDialog _progress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.Setting);

            base.OnCreate(savedInstanceState);

        }


        public override void FindViews()
        {
            _dictionaryDelete = FindViewById<Button>(Resource.Id.DictionaryDelete);
            _dictionaryLoad = FindViewById<Button>(Resource.Id.DictionaryLoad);
            _saveSetting = FindViewById<Button>(Resource.Id.DeleteFavorites);
            _switchSoundEffects = FindViewById<Switch>(Resource.Id.SwitchSoundEffects);
            _loadingProgressBar = FindViewById<ProgressBar>(Resource.Id.LoadingProgressBar);

            _loadingProgressBar.Visibility = ViewStates.Invisible;
            _loadingProgressBar.Max = 100;
            _loadingProgressBar.Progress = 0;
        }

        public override void HandleEvents()
        {
            _dictionaryDelete.Click += DictionaryDelete_Click;
            _dictionaryLoad.Click += DictionaryLoad_Click;
            _saveSetting.Click += _saveSetting_Click;
        }

        public override void LoadViews()
        {
        }

        public override void SetToolBar()
        {
            //var toolbarTop = FindViewById<Toolbars>(Resource.Id.SettingToolbarTop);
            //toolbarTop.Title = "Setting";
            //SetSupportActionBar(toolbarTop);
        }

        public override void SetRepository()
        {
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.SettingTollbarItem, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.TitleFormatted.ToString())
            {
                case "Add":
                    ToAdd();
                    break;
                case "List":
                    ToList();
                    break;
                case "Home":
                    ToHome();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void _saveSetting_Click(object sender, EventArgs e)
        {
            var gameSetting = new GameSetting()
            {
                SoundActive = _switchSoundEffects.Checked

            };

            if (ManagerRepository.Instance.GameSetting.Insert(gameSetting))
            {
                var toast = Toast.MakeText(this, "Sucessfull saved ...  ", ToastLength.Short);
                toast.Show();
            }
            else
            {
                var toast = Toast.MakeText(this, "Error.Please try again ...  ", ToastLength.Short);
                toast.Show();
            }
        }

        private void DictionaryLoad_Click(object sender, EventArgs e)
        {
            Task task = new Task(() =>
            {
                ManagerDictionary.Delete();
                _progress.Hide();
            });

            _progress = new ProgressDialog(this) { Indeterminate = true };
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
                ManagerDictionary.LoadDictionary(this, true);
                _progress.Hide();
            });

            _progress = new ProgressDialog(this) { Indeterminate = true };
            _progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progress.SetMessage("Loading... Please wait...");
            _progress.SetCancelable(false);
            _progress.Show();
            task.Start();

        }




    }
}