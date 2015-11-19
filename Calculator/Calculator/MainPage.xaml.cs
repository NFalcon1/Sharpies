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
    class NumOrString
    {
        public int? Int;
        public double? Double;
        public string String = string.Empty;
        public string Char;

        public void AddCharacter(char ch)
        {
            if (ch == '.')
            {
                if (this.Double.HasValue)
                {
                    throw new Exception("Can't add multiple decimals");
                }

                if (this.Int.HasValue)
                {
                    this.Double = this.Int.Value;
                    this.Int = null;
                }
                else
                {
                    this.Double = 0.0;
                }

                this.String += ".";
            }
            else if (ch >= '0' && ch <= '9')
            {
                this.String += ch;
                if (this.Int.HasValue)
                {
                    this.Int = this.Int.Value * 10 + (ch - '0');
                }
                else
                {
                    this.Double = double.Parse(this.String);
                }
            }
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int numOfBrackets = 0;
        int endingBracket = 0;
        int itemNumberCheck = 0;
        int shift = 0;
        int item = 0;

        List<NumOrString> bracketSolver = new List<NumOrString>();
        List<string> dataSolver = new List<string>();
        List<string> complexList = new List<string>
        (
            new string[]
            {
                "tan ",
                "atan ",
                "sin ",
                "asin ",
                "cos ",
                "acos ",
                "log ",
                "10^ ",
                "ln",
                "e^ ",
                "^",
                "√"

            }
        );
        List<string> simpleList = new List<string>
        (
            new string[]
            {
                "+",
                "-",
                "*",
                "÷"
            }
        );
        List<string> numberList = new List<string>
        (
            new string[]
            {
                ".",
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9"
            }
        );


        public MainPage()
        {
            this.InitializeComponent();
            this.DisplayTopText.Text = string.Empty;
            int numOfBrackets = 0;
            int endingBracket = 0;
            int itemNumberCheck = 0;
            int shift = 0;
            int item = 1;
            List<NumOrString> bracketSolver = new List<NumOrString>();
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += ".";
            dataSolver[item] += '.';
            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void Zero_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "0";
            dataSolver[item] += '0';

            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void One_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "1";
            dataSolver[item] += '1';
            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void Two_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "2";
            dataSolver[item] += '2';
            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void Three_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "3";
            dataSolver[item] += '3';
            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void Four_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "4";
            dataSolver[item] += '4';
            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void Five_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "5";
            dataSolver[item] += '5';
            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void Six_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "6";
            dataSolver[item] += '6';
            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void Seven_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "7";
            dataSolver[item] += '7';
            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void Eight_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "8";
            dataSolver[item] += '8';
            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void Nine_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "9";
            dataSolver[item] += '9';
            if (dataSolver.Count != item + 1)
            {
                dataSolver.Add(null);
            }
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            //this.DisplayTopText.Text += ".";
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "+";

            if (endingBracket == 2)
            {
                dataSolver[dataSolver.Count - 1] = ")";
            }
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "-";
        }

        private void Times_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "·";
        }

        private void Divide_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "÷";
        }

        private void Cos_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "cos ";
        }

        private void Sin_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "sin ";
        }

        private void Tan_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "tan ";
        }

        private void ln_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "ln ";
        }

        private void log_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "log ";
        }

        private void SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "√";
        }

        private void Exponents_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "^";
        }

        private void Shift_Click(object sender, RoutedEventArgs e)
        {
            shift = 1;
        }

        private void AllClear_Click(object sender, RoutedEventArgs e)
        {
            this.InitializeComponent();
            this.DisplayTopText.Text = string.Empty;
            int numOfBrackets = 0;
            int endingBracket = 0;
            int itemNumberCheck = 0;
            int shift = 0;
            int item = 1;
            List<NumOrString> bracketSolver = new List<NumOrString>();
            List<NumOrString> dataSolver = new List<NumOrString>()
            {
                new NumOrString {String = String.Empty }
            };
        }

        private void StartingBracket_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += "(";
        }

        private void EndingBracket_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayTopText.Text += ")";
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
