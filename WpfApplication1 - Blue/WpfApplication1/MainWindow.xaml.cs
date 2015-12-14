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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Visibility extraButton;
        public MainWindow()
        {
            InitializeComponent();
            extraButton = System.Windows.Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void changeblue1_Click(object sender, RoutedEventArgs e)
        {
            _0.Background = Brushes.Blue;
        }

        private void extra_Click(object sender, RoutedEventArgs e)
        {
            if (extraButton == System.Windows.Visibility.Hidden)
            {
                Y.Visibility = System.Windows.Visibility.Visible;
                graph.Visibility = System.Windows.Visibility.Visible;
                table.Visibility = System.Windows.Visibility.Visible;
                oleg.Visibility = System.Windows.Visibility.Visible;
                sin.Visibility = System.Windows.Visibility.Visible;
                cos.Visibility = System.Windows.Visibility.Visible;
                log.Visibility = System.Windows.Visibility.Visible;
                Ln.Visibility = System.Windows.Visibility.Visible;
                tan.Visibility = System.Windows.Visibility.Visible;
                extraButton = System.Windows.Visibility.Visible;
            }
            else
            {
                Y.Visibility = System.Windows.Visibility.Hidden;
                graph.Visibility = System.Windows.Visibility.Hidden;
                table.Visibility = System.Windows.Visibility.Hidden;
                oleg.Visibility = System.Windows.Visibility.Hidden;
                sin.Visibility = System.Windows.Visibility.Hidden;
                cos.Visibility = System.Windows.Visibility.Hidden;
                tan.Visibility = System.Windows.Visibility.Hidden;
                log.Visibility = System.Windows.Visibility.Hidden;
                Ln.Visibility = System.Windows.Visibility.Hidden;
                extraButton = System.Windows.Visibility.Hidden;
                
            }
            
        }

        private void Y_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ans_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
