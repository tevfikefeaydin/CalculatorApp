using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var content = (sender as Button).Content.ToString();
            Display.Text += content;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "";
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = new DataTable().Compute(Display.Text, null);
                Display.Text = result.ToString();
            }
            catch
            {
                Display.Text = "Hata";
            }
        }
    }
}