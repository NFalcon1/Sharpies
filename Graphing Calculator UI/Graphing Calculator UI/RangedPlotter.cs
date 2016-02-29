using Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graphing_Calculator_UI
{
    public class RangedPlotter
    {
        double xMin, xMax, yMin, yMax;
        bool dirty = true;
        string equation;
        List<Point> points;
        private Point canvasDimensions;

        public double XMin
        {
            get { return this.xMin; }
            set
            {
                if (this.xMax == value) return;
                this.xMin = value;
                this.dirty = true;
            }
        }

        public double XMax
        {
            get { return this.xMax; }
            set
            {
                if (this.xMax == value) return;
                this.xMax = value;
                this.dirty = true;
            }
        }

        public double YMin
        {
            get { return this.yMin; }
            set { this.yMin = value; }
        }

        public double YMax
        {
            get { return this.yMax; }
            set
            {
                if (this.yMax == value) return;
                this.yMax = value;
                this.dirty = true;
            }
        }

        public Point CanvasDimensions
        {
            get { return this.canvasDimensions; }
            set
            {
                if (this.canvasDimensions == value) return;
                this.canvasDimensions = value;
                this.dirty = true;
            }
        }

        public double YRange
        {
            get { return this.yMax - this.yMin; }
        }

        public double XRange
        {
            get { return this.xMax - this.xMin; }
        }

        public void MoveXBy(double delta)
        {
            this.xMin += delta;
            this.xMax += delta;
        }

        public void MoveLeft()
        { this.MoveXBy(-this.XRange / 3); }

        public void MoveRight()
        { this.MoveXBy(this.XRange / 3); }

        public void MoveYBy(double delta)
        {
            this.yMin += delta;
            this.yMax += delta;
        }

        public void MoveDown()
        { this.MoveXBy(-this.YRange / 3); }

        public void MoveUp()
        { this.MoveXBy(this.YRange / 3); }

        public void ZoomInY()
        {
            double yMean = (yMin + yMax) / 2;
            var yRange = this.YRange / 1.5;
            yMin = yMean - yRange / 2;
            yMax = yMean + yRange / 2;
        }

        public void ZoomInX()
        {
            double xMean = (xMin + xMax) / 2;
            var xRange = this.XRange / 1.5;
            xMin = xMean - xRange / 2;
            xMax = xMean + xRange / 2;
        }

        public void ZoomIn()
        {
            this.ZoomInX();
            this.ZoomInY();
        }

        public string Equation
        {
            get { return this.equation; }
            set
            {
                this.equation = value;
                this.dirty = true;
            }
        }

        public List<Point> GetPoints()
        {
            double xMin = this.xMin, yMin = this.yMin, xMax = this.xMax, yMax = this.yMax;
            double xPixel = 2 * (xMax - xMin) / (this.CanvasDimensions.X);
            double yPixel = 2 * (yMax - yMin) / (this.CanvasDimensions.Y);

            double x = 0;

            var expression = Solver.Parse(this.equation, new Dictionary<string, VariableNode> { { "x", new VariableNode(() => x) } });
            LinkedList<Point> pointList = new LinkedList<Point>();
            LinkedListNode<Point> pointNode = null;

            x = xMin;
            double y = expression.Value;

            pointList.AddLast(new Point(xMin, y));
            pointNode = pointList.First;

            // The logic is pretty simple
            // 1. Add points 1 x pixel apart.
            // 2. If the y difference is greater than 1 yPixel,
            //      Then pick point between these two points and insert yPixel.
            //      Now again compare current point and next point, if the difference is
            //          still greater than 1 y pixel, divide it in half and keep looping.
            //
            //      We use exit points if y is outside of graph or y is invalid value, e.g. NAN, or infinite.
            //
            // Note that we did not have to use any slope in this alogrithm. Neat Right :)c
            while (pointNode.Value.X <= xMax)
            {
                if (pointNode == pointList.Last)
                {
                    x = pointNode.Value.X + xPixel;
                    pointList.AddLast(new Point(pointNode.Value.X + xPixel, expression.Value));
                    continue;
                }

                double y1 = pointNode.Value.Y, y2 = pointNode.Next.Value.Y;
                double yDiff = y2 - y1;
                if ((IsValid(yDiff) && Math.Abs(yDiff) <= yPixel)
                    || (!IsValid(pointNode.Next.Value.Y) && !IsValid(pointNode.Value.Y)))
                {
                    pointNode = pointNode.Next;
                    continue;
                }

                double xDiff = pointNode.Next.Value.X - pointNode.Value.X;
                if ((IsValid(yDiff) && xDiff > (xPixel / 10))
                     && ((y1 <= yMax && y1 >= yMin) || (y2 <= yMax && y2 >= yMin)))
                {
                    x = (pointNode.Value.X + pointNode.Next.Value.X) / 2;
                    pointList.AddAfter(pointNode, new Point(x, expression.Value));
                }
                else
                {
                    pointNode = pointNode.Next;
                }
            }

            List<Point> returnValue = new List<Point>();
            foreach (var item in pointList)
            {
                AddPoint(item.X, item.Y, returnValue);
            }

            this.points = returnValue;
            return this.points;
        }

        private bool IsValid(double d)
        { return !double.IsNaN(d) && !double.IsInfinity(d); }

        private void AddPoint(double x, double y, List<Point> points)
        {
            if (double.IsNaN(y))
            { return; }

            points.Add(new Point(
                    ScaleCordinates(
                        this.xMin,
                        this.xMax,
                        x,
                        this.CanvasDimensions.X),
                    ScaleCordinates(
                        this.yMin,
                        this.yMax,
                        y,
                        this.CanvasDimensions.Y)));
        }

        private double ScaleCordinates(double minValue, double maxValue, double value, double canvasSize)
        {
            if (double.IsInfinity(value)
                || value > maxValue)
            { return canvasSize; }

            return (value - minValue) * canvasSize / (maxValue - minValue);
        }
    }
}
