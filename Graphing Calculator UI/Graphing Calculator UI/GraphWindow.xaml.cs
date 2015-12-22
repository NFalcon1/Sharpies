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

        const double Zoom = 5;

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

        public GraphWindow(double xMin, double xMax)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = -(xMax - xMin)/2;
            this.yMax = (xMax - xMin)/2;

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

        private void DrawGraph(double[] x, double[] y)
        {

            for (int i = 0; i + 1 < this.GraphCanvas.ActualWidth; i++)
            {
                GraphTools.AddLine(GraphCanvas, x[i], y[i], x[i + 1], y[i + 1]);
            }

        }

        private double[] CalcCanvasYCoord()
        {
            double[] y = new double[this.NumberOfPoints];
            double dy;
            int height = Convert.ToInt32(this.GraphCanvas.ActualHeight);

            for (int i = 0; i < this.NumberOfPoints; i++)
            {
                // uncoment once GraphTools.CalcFromString is ready 
                // y[i] = GraphTools.CalcFromString(this.FunctionBox.Text, xMin + xDelta * i);
                dy = GraphTools.Calc(this.xMin + this.Delta * i);
                y[i] = GraphTools.CalcCoord(this.yMin, this.yMax, dy, height);
            }

            return y;
        }

        private double[] CalcCanvasXCoord()
        {
            double[] x = new double[this.NumberOfPoints];

            for (int i = 0; i < this.NumberOfPoints; i++)
            {
                x[i] = GraphTools.CalcCoord(xMin, xMax, xMin + this.Delta * i, this.NumberOfPoints);
            }

            return x;
        }

        public void CanvasRefresh()
        {
            double[] x = this.CalcCanvasXCoord();
            double[] y = this.CalcCanvasYCoord();
            this.DrawCoord();
            Text(10, GraphCanvas.ActualHeight / 2, (""+ xMin), Colors.Black);
            Text(GraphCanvas.ActualWidth/2 + 10 , GraphCanvas.ActualHeight-10, (""+ yMax), Colors.Black);
            Text(GraphCanvas.ActualWidth - 30, GraphCanvas.ActualHeight / 2 - 10, (""+ xMax), Colors.Black);
            Text(GraphCanvas.ActualWidth/2 +30, 30, (""+yMin), Colors.Black);
            this.DrawGraph(x, y);
        }

        private void GraphCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.CanvasRefresh();
        }

        private void ZoomOut()
        {
            this.xMin -= GraphWindow.Zoom;
            this.xMin += GraphWindow.Zoom;
            this.yMin -= GraphWindow.Zoom;
            this.yMax += GraphWindow.Zoom;
        }

        private void ZoomIn()
        {
            this.xMin += GraphWindow.Zoom;
            this.xMin -= GraphWindow.Zoom;
            this.yMin += GraphWindow.Zoom;
            this.yMax -= GraphWindow.Zoom;
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
        }
    }
}
