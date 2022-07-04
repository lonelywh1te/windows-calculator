using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace windows_calculator
{
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
            if (Value.Length > 10 && Value.Length < 14)
            {
                Display.FontSize = 30;
                Display.Margin = new Thickness(0, 8, 10, 0);
            }
            else if (Value.Length >= 14 && Value.Length < 21) {
                Display.FontSize = 23;
                Display.Margin = new Thickness(0 , 15, 10, 0);
            }
            else if (Value.Length >= 21 && Value.Length < 25) {
                Display.FontSize = 22;
                Display.Margin = new Thickness(0 , 15, 10, 0);
            }
            else if (Value.Length >= 25)
            {
                Display.FontSize = 15;
                Display.Margin = new Thickness(0 , 20, 10, 0);
            }
            else {
                Display.FontSize = 40;
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
            UpdateDisplay(Calculator.Result.ToString());
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
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            
            if (e.Key == Key.Back) btn_backspace.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.Delete) btn_clear_entry.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.D1) btn_1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.D2) btn_2.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.D3) btn_3.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.D4) btn_4.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.D5) btn_5.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.D6) btn_6.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.D7) btn_7.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.D8) btn_8.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.D9) btn_9.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.D0) btn_0.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.OemPlus) btn_plus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract) btn_minus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.OemQuestion || e.Key == Key.Divide) btn_division.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.Multiply || (shift && e.Key == Key.D8)) btn_multiplication.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            if (e.Key == Key.Enter) btn_equal.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        private void HelpClicked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://support.microsoft.com/ru-ru/windows");
        }
        private void AboutClicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("windows-calculator \n lonelywh1te \n https://github.com/lonelywh1te/windows-calculator");
        }
        private void InBufferClicked(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Display.Text);
        }
        private void OutBufferClicked(object sender, RoutedEventArgs e)
        {
            double number;
            if (double.TryParse(Clipboard.GetText(), out number)) {
                Calculator.UserInput = Clipboard.GetText();
                Display.Text = Clipboard.GetText();
            }
        }
    }
}