using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Threading.Tasks;

namespace MirappDictionaryGame
{
    public class SettingFragment : BaseFragment
    {
        private Button _dictionaryDelete;
        private Button _dictionaryLoad;
        private Button deleteFavorites;
        private ProgressBar _loadingProgressBar;
        private ProgressDialog _progress;
        private Switch _switchSoundEffects;
        //private RepositoryGameSetting<GameSetting> _repository;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();

            SetGameSetting();

        }

        private void SetGameSetting()
        {
            if (ManagerGameSetting.Instance.SettingAvaliable)
            {
                _switchSoundEffects.Checked = ManagerGameSetting.Instance.Sound;
            }
            else
            {
                ManagerRepository.Instance.GameSetting.Insert(new GameSetting { SoundActive = true });
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Setting, container, false);
            return view;
        }

        protected override void FindViews()
        {
            _dictionaryDelete = View.FindViewById<Button>(Resource.Id.DictionaryDelete);
            _dictionaryLoad = View.FindViewById<Button>(Resource.Id.DictionaryLoad);
            deleteFavorites = View.FindViewById<Button>(Resource.Id.DeleteFavorites);
            _switchSoundEffects = View.FindViewById<Switch>(Resource.Id.SwitchSoundEffects);
            _loadingProgressBar = View.FindViewById<ProgressBar>(Resource.Id.LoadingProgressBar);

            _loadingProgressBar.Visibility = ViewStates.Invisible;
            _loadingProgressBar.Max = 100;
            _loadingProgressBar.Progress = 0;
        }

        protected override void HandleEvents()
        {
            _dictionaryDelete.Click += DictionaryDelete_Click;
            _dictionaryLoad.Click += DictionaryLoad_Click;
            deleteFavorites.Click += DeleteFavorites_Click;

            //_switchSoundEffects.CheckedChange += SwitchSoundEffects_CheckedChange;
        }

        private void DeleteFavorites_Click(object sender, EventArgs e)
        {
            var gameSetting = new GameSetting()
            {
                SoundActive = _switchSoundEffects.Checked

            };

            if (ManagerRepository.Instance.GameSetting.Insert(gameSetting))
            {
                var toast = Toast.MakeText(Activity, "Sucessfull saved ...  ", ToastLength.Short);
                toast.Show();
            }
            else
            {
                var toast = Toast.MakeText(Activity, "Error.Please try again ...  ", ToastLength.Short);
                toast.Show();
            }
        }



        private void SwitchSoundEffects_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var gameSetting = new GameSetting()
            {
                SoundActive = e.IsChecked

            };
            ManagerRepository.Instance.GameSetting.Insert(gameSetting);
            var toast = Toast.MakeText(Activity, "Your answer is " + (e.IsChecked ? "correct" : "incorrect"), ToastLength.Short);
            toast.Show();
        }

        private void DictionaryLoad_Click(object sender, EventArgs e)
        {
            Task task = new Task(() =>
            {
                ManagerDictionary.Delete();
                _progress.Hide();
            });

            _progress = new ProgressDialog(Activity) { Indeterminate = true };
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
                ManagerDictionary.LoadDictionary(Activity, true);
                _progress.Hide();
            });

            _progress = new ProgressDialog(Activity) { Indeterminate = true };
            _progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progress.SetMessage("Loading... Please wait...");
            _progress.SetCancelable(false);
            _progress.Show();
            task.Start();

        }
    }
}