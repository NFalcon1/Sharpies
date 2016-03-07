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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Graphing_Calculator_UI;

namespace Graphing_Calculator_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RangedPlotter rangedPlotter = new RangedPlotter();
        const double Zoom = 1.05;
        const double Move = .1;
        bool mouseTrack = false;
        Point lastPosition;

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

        public MainWindow()
        {
            this.rangedPlotter.XMin = -100;
            this.rangedPlotter.XMax = 100;
            this.rangedPlotter.YMin = -100;
            this.rangedPlotter.YMax = 100;
            this.rangedPlotter.Equation = string.Empty;
            InitializeComponent();
        }

        public void DrawCoord()
        {
            GraphTools.AddCoordLine(this.GraphCanvas, 0, this.GraphCanvas.ActualHeight / 2, this.GraphCanvas.ActualWidth, this.GraphCanvas.ActualHeight / 2);
            GraphTools.AddCoordLine(this.GraphCanvas, this.GraphCanvas.ActualWidth / 2, 0, this.GraphCanvas.ActualWidth / 2, this.GraphCanvas.ActualHeight);
        }

        public void CanvasRefresh()
        {
            this.GraphCanvas.Children.Clear();
            this.rangedPlotter.CanvasDimensions = new Point(this.GraphCanvas.ActualWidth, this.GraphCanvas.ActualHeight);
            this.DrawCoord();

            Text(GraphCanvas.ActualWidth / 2 + 10, GraphCanvas.ActualHeight - 5, ("" + this.rangedPlotter.YMax), Colors.Black);
            Text(GraphCanvas.ActualWidth / 2 + 10, 26, ("" + this.rangedPlotter.YMin), Colors.Black);
            Text(GraphCanvas.ActualWidth - 30, GraphCanvas.ActualHeight / 2 - 10, ("" + this.rangedPlotter.XMax), Colors.Black);
            Text(10, GraphCanvas.ActualHeight / 2 - 10, ("" + this.rangedPlotter.XMin), Colors.Black);

            List<List<Point>> points = rangedPlotter.GetPoints();
            foreach (var item in points)
            { GraphTools.AddPolyLine(this.GraphCanvas, item); }
        }

        public void TextChanged(object sender, TextChangedEventArgs e)
        {
            this.rangedPlotter.Equation = this.FunctionBox.Text;
            this.CanvasRefresh();
        }

        private void ZoomOut()
        {
            this.rangedPlotter.ZoomOut();
        }

        private void ZoomIn()
        { this.rangedPlotter.ZoomIn(); }

        private void MoveRight()
        { this.rangedPlotter.MoveRight(); }

        private void MoveLeft()
        { this.rangedPlotter.MoveLeft(); }

        private void MoveUp()
        { this.rangedPlotter.MoveUp(); }

        private void MoveDown()
        { }

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

        private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.rangedPlotter.CanvasDimensions = new Point(this.GraphCanvas.ActualWidth, this.GraphCanvas.ActualHeight);
            this.CanvasRefresh();
        }

        private void GraphCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.lastPosition = e.GetPosition(this.GraphCanvas);
            mouseTrack = true;
        }

        private static double GetReciprocalScale(double increaseBy)
        {
            if (increaseBy < 0)
            { increaseBy = -1 + (1 / (1 - increaseBy)); }

            return increaseBy;
        }

        private void GraphCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta == 0)
            { return; }

            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
            { this.rangedPlotter.ZoomX(1 + GetReciprocalScale(Math.Atan(e.Delta / 100) / Math.PI)); }
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
            { this.rangedPlotter.ZoomY(1 + GetReciprocalScale(Math.Atan(e.Delta / 100) / Math.PI)); }
            this.CanvasRefresh();
        }

        private void GraphCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.mouseTrack = false;
        }

        private void GraphCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mouseTrack)
            {
                var newPos = e.GetPosition(this.GraphCanvas);
                var delta = this.lastPosition - newPos;
                this.lastPosition = newPos;
                
                this.rangedPlotter.MoveXBy(delta.X * (this.rangedPlotter.XMax - this.rangedPlotter.XMin) / this.GraphCanvas.ActualWidth);
                this.rangedPlotter.MoveYBy(delta.Y * (this.rangedPlotter.YMax - this.rangedPlotter.YMin) / this.GraphCanvas.ActualHeight);

                this.CanvasRefresh();
            }
        }
    }
}
