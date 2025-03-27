using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Security;
using System.Windows;

namespace Calculator
{
    public class ButtonCommandHandler
    {
        private readonly MainWindowViewModel viewModel;
        private double firstOperand = 0;
        private double secondOperand = 0;
        private string currentOperator = string.Empty;
        private string nextOperator = string.Empty;
        private bool isOperatorPressed = false;
        private double memoryValue = 0;
        private bool isError = false;
        private bool isResult = false;
        private string accumulatedExpression = string.Empty;
      
        public ButtonCommandHandler(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool IsResult
        {
            get { return isResult; }
            set
            {
                if (isResult != value)
                {
                    isResult = value;
                }
            }
        }

        public void ExecuteButtonCommand(string command)
        {
            if (isError && command != "CDelete" && command != "CE" && command != "=")
            {
                return;
            }
            switch (command)
            {
                case "CDelete":
                    viewModel.InternalValue = "0";
                    viewModel.DisplayText = viewModel.InternalValue;
                    accumulatedExpression = string.Empty;
                    viewModel.IsExprChecked = false;
                    ResetOperands();
                    isError = false;
                    isResult = false;
                    break;
                case "CE":
                    if (isError)
                    {
                        ResetOperands();
                    }
                    if (!isOperatorPressed)
                    {
                        viewModel.InternalValue = "0";
                        viewModel.DisplayText = viewModel.InternalValue;
                    }
                    else
                    {
                        secondOperand = 0;
                        viewModel.InternalValue = "0";
                        viewModel.DisplayText = viewModel.InternalValue;
                    }
                    isResult = false;
                    isError = false;
                    break;
                case "Back":
                    if (viewModel.IsExprChecked)
                    {
                        accumulatedExpression = accumulatedExpression.Substring(0, accumulatedExpression.Length - 1);
                        viewModel.DisplayText = accumulatedExpression;
                    }
                    if (viewModel.InternalValue != "0" && !isResult)
                    {
                        if (viewModel.InternalValue.StartsWith("-") || viewModel.InternalValue.Length == 1)
                        {
                            viewModel.InternalValue = "0";
                        }
                        else
                        {
                            viewModel.InternalValue = viewModel.InternalValue.Substring(0, viewModel.InternalValue.Length - 1);
                        }
                        viewModel.DisplayText = viewModel.InternalValue;
                        viewModel.FormatDisplayText();
                    }
                    break;
                case "dot":
                    if (isResult)
                    {
                        return;
                    }
                    if (viewModel.InternalValue.Contains(".") || viewModel.InternalValue.Contains(","))
                        break;
                    if (string.IsNullOrEmpty(viewModel.InternalValue) || viewModel.InternalValue == "0")
                    {
                        viewModel.InternalValue = "0.";
                    }
                    else
                    {
                        viewModel.InternalValue += ".";
                    }
                    viewModel.DisplayText = viewModel.InternalValue;
                    viewModel.FormatDisplayText();
                    break;
                case var _ when command.Length == 1 && char.IsDigit(command[0]):
                    if (isResult)
                    {
                        return;
                    }
                    if (viewModel.IsExprChecked)
                    {
                        accumulatedExpression += command;
                        viewModel.DisplayText = accumulatedExpression;
                    }
                    else
                    {
                        if ((viewModel.IsBinChecked && !"01".Contains(command)) ||
                        (viewModel.IsOctChecked && !"01234567".Contains(command)))
                        {
                            return;
                        }
                        if (viewModel.InternalValue == "0")
                        {
                            viewModel.InternalValue = command;
                        }
                        else
                        {
                            viewModel.InternalValue += command;
                        }
                        viewModel.DisplayText = viewModel.InternalValue;
                        viewModel.FormatDisplayText();
                    }
                    break;
                case var _ when command.Length == 1 && "ABCDEFabcdef".Contains(command) && viewModel.IsHexChecked:
                    if (isResult)
                    {
                        return;
                    }
                    if (viewModel.IsExprChecked)
                    {
                        accumulatedExpression += command;
                        viewModel.DisplayText = accumulatedExpression;
                    }
                    else
                    {
                        if (viewModel.InternalValue == "0")
                        {
                            viewModel.InternalValue = command.ToUpper();
                        }
                        else
                        {
                            viewModel.InternalValue += command.ToUpper();
                        }
                        viewModel.DisplayText = viewModel.InternalValue;
                        viewModel.FormatDisplayText();
                    }
                    break;
                case "+":
                case "-":
                case "*":
                case "/":
                case "%":
                    isResult = false;
                    HandleOperator(command);
                    break;
                case "=":
                    HandleEquals();
                    break;
                case "Sqrt":
                    HandleSqrt();
                    break;
                case "Pow":
                    HandlePow();
                    break;
                case "1/x":
                    HandleFrac();
                    break;
                case "Sign":
                    HandleSign();
                    break;

                case "MC":
                    viewModel.InternalValue = memoryValue.ToString();
                    memoryValue = 0;
                    viewModel.MemoryValues.Clear();
                    break;

                case "MR":
                    viewModel.InternalValue = memoryValue.ToString();
                    viewModel.DisplayText = viewModel.InternalValue;
                    break;

                case "M+":
                    if (double.TryParse(viewModel.InternalValue, out double addValue))
                    {
                        memoryValue += addValue;
                    }
                    break;

                case "M-":
                    if (double.TryParse(viewModel.InternalValue, out double subValue))
                    {
                        memoryValue -= subValue;
                    }
                    break;

                case "MS":
                    if (double.TryParse(viewModel.InternalValue, out double storeValue))
                    {
                        memoryValue = storeValue;
                        viewModel.MemoryValues.Add(memoryValue.ToString());
                    }
                    break;

                case "M>":
                    viewModel.IsMemoryVisible = !viewModel.IsMemoryVisible;
                    break;
                default:
                    break;
            }
        }

        private void HandleEquals()
        {
            if(viewModel.IsExprChecked)
            {
                string prefixExpression = ExpressionEvaluator.InfixToPrefix(accumulatedExpression);
                double result = ExpressionEvaluator.EvaluateEntireExpression(prefixExpression);
                accumulatedExpression = result.ToString();
                viewModel.DisplayText = accumulatedExpression;
                return;
            }
            if (isOperatorPressed)
            {
                HandleOperator(currentOperator);
                isResult = true;
            }
        }


        private void HandleOperator(string operatorCommand)
        {
            if (viewModel.IsExprChecked)
            {
                accumulatedExpression += operatorCommand;
                viewModel.DisplayText = accumulatedExpression;
            }
            else
            {
                if (!isOperatorPressed)
                {
                    if (viewModel.BaseInternalValue != 10)
                    {
                        viewModel.FormatBase();
                    }
                    firstOperand = double.Parse(viewModel.InternalValue);
                    viewModel.InternalValue = "0";
                    currentOperator = operatorCommand;
                    isOperatorPressed = true;
                }
                else
                {
                    viewModel.FormatBase();
                    secondOperand = double.Parse(viewModel.InternalValue);
                    double result = 0;
                    switch (currentOperator)
                    {
                        case "+":
                            result = ExpressionEvaluator.CalculateSum(firstOperand, secondOperand);
                            break;
                        case "-":
                            result = ExpressionEvaluator.CalculateDiff(firstOperand, secondOperand);
                            break;
                        case "%":
                            if (secondOperand == 0)
                            {
                                viewModel.DisplayText = "Cannot divide by 0";
                                isError = true;
                                return;
                            }
                            result = ExpressionEvaluator.CalculateModulo(firstOperand, secondOperand);
                            break;
                        case "/":
                            if (secondOperand == 0)
                            {
                                viewModel.DisplayText = "Cannot divide by 0";
                                isError = true;
                                return;
                            }
                            result = ExpressionEvaluator.CalculateDiv(firstOperand, secondOperand);
                            break;
                        case "*":
                            result = ExpressionEvaluator.CalculateMul(firstOperand, secondOperand);
                            break;
                        default:
                            break;
                    }
                    viewModel.InternalValue = result.ToString();
                    firstOperand = double.Parse(viewModel.InternalValue);
                    if (!viewModel.IsDecChecked)
                    {
                        viewModel.formatInNewBase();
                    }
                    else
                    {
                        viewModel.DisplayText = viewModel.InternalValue;
                        viewModel.FormatDisplayText();
                    }
                    nextOperator = operatorCommand;
                    currentOperator = nextOperator;
                    isOperatorPressed = false;
                }
            }
        }

        private void HandleSqrt()
        {
            double value = double.Parse(viewModel.InternalValue);
            if (value < 0)
            {
                viewModel.DisplayText = "Invalid Input";
                isError = true;
            }
            else
            {
                double result = ExpressionEvaluator.CalculateSqrt(value);
                viewModel.InternalValue = result.ToString();
                viewModel.DisplayText = viewModel.InternalValue;
            }
            viewModel.FormatDisplayText();

        }

        private void HandlePow()
        {
            double value = double.Parse(viewModel.InternalValue);
            double result = ExpressionEvaluator.CalculatePow(value);
            viewModel.InternalValue = result.ToString();
            viewModel.DisplayText = viewModel.InternalValue;
            viewModel.FormatDisplayText();

        }

        private void HandleFrac()
        {
            double value = double.Parse(viewModel.InternalValue);
            if (value == 0)
            {
                viewModel.DisplayText = "Cannot divide by 0";
                isError = true;
            }
            else
            {
                double result = ExpressionEvaluator.CalculateFrac(value);
                viewModel.InternalValue = result.ToString();
                viewModel.DisplayText = viewModel.InternalValue;
            }
            viewModel.FormatDisplayText();

        }

        private void HandleSign()
        {
            double value = double.Parse(viewModel.InternalValue);
            double result = ExpressionEvaluator.SwitchSign(value);
            viewModel.InternalValue = result.ToString();
            viewModel.DisplayText = viewModel.InternalValue;
            viewModel.FormatDisplayText();

        }

        private void ResetOperands()
        {
            firstOperand = 0;
            secondOperand = 0;
            currentOperator = string.Empty;
            nextOperator = string.Empty;
            isOperatorPressed = false;
        }
    }
}
