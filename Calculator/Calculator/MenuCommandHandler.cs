using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator
{
    public class MenuCommandHandler
    {
        private readonly MainWindowViewModel viewModel;
        public enum DigitGrouping
        {
            RO,
            US
        };
        public MenuCommandHandler(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void ExecuteMenuCommand(string command)
        {
            switch (command)
            {
                case "RO":
                    viewModel.SelectedDigitGrouping = DigitGrouping.RO;
                    viewModel.IsROChecked = true;
                    viewModel.IsUSChecked = false;
                    viewModel.FormatDisplayText();
                    Properties.Settings.Default.LastDigitGrouping = "RO";
                    break;

                case "US":
                    viewModel.SelectedDigitGrouping = DigitGrouping.US;
                    viewModel.IsROChecked = false;
                    viewModel.IsUSChecked = true;
                    viewModel.FormatDisplayText();
                    Properties.Settings.Default.LastDigitGrouping = "US";
                    break;

                case "Cut":
                    HandleCut();
                    break;

                case "Copy":
                    HandleCopy();
                    break;

                case "Paste":
                    HandlePaste();
                    break;
                case "Expr":
                     viewModel.IsExprChecked = true;
                    break;

                default:
                    break;
            }

            Properties.Settings.Default.Save();
        }

        private void HandleCut()
        {
            Clipboard.SetText(viewModel.InternalValue);
            viewModel.InternalValue = "0"; 
            viewModel.DisplayText = viewModel.InternalValue;
        }

        private void HandleCopy()
        {
            Clipboard.SetText(viewModel.InternalValue);
        }

        private void HandlePaste()
        {
            if (Clipboard.ContainsText())
            {
                string clipboardContent = Clipboard.GetText();
                if (double.TryParse(clipboardContent, out double parsedValue))
                {
                    viewModel.InternalValue = clipboardContent;
                    viewModel.DisplayText = viewModel.InternalValue;
                }
            }
        }

    }
}
