using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace drawableObjects
{
    public class Pie : Shape
    {
        
		
        public override void Draw(Graphics g)
        {
            if (parent != null)
            {
				dUtility.drawPie(Xtr(absX), Ytr(absY), absWidth, absHeight,
                    startAngle, sweepAngle,fillBrush, strokePen, g);
                drawText(g);//TODO: override
            }

        }

        float startAngle, sweepAngle;

        //constructor
        public Pie(float x, float y, float width, float height, Brush fill, Pen border, 
            float startAng, float sweepAng)
        {

            this.Vx = x;
            this.Vy = y;
            this.Vw = width;
            this.Vh = height;
            this.fillBrush = fill;
            this.bPen = border;
            //
            this.startAngle = startAng;
            this.sweepAngle = sweepAng;
            //
        }

        //TODO: override clickable methods

    }
}
