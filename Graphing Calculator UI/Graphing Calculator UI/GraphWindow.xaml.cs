using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Graphing_Calculator_UI;
using MathNet.Symbolics;
using Calculator;

namespace Graphing_Calculator_UI
{
    /// <summary>
    /// Interaction logic for this.xaml
    /// </summary>
    public partial class GraphWindow : Window
    {
        private double xMin;
        private double xMax;
        private double yMin;
        private double yMax;
        private string funcText;

        const double Zoom = 1.05;
        const double Move = .1;

        SolidColorBrush redBrush = new SolidColorBrush();
        
        private void Text(double x, double y, string text, Color color)
        {

            TextBlock textBlock = new TextBlock();

            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleY = -1;
            textBlock.RenderTransform = scaleTransform;
            textBlock.Text = text;
            textBlock.Foreground = new SolidColorBrush(color);
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            this.GraphCanvas.Children.Add(textBlock);
        }

        public int NumberOfPoints
        {
            get { return Convert.ToInt32(this.GraphCanvas.ActualWidth); }
        }

        public double Delta
        {
            get { return (this.xMax - this.xMin) / this.NumberOfPoints; }
        }

        public GraphWindow(double xMin, double xMax, string funcText)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = -(xMax - xMin)/2;
            this.yMax = (xMax - xMin)/2;
            this.funcText = funcText;

            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {


            //DrawGraph(x, y);

        }

        public void DrawCoord()
        {
            GraphTools.AddCoordLine(this.GraphCanvas, 0, this.GraphCanvas.ActualHeight / 2, this.GraphCanvas.ActualWidth, this.GraphCanvas.ActualHeight / 2);
            GraphTools.AddCoordLine(this.GraphCanvas, this.GraphCanvas.ActualWidth/2, 0, this.GraphCanvas.ActualWidth/2, this.GraphCanvas.ActualHeight);
        }

        private void DrawGraph(Point[] x)
        {
            GraphTools.AddPolyLine(GraphCanvas, x);
/*            for (int i = 0; i < x.Length; i++)
            { GraphTools.AddPoint(GraphCanvas, x[i]); }*/
        }

        private List<Point> FillPoints()
        {
            double xMin = this.xMin, yMin = this.yMin, xMax = this.xMax, yMax = this.yMax;
            double xPixel = 2 * (xMax - xMin) / (this.GraphCanvas.ActualWidth);
            double yPixel = 2 * (yMax - yMin) / (this.GraphCanvas.ActualHeight);

            double x = 0;

            var expression = Solver.Parse(this.funcText, new Dictionary<string, VariableNode> { { "x", new VariableNode(() => x) } });
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
            while(pointNode.Value.X <= xMax)
            {
                if (pointNode == pointList.Last)
                {
                    x = pointNode.Value.X + xPixel;
                    pointList.AddLast(new Point(pointNode.Value.X + xPixel, expression.Value));
                    continue;
                }

                double y1 = pointNode.Value.Y, y2 = pointNode.Next.Value.Y;
                double yDiff = y2-y1;
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
                if (double.IsNaN(item.Y))
                { continue; }

                y = item.Y;
                if (double.IsInfinity(y))
                { y = double.MaxValue; }
                else if (double.IsNegativeInfinity(y))
                { y = double.MinValue; }

                AddPoint(item.X, y, returnValue);
            }

            return returnValue;
        }

        private bool IsValid(double d)
        { return !double.IsNaN(d) && !double.IsInfinity(d); }

        private double GetNextPixelValue(double xMin, double xPixel, double curX)
        { return xPixel * Math.Ceiling((curX - xMin) / xPixel); }

        private void AddPoint(double x, double y, List<Point> points)
        {
            points.Add(new Point(
                    GraphTools.CalcCoord(this.xMin, this.xMax, x, this.GraphCanvas.ActualWidth),
                    GraphTools.CalcCoord(this.yMin, this.yMax, y, this.GraphCanvas.ActualHeight)));
        }

        private double GetY(MathNet.Symbolics.Expression expression, Dictionary<string, FloatingPoint> dict)
        {
            var fp = Evaluate.Evaluate(dict, expression);

            if (fp.IsComplex
                || fp.IsUndef)
            { return double.NaN; }

            if (fp.IsPosInf)
            { return double.MaxValue; }
            else if (fp.IsNegInf)
            { return double.MinValue; }

            return fp.RealValue;
        }

