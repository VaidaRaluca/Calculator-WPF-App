using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class ExpressionEvaluator
    {
        public static double CalculateSum(double operand1, double operand2)
        {
            return operand1 + operand2;
        }
        public static double CalculateDiff(double operand1, double operand2)
        {
            return operand1 - operand2;
        }

        public static double CalculateModulo(double operand1, double operand2)
        {
            if (operand2 == 0)
            {
                throw new InvalidOperationException("Cannot divide by zero.");
            }
            return operand1 % operand2;
        }

        public static double CalculateDiv(double operand1, double operand2)
        {
            return operand1 / operand2;
        }

        public static double CalculateMul(double operand1, double operand2)
        {
            return operand1 * operand2;
        }

        public static double CalculateSqrt(double operand)
        {
            return Math.Sqrt(operand);
        }

        public static double CalculatePow(double operand)
        {
            return Math.Pow(operand, 2);
        }

        public static double CalculateFrac(double operand)
        {
            return 1/operand;
        }

        public static double SwitchSign(double operand)
        {
            return (-1) * operand;
        }
        public static string InfixToPrefix(string infix)
        {
            infix = ReverseExpression(infix);
            string postfix = InfixToPostfix(infix);
            return ReverseExpression(postfix);
        }

        private static string InfixToPostfix(string infix)
        {
            Stack<char> operators = new Stack<char>();
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < infix.Length; i++)
            {
                char current = infix[i];

                if (char.IsDigit(current))
                {
                    output.Append(current);
                    while (i + 1 < infix.Length && char.IsDigit(infix[i + 1]))
                    {
                        i++;
                        output.Append(infix[i]);
                    }
                    output.Append(" "); 
                }
                else if (IsOperator(current))
                {
                    while (operators.Count > 0 && HasHigherPrecedence(operators.Peek(), current))
                    {
                        output.Append(operators.Pop()).Append(" ");
                    }
                    operators.Push(current); 
                }
            }
            while (operators.Count > 0)
            {
                output.Append(operators.Pop()).Append(" ");
            }

            return output.ToString().Trim();
        }

        private static bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '%';
        }

        private static bool HasHigherPrecedence(char op1, char op2)
        {
            int precedence1 = GetPrecedence(op1);
            int precedence2 = GetPrecedence(op2);
            return precedence1 >= precedence2;
        }

        private static int GetPrecedence(char op)
        {
            if (op == '+' || op == '-') return 1;
            if (op == '*' || op == '/') return 2;
            return 0;
        }

        private static string ReverseExpression(string expression)
        {
            char[] arr = expression.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static double EvaluateEntireExpression(string expression)
        {
            Stack<double> stack = new Stack<double>();

            string[] tokens = expression.Split(' '); 

            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                string token = tokens[i];

                if (IsOperator(token)) 
                {
                    double operand1 = stack.Pop(); 
                    double operand2 = stack.Pop();

                    double result = ApplyOperator(token, operand1, operand2); 
                    stack.Push(result); 
                }
                else 
                {
                    stack.Push(double.Parse(token));
                }
            }

            return stack.Pop();
        }

        private static bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/" || token == "%";
        }

        private static double ApplyOperator(string operatorToken, double operand1, double operand2)
        {
            switch (operatorToken)
            {
                case "+":
                    return operand1 + operand2;
                case "-":
                    return operand1 - operand2;
                case "*":
                    return operand1 * operand2;
                case "/":
                    if (operand2 == 0) throw new DivideByZeroException("Cannot divide by zero.");
                    return operand1 / operand2;
                case "%":
                    return operand1 % operand2;
                default:
                    throw new InvalidOperationException("Invalid operator");
            }
        }

    }
}
