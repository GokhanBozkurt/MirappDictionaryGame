using System;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Webkit;
public class ItemTouchHelperSimpleCallback : ItemTouchHelper.SimpleCallback
{
    private Paint p = new Paint();
    public ItemTouchHelperSimpleCallback(int dragDirs, int swipeDirs) : base(dragDirs, swipeDirs)
    {

    }
    public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
    {
        return false;
    }

    public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
    {
        int position = viewHolder.AdapterPosition;

        //if (direction == ItemTouchHelper.Left)
        //{
        //    viewHolder. adapter.removeItem(position);
        //}
        //else
        //{
        //    removeView();
        //    edit_position = position;
        //    alertDialog.setTitle("Edit Country");
        //    et_country.setText(countries.get(position));
        //    alertDialog.show();
        //}
    }
    public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
    {
        //base.OnChildDraw(cValue, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        Bitmap icon=null;
        if (actionState == ItemTouchHelper.ActionStateSwipe)
        {

            View itemView = viewHolder.ItemView;
            float height = (float)itemView.Bottom - (float)itemView.Top;
            float width = height / 3;

            if (dX > 0)
            {
                p.Color=(Color.ParseColor("#388E3C"));
                RectF background = new RectF((float)itemView.Left, (float)itemView.Top, dX, (float)itemView.Bottom);
                c.DrawRect(background, p);
                //icon = BitmapFactory.DecodeResource(ResourceVideoCapture(), R.drawable.ic_edit_white);
                RectF icon_dest = new RectF((float)itemView.Left + width, (float)itemView.Top + width, (float)itemView.Left + 2 * width, (float)itemView.Bottom - width);
                c.DrawBitmap(icon, null, icon_dest, p);
            }
            else
            {
                p.Color=Color.ParseColor("#D32F2F");
                RectF background = new RectF((float)itemView.Right + dX, (float)itemView.Top, (float)itemView.Right, (float)itemView.Bottom);
                c.DrawRect(background, p);
                //icon = BitmapFactory.DecodeResource(GetResources(), R.drawable.ic_delete_white);
                RectF icon_dest = new RectF((float)itemView.Right - 2 * width, (float)itemView.Top + width, (float)itemView.Right - width, (float)itemView.Bottom - width);
                c.DrawBitmap(icon, null, icon_dest, p);
            }
        }
        base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
    }
}
/*
public class dd: ItemTouchHelper.SimpleCallback
{
    private void initSwipe()
    {

    

    
            public void onSwiped(RecyclerView.ViewHolder viewHolder, int direction)
    {
        int position = viewHolder.getAdapterPosition();

        if (direction == ItemTouchHelper.LEFT)
        {
            adapter.removeItem(position);
        }
        else
        {
            removeView();
            edit_position = position;
            alertDialog.setTitle("Edit Country");
            et_country.setText(countries.get(position));
            alertDialog.show();
        }
    }

    @Override
            public void onChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, boolean isCurrentlyActive)
    {

        Bitmap icon;
        if (actionState == ItemTouchHelper.ACTION_STATE_SWIPE)
        {

            View itemView = viewHolder.itemView;
            float height = (float)itemView.Bottom - (float)itemView.Top;
            float width = height / 3;

            if (dX > 0)
            {
                p.setColor(Color.parseColor("#388E3C"));
                RectF background = new RectF((float)itemView.Left, (float)itemView.Top, dX, (float)itemView.Bottom);
                c.drawRect(background, p);
                icon = BitmapFactory.decodeResource(getResources(), R.drawable.ic_edit_white);
                RectF icon_dest = new RectF((float)itemView.Left + width, (float)itemView.Top + width, (float)itemView.Left + 2 * width, (float)itemView.Bottom - width);
                c.drawBitmap(icon, null, icon_dest, p);
            }
            else
            {
                p.setColor(Color.parseColor("#D32F2F"));
                RectF background = new RectF((float)itemView.Right + dX, (float)itemView.Top, (float)itemView.Right, (float)itemView.Bottom);
                c.drawRect(background, p);
                icon = BitmapFactory.decodeResource(getResources(), R.drawable.ic_delete_white);
                RectF icon_dest = new RectF((float)itemView.Right - 2 * width, (float)itemView.Top + width, (float)itemView.Right - width, (float)itemView.Bottom - width);
                c.drawBitmap(icon, null, icon_dest, p);
            }
        }
        super.onChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
    }
};
ItemTouchHelper itemTouchHelper = new ItemTouchHelper(simpleItemTouchCallback);
itemTouchHelper.attachToRecyclerView(recyclerView);
    }
}
*/