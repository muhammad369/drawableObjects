using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace drawableObjects
{
    public class Rect : Shape
    {
      

        public override void Draw(Graphics g)
        {
            if (parent != null)
            {
				dUtility.drawRect( Xtr(absX), Ytr(absY), absWidth, absHeight, fillBrush, strokePen, g);
                drawText(g);
            }
            
        }

        //constructor
        public Rect(float x, float y, float width, float height, Brush fill, Pen border)
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

    
	//===========================================================================


    /// <summary>
    /// a text container changes its size to fit the text
    /// </summary>
    class Text : Rect 
    {
        private int rotationDegree=0;
        //string txt ==> drawable

        public Text(string text, Font fnt,Brush textBrush, float x, float y)
            :base(x, y, 0, 0, null, null)
        {
			this.setText(text, fnt, textBrush);
            
        }

		/// <summary>
		/// only scales position, because sise is useless for text, it is calculated with every draw
		/// </summary>
		internal override void scaleToParent()
		{
			if (parent != null)
			{
				this.absX = parent.absX + ((float)this.Vx * ((float)parent.absWidth / (float)parent.maxX));
				this.absY = parent.absY + ((float)this.Vy * ((float)parent.absHeight / (float)parent.maxY));
			}
		}

        void fitSize(Graphics g)
        {
            SizeF s= dUtility.mesureText(this.text+"l", this.font, g);
            this.absWidth = s.Width;
            this.absHeight = s.Height;
        }


        public Text setRotation(int degrees)
        {
            this.rotationDegree = degrees;
            //
            return this;
        }

        public override void Draw(Graphics g)
        {
            if (parent != null)
            {
                fitSize(g); //fit sise with every draw
                //
				g.TranslateTransform(Xtr(absX), Ytr(absY));
				g.RotateTransform(rotationDegree);
                //back color config exists
                if (fillBrush != null || strokePen != null)
                {
                    dUtility.drawRect(0, 0, absWidth, absHeight, fillBrush, strokePen, g);
                }
                //draw text
                if (txtFormat != null)
                {
                    dUtility.drawText(txt, 0, 0, textBrush, fnt, txtFormat, g);
                }
                else
                {
					dUtility.drawText(txt, 0, 0, textBrush, fnt, g);
                }
                //
                g.ResetTransform();
            }
            
        }

    }
}
