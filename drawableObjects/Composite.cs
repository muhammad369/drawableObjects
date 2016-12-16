using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace drawableObjects
{
    public class CompositeDrawable : Drawable
    {

        public CompositeDrawable(float x, float y, float w, float h)
        {
            this.Vx = x;
            Vy = y;
            Vw = w;
            Vh = h;
        }

        #region fields
        //the dimensions that every dim in inner drws is to be scaled against
        internal float maxX=100, maxY=100;
        internal float Vw, Vh, Vx, Vy; //virtual width, and virtual height
        internal float absX, absY, absWidth, absHeight;
        protected Brush fBrush;
        //
        public List<Drawable> drawables=new List<Drawable>();
		public List<Drawable> clickables = new List<Drawable>();
        //
        protected Bitmap cacheImg;
        protected Graphics g;

        #endregion

        /// <summary>
        /// initialises the cache image and it's Graphics instance 
        /// with the real width and height of the composite drw - 
        /// to called by parent only
        /// </summary>
        public void initCache()
        {
            cacheImg = new Bitmap((int)this.absWidth, (int)this.absHeight);
            g = dUtility.getImageGraphics(cacheImg);
        }

        /// <summary>
        /// only set before drawing anything, poth are defaulted to 100
        /// </summary>
        public CompositeDrawable setMaxDims(float maxX, float maxY)
        {
            this.maxX = maxX;
            this.maxY = maxY;
            //
            return this;
        }

        /// <summary>
        /// adds the drw to inner drws of (this), and sets (this) as the drw parent, 
        /// and adds the drw to the DC if it is initialized, 
        /// this method doesn't draw the drawable
        /// </summary>
        public CompositeDrawable add(Drawable d)
        {
            //set changed to true, because it might has been drawn
            this.setChanged(true);
            //
            this.drawables.Add(d);
			d.parent = this;
            if (this.dc != null)
            {
                d.setDC(this.dc);
            }
            else
            {
                //dc must be set to parent before adding any children
                throw new Exception("'drawing controller' must be set to a parent before adding any children");
            }
            d.scaleToParent();
            //if composite init cache
            if (d is CompositeDrawable)
            {
                ((CompositeDrawable)d).initCache();
            }

			//
			return this;
        }

		public void remove(Drawable d)
		{
			//
			this.drawables.Remove(d);
			//remove from clickables if does exist
			if (this.clickables.Contains(d))
			{
				clickables.Remove(d);
			}
			this.update();
		}

        public override void setDC(DrawingController dc)
        {
            this.dc = dc;
            foreach (Drawable d in this.drawables) //useless
            {
                d.setDC(dc);
            }
        }

        /// <summary>
        /// register clickable within it's parent, and the parent within it's own parent, recursively, 
        /// it better to use drawingController.registerClickable()
        /// </summary>
		public virtual void registerClickable(Drawable clk)
		{
			if (!this.clickables.Contains(clk)) //check if it already exists
			{
				this.clickables.Add(clk);
                if (parent != null) //the layer won't have a parent
                {
                    this.parent.registerClickable(this);
                }
			}
		}




        #region shape methods


        internal override void scaleToParent()
        {
            if (parent != null)
            {
                this.absWidth =(float)( (((double)parent.absWidth / (double)parent.maxX) * (double)this.Vw) );
                this.absHeight =(float)( (((double)parent.absHeight / (double)parent.maxY) * (double)this.Vh) );
                //
                this.absX =(float)( (double)parent.absX + ((double)this.Vx * ((double)parent.absWidth / (double)parent.maxX)) );
                this.absY =(float)( (double)parent.absY + ((double)this.Vy * ((double)parent.absHeight / (double)parent.maxY)) );
            }
        }

		internal override float scaleBackX(float absX)
		{
			if (parent != null)
			{
                return (float) (
                    ((double)absX - (double)parent.absX) * ((double)parent.maxX / (double)parent.absWidth)
                    )- Vx;
			}
			return 0;
		}

		internal override float scaleBackY(float absY)
		{
			if (parent != null)
			{
                return (float) (
                    ((double)absY - (double)parent.absY) * ((double)parent.maxY / (double)parent.absHeight)
                    )-Vy;
			}
			return 0;
		}

        /// <summary>
        /// the virtual x
        /// </summary>
        public float x
        {
            get
            {
                return Vx;
            }
            set
            {
                Vx = value;
                scaleToParent();
                update();
                fireMove(this);
            }
        }

        /// <summary>
        /// the virtual y
        /// </summary>
        public float y
        {
            get
            {
                return Vy;
            }
            set
            {
                Vy = value;
                scaleToParent();
                update();
                fireMove(this);
            }
        }

        /// <summary>
        /// the virtual width
        /// </summary>
        public float width
        {
            get
            {
                return Vw;
            }
            set
            {
                Vw = value;
                scaleToParent();
                update();
                fireMove(this);
            }
        }

        /// <summary>
        /// the virtual height
        /// </summary>
        public float height
        {
            get
            {
                return Vh;
            }
            set
            {
                Vh = value;
                scaleToParent();
                update();
                fireMove(this);
            }
        }

        public Brush fillBrush
        {
            get
            {
                return fBrush;
            }
            set
            {
                fBrush = value;
                reDraw();
            }
        }

        /// <summary>
        /// sets the virtual center
        /// </summary>
        public void setCenter(PointF c)
        {
            this.Vx = (c.X - (Vw / 2));
            this.Vy = (c.Y - (Vw / 2));
            scaleToParent();
            update();
            fireMove(this);
        }

        /// <summary>
        /// the virtual center
        /// </summary>
        public PointF getCenter()
        {
            return new PointF(Vx + (Vw / 2), Vy + (Vh / 2));
        }

        public PointF getMiddleTop()
        {
            return new PointF(Vx + (Vw / 2), Vy);
        }

        public void setMiddleTop(PointF p)
        {
            this.Vx = (int)(p.X - (Vw / 2));
            this.Vy = (int)(p.Y);
            scaleToParent();
            update();
            fireMove(this);
        }

        public PointF getMiddleButtom()
        {
            return new PointF(Vx + (Vw / 2), Vy + Vh);
        }

        public void setMiddleButtom(PointF p)
        {
            this.Vx = (p.X - (Vw / 2));
            this.Vy = (p.Y - Vh);
            scaleToParent();
            update();
            fireMove(this);
        }

        public PointF getMiddleRight()
        {
            return new PointF(Vx + Vw, Vy + (Vh / 2));

        }

        public void setMiddleRight(PointF p)
        {
            this.Vx = (p.X - Vw);
            this.Vy = (p.Y - (Vh / 2));
            scaleToParent();
            update();
            fireMove(this);
        }

        public PointF getMiddleLeft()
        {
            return new PointF(Vx, Vy + (Vh / 2));
        }

        public void setMiddleLeft(PointF p)
        {
            this.Vx = (p.X);
            this.Vy = (p.Y - (Vh / 2));
            scaleToParent();
            update();
            fireMove(this);
        }

        public void move(PointF p)
        {
            this.Vx += p.X;
            this.Vy += p.Y;
            scaleToParent();
            update();
            fireMove(this);
        }

        public PointF getPoint(ShapePoint pnt)
        {
            switch (pnt)
            {
                case ShapePoint.center:
                    return getCenter();

                case ShapePoint.middleTop:
                    return getMiddleTop();

                case ShapePoint.middleButtom:
                    return getMiddleButtom();

                case ShapePoint.middleRight:
                    return getMiddleRight();

                case ShapePoint.middleLeft:
                    return getMiddleLeft();

                default:
                    return getCenter();

            }
        }

        public void setPoint(ShapePoint pnt, PointF p)
        {
            switch (pnt)
            {
                case ShapePoint.center:
                    setCenter(p);

                    break;

                case ShapePoint.middleTop:
                    setMiddleTop(p);

                    break;

                case ShapePoint.middleButtom:
                    setMiddleButtom(p);

                    break;

                case ShapePoint.middleRight:
                    setMiddleRight(p);

                    break;

                case ShapePoint.middleLeft:
                    setMiddleLeft(p);

                    break;
            }
        }

        public CompositeDrawable setFillBrush(Brush br)
        {
            fBrush = br;
            reDraw();
            return this;
        }


        #endregion


        protected virtual void drawCache(Graphics g)//graphics of the parent
        {
            g.DrawImage(this.cacheImg, Xtr(absX), Ytr(absY), absWidth, absHeight);

        }

        public override void Draw(Graphics g)
        {
            if (this.Changed())
            {
				clear();
                dUtility.drawRect(0, 0, absWidth, absHeight, fBrush, bPen, this.g);
                foreach (Drawable drw in drawables)
                {
                    drw.Draw(this.g);
                }
                this.setChanged(false);
            }
            
            drawCache(g);
        }

		/// <summary>
		/// if there is a fillBrush, so no need to clear 
		/// </summary>
		private void clear()
		{
			if (this.fillBrush == null)
			{
				g.Clear(Color.Transparent);
			}
		}

		#region clickable


		public override bool checkPoint(float x0, float y0)
		{
			return
				dUtility.checkPoint(x0, y0, this.absX, this.absY, this.absWidth, this.absHeight);
		}

		public override bool checkRegion(float x0, float y0, float x1, float y1)
		{
			return
				dUtility.checkRegion(x0, y0, x1, y1, this.absX, this.absY, this.absWidth, this.absHeight);
		}

		#endregion

	}


}
