using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace drawableObjects
{

     public abstract class CollisionResolver
    {
        protected List<Shape> shapes = new List<Shape>();
		//protected HashSet<int> movedIndices = new HashSet<int>();
		//
		public int margin=2;
		public int allowedResolves = 30;

        protected bool checkCollision(Shape s1, Shape s2)
        {
            float xa = s2.x, xz = s2.x + s2.width;
            float ya = s2.y, yz = s2.y + s2.height;
           
            //
            bool xColFlag = false;
            bool yColFlag = false;
            //
            if (xa > s1.x)
            {
                if (xa < s1.x + s1.width)
                {
                    xColFlag = true;
                }
                else // xa < x+width
                {
                    //return;
                }
            }
            else // xa < x
            {
                if (xz > s1.x)
                {
                    xColFlag = true;
                }
                else
                {
                    //return;
                }
            }
            //
            if (ya > s1.y)
            {
                if (ya < s1.y + s1.height)
                {
                    yColFlag = true;
                }
                else // ya < y+height
                {
                    //return;
                }
            }
            else // ya < y
            {
                if (yz > s1.y)
                {
                    yColFlag = true;
                }
                else
                {
                    //return;
                }
            }
            // finally
            return xColFlag && yColFlag;
           
        }

    }


	public class UpDownCollResolver : CollisionResolver
	{
		private float upperBound;
		private float downBound;

		public UpDownCollResolver( float upperBound, float downBound)
		{
			this.upperBound = upperBound;
			this.downBound = downBound;
			
		}

		#region resolve

		/// <summary>
		/// move s1 up to avoid collision with s2
		/// </summary>
		void resolveUp(Shape s1, Shape s2)
		{
			s1.y = s2.y - s1.height - margin;
		}

		/// <summary>
		/// move s1 down to avoid collision with s2
		/// </summary>
		void resolveDown(Shape s1, Shape s2)
		{
			s1.y = s2.y + s2.height + margin;
		}

		/// <summary>
		/// move s1 up to avoid collision with down border
		/// </summary>
		void resolveUp(Shape s1)
		{
			s1.y = downBound - s1.height - margin;
		}

		/// <summary>
		/// move s1 down to avoid collision with up bound
		/// </summary>
		void resolveDown(Shape s1)
		{
			s1.y = upperBound + margin;
		}

		#endregion

		#region add

		public void addUp(Shape s)
		{
			shapes.Insert(0, s);
			checkDown(0);
		}

		public void addDown(Shape s)
		{
			shapes.Add(s);
			checkUp(shapes.Count - 1);
		}

		public void addAtIndex(Shape s, int index)
		{
			shapes.Insert(index, s);
			checkDown(index);
			checkUp(index);
		}

		#endregion

		#region check

		/// <summary>
		/// for the elements put to the bottom of the list, 
		/// typically moves the preceeding element up
		/// </summary>
		void checkUp(int index)
		{
			if (index == 0) //anchor
			{
				if (checkUpperBound(shapes.ElementAt(index)))
				{
					resolveDown(shapes.ElementAt(index));
					checkDown(index);
				}
				return;
			}
			//
			Shape a = shapes.ElementAt(index);
			Shape b = shapes.ElementAt(index - 1);
			//
			if (checkCollision(a, b))
			{
				resolveUp(b, a);
				checkUp(index - 1); //recursion
			}
		}

		void checkDown(int index)
		{
			if (index == shapes.Count-1) //anchor
			{
				if (checkDownBound(shapes.ElementAt(index)))
				{
					resolveUp(shapes.ElementAt(index));
					checkUp(index);
				}
				return;
			}
			//
			Shape a = shapes.ElementAt(index);
			Shape b = shapes.ElementAt(index + 1);
			//
			if (checkCollision(a, b))
			{
				resolveDown(b, a);
				checkDown(index + 1); //recursion
			}
		}

		bool checkUpperBound(Shape s)
		{
			return s.y <= upperBound;
		}

		bool checkDownBound(Shape s)
		{
			return (s.y + s.height) >= downBound;
		}

		#endregion

	}

}
