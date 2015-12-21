using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public abstract class Node
    {
        public abstract double Value
        { get; }
    }

    public class BinaryNode : Node
    {
        public readonly Func<double, double, double> Operator;
        public readonly Node Left;
        public readonly Node Right;

        public BinaryNode(
            Func<double, double, double> op,
            Node left,
            Node right)
        {
            this.Operator = op;
            this.Left = left;
            this.Right = right;
        }

        public override double Value
        { get { return this.Operator(this.Left.Value, this.Right.Value); } }
    }

    public class UnaryNode : Node
    {
        public readonly Func<double, double> Operator;
        public readonly Node Inner;

        public UnaryNode(Func<Double, double> op, Node inner)
        {
            this.Operator = op;
            this.Inner = inner;
        }

        public override double Value
        { get { return this.Operator(this.Inner.Value); } }
    }

    public class ConstNode : Node
    {
        public readonly double value;

        public ConstNode(double value)
        { this.value = value; }

        public override double Value
        { get { return this.value; } }
    }

    public class VariableNode : Node
    {
        public readonly Func<double> variable;
        public VariableNode(Func<double> variable)
        { this.variable = variable; }

        public override double Value
        { get { return this.variable(); } }
    }

    class Solver
    {
        const string binaryExpr = "^·÷%+-";

        static Func<double, double, double>[] binaryOperators = new Func<double, double, double>[]
        {
            (l,r) => Math.Pow(l, r),
            (l,r) => l * r,
            (l,r) => l / r,
            (l,r) => l % r,
            (l,r) => l + r,
            (l,r) => l - r
        };

        readonly static Dictionary<string, Func<double, double>> unaryMathFunctions =
            new Dictionary<string, Func<double, double>>
            {
                ["√"] = (l) => Math.Sqrt(l),
                ["sin"] = (l) => Math.Sin(l * Math.PI / 180),
                ["cos"] = (l) => Math.Cos(l * Math.PI / 180),
                ["tan"] = (l) => Math.Tan(l * Math.PI / 180),
                ["cosec"] = (l) => 1 / Math.Sin(l * Math.PI / 180),
                ["sec"] = (l) => 1 / Math.Sin(l * Math.PI / 180),
                ["cot"] = (l) => 1 / Math.Tan(l * Math.PI / 180),
                ["asin"] = (l) => Math.Asin(l) * 180 / Math.PI,
                ["acos"] = (l) => Math.Acos(l) * 180 / Math.PI,
                ["atan"] = (l) => Math.Atan(l) * 180 / Math.PI,
                ["log"] = (l) => Math.Log10(l),
                ["ln"] = (l) => Math.Log(l),
                ["alog"] = (l) => Math.Pow(10, l),
                ["aln"] = (l) => Math.Pow(Math.E, l)
            };

        static int IndexOf(string expr, int startIdx, int endIdx, char find)
        {
            int scopeCnt = 0;
            for (int idx = startIdx; idx < endIdx; idx++)
            {
                char ch = expr[idx];
                if (ch == '(')
                { scopeCnt++; }
                else if (ch == ')')
                { scopeCnt--; }

                if (scopeCnt > 0)
                { continue; }

                if (ch == find)
                { return idx; }
            }

            return -1;
        }

        static string FixExpr(string expr)
        {
            StringBuilder sb = new StringBuilder();
            for (int check = 0; check < expr.Length - 1; check++)
            {
                char ch = expr[check];
                char chNext = expr[check + 1];

                if (expr[check] != ' ')
                {
                    sb.Append(ch);
                }
                if ((char.IsDigit(ch) || ch == ')')
                    && (char.IsLetter(chNext) || chNext == '('))
                { sb.Append('*'); }
            }

            sb.Append(expr[expr.Length - 1]);
            return sb.ToString();
        }

        static Node Parse(string expr)
        {
            string fixedExpr = FixExpr(expr);
            return Parse(fixedExpr, 0, fixedExpr.Length);
        }

        static Node Parse(
            string expr,
            int startIdx,
            int endIdx)
        {
            if (startIdx == endIdx)
            { return new ConstNode(0); }

            int scopeCnt = 0;
            for (int iE = binaryExpr.Length - 1; iE >= 0; iE--)
            {
                char op = binaryExpr[iE];
                int idx = IndexOf(expr, startIdx, endIdx, op);
                if (idx >= 0)
                {
                    return new BinaryNode(
                        binaryOperators[iE],
                        Parse(expr, startIdx, idx),
                        Parse(expr, idx + 1, endIdx));
                }
            }

            foreach (var kvPair in Solver.unaryMathFunctions)
            {
                if (MatchAt(expr, startIdx, endIdx, kvPair.Key))
                {
                    return new UnaryNode(
                        kvPair.Value,
                        Parse(expr, startIdx + kvPair.Key.Length, endIdx));
                }
            }

            if (expr[startIdx] == '(')
            {
                for (int idx = startIdx; idx < endIdx - 1; idx++)
                {
                    char ch = expr[idx];
                    if (ch == '(')
                    { scopeCnt++; }
                    else if (ch == ')')
                    { scopeCnt--; }

                    if (scopeCnt == 0)
                    { return new ConstNode(0); }
                }

                return Parse(expr, startIdx + 1, expr[endIdx - 1] == ')' ? endIdx - 1 : endIdx);
            }

            double rv = 0;
            string subStr = expr.Substring(startIdx, endIdx - startIdx);
            if (double.TryParse(subStr, out rv))
            { return new ConstNode(rv); }
            else if (subStr == "PI")
            { return new ConstNode(Math.PI); }
            else if (subStr == "E")
            { return new ConstNode(Math.E); }
            return new ConstNode(0);
        }

        static bool MatchAt(string expr, int idx, int endIdx, string op)
        {
            if (endIdx - idx < op.Length)
            { return false; }

            for (int i = 0; i < op.Length; i++)
            {
                if (expr[i + idx] != op[i])
                { return false; }
            }

            return true;
        }

        public static double Solve(string str)
        {
            double res = Parse(str).Value;
            return res;
        }
    }
}
