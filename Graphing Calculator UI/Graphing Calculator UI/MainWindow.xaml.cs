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
        static public void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int iValue = 0;

            if (Int32.TryParse(textBox.Text, out iValue) == false)
            {
                TextChange textChange = e.Changes.ElementAt<TextChange>(0);
                int iAddedLength = textChange.AddedLength;
                int iOffset = textChange.Offset;

                textBox.Text = textBox.Text.Remove(iOffset, iAddedLength);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            MyButton.Click += MyButton_Click;
            Graph.Click += Graph_Click;
        }

        private void Graph_Click(object sender, RoutedEventArgs e)
        {
            double xMin = -10.0;
            double xMax = 10.0;

            GraphWindow graphWindow = new GraphWindow(xMin, xMax);
            graphWindow.ShowDialog();
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

