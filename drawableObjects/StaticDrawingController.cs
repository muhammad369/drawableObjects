using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace drawableObjects
{
    /// <summary>
    /// redraws a drawable whenever any of its properties set<br/>
    /// doesn't erase any thing
    /// </summary>
    public class StaticDrawingController : DrawingController
    {

        public StaticDrawingController(PictureBox p) : base(p) { }

        
        public override void add(Layer drw)
        {
			this.layers.Add(drw);
			//
            setDrwController(drw);
        }

        private void setDrwController(Drawable drw)
        {
            
            drw.setDC(this);
            if(drw is CompositeDrawable)
            {
                foreach (Drawable d in ((CompositeDrawable)drw).drawables)
                {
                    setDrwController(d);
                }
            }
        }

        /// <summary>
        /// almost redrawing it's parent
        /// </summary>
        public override void update(Drawable drw)
        {
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
			drawable.setChanged(true);
			this.refresh();
		}

		public override void start()
		{
			this.refresh();
		}
	}
}
