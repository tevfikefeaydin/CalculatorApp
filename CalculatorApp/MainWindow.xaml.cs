using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private double _storedValue;
        private string? _pendingOperator;
        private bool _startNewEntry = true;

        public MainWindow() => InitializeComponent();

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            var digit = ((Button)sender).Content.ToString()!;

            if (_startNewEntry || Display.Text == "0")
            {
                Display.Text = digit;
                _startNewEntry = false;
            }
            else
            {
                Display.Text += digit;
            }
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (_startNewEntry)
            {
                Display.Text = "0";
                _startNewEntry = false;
            }

            if (!Display.Text.Contains('.'))
                Display.Text += ".";
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            var op = (string)((Button)sender).Tag;

            if (_pendingOperator != null && !_startNewEntry)
                Calculate();
            else
                _storedValue = ParseDisplay();

            _pendingOperator = op;
            History.Text = $"{Format(_storedValue)} {OperatorSymbol(op)}";
            _startNewEntry = true;
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (_pendingOperator == null)
                return;

            History.Text = $"{Format(_storedValue)} {OperatorSymbol(_pendingOperator)} {Display.Text} =";
            Calculate();
            _pendingOperator = null;
            _startNewEntry = true;
        }

        private void Calculate()
        {
            var right = ParseDisplay();
            var result = _pendingOperator switch
            {
                "+" => _storedValue + right,
                "-" => _storedValue - right,
                "*" => _storedValue * right,
                "/" => _storedValue / right,
                _ => right
            };

            if (double.IsNaN(result) || double.IsInfinity(result))
            {
                Display.Text = "Hata";
                _pendingOperator = null;
                return;
            }

            _storedValue = result;
            Display.Text = Format(result);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            History.Text = "";
            _storedValue = 0;
            _pendingOperator = null;
            _startNewEntry = true;
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (_startNewEntry)
                return;

            Display.Text = Display.Text.Length > 1
                ? Display.Text[..^1]
                : "0";
        }

        private void SignToggle_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text.StartsWith("-"))
                Display.Text = Display.Text[1..];
            else if (Display.Text != "0")
                Display.Text = "-" + Display.Text;
        }

        private void Percent_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = Format(ParseDisplay() / 100.0);
        }

        private double ParseDisplay() =>
            double.TryParse(Display.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                ? value
                : 0;

        private static string Format(double value) =>
            value.ToString("0.##########", CultureInfo.InvariantCulture);

        private static string OperatorSymbol(string op) => op switch
        {
            "/" => "÷",
            "*" => "×",
            "-" => "−",
            _ => op
        };
    }
}
