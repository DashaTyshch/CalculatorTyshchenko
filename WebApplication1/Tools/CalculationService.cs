using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebApplication1.Tools
{
    public class CalculationService
    {
        private readonly List<string> operators;
        private readonly Dictionary<string, Func<double, double, double>> opsDict;

        public CalculationService()
        {
            operators = new List<string>
            {
                "(", ")", "+", "-", "*", "/"
            };

            opsDict = new Dictionary<string, Func<double, double, double>>
            {
                ["+"] = (x, y) => x + y,
                ["-"] = (x, y) => x - y,
                ["*"] = (x, y) => x * y,
                ["/"] = (x, y) => y == 0 ? throw new InvalidExpressionException() : x / y
            };

        }

        public double Calculate(string input)
        {
            var rpn = RPN(input);
            if (!Valid(rpn))
                throw new InvalidExpressionException();

            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>(rpn);

            string str = queue.Dequeue();
            while (queue.Count >= 0)
            {
                if (!operators.Contains(str))
                {
                    stack.Push(str);
                    str = queue.Dequeue();
                }
                else
                {
                    var leftValid = double.TryParse(stack.Pop(), out var a);
                    var rightValid = double.TryParse(stack.Pop(), out var b);
                    if (!leftValid || !rightValid)
                        throw new InvalidExpressionException();
                    stack.Push(opsDict[str](b, a).ToString());
                    if (queue.Count > 0)
                        str = queue.Dequeue();
                    else
                        break;
                }

            }
            return Convert.ToDouble(stack.Pop());
        }

        private bool Valid(string[] rpn)
        {
            var check = 0;
            foreach (var tocken in rpn)
            {
                check += operators.Contains(tocken) ? -1 : 1;
                if (check < 0)
                    return false;
            }
            return check == 1;
        }

        private string[] RPN(string exp)
        {
            List<string> outputSeparated = new List<string>();
            Stack<string> stack = new Stack<string>();
            string[] v = Separate(exp);
            foreach (string c in v)
            {
                if (operators.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            string s = stack.Pop();
                            while (s != "(")
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
                            stack.Push(c);
                        else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
                                outputSeparated.Add(stack.Pop());
                            stack.Push(c);
                        }
                    }
                    else
                        stack.Push(c);
                }
                else
                    outputSeparated.Add(c);
            }
            if (stack.Count > 0)
                foreach (string c in stack)
                    outputSeparated.Add(c);

            return outputSeparated.ToArray();

        }

        private string[] Separate(string input)
        {
            var delims = @"([+\-/*()])";

            return Regex.Split(input, delims).Where(token => string.Empty != token).ToArray();
        }

        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(":
                case ")":
                    return 0;
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                default:
                    return 3;
            }
        }

    }

    public class InvalidExpressionException : Exception { }

}