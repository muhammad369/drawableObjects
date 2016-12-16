using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace drawableObjects
{
	/*	  for the case when assigning width
	 * _______________________________________________
	 * 	  Vw  >===(scaleToParent)=====>   absW
	 * 									   ||
	 * 									   ()
	 * 									   ||
	 * 									   \/
	 * 	  Vh <====(scaleBack)========<    absH
	 */   

	/// <summary>
	/// a box but with equal absWidth and absHeight
	/// </summary>
	public class Square : Rect
	{
		bool WorH;
		public Square(float x, float y, float side, bool WorH, Brush fill, Pen border)
			: base(x, y, WorH? side:0, WorH? 0:side, fill, border)
		{
			this.WorH = WorH;
		}

		internal override void scaleToParent()
		{
			base.scaleToParent();
			//
			if (WorH)
			{
				absHeight = absWidth;
				Vh = scaleBackY(absHeight);
			}
			else
			{
				absWidth = absHeight;
				Vw = scaleBackX(absWidth);
			}
		}

	}
}
