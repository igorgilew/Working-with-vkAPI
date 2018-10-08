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
using System.IO;

namespace vkStoreAPI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string urlWithToken;
        private Authorization fw_a;
        //авторизовавшийся пользователь
        private User person;
        
        public MainWindow(string urlWithToken, Authorization fw_a)
        {
            InitializeComponent();
            this.urlWithToken = urlWithToken;
            this.fw_a = fw_a;
            person = new User(urlWithToken);
            
                                 
            //короче оказалось что апи работает только с товарами сообществ, поэтому предлагается
            //выводить список сообществ пользователя в которых он является админом
            //затем пользователь выбирает сообщество и может выполнять круд операции с товарами выбранного с-ва.
        }
        private void BuildListBoxGroups()
        {
            //создаем запрос
            WebRequest request = WebRequest.Create(GetQuerryGroups(person.id, person.access_token));
            //получаем ответ в виде json-схемы
            string jsonResponse = GetGroupResponseJson(request, "GET");
        }
        private string GetQuerryGroups(string user_id, string access_token)
        {
            return string.Format("https://api.vk.com/method/groups.get?user_id={0}&extended=1&filter=admin&access_token={1}&v=5.85", user_id, access_token);
        }
        private string GetGroupResponseJson(WebRequest request, string method)
        {
            string json = "";
            request.Method = method;
            //выполняем запрос и получаем ответ
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    json = reader.ReadLine();                    
                }
            }
            response.Close();
            return json;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            fw_a.Visibility = Visibility.Visible;
        }       
    }
}
