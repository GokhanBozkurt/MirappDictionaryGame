
using Android.App;
using Android.Content;
using Android.Widget;

namespace MirappDictionaryGame
{
    class ManagerAlert
    {
        public static void ShowAlert(Activity activity,string message)
        {

            try
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(activity);
                alert.SetTitle("Alert");
                alert.SetMessage(message);
                alert.SetPositiveButton("OK", (senderAlert, args) =>
                {
                    var intent = new Intent();
                    intent.SetClass(activity, typeof(AppStartActivity));
                    activity.StartActivityForResult(intent, 100);
                    activity.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
                });

                //alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                //    Toast.MakeText(activity, "Cancelled!", ToastLength.Short).Show();
                //});
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            catch (System.Exception ex)
            {

                ShowToast(activity, string.Format("Hata :{0}  StackTrace:{1}", ex.Message, ex.StackTrace), ToastLength.Long);
            }
        }

        public static void ShowToast(Activity activity, string message, ToastLength toastLength)
        {
            var toast1 = Toast.MakeText(activity, message, toastLength);
            toast1.Show();
        }
    }
}