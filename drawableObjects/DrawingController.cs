using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;


namespace drawableObjects
{


    /// <summary>
    /// an abstract class to control the drawing process of the drawables that it contains
    /// </summary>
    public abstract class DrawingController
    {
        bool active;
        PictureBox control;
        protected Image img;
        protected Graphics g;
        protected List<Layer> layers = new List<Layer>();
        
		

        //constructor
        public DrawingController(PictureBox cntrl)
        {
            
            active = true;

            control = cntrl;
            img = new Bitmap(control.Width, control.Height);
            //
            g = Graphics.FromImage(img);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            control.Image = img;
            //assign events
            control.MouseClick += new MouseEventHandler(control_MouseClick);
            control.MouseDown += new MouseEventHandler(control_MouseDown);
            control.MouseUp += new MouseEventHandler(control_MouseUp);
            control.MouseMove += new MouseEventHandler(control_MouseMove);
        }

		#region mouse events
		int x0, y0; //for include event
		bool dragMode = false;

        void control_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.active)
            {
                if (dragMode) //hover include
                {
                    foreach (Layer l in layers) 
                    {
						checkHIncluded(l, x0, y0, e.X, e.Y);
                    }
                    
                }
				else //just hover
				{
					hoverHandled = false;
					for (int i = layers.Count - 1; i >= 0; i--) //backward loop
					{
						checkHovered(layers[i], e.X, e.Y);
						if (hoverHandled) break;
					}
				}
                control.Refresh();

            }
        }


        void control_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.active)
            {
                if (dragMode) //include
                {
                    foreach (Layer l in layers)
                    {
						check_Included(l, x0, y0, e.X, e.Y);
                    }
                    dragMode = false;
                    control.Refresh();
                }
            }
        }

        void control_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.active)
            {
                dragMode = true;
                x0 = e.X;
                y0 = e.Y;
				//foreach (Clickable item in clickables)
				//{
				//	item.checkMouseDown(e.X, e.Y);
				//}
				//control.Refresh();
            }
        }

        void control_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.active)
            {
				clickHandled = false;
				for (int i = layers.Count - 1; i >= 0; i--)
				{
                    checkClicked(layers[i], e.X, e.Y);
					if (clickHandled) break;
                }
                control.Refresh();
            }
        }

		//
		//=====
		bool clickHandled = false, hoverHandled = false;

		void checkClicked(Drawable drw, float x, float y)
		{
			if(! drw.checkPoint(x, y)) return;
			//loop backward
			if (drw is CompositeDrawable)
			{
				CompositeDrawable cd= (CompositeDrawable)drw;
				//todo: apply inverse transformation
				for (int i = cd.clickables.Count-1; i >= 0; i--)
				{
					checkClicked(cd.clickables[i], x, y);
					if (clickHandled) return; //don't continue looping
				}
			}
			//fire
            if (clickHandled) return;
			clickHandled = drw.fireClick(x, y);
		}

		void checkHovered(Drawable drw, float x, float y)
		{
			if (!drw.checkPoint(x, y)) return;
			//loop backward
			if (drw is CompositeDrawable)
			{
				CompositeDrawable cd = (CompositeDrawable)drw;
				//todo: apply inverse transformation
				for (int i = cd.clickables.Count - 1; i >= 0; i--)
				{
					checkHovered(cd.clickables[i], x, y);
					if (hoverHandled) return;
				}
			}
			//fire
			hoverHandled = drw.fireHover();
		}

		void checkHIncluded(Drawable drw, float x0, float y0, float x1, float y1)
		{
			if (drw.checkRegion(x0, y0, x1, y1))
			{
				drw.fireHInclude(true);
				if (drw is CompositeDrawable)
				{
					foreach (Drawable d in ((CompositeDrawable)drw).clickables)
					{
						checkHIncluded(d, x0, y0, x1, y1);
					}
				}
			}
			else
			{
				drw.fireHInclude(false);
				if (drw is CompositeDrawable)
				{
					foreach (Drawable d in ((CompositeDrawable)drw).clickables)
					{
						d.fireHInclude(false);
					}
				}
			}
		}

		void check_Included(Drawable drw, float x0, float y0, float x1, float y1)
		{
			if (drw.checkRegion(x0, y0, x1, y1))
			{
				drw.fireInclude();
				if (drw is CompositeDrawable)
				{
					foreach (Drawable d in ((CompositeDrawable)drw).clickables)
					{
						check_Included(d, x0, y0, x1, y1);
					}
				}
			}
			else
			{
				
			}
		}

		#endregion



		/// <summary>
        /// active is for the ability to listen to the picture box events
        /// </summary>
        public bool Active
        {
            set
            {
                if (value)
                {
                    this.active = true;
                    this.control.Image = this.img;
                    refresh();
					start();
                }
                else
                {
                    this.active = false;
					stop();
                }
            }
        }

		protected virtual void stop()
		{
			//does nothing here
		}

        public int Width()
        {
            return img.Width;
        }

        public int Height()
        {
            return img.Height;
        }

        public void refresh()
        {
			g.Clear(Color.White);
			//
			foreach(Layer l in this.layers)
			{
				l.Draw(g);
			}
            control.Refresh();
        }

        /// <summary>
        /// erases all drawings and removes them from drawings and clickables lists
        /// </summary>
        public void erase()
        {
            
            this.layers.Clear();
            g.Clear(Color.White);
        }

		/// <summary>
		/// register clickable within it's parent, and the parent within it's own parent, 
        /// it's enouph to register the last inner
		/// </summary>
        public void registerClickable(Drawable c)
        {
			c.parent.registerClickable(c);
        }

        /// <summary>
        /// adds a layer to the dc layers
        /// </summary>
        public abstract void add(Layer lyr);


        /// <summary>
        /// not intended to be used directly by code, but to implement the Visitor pattern, 
        /// <br/> means to be called by any of the drawable setter methods that requires an
        ///  update (position and visibility change)<br/>
        ///  behavior changes from child to other(drawing controller)
        /// </summary>
        /// <param name="drw"></param>
        public abstract void update(Drawable drw);


		public abstract void reDraw(Drawable drawable);

		/// <summary>
		/// starts drawing
		/// </summary>
		public abstract void start();

	}
}
