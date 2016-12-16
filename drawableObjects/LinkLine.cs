using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace drawableObjects
{
    
	/// <summary>
	/// links two shapes from the specified points,the start and end points moves with the shapes, 
	/// but optional intermediate points don't
	/// </summary>
    public class LinkLine : Line
    {

        Shape s1, s2;
        ShapePoint lp1, lp2;
        
        public LinkLine(Pen pen, Shape shape1, ShapePoint linkPoint1, 
			Shape shape2, ShapePoint linkPoint2, params DrawablePoint[] intermediatePoints)
            :base(pen)
        {
            s1 = shape1;
            s2 = shape2;
            lp1 = linkPoint1;
            lp2 = linkPoint2;
            //
            points.Add( new DrawablePoint( shape1.getPoint(linkPoint1)) );
			

            for (int i = 0; i < intermediatePoints.Length; i++)
            {
                points.Add( intermediatePoints[i] );
            }

            points.Add( new DrawablePoint( shape2.getPoint(linkPoint2) ));

            //move events
            shape1.onMove += new moveHandler(shape1_onMove);
            shape2.onMove += new moveHandler(shape2_onMove);

        }

        void shape2_onMove(Drawable sender)
        {
            this.setEndPoint(new DrawablePoint( s2.getPoint(lp2) ));
        }

        void shape1_onMove(Drawable sender)
        {
            this.setStartPoint( new DrawablePoint( s1.getPoint(lp1) ));
        }

    }
}
