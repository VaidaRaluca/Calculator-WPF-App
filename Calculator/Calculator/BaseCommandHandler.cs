using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class BaseCommandHandler
    {
        private readonly MainWindowViewModel viewModel;
        public enum Base
        {
            HEX,
            DEC,
            OCT,
            BIN
        };
        public BaseCommandHandler(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public static string FromDecToHex(string value)
        {
            int decimalValue = int.Parse(value);
            if (decimalValue == 0)
            {
                return "0";
            }
            string hexString = string.Empty;
            while (decimalValue > 0)
            {
                int digit = decimalValue % 16;
                if (digit > 9)
                {
                    hexString += (char)('A' + digit - 10);
                }
                else
                {
                    hexString += (char)('0' + digit);
                }
                decimalValue /= 16;
            }
            return new string(hexString.Reverse().ToArray());
        }

        public static string FromHexToDec(string hexValue)
        {
            hexValue = hexValue.Trim();
            int decimalValue = Convert.ToInt32(hexValue, 16);
            return decimalValue.ToString();
        }

        public static string FromDecToBase(string value, int toBase)
        {
            int decimalValue = int.Parse(value);
            string newBaseString = string.Empty;
            if (decimalValue == 0)
            {
                return "0";
            }
            while (decimalValue > 0)
            {
                int digit = decimalValue % toBase;
                newBaseString += (char)('0' + digit);

                decimalValue /= toBase;
            }
            return new string(newBaseString.Reverse().ToArray());
        }

        public static string FromBaseToDec(string value, int fromBase)
        {
            if(value=="0")
            {
                return "0";
            }
            int newBaseString = 0;
            value = new string(value.Reverse().ToArray());
            int counter = 0;
            foreach(char c in value)
            {
                int digit = c - '0';
                newBaseString += digit * (int)Math.Pow(fromBase,counter++);
            }
            return newBaseString.ToString();
        }


        public void ExecuteBaseCommand(string command)
        {
            if (command == "HEX")
            {
                viewModel.SelectedBase = Base.HEX;
                Properties.Settings.Default.LastSelectedBase = "HEX";
            }
            else if (command == "DEC")
            {
                viewModel.SelectedBase = Base.DEC;
                Properties.Settings.Default.LastSelectedBase = "DEC";
            }
            else if (command == "OCT")
            {
                viewModel.SelectedBase = Base.OCT;
                Properties.Settings.Default.LastSelectedBase = "OCT";
            }
            else if (command == "BIN")
            {
                viewModel.SelectedBase = Base.BIN;
                Properties.Settings.Default.LastSelectedBase = "BIN";
            }
            Properties.Settings.Default.Save();
        }

    }
}
