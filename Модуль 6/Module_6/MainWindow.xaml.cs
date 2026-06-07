using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Module_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void getDataButton(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = await getDataFromApi();

                string value = ExtractJsonValue(text, "value");
                dataContext.Text = value ?? "Данные не получены";
            }
            catch (Exception ex)
            {
                dataContext.Text = $"Ошибка: {ex.Message}";
            }
        }

        private string ExtractJsonValue(string json, string key)
        {
            string searchKey = $"\"{key}\"";
            int keyIndex = json.IndexOf(searchKey);
            if (keyIndex == -1) return null;

            int colonIndex = json.IndexOf(':', keyIndex);
            if (colonIndex == -1) return null;

            int startQuote = json.IndexOf('"', colonIndex + 1);
            if (startQuote == -1) return null;

            int endQuote = json.IndexOf('"', startQuote + 1);
            if (endQuote == -1) return null;

            return json.Substring(startQuote + 1, endQuote - startQuote - 1);
        }

        private void sendTestResultButton(object sender, RoutedEventArgs e)
        {
            string pattern = @"^[А-ЯЁ][а-яё]+\s[А-ЯЁ][а-яё]+\s[А-ЯЁ][а-яё]+$";

            //string pattern = @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$";    +7(999)123-45-67

            //string pattern = @"^[78]\d{10}$"; 79991234567

            //string pattern = @"^\+7\s\d{3}\s\d{3}\s\d{2}\s\d{2}$";  +7 999 123 45 67

            if (Regex.IsMatch(dataContext.Text, pattern))
            {
                result.Text = "Хорошо";
            }
            else
            {
                result.Text = "Плохо";
            }

        }

        public async Task<string> getDataFromApi()
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetStringAsync("http://localhost:4444/TransferSimulator/fullName");
                return result;
            }
        }
    }
}
