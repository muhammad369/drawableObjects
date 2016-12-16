using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace drawableObjects
{

    public enum corner { None, UpRight, UpLeft, DownRight, DownLeft }

    public class dUtility
    {
        //
        public Brush ONbrush = new SolidBrush(Color.FromArgb(68, 131, 183));
        public Brush OFFbrush = new SolidBrush(Color.FromArgb(255, 154, 102));
        public Pen bluePen = new Pen(Color.FromArgb(33, 41, 147));
        public Brush whiteBrush = new SolidBrush(Color.White);
        public Font tFont = new Font("arial", 10, FontStyle.Regular);
        
        //
        public Pen whitePen = new Pen(Color.White);
        //
        Random r = new Random();
        public Color randomColor()
        {
            return Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
        }

        public Brush randomBrush()
        {
            return new SolidBrush(randomColor());
        }

        public Brush randomDarkBrush()
        {
            return new SolidBrush(randomDarkColor());
        }

        private Color randomDarkColor()
        {
            return Color.FromArgb(r.Next(100), r.Next(50), 100);
        }

        public static PointF add( PointF a, PointF b)
        {
            return new PointF(a.X + b.X, a.Y + b.Y);
        }

        /// <param name="angle">angle in degrees</param>
        /// <returns></returns>
        public corner angleCorner(float angle)
        {
            if (angle >= 0 && angle < 90)
            {
                return corner.DownRight;
            }
            else if (angle >= 90 && angle < 180)
            {
                return corner.DownLeft;
            }
            else if (angle >= 180 && angle < 270)
            {
                return corner.UpLeft;
            }
            else if (angle >= 270 && angle <= 360)
            {
                return corner.UpRight;
            }
            return corner.None;
		}



		#region drawing functions

        public static StringFormat defaultStringFormat = new StringFormat() 
        {
            Alignment= StringAlignment.Center,
            LineAlignment= StringAlignment.Center
        };

		public static void drawRect(float x, float y, float w, float h, Brush fill, Pen stroke, Graphics g)
		{
            if (fill != null)
            {
                g.FillRectangle(fill, x, y, w, h);
            }
            if (stroke == null) return;
			g.DrawRectangle(stroke, x, y, w, h);
		}

		public static void drawCircle(float x, float y, float w, float h, Brush fill, Pen stroke, Graphics g)
		{
            if (fill != null)
            {
                g.FillEllipse(fill, x, y, w, h);
            }
            if (stroke == null) return;
			g.DrawEllipse(stroke, x, y, w, h);
		}

		public static void drawPie(float x, float y, float w, float h, 
			float startAngle, float sweepAngle, Brush fill, Pen stroke, Graphics g)
		{
            if (fill != null)
            {
                g.FillPie(fill, x, y, w, h, startAngle, sweepAngle);
            }
            if (stroke == null) return;
			g.DrawPie(stroke, x, y, w, h, startAngle, sweepAngle);
		}

		public static void drawPolygon(PointF[] points, Brush fill, Pen stroke, Graphics g)
		{
            if (fill != null)
            {
                g.FillPolygon(fill, points);
            }
            if (stroke == null) return;
			g.DrawPolygon(stroke, points);
		}

		public static void drawLine(float x1, float y1, float x2, float y2, Pen stroke, Graphics g)
		{
			g.DrawLine(stroke, x1, y1, x2, y2);
		}
		
		public static void drawLine(PointF[] points, Pen stroke, Graphics g)
		{
			g.DrawLines(stroke, points);
		}

		public static void drawText(string text,float x, float y, Brush fill,Font font, Graphics g)
		{
			g.DrawString(text, font, fill, x, y);
		}

        public static void drawText(string text, float x, float y, Brush fill, Font font,StringFormat sf , Graphics g)
        {
            
            g.DrawString(text, font, fill, x, y, sf);
        }

        public static void drawTextInRegion(string text, float x, float y,float w, float h, Brush fill, Font font, StringFormat sf, Graphics g)
        {
            g.DrawString(text, font, fill, new RectangleF(x, y, w, h), sf);
        }

        public static void drawTextInRegion(string text, float x, float y, float w, float h, Brush fill, Font font, Graphics g)
        {
            g.DrawString(text, font, fill, new RectangleF(x, y, w, h), defaultStringFormat);
        }

        public static void drawImg(Image img, float x, float y, float w, float h,Graphics g)
        {
            g.DrawImage(img, x, y, w, h);
        }

		public static SizeF mesureText(string text, int width,Font font, Graphics g)
		{
			return g.MeasureString(text, font, width); //formating for line spacing etc
		}

        public static SizeF mesureText(string text, Font font, Graphics g)
        {
            return g.MeasureString(text, font); //formating for line spacing etc
        }

		#endregion

		#region mouse-events-check


		public static bool checkPoint(float x0, float y0, float x, float y, float width, float height)
		{
			if (x0 > x && x0 < x + width && y0 > y && y0 < y + height)
			{
				return true;
			}
			return false;
		}

		public static bool checkRegion(float x0, float y0, float x1, float y1,
								float x, float y, float width, float height)
		{
			float xa, xz, ya, yz;
			if (x0 > x1)
			{
				xa = x1;
				xz = x0;
			}
			else
			{
				xa = x0;
				xz = x1;
			}
			//
			if (y0 > y1)
			{
				ya = y1;
				yz = y0;
			}
			else
			{
				ya = y0;
				yz = y1;
			}
			//
			bool xflag = false;
			bool yflag = false;
			//
			if (xa > x)
			{
				if (xa < x + width)
				{
					xflag = true;
				}
				else // xa < x+width
				{
					return false;
				}
			}
			else // xa < x
			{
				if (xz > x)
				{
					xflag = true;
				}
				else
				{
					return false;
				}
			}
			//
			if (ya > y)
			{
				if (ya < y + height)
				{
					yflag = true;
				}
				else // ya < y+height
				{
					return false;
				}
			}
			else // ya < y
			{
				if (yz > y)
				{
					yflag = true;
				}
				else
				{
					return false;
				}
			}
			// finally
			if (xflag && yflag)
			{
				return true;
			}
			return false;
		}


		#endregion

        #region graphics and img

        public static Graphics getImageGraphics(Image img)
        {
            Graphics g = Graphics.FromImage(img);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			
            //
            return g;
        }

        
		#endregion

		#region Color spaces

		/// <summary>
		/// gives a color with its Hue dependent on a value from 0 to 1, 
		/// with a fixed saturation and illumination(value), the max hue is 100, 
		/// so it maps from red to green, 
		/// illumination takes values from 0 to 1
		/// </summary>
		public static Color mappedColor(double mapValue, double illumination)
		{
			return HsvColor(mapValue * 100, 1, illumination);
		}

		public static Color mapFairColor(double mapValue)
		{
			return mappedColor(mapValue, 1);
		}

		public static Brush mapFairBrush(double mapValue)
		{
			return new SolidBrush(mapFairColor(mapValue));
		}

		public static Color mapDarkColor(double mapValue)
		{
			return mappedColor(mapValue, .5);
		}

		public static Brush mapDarkBrush(double mapValue)
		{
			return new SolidBrush( mapDarkColor(mapValue));
		}

		//______________________________________________________________


        public static Color HsvColor(double h, double S, double V)
        {
            

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            int r = Clamp((int)(R * 255.0));
            int g = Clamp((int)(G * 255.0));
            int b = Clamp((int)(B * 255.0));
            //
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

        #endregion


    }

}
