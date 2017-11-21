using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using AndroidSwipeLayout;
using Android.Widget;

namespace MirappDictionaryGame 
{
    [Activity(Label = 
                "SwipeLayoutSampleActivity",
                Theme = "@style/AppTheme", 
                MainLauncher = false,
                Icon = "@drawable/logo"
                )]
    public class SwipeLayoutSampleActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SwipeLayoutSampleLayout);

            var sample1 = FindViewById<SwipeLayout>(Resource.Id.sample1);
            sample1.SetShowMode(SwipeLayout.ShowMode.PullOut);
            sample1.AddDrag(SwipeLayout.DragEdge.Left, sample1.FindViewById(Resource.Id.bottom_wrapper));
            sample1.AddDrag(SwipeLayout.DragEdge.Right, sample1.FindViewById(Resource.Id.bottom_wrapper_2));
            sample1.AddDrag(SwipeLayout.DragEdge.Top, sample1.FindViewById(Resource.Id.starbott));
            sample1.AddDrag(SwipeLayout.DragEdge.Bottom, sample1.FindViewById(Resource.Id.starbott));
            sample1.AddRevealListener(Resource.Id.delete, (sender, e) => {
            });
            sample1.SurfaceView.Click += (sender, e) => {
                Toast.MakeText(this, "Click on surface", ToastLength.Short).Show();
            };
            sample1.SurfaceView.LongClick += (sender, e) => {
                Toast.MakeText(this, "longClick on surface", ToastLength.Short).Show();
                e.Handled = true;
            };
            sample1.FindViewById(Resource.Id.star2).Click += (sender, e) => {
                Toast.MakeText(this, "Star", ToastLength.Short).Show();
            };
            sample1.FindViewById(Resource.Id.trash2).Click += (sender, e) => {
                Toast.MakeText(this, "Trash Bin", ToastLength.Short).Show();
            };
            sample1.FindViewById(Resource.Id.magnifier2).Click += (sender, e) => {
                Toast.MakeText(this, "Magnifier", ToastLength.Short).Show();
            };
            sample1.AddRevealListener(Resource.Id.starbott, (sender, e) => {
                View star = e.Child.FindViewById(Resource.Id.star);
                float d = e.Child.Height / 2 - star.Height / 2;
                //ViewHelper.SetTranslationY(star, d * e.Fraction);
                //ViewHelper.SetScaleX(star, e.Fraction + 0.6f);
                //ViewHelper.SetScaleY(star, e.Fraction + 0.6f);
            });
        }
    }
}