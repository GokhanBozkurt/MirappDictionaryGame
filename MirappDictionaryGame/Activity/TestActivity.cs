using System.Timers;
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Java.Lang;
using String = System.String;
using NotificationCompat = Android.Support.V4.App.NotificationCompat;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

namespace MirappDictionaryGame
{
    [Activity(Label = "TestActivity",
                MainLauncher = false,
                Icon = "@drawable/icon",
                Theme = "@style/MyTheme"
        )]
    public class TestActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TestLayout);
            //Button button = (Button)FindViewById(Resource.Id.testbutton);
            //button.Click += Button_Click;
            var testbutton = FindViewById<Button>(Resource.Id.testbutton);
            testbutton.Click += ButtonOnClick;

            //var demoServiceIntent = new Intent(this, typeof(DictionaryService));
            //var demoServiceConnection = new ServiceBinder(this);
            //ApplicationContext.BindService(demoServiceIntent, demoServiceConnection, Bind.AutoCreate);

        }
        private int count = 1;

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            // Pass the current button press count value to the next activity:
            Bundle valuesForActivity = new Bundle();
            valuesForActivity.PutInt("count", count);

            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent = new Intent(this, typeof(AppStartActivity));

            // Pass some values to SecondActivity:
            resultIntent.PutExtras(valuesForActivity);

            // Construct a back stack for cross-task navigation:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(Class.FromType(typeof(AppStartActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

            // Build the notification:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetAutoCancel(true)                    // Dismiss the notification from the notification area when the user clicks on it
                .SetContentIntent(resultPendingIntent)  // Start up this activity when the user clicks the intent.
                .SetContentTitle("Button Clicked")      // Set the title
                .SetNumber(count)                       // Display the count in the Content Info
                .SetSmallIcon(Resource.Drawable.ic_stat_button_click) // This is the icon to display
                .SetContentText(String.Format("The button has been clicked {0} times.", count)); // the message to display.

            // Finally, publish the notification:
            NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(1000, builder.Build());

            // Increment the button press count:
            count++;
        }



    }

    /*

    [Service]
    [IntentFilter(new String[] { "com.Mirapp.DictionaryService" })]
    public class MyService : Service
    {
        Context mContext;
        private Timer _timer;

        public MyService()
        {
            mContext = Android.App.Application.Context;
        }

        [Obsolete("deprecated")]
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }

        #region implemented abstract members of Service

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        #endregion

        public override void OnDestroy()
        {
            base.OnDestroy();
            // cleanup code
        }

        public override void OnCreate()
        {
            base.OnCreate();

            _timer = new Timer();
            _timer.Enabled = true;
            _timer.Interval = 60000;
            _timer.Elapsed += OnTimeEvent;
        }

        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
            // Build the notification:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetAutoCancel(true)                    // Dismiss the notification from the notification area when the user clicks on it
                .SetContentTitle("Button Clicked")      // Set the title
                .SetSmallIcon(Resource.Drawable.logo) // This is the icon to display
                .SetContentText(String.Format("Timer")); // the message to display.

            // Finally, publish the notification:
            NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(1001, builder.Build());
        }
    }
   

    public class MyBinder : Java.Lang.Object, IServiceConnection
    {
        private Context mnContext;

        public MyBinder()
        {
            mnContext = Android.App.Application.Context;
        }

        public MyBinder(Context context)
        {
            mnContext = context;
        }

        #region IServiceConnection implementation

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
        }

        public void OnServiceDisconnected(ComponentName name)
        {
        }

        #endregion
    }
 */

}