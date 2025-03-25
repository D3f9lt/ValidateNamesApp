using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using ValidateNames_API.Model;

namespace ValidateNamesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string JSON_PATH = "https://localhost:7044/api/User";
        private HttpClient _httpClient;
        private string _fullName;
        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var response = await _httpClient.GetStringAsync(JSON_PATH);
            FullNameLV.ItemsSource = JsonConvert.DeserializeObject<List<User>>(response);
        }

        private void TakeDataBTN_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = FullNameLV.SelectedItem as User;

            if (selectedUser != null)
            {
                FullNameTBL.Text = _fullName = selectedUser.Fullname;
            }
            else
            {
                MessageBox.Show("Сначала выберите ФИО из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SendResultBTN_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            foreach (char character in _fullName)
            {
                if (!char.IsLetter(character) && !char.IsWhiteSpace(character))
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                TestResultTBL.Text = "ФИО не содержит запрещённые символы.";
                TestResultTBL.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                TestResultTBL.Text = "ФИО содержит запрещённые символы.";
                TestResultTBL.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
