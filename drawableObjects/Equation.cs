using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace drawableObjects
{
    public interface ParametricEquation
    {

        PointF pointAt(double t);

    }


    class QuadCurveEqu : ParametricEquation
    {
        private double P0_x;
        private double P0_y;
        private double P3_x;
        private double P3_y;
        private double P1_x;
        private double P1_y;
        private double _t2;
        private double _t;
        private double t2;

        public QuadCurveEqu(double P0_x, double P0_y, double P3_x, double P3_y, double P1_x, double P1_y)
        {
            this.P0_x = P0_x;
            this.P0_y = P0_y;
            this.P3_x = P3_x;
            this.P3_y = P3_y;
            this.P1_x = P1_x;
            this.P1_y = P1_y;
        }

        public PointF pointAt(double t)
        {
            _t = 1 - t;
            t2 = Math.Pow(t, 2);
            _t2 = Math.Pow(_t, 2);
            return new PointF(
                    (float)(_t2 * P0_x + (2 * _t * t) * P1_x + t2 * P3_x),
                    (float)(_t2 * P0_y + (2 * _t * t) * P1_y + t2 * P3_y));
        }
    }

    class CubicCurveEqu : ParametricEquation
    {

        private double P0_x;
        private double P0_y;
        private double P3_x;
        private double P3_y;
        private double P1_x;
        private double P1_y;
        private double _t2;
        private double _t;
        private double t2;
        //
        private double P2_x;
        private double P2_y;
        private double t3;
        private double _t3;

        public CubicCurveEqu(double P0_x, double P0_y, double P3_x, double P3_y, double P1_x, double P1_y, double P2_x, double P2_y)
        {
            this.P0_x = P0_x;
            this.P0_y = P0_y;
            this.P3_x = P3_x;
            this.P3_y = P3_y;
            this.P1_x = P1_x;
            this.P1_y = P1_y;
            this.P2_x = P2_x;
            this.P2_y = P2_y;
        }

        public PointF pointAt(double t)
        {
            _t = 1 - t;
            t2 = Math.Pow(t, 2);
            _t2 = Math.Pow(_t, 2);
            t3 = Math.Pow(t, 3);
            _t3 = Math.Pow(_t, 3);
            return new PointF(
                    (float)(_t3 * P0_x + (3 * _t2 * t) * P1_x + (3 * _t * t2) * P2_x + t3 * P3_x),
                    (float)(_t3 * P0_y + (3 * _t2 * t) * P1_y + (3 * _t * t2) * P2_y + t3 * P3_y));
        }
    }

    class Cubic1CurveEqu : ParametricEquation
    {

        private double P0_x;
        private double P0_y;
        private double P3_x;
        private double P3_y;
        private double P1_x;
        private double P1_y;
        private double _t2;
        private double _t;
        private double t2;
        //
        private double P2_x;
        private double P2_y;
        private double t3;
        private double _t3;

        public Cubic1CurveEqu(double P0_x, double P0_y, double P3_x, double P3_y, double P1_x, double P1_y)
        {
            this.P0_x = P0_x;
            this.P0_y = P0_y;
            this.P3_x = P3_x;
            this.P3_y = P3_y;
            this.P1_x = P1_x;
            this.P1_y = P1_y;
           
        }

        public PointF pointAt(double t)
        {
            _t = 1 - t;
            t2 = Math.Pow(t, 2);
            _t2 = Math.Pow(_t, 2);
            t3 = Math.Pow(t, 3);
            _t3 = Math.Pow(_t, 3);
            return new PointF(
                   (float)( _t3 * P0_x + (3 * _t2 * t + 3 * _t * t2) * P1_x + t3 * P3_x),
                   (float)( _t3 * P0_y + (3 * _t2 * t + 3 * _t * t2) * P1_y + t3 * P3_y));
        }
    }

    class CircleSegmentEqu : ParametricEquation
    {

        public PointF pointAt(double t)
        {
            throw new NotImplementedException();
        }
    }

}
