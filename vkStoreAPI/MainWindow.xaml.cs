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
using System.Net;
using Newtonsoft.Json;



namespace vkStoreAPI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string urlWithToken;
        private Authorization fw_a;
        private User person;
        public MainWindow(string urlWithToken, Authorization fw_a)
        {
            InitializeComponent();
            this.urlWithToken = urlWithToken;
            this.fw_a = fw_a;
            person = new User(urlWithToken);
            MessageBox.Show(person.id);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            fw_a.Visibility = Visibility.Visible;
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
