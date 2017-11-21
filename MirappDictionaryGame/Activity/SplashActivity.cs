
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System.Threading.Tasks;
using Android.Content.PM;

namespace MirappDictionaryGame
{
    [Activity(
        Theme = "@style/MyTheme.Splash",
        Icon = "@drawable/logo",
        ScreenOrientation = ScreenOrientation.Portrait,
        MainLauncher = true,
        NoHistory = true
        , Label = "Mirapp Dict Game")]
    public class SplashActivity : Activity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {

            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            try
            {

                base.OnResume();

                Task startupWork = new Task(() =>
                {
                    Task.Delay(8000); // Simulate a bit of startup work.
                });

                startupWork.ContinueWith(t =>
                {
                    StartActivity(new Intent(Application.Context, typeof(AppStartActivity)));
                    this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
                }, TaskScheduler.FromCurrentSynchronizationContext());

                Task dictonary = new Task(() =>
                {
                    ManagerDictionary.LoadDictionary(this, true);
                }
                                                );
                startupWork.Start();
                dictonary.Start();
            }
            catch (System.Exception ex)
            {
                ManagerAlert.ShowAlert(this, $"{ex.Message}  {ex.StackTrace}"); ;
            }
        }
    }
}