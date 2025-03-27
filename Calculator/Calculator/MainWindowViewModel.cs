using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Calculator
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string displayText = "0";
        private string internalValue = "0";
        private int baseInternalValue;

        private MenuCommandHandler.DigitGrouping selectedDigitGrouping;
        private bool isROChecked;
        private bool isUSChecked;

        private int selectedTabIndex;

        private BaseCommandHandler.Base selectedBase;
        private bool isHexChecked;
        private bool isDecChecked ;
        private bool isOctChecked;
        private bool isBinChecked ;

        private bool isMenuVisible;
        public ICommand ToggleMenuCommand { get; }

        public ICommand TabSelectionChangedCommand { get; }
        public ICommand ButtonCommand { get; set; }
        private readonly ButtonCommandHandler buttonCommandHandler;
        public ICommand MenuCommand { get; set; }
        private readonly MenuCommandHandler menuCommandHandler;

        public ICommand BaseCommand { get; set; }
        private readonly BaseCommandHandler baseCommandHandler;

        private bool isExprChecked = false;

        private bool isMemoryVisible = false;

        private ObservableCollection<string> memoryValues = new ObservableCollection<string>();
        public ObservableCollection<string> MemoryValues
        {
            get { return memoryValues; }
            set
            {
                memoryValues = value;
                OnPropertyChanged(nameof(MemoryValues));
            }
        }

        public bool IsMemoryVisible
        {
            get { return isMemoryVisible; }
            set
            {
                isMemoryVisible = value;
                OnPropertyChanged(nameof(IsMemoryVisible));
            }
        }

        public int BaseInternalValue
        {
            get => baseInternalValue;
            set
            {
                if (baseInternalValue != value)
                {
                    baseInternalValue = value;
                }
            }
        }

        public bool IsExprChecked
        {
            get => isExprChecked;
            set
            {
                if (isExprChecked != value)
                {
                    isExprChecked = value;
                    OnPropertyChanged(nameof(isExprChecked));
                }
            }
        }

        public bool IsMenuVisible
        {
            get => isMenuVisible;
            set
            {
                if (isMenuVisible != value)
                {
                    isMenuVisible = value;
                    OnPropertyChanged(nameof(IsMenuVisible));
                }
            }
        }

        public string DisplayText
        {
            get => displayText;
            set
            {
                displayText = value;
                OnPropertyChanged(nameof(DisplayText));
            }
        }

        public string InternalValue
        {
            get => internalValue;
            set
            {
                if (internalValue != value)
                {
                    internalValue = value;
                    OnPropertyChanged(nameof(InternalValue));
                }
            }
        }

        public MenuCommandHandler.DigitGrouping SelectedDigitGrouping
        {
            get => selectedDigitGrouping;
            set
            {
                if (selectedDigitGrouping != value)
                {
                    selectedDigitGrouping = value;
                    OnPropertyChanged(nameof(SelectedDigitGrouping));
                }
            }
        }


        public bool IsROChecked
        {
            get => isROChecked;
            set
            {
                if (isROChecked != value)
                {
                    isROChecked = value;
                    OnPropertyChanged(nameof(IsROChecked));
                }
            }
        }

        public bool IsUSChecked
        {
            get => isUSChecked;
            set
            {
                if (isUSChecked != value)
                {
                    isUSChecked = value;
                    OnPropertyChanged(nameof(IsUSChecked));
                }
            }
        }

        public int SelectedTabIndex
        {
            get => selectedTabIndex;
            set
            {
                if (selectedTabIndex != value)
                {
                    selectedTabIndex = value;
                    OnPropertyChanged(nameof(SelectedTabIndex));
                    Properties.Settings.Default.LastSelectedTab = selectedTabIndex;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool IsHexChecked
        {
            get => isHexChecked;
            set
            {
                if (isHexChecked != value)
                {
                    isHexChecked = value;
                    OnPropertyChanged(nameof(IsHexChecked));
                }
            }
        }

        public bool IsDecChecked
        {
            get => isDecChecked;
            set
            {
                if (isDecChecked != value)
                {
                    isDecChecked = value;
                    OnPropertyChanged(nameof(IsDecChecked));
                }
            }
        }

        public bool IsOctChecked
        {
            get => isOctChecked;
            set
            {
                if (isOctChecked != value)
                {
                    isOctChecked = value;
                    OnPropertyChanged(nameof(IsOctChecked));
                }
            }
        }

        public bool IsBinChecked
        {
            get => isBinChecked;
            set
            {
                if (isBinChecked != value)
                {
                    isBinChecked = value;
                    OnPropertyChanged(nameof(IsBinChecked));
                }
            }
        }


        public BaseCommandHandler.Base SelectedBase
        {
            get => selectedBase;
            set
            {
                selectedBase = value;
                OnPropertyChanged(nameof(SelectedBase));
                IsHexChecked = selectedBase == BaseCommandHandler.Base.HEX;
                IsDecChecked = selectedBase == BaseCommandHandler.Base.DEC;
                IsOctChecked = selectedBase == BaseCommandHandler.Base.OCT;
                IsBinChecked = selectedBase == BaseCommandHandler.Base.BIN;
            }
        }


        public MainWindowViewModel()
        {
            selectedTabIndex = Properties.Settings.Default.LastSelectedTab;
            buttonCommandHandler = new ButtonCommandHandler(this);
            ButtonCommand = new RelayCommand<string>(executeButtonCommand);

            menuCommandHandler = new MenuCommandHandler(this);
            MenuCommand = new RelayCommand<string>(executeMenuCommand);
            string savedGrouping = Properties.Settings.Default.LastDigitGrouping;
            if (string.IsNullOrEmpty(savedGrouping))
            {
                savedGrouping = "RO";
            }
            if (savedGrouping == "RO")
            {
                SelectedDigitGrouping = MenuCommandHandler.DigitGrouping.RO;
                IsROChecked = true;
                IsUSChecked = false;
            }
            else
            {
                SelectedDigitGrouping = MenuCommandHandler.DigitGrouping.US;
                IsROChecked = false;
                IsUSChecked = true;
            }

            TabSelectionChangedCommand = new RelayCommand<int>(executeTabSelectionChanged);

            baseCommandHandler = new BaseCommandHandler(this);
            BaseCommand = new RelayCommand<string>(executeBaseCommand);
            string savedBase = Properties.Settings.Default.LastSelectedBase;
            if (string.IsNullOrEmpty(savedBase) || selectedTabIndex == 0)
            {
                savedBase = "DEC";
                IsDecChecked = true;
            }
            if (savedBase == "HEX")
            {
                SelectedBase = BaseCommandHandler.Base.HEX;
                IsHexChecked = true;
            }
            else if (savedBase == "DEC" || selectedTabIndex == 0)
            {
                SelectedBase = BaseCommandHandler.Base.DEC;
                IsDecChecked = true;
            }
            else if (savedBase == "OCT")
            {
                SelectedBase = BaseCommandHandler.Base.OCT;
                IsOctChecked = true;
            }
            else if (savedBase == "BIN")
            {
                SelectedBase = BaseCommandHandler.Base.BIN;
                IsBinChecked = true;
            }
            IsMenuVisible = false;
            ToggleMenuCommand = new RelayCommand<object>(executeToggleMenuCommand);

            MemoryValues = new ObservableCollection<string>();
            IsMemoryVisible = false;
        }


        private void executeToggleMenuCommand(object parameter)
        {
            IsMenuVisible = !IsMenuVisible;
            if (IsMenuVisible)
            {
                (Application.Current.MainWindow.Resources["SlideMenuStoryboard"] as Storyboard)?.Begin();
            }
            else
            {
                (Application.Current.MainWindow.Resources["HideMenuStoryboard"] as Storyboard)?.Begin();
            }
        }

        private void executeTabSelectionChanged(int tabIndex)
        {
            SelectedTabIndex = tabIndex;
        }

        public void FormatDisplayText()
        {
            if (internalValue == "0")
            {
                displayText = "0";
                return;
            }
            CultureInfo cultureInfo;
            int decimalPosition = InternalValue.IndexOf('.');
            if (SelectedDigitGrouping == MenuCommandHandler.DigitGrouping.RO)
            {
                cultureInfo = new CultureInfo("ro-RO");
            }
            else
            {
                cultureInfo = new CultureInfo("en-US");
            }

            bool hasDecimal = decimalPosition != -1;
            if (double.TryParse(internalValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double numberValue))
            {
                if (hasDecimal)
                {
                    string[] parts = internalValue.Split('.');
                    string formattedWholePart = string.Format(cultureInfo, "{0:N0}", double.Parse(parts[0]));

                    DisplayText = formattedWholePart + cultureInfo.NumberFormat.NumberDecimalSeparator + parts[1];
                }
                else
                {
                    DisplayText = string.Format(cultureInfo, "{0:N0}", numberValue);
                }
            }
        }

        public void FormatBase()
        {
            if (IsHexChecked)
            {
                InternalValue = BaseCommandHandler.FromHexToDec(InternalValue);
                baseInternalValue = 10;
            }
            else if (IsOctChecked)
            {
                InternalValue = BaseCommandHandler.FromBaseToDec(InternalValue, 8);
                baseInternalValue = 10;
            }
            else if (IsBinChecked)
            {
                InternalValue = BaseCommandHandler.FromBaseToDec(InternalValue, 2);
                baseInternalValue = 10;
            }
        }

        public void formatInNewBase()
        {
            if (IsHexChecked)
            {
                DisplayText = BaseCommandHandler.FromDecToHex(InternalValue);
            }
            else if (IsOctChecked)
            {
                DisplayText = BaseCommandHandler.FromDecToBase(InternalValue,8);
            }
            else if (IsBinChecked)
            {
                DisplayText = BaseCommandHandler.FromDecToBase(InternalValue, 2);
            }
        }

        private void executeButtonCommand(string parameter)
        {
            buttonCommandHandler.ExecuteButtonCommand(parameter);
        }

        private void executeMenuCommand(string parameter)
        {
            menuCommandHandler.ExecuteMenuCommand(parameter);
        }

        private void executeBaseCommand(string parameter)
        {
            baseCommandHandler.ExecuteBaseCommand(parameter);
        }

        public event  PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
