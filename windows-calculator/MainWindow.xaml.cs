using System.Windows;
using System.Windows.Controls;

namespace windows_calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            HistoryDisplay.Text = "";
            Display.Text = "0";
            MemoryLED.Visibility = Visibility.Hidden;
        }
        Calculate Calculator = new Calculate();

        private void UpdateDisplay(string Value)
        {
            if (Calculator.isBlocked) {
                Value = Calculator.ErrorMessage;
            }
            if (Value.Length > 12 && Value.Length < 16)
            {
                Display.FontSize = 20;
                Display.Margin = new Thickness(0, 5, 10, 0);
            }
            else if (Value.Length >= 16 && Value.Length < 21) {
                Display.FontSize = 15;
                Display.Margin = new Thickness(0 , 8, 10, 0);
            }
            else if (Value.Length >= 21 && Value.Length < 25) {
                Display.FontSize = 13;
                Display.Margin = new Thickness(0 , 10, 10, 0);
            }
            else if (Value.Length >= 25)
            {
                Display.FontSize = 12;
                Display.Margin = new Thickness(0 , 12, 10, 0);
            }
            else {
                Display.FontSize = 25;
                Display.Margin = new Thickness(0, 0, 10, 0);
            }
            Display.Text = Value == "" ? "0" : Value;
        }
        private void UpdateHistory(string Data)
        {
            if (Data.Length < 25) {
                HistoryDisplay.Text = Data;
            }
            else {
                string HistorySubString = $"«{Data.Substring(Data.Length-25, 25)}";
                HistoryDisplay.Text = HistorySubString;
            }
        }
        
        private void DigitClicked(object sender, RoutedEventArgs e)
        {
            Calculator.AddDigit((sender as Button).Content.ToString());
            UpdateDisplay(Calculator.UserInput);
        }
        private void CommaClicked(object sender, RoutedEventArgs e)
        {
            Calculator.UserInput = Display.Text; 
            Calculator.AddComma();
            UpdateDisplay(Calculator.UserInput);
        }
        private void OperationClicked(object sender, RoutedEventArgs e)
        {
            Calculator.ExecuteOperation((sender as Button).Content.ToString());
            UpdateDisplay(Calculator.Result.ToString());
            UpdateHistory(Calculator.UserHistory);
        }
        private void SignChangeClicked(object sender, RoutedEventArgs e)
        {
            Calculator.UserInput = Display.Text;
            Calculator.Negate();
            UpdateHistory(Calculator.UserHistory);
            UpdateDisplay(Calculator.UserInput);
        }
        private void SqrtClicked(object sender, RoutedEventArgs e)
        {
            Calculator.UserInput = Display.Text;
            Calculator.Sqrt();
            UpdateHistory(Calculator.UserHistory);
            UpdateDisplay(Calculator.UserInput);
        }

        private void PercentClicked(object sender, RoutedEventArgs e)
        {
            Calculator.UserInput = Display.Text;
            Calculator.Percent();
            UpdateHistory(Calculator.UserHistory);
            UpdateDisplay(Calculator.UserInput);
        }
        
        private void ReciprocClicked(object sender, RoutedEventArgs e)
        {
            Calculator.UserInput = Display.Text;
            Calculator.Reciproc();
            UpdateHistory(Calculator.UserHistory);
            UpdateDisplay(Calculator.UserInput);
        }
        private void MemoryClicked(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.ToString() == "MS" && !Calculator.isBlocked) {
                MemoryLED.Visibility = Visibility.Visible;
            } 
            else if ((sender as Button).Content.ToString() == "MC") {
                MemoryLED.Visibility = Visibility.Hidden;
            }
            
            Calculator.UserInput = Display.Text;
            Calculator.Memory((sender as Button).Content.ToString());
            UpdateDisplay(Calculator.UserInput);
        }
        private void EqualClicked(object sender, RoutedEventArgs e)
        {
            Calculator.UserInput = Display.Text;
            Calculator.Equal();
            UpdateHistory(Calculator.UserHistory);
            UpdateDisplay(Calculator.Result.ToString("0.###############"));
        }
        private void ClearClicked(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Content.ToString())
            {
                case "⌫":
                    Calculator.Backspace();
                    UpdateDisplay(Calculator.UserInput);
                    break;
                case "CE":
                    Calculator.ClearInput();
                    UpdateDisplay(Calculator.UserInput);
                    break;
                case "C":
                    Calculator.ClearAll();
                    UpdateHistory(Calculator.UserHistory);
                    UpdateDisplay(Calculator.UserInput);
                    break;
            }
        }
    }
}