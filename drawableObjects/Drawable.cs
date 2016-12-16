using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace drawableObjects
{

    /// <summary>
    /// a base class for all drawables, types of dimensions for a drawable are: (abs) is the real dimensions with respect to the 
    /// pictureBox height and width, (virtual) is the scaled dims according to parent maxX, maxY, (maxX,maxY) 
    /// are the width and height that children will be drawn and scalled accordingly
    /// </summary>
    public abstract class Drawable
    {
        #region fields

        private bool changed = true;
        
        protected Pen bPen;
        public object _object;
        protected DrawingController dc;
		protected bool visible= true;
		public CompositeDrawable parent;
        
        #endregion

        #region text fields and methods

        protected Font fnt;
        protected string txt= null;
        protected Brush tbrsh;
        protected StringFormat txtFormat;

        public Drawable setText(string text, Font font, Brush brush)
        {
            this.txt = text;
            this.fnt = font;
            this.tbrsh = brush;
			//
			reDraw();
            //
            return this;
        }

        public Drawable setTextFormat(StringFormat sf)
        {
            this.txtFormat = sf;
            reDraw();
            //
            return this;
        }

        public string text
        {
            get
            {
                return txt;
            }
            set
            {
                txt = value;
                reDraw();
            }
        }

        public Font font
        {
            get
            {
                return fnt;
            }
            set
            {
                fnt = value;
                reDraw();
            }
        }

        public Brush textBrush
        {
            get
            {
                return tbrsh;
            }
            set
            {
                tbrsh = value;
                reDraw();
            }
        }

        #endregion

        #region stroke
        public Pen strokePen
        {
            get
            {
                return bPen;
            }
            set
            {
                bPen = value;
                reDraw();
            }
        }

        public Drawable setStroke(Pen pen)
        {
            bPen = pen;
            reDraw();
            return this;
        }
        
		/// <summary>
		/// draws the drw on a transformed dimensions
		/// </summary>
        public abstract void Draw(Graphics g);

        #endregion

		#region change

		public bool Changed() { return changed; }

		public void setChanged(bool chng) 
		{
			if (chng)
			{
				this.changed = true;
				if (parent!=null)
				{
					parent.setChanged(true);
				}
			}
			else //false
			{
				this.changed = false;
			}
		}

		#endregion

		/// <summary>
        /// calculates abs dimensions from virtual ones   
        /// - to be called only by parent
        /// </summary>
        internal abstract void scaleToParent();

		//TODO: scale back methods have to be abstract, but virtual for now to accept default 
		//implementaion for drws that won't use any

		/// <summary>
		/// converts absX to Vx inside this kind of shape
		/// </summary>
		internal virtual float scaleBackX(float absX)
		{
			return absX;
		}

		/// <summary>
		/// converts absY to Vy inside this kind of shape
		/// </summary>
		internal virtual float scaleBackY(float absY)
		{
			return absY;
		}

        /// <summary>
        /// called whenever a change in position, size or visibility takes place
        /// </summary>
        public void update()
        {
            if (this.dc != null && this.visible)
            {
                this.dc.update(this);
            }
        }

		/// <summary>
		/// called after a chenge in color, border or inner text occurs
		/// </summary>
		public void reDraw()
		{
			if (this.dc != null && this.visible)
			{
				this.dc.reDraw(this);
			}
		}

		/// <summary>
		/// also fired on visibilityChanged
		/// </summary>
        public void fireMove(Drawable w)
        {
            if (onMove != null)
            {
                onMove(w);
            }
        }

        public virtual void setDC(DrawingController dc)
        {
            this.dc = dc;
        }

		public DrawingController getDc()
		{
			return this.dc;
		}

		/// <summary>
		/// adds this instance to the param parent, just equivalent to parent.add(this)
		/// </summary>
		public Drawable addTo(CompositeDrawable parent)
		{
			parent.add(this);
			return this;
		}

		public void setVisible(bool show)
		{
			this.visible = show;
			update();
			fireMove(this);
		}

        //move event
        public delegate void moveHandler(Drawable sender);
        public event moveHandler onMove;


		#region clickable

		public abstract bool checkPoint(float x0, float y0);
		public abstract bool checkRegion(float x0, float y0, float x1, float y1);


		//events
		public delegate void onClick(Drawable sender, float vx, float vy);
		public delegate void onIncluded(Drawable sender);
		public delegate void onHIncluded(Drawable sender);
		public delegate void onHExcluded(Drawable sender);
		public delegate void onMouseDown(Drawable sender);
		public delegate void onHover(Drawable sender);
		//
		public event onClick clicked;
		public event onIncluded included;
		public event onHIncluded hIncluded;
		public event onHExcluded hExcluded;
		public event onHover hovered;
		/// <summary>
		/// may be used to initialize something before the included event
		/// </summary>
		public event onMouseDown mouseDown;


		public void fireInclude()
		{
			if (included != null )
			{
				included(this);
			}
		}

		/// <summary>
		/// it's a trigger for both hinclude and hexclude, when called it must fire one of them
		/// </summary>
		public void fireHInclude(bool included)
		{
			if (included)
			{
				if (hIncluded != null)
				{
					hIncluded(this);
				}
			}
			else
			{
				if (hExcluded != null)
				{
					hExcluded(this);
				}
			}

		}

		/// <summary>
		/// checks if a click event is assigned, if yes it fires it and returns true
		/// </summary>
		public bool fireClick(float x, float y)
		{
				if (clicked != null)
				{
					clicked(this, scaleBackX(x), scaleBackY(y));
					return true;
				}
				return false;
		}

		public bool fireMouseDown()
		{
				if (mouseDown != null)
				{
					mouseDown(this);
					return true;
				}
				return false;
		}

		public bool fireHover()
		{
				if (hovered != null)
				{
					hovered(this);
					return true;
				}
				return false;
		}

		#endregion

		#region transformation inside composite

		/// <summary>
		/// takes abs x inside the entire viewpor, returns abs X for drw inside composite
		/// </summary>
		protected float Xtr(float X)
		{
			if (parent != null)
			{
				return X - parent.absX;
			}
			//else - only in layer
			return X;
		}

		/// <summary>
		/// takes abs y inside the entire viewpor, returns abs y for drw inside composite
		/// </summary>
		protected float Ytr(float Y)
		{
			if (parent != null)
			{
				return Y - parent.absY;
			}
			//else - only in layer
			return Y;
		}

		#endregion

		public void remove()
		{
			if (this.parent != null)
			{
				this.parent.remove(this);
			}
        }


        #region image

        protected Image img;

        public Drawable setImage(Image img)
        {
            this.img = img;

            //
            return this;
        }

        #endregion

    }

    

    
}