        private Point[] CalcCanvasYCoord()
        {
            Point[] y = new Point[this.NumberOfPoints];
            double dy;
            int height = Convert.ToInt32(this.GraphCanvas.ActualHeight);
            int distance = 1;

            var solver = MathNet.Symbolics.Infix.ParseOrUndefined(this.funcText);
            Dictionary<string, FloatingPoint> dict = new Dictionary<string, FloatingPoint>();
            for (int i = 0; i < this.NumberOfPoints; i++)
            {
                double x = this.xMin + this.Delta * i;
                dict["x"] = x;
                var val1 = Evaluate.Evaluate(dict, solver);
                if (val1 == FloatingPoint.PosInf)
                { dy = this.yMax; }
                else if (val1 == FloatingPoint.NegInf)
                { dy = this.yMin; }
                else if (val1.IsUndef)
                { continue; }
                else
                { dy = val1.RealValue; }
                y[i] = new Point(
                    GraphTools.CalcCoord(this.xMin, this.xMax, x, this.GraphCanvas.ActualWidth),
                    GraphTools.CalcCoord(this.yMin, this.yMax, dy, height));
                while (distance >= 1)
                {

                }
            }

            return y;
        }

        public void CanvasRefresh()
        {
            this.GraphCanvas.Children.Clear();
            Point[] points = this.FillPoints().ToArray(); // this.CalcCanvasYCoord();
            this.DrawCoord();
            Text(10, GraphCanvas.ActualHeight / 2, (""+ xMin), Colors.Black);
            Text(GraphCanvas.ActualWidth/2 + 10 , GraphCanvas.ActualHeight-10, (""+ yMax), Colors.Black);
            Text(GraphCanvas.ActualWidth - 30, GraphCanvas.ActualHeight / 2 - 10, (""+ xMax), Colors.Black);
            Text(GraphCanvas.ActualWidth/2 +30, 30, (""+yMin), Colors.Black);
            this.DrawGraph(points);
        }

        private void GraphCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.CanvasRefresh();
        }

        private void ZoomOut()
        {
            var tmp = this.xMin;
            this.xMin -= (GraphWindow.Zoom * (xMax - xMin)) / 2 + xMin;
            this.xMax += (GraphWindow.Zoom * (xMax - tmp)) / 2 + tmp;
            tmp = this.yMin;
            this.yMin -= (GraphWindow.Zoom * (yMax - yMin)) / 2 + yMin;
            this.yMax += (GraphWindow.Zoom * (yMax - tmp)) / 2 + tmp;
        }

        private void ZoomIn()
        {
            var tmp = this.xMin;
            this.xMin -= ((xMax - xMin) / (GraphWindow.Zoom)) / 2 + xMin;
            this.xMax += ((xMax - tmp) / (GraphWindow.Zoom)) / 2 + tmp;
            tmp = this.yMin;
            this.yMin -= ((yMax - yMin) / (GraphWindow.Zoom)) / 2 + yMin;
            this.yMax += ((yMax - tmp) / (GraphWindow.Zoom)) / 2 + tmp;
        }

        private void MoveRight()
        {
            var tmp = this.xMin;
            this.xMin += GraphWindow.Move * (xMax- xMin);
            this.xMax += GraphWindow.Move * (xMax - tmp);
        }

        private void MoveLeft()
        {
            var tmp = this.xMin;
            this.xMin -= GraphWindow.Move * (xMax - xMin);
            this.xMax -= GraphWindow.Move * (xMax - tmp);
        }

        private void MoveUp()
        {
            var tmp = this.yMin;
            this.yMin += GraphWindow.Move * (yMax - yMin);
            this.yMax += GraphWindow.Move * (yMax - tmp);
        }

        private void MoveDown()
        {
            var tmp = this.yMin;
            this.yMin -= GraphWindow.Move * (yMax - yMin);
            this.yMax -= GraphWindow.Move * (yMax - tmp);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemMinus)
            {
                this.ZoomOut();
                this.CanvasRefresh();
            }
            if (e.Key == Key.OemPlus)
            {
                this.ZoomIn();
                this.CanvasRefresh();
            }
            if (e.Key == Key.Up)
            {
                this.MoveUp();
                this.CanvasRefresh();
            }
            if (e.Key == Key.Down)
            {
                this.MoveDown();
                this.CanvasRefresh();
            }
            if (e.Key == Key.Right)
            {
                this.MoveRight();
                this.CanvasRefresh();
            }
            if (e.Key == Key.Left)
            {
                this.MoveLeft();
                this.CanvasRefresh();
            }
        }
    }
}
