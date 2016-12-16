using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace drawableObjects
{
	public class DrawablePoint
	{
		public float Vx, Vy;
		public float absX, absY;
		public float trnsX, trnsY;

		public DrawablePoint(float x, float y)
		{
			this.Vx = x;
			this.Vy = y;
		}

		public DrawablePoint(Point p)
		{
			this.Vx = p.X;
			this.Vy = p.Y;
		}

		public DrawablePoint(PointF p)
		{
			this.Vx = (int)p.X;
			this.Vy = (int)p.Y;
		}

		public DrawablePoint scaleToParent(CompositeDrawable parent)
		{
			trnsX = (int)((float)Vx * ((float)parent.absWidth / (float)parent.maxX));
			absX = parent.absX + trnsX;
			//
			trnsY = (int)((float)Vy * ((float)parent.absHeight / (float)parent.maxY));
			absY = parent.absY + trnsY;
			//
			return this;
		}

		#region point and pointF


		public PointF AbsolutePoint()
		{
			return new PointF(absX, absY);
		}

		


		public PointF VirtualPoint()
		{
			return new PointF(Vx, Vy);
		}
		
		#endregion

	}
}
