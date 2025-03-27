using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Calculator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        MyTextBox.Focus();
        MyTextBox.PreviewKeyDown += MyTextBox_PreviewKeyDown;
        MyTextBox.PreviewTextInput += MyTextBox_PreviewTextInput;
    }
    private void MyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
       MyTextBox.Focus();
    }
    private void MyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;

        if (e.Key == Key.Back)
        {
            viewModel.ButtonCommand.Execute("Back");
            e.Handled = true;
        }
        else if (e.Key == Key.Delete)
        {
            viewModel.ButtonCommand.Execute("CE");
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            viewModel.ButtonCommand.Execute("C");
            e.Handled = true;
        }
        else if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
        {
            string digit = e.Key.ToString().Last().ToString();
            viewModel.ButtonCommand.Execute(digit);
            e.Handled = true;
        }
        else if (e.Key == Key.Add)
        {
            viewModel.ButtonCommand.Execute("+");
            e.Handled = true;
        }
        else if (e.Key == Key.Subtract)
        {
            viewModel.ButtonCommand.Execute("-");
            e.Handled = true;
        }
        else if (e.Key == Key.Enter)
        {
            viewModel.ButtonCommand.Execute("=");
            e.Handled = true;
        }
        else if (e.Key == Key.Multiply)
        {
            viewModel.ButtonCommand.Execute("*");
            e.Handled = true;
        }
        else if (e.Key == Key.Divide)
        {
            viewModel.ButtonCommand.Execute("/");
            e.Handled = true;
        }

        MyTextBox.Focus();
    }

}