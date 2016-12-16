using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace drawableObjects
{
    /// <summary>
    /// draws every thing every time interval after checking if any changes happened 
    /// , controlled by a timer, call start() to start drawing
    /// </summary>
    public class AnimationDrawingController : DrawingController
    {

        #region fields
        private bool change;
        Timer timer;

        #endregion

        public AnimationDrawingController(PictureBox p, int updatesPerSecond) 
            : base(p) 
        {
            timer = new Timer();
            timer.Interval = (1000 / updatesPerSecond);
            timer.Tick += new EventHandler(timer_Tick);
            change = true;
        }

		public AnimationDrawingController(PictureBox p)
			: base(p)
		{
			timer = new Timer();
			timer.Interval = (1000 / 5);
			timer.Tick += new EventHandler(timer_Tick);
			change = true;
		}


       
        public override void add(Layer drw)
        {
			this.layers.Add(drw);
			//
			setDrwController(drw);
        }


        private void setDrwController(Drawable drw)
        {
			drw.setDC(this);
			if (drw is CompositeDrawable)
			{
				foreach (Drawable d in ((CompositeDrawable)drw).drawables)
				{
					setDrwController(d);
				}
			}
        }

        public override void update(Drawable drw)
        {
            change = true;
			//
			if (drw.parent != null)
			{

				reDraw(drw.parent);
			}
			else
			{
				reDraw(drw);
			}
        }

		public override void reDraw(Drawable drawable)
		{
			change = true;
			drawable.setChanged(true);
		}

        //
        //Animation Drawing Controller methods
        //

        private void drawAll()
        {
                refresh();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (change)
            {
                change = false;
                drawAll();
            }
        }

        /// <summary>
        /// starts drawing
        /// </summary>
        public override void start()
        {
            timer.Start();
        }

		protected override void stop()
		{
			timer.Stop();
		}


	}
}
