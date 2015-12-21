using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Calculator
{
    public sealed partial class MainPage : Page
    {
        int shift = 0;

        public MainPage()
        {
            this.InitializeComponent();
            this.DisplayTopText.Text = "";
            this.DisplayTopText_Copy.Text = "0";
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += ".";
        }

        private void Zero_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "0";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void One_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "1";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Two_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "2";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Three_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "3";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Four_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "4";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Five_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "5";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Six_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "6";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Seven_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "7";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Eight_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "8";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Nine_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "9";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += this.DisplayTopText_Copy;
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "+";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "-";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Times_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "·";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Divide_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "÷";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Cos_Click(object sender, RoutedEventArgs e)
        {
            if (shift == 1)
            {
                this.DisplayTopText.Text += "acos ";
                shift = 0;
            }
            else
            {
                this.DisplayTopText.Text += "cos ";
            }
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Sin_Click(object sender, RoutedEventArgs e)
        {
            if (shift == 1)
            {
                this.DisplayTopText.Text += "asin ";
                shift = 0;
            }
            else
            {
                this.DisplayTopText.Text += "sin ";
            }
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Tan_Click(object sender, RoutedEventArgs e)
        {
            if (shift == 1)
            {
                this.DisplayTopText.Text += "atan ";
                shift = 0;
            }
            else
            {
                this.DisplayTopText.Text += "tan ";
            }
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void ln_Click(object sender, RoutedEventArgs e)
        {
            if (shift == 1)
            {
                this.DisplayTopText.Text += "aln ";
                shift = 0;
            }
            else
            {
                this.DisplayTopText.Text += "ln ";
            }
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void log_Click(object sender, RoutedEventArgs e)
        {
            if (shift == 1)
            {
                this.DisplayTopText.Text += "alog ";
                shift = 0;
            }
            else
            {
                this.DisplayTopText.Text += "log ";
            }
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "√";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Exponents_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "^";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Shift_Click(object sender, RoutedEventArgs e)
        {
            shift = 1;
        }

        private void AllClear_Click(object sender, RoutedEventArgs e)
        {
            this.InitializeComponent();
            this.DisplayTopText.Text = "";
        }

        private void StartingBracket_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "(";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void EndingBracket_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += ")";
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            double answer = Solver.Solve(this.DisplayTopText.Text);
            this.DisplayTopText_Copy.Text = answer.ToString();
            this.DisplayTopText.Text = "";
        }
    }
}
