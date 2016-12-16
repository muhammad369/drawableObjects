using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace drawableObjects
{

	public class Layer : CompositeDrawable
	{

		//constructor
		/// <summary>
		/// virtual and real(abs) w,h are the same as viewport img
		/// </summary>
		public Layer(int maxX, int maxY, DrawingController dc)
			: base(0, 0, 0, 0)
		{
			//Vx = Vy = 0;
			//
			this.maxX = maxX;
			this.maxY = maxY;
			//
			dc.add(this);
			//
			scaleToParent();
			//
			Vw = absWidth;
			Vh = absHeight;
			//
			initCache();
		}


		#region clickable methods
		//the layer is supposed to cover the entire viewport, so any click will hit

		public override bool checkPoint(float x, float y)
		{
			return true;
		}

		public override bool checkRegion(float x1, float y1, float x2, float y2)
		{
			return true;
		}

		#endregion


		internal override void scaleToParent()
		{
			absX = 0;
			absY = 0;
            absWidth = dc.Width() -1;
            absHeight = dc.Height() -1;
		}

        internal override float scaleBackX(float absX)
        {
			return absX * maxX / absWidth;
        }

        internal override float scaleBackY(float absY)
        {
			return absY * maxY / absHeight;
        }

	}

}
