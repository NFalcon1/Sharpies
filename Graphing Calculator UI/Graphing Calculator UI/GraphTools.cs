using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

// draw graph

namespace Graphing_Calculator_UI
{
    class GraphTools
    {

        /// <summary>
        /// Integration point with Calculator
        /// </summary>
        static public double CalcFromString(string y, double x)
        {
            throw new NotImplementedException("Guys implement me!");
        }

        static public double Calc(double x)
        {
            return x*x*x;
        }
        
        static public double CalcCoord(double min, double max, double val, double coef)
        {
            return (val - min) * coef / (max - min);
        }

        static public double MiN(double[] y, int size)
        {
            return 0;
        }

        static public double MaX(double[] y, int size)
        {
            return 100;
        }

        static public void AddLine(Canvas canvas, double x1, double y1, double x2, double y2)
        {
            // Create a Line
            Line line = new Line();

            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;

            // Create a red Brush
            SolidColorBrush redBrush = new SolidColorBrush();
            redBrush.Color = Colors.Red;

            // Set Line's width and color
            line.StrokeThickness = 2;
            line.Stroke = redBrush;

            // Add line to the Grid.
            canvas.Children.Add(line);
        }
        static public void AddCoordLine(Canvas canvas, double x1, double y1, double x2, double y2)
        {
            // Create a Line
            Line line = new Line();

            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;

            // Create a red Brush
            SolidColorBrush redBrush = new SolidColorBrush();
            redBrush.Color = Colors.Black;

            // Set Line's width and color
            line.StrokeThickness = 2;
            line.Stroke = redBrush;

            // Add line to the Grid.
            canvas.Children.Add(line);
        }

    }
}
