using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;


namespace drawableObjects
{

	public abstract class Shape : Drawable
	{


		#region fields
        protected float Vw, Vh, Vx, Vy; //virtual width, and virtual height
		protected float absX, absY, absWidth, absHeight; //the real values
		protected Brush fillBrush;
		#endregion


        #region shape methods


        internal override void scaleToParent()
        {
            if (parent != null)
            {
                this.absWidth = ((parent.absWidth / parent.maxX) * this.Vw);
                this.absHeight = ((parent.absHeight / parent.maxY) * this.Vh);
                //
                this.absX = parent.absX + (this.Vx * (parent.absWidth / parent.maxX));
                this.absY = parent.absY + (this.Vy * (parent.absHeight / parent.maxY));
            }
        }

		internal override float scaleBackX(float absX)
		{
			return (absX - parent.absX) * (parent.maxX / parent.absWidth)-Vx;
		}

		internal override float scaleBackY(float absY)
		{
			return (absY - parent.absY) * (parent.maxY / parent.absHeight)-Vy;
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

		public Brush FillBrush
		{
			get
			{
				return fillBrush;
			}
			set
			{
				fillBrush = value;
				reDraw();
			}
		}

        /// <summary>
        /// sets the virtual center
        /// </summary>
		public Shape setCenter(PointF c)
		{
			this.Vx = (c.X - (Vw / 2));
			this.Vy = (c.Y - (Vw / 2));
            scaleToParent();
			update();
			fireMove(this);
			//
			return this;
		}

		//TODO: all setters return instance

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
			this.Vx = (p.X - (Vw / 2));
			this.Vy = (p.Y);
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

        public Shape setFillBrush(Brush br)
        {
            fillBrush = br;
            reDraw();
            return this;
        }


        #endregion

        #region draw text

        /// <summary>
        /// the default way any shape may draw it's text, it checks if text is null, 
        /// and uses the custom stringFormat if exists
        /// </summary>
        protected void drawText(Graphics g)
        {
            if (this.txt != null)
            {
                if (this.txtFormat == null)
                {
					dUtility.drawTextInRegion(this.txt, Xtr(absX), Ytr(absY), absWidth, absHeight, tbrsh, fnt, g);
                }
                else
                {
					dUtility.drawTextInRegion(this.txt, Xtr(absX), Ytr(absY), absWidth, absHeight, tbrsh, fnt, txtFormat, g);
                }
            }
        }

        #endregion

		#region gradient fill
		/// <summary>
		/// setting a gradient fill requires determining the rectangle area to apply on, so this special 
		/// function is for, this method has to be recalled every time any change in size takes place, 
		/// must be assined to a parent prior to calling, because it uses abs dimensions
		/// </summary>
		public Shape setLinearGradientFill(Color clr1, Color clr2, float angle)
		{
			this.fillBrush = new LinearGradientBrush(new RectangleF( Xtr(absX)-0.5f, Ytr(absY), absWidth+1, absHeight), 
				clr1, clr2, angle);
			//
			return this;
		}

		#endregion

	
        #region draw image

        protected void drawImg(Graphics g)
        {
            if (this.img != null)
            {
                 dUtility.drawImg(this.img, Xtr(absX), Ytr(absY), absWidth, absHeight, g);
            }
        }

        #endregion

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

	public enum ShapePoint { center, middleTop, middleButtom, middleRight, middleLeft }

}
