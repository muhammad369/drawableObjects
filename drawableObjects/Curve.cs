using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace drawableObjects
{
    public abstract class Curve : Drawable
    {

        public Curve(Pen p)
        {
            this.bPen = p;
        }

		public void setPen(Pen p)
		{
			this.bPen = p;
			reDraw();
		}


	}


    public class Line : Curve
    {
		/// <summary>
		/// the virtual points
		/// </summary>
        protected List<DrawablePoint> points = new List<DrawablePoint>();

        public Line(Pen p,params DrawablePoint[] points)
            : base(p)
        {
            this.points.AddRange(points);
			
        }

		public Line(Pen p, int x1, int y1, int x2, int y2)
			: base(p)
		{
			this.points.Add(new DrawablePoint(x1, y1));
			this.points.Add(new DrawablePoint(x2, y2));
		}

		public Line(Pen p, float x1, float y1, float x2, float y2)
			: base(p)
		{
			this.points.Add(new DrawablePoint(x1, y1));
			this.points.Add(new DrawablePoint(x2, y2));
		}

        /// <summary>
        /// polar points will be calculated with respect to the last point, 
        /// you can say polar movement
        /// </summary>
        public Line(Pen p, DrawablePoint startPoint, params PolarPoint[] points)
            : base(p)
        {
            DrawablePoint lastPoint=startPoint;
            this.points.Add(startPoint);
			
            foreach (PolarPoint item in points)
            {
                this.points.Add( add( item.toCartesian(),  lastPoint) );
				
            }
        }

        //public Line(Pen pen) : base(pen) { }


		DrawablePoint add(PointF a, DrawablePoint b)
        {
			return new DrawablePoint(a.X + b.Vx, a.Y + b.Vy);
        }


        public DrawablePoint getEndPoint()
        {
            return points.Last();
        }

		//
		//setters

		public Line addPoint(DrawablePoint pnt)
		{
			this.points.Add(pnt);
			if (this.parent != null)
			{
				pnt.scaleToParent(this.parent);
				update();
				fireMove(this);
			}
			//
			return this;
		}

		public Line setPoint(int index, float x, float y)
		{
			var pnt = points[index];
			pnt.Vx = x; pnt.Vy = y;

			if (this.parent != null)
			{
				pnt.scaleToParent(this.parent);
				update();
				fireMove(this);
			}
			//
			return this;
		}

        public void setStartPoint(DrawablePoint p)
        {
            this.points[0] = p;
			scaleToParent();
            update();
            fireMove(this);
        }

		public void setEndPoint(DrawablePoint p)
        {
            this.points[points.Count-1] = p;
			scaleToParent();
            update();
            fireMove(this);
        }


		public override void Draw(Graphics g)
		{
			g.DrawLines(
				bPen, 
				points.Select(p=> new PointF(p.trnsX, p.trnsY)).ToArray()
				);
		}

		internal override void scaleToParent()
		{
			if (parent == null) return;
			//
			foreach (DrawablePoint drP in points)
			{
				drP.scaleToParent(parent);
			}
		}

		#region clickable - not implemented
		//TODO: implement

		public override bool checkPoint(float x0, float y0)
		{
			throw new NotImplementedException();
		}

		public override bool checkRegion(float x0, float y0, float x1, float y1)
		{
			throw new NotImplementedException();
		}

		#endregion

	}



    public struct PolarPoint
    {
        public double r, th;

        public static PolarPoint fromCartesian(PointF cp)
        {
            return new PolarPoint( Math.Sqrt(Math.Pow(cp.X,2)+Math.Pow(cp.Y,2)) ,RadToDeg( Math.Atan2(cp.Y, cp.X) ) );
        }

        public static double RadToDeg(double rad)
        {
            return rad * (180/Math.PI);  
        }

        public static double DegToRad(double deg )
        {
            return deg * (Math.PI/180);
        }

        /// <summary>
        /// initializes a new polar point, with length and angle
        /// </summary>
        /// <param name="r">the length from the origin to the point</param>
        /// <param name="th">the angle with the positive x coordinate in degrees</param>
        public PolarPoint(double r,double th)
        {
            this.r = r;
            this.th = th;
        }

       
		public PointF toCartesian()
		{
			return new PointF((float)(r * Math.Cos(DegToRad(th))), (float)(r * Math.Sin(DegToRad(th))));
		}

    }

    

}
