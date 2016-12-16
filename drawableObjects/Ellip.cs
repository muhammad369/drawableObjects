using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace drawableObjects
{
    public class Ellip : Shape
    {
   

        public override void Draw(Graphics g)
        {
            if (parent != null)
            {
				dUtility.drawCircle(Xtr(absX), Ytr(absY), absWidth, absHeight, fillBrush, strokePen, g);
                drawImg(g);
                drawText(g);
            }
            
        }

        //constructor
        public  Ellip(float x, float y, float width, float height, Brush fill, Pen border)
        {

            this.Vx = x;
            this.Vy = y;
            this.Vw = width;
            this.Vh = height;
            this.fillBrush = fill;
            this.bPen = border;
            //
        }

        
    }
    
    
}
