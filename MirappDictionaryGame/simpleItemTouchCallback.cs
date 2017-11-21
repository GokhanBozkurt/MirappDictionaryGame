using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using static Android.Support.V7.Widget.Helper.ItemTouchHelper;
using Android.Support.V7.Widget.Helper;

namespace MirappDictionaryGame
{
    class simpleItemTouchCallback : SimpleCallback
    {
        public DataAdapter adapter;
        private int edit_position;
        private View view;
        MyFragmentBase<MyDictonaryWord> fragment;
        public AlertDialog alertDialog;
        public EditText et_country;
        public List<string> list;

        public delegate void myDelegate(MyDictonaryWord myDictonaryWord);
        public event myDelegate EndEvent;

        public simpleItemTouchCallback(MyFragmentBase<MyDictonaryWord> fragment) :base(0, Left | Right)
        {
            this.fragment = fragment;
        }
        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return false;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            int position = viewHolder.AdapterPosition;
             var Id = Convert.ToInt32(((TextView)viewHolder.ItemView.FindViewById(Resource.Id.listwordItemWordId)).Text);

            if (direction == Left)
            {
                var item = fragment.GetItem(Id);
                adapter.removeItem(position,item);
                EndEvent(item);
            }
            else
            {
                //removeView();
                //edit_position = position;
                //alertDialog.SetTitle("Edit Word");
                //et_country.Text= list[position];
                //alertDialog.Show();
            }
        }

        private void removeView()
        {
            if (view.Parent != null)
            {
                ((ViewGroup)view.Parent).RemoveView(view);
            }
        }
        private Paint p = new Paint();
        public override void OnChildDraw(Canvas cValue, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            Bitmap icon;
            if (actionState == ItemTouchHelper.ActionStateSwipe)
            {

                View itemView = viewHolder.ItemView;
                float height = (float)itemView.Bottom - (float)itemView.Top;
                float width = height / 3;

                if (dX > 0)
                {
                    p.Color=Color.ParseColor("#929292");
                    RectF background = new RectF((float)itemView.Left, (float)itemView.Top, dX, (float)itemView.Bottom);
                    cValue.DrawRect(background, p);
                    icon = BitmapFactory.DecodeResource( fragment.Activity.Resources, Resource.Drawable.edit);
                    RectF icon_dest = new RectF((float)itemView.Left + width, (float)itemView.Top + width, (float)itemView.Left + 2 * width, (float)itemView.Bottom - width);
                    cValue.DrawBitmap(icon, null, icon_dest, p);
                }
                else
                {
                    p.Color = Color.ParseColor("#F06292");
                    RectF background = new RectF((float)itemView.Right + dX, (float)itemView.Top, (float)itemView.Right, (float)itemView.Bottom);
                    cValue.DrawRect(background, p);
                    icon = BitmapFactory.DecodeResource(fragment.Activity.Resources, Resource.Drawable.delete);
                    RectF icon_dest = new RectF((float)itemView.Right - 2 * width, (float)itemView.Top + width, (float)itemView.Right - width, (float)itemView.Bottom - width);
                    cValue.DrawBitmap(icon, null, icon_dest, p);
                }
            }

            base.OnChildDraw(cValue, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        }
    }
}