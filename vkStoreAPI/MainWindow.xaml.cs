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
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections;
using System.Threading;

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

            BuildListBoxGroups();
                       
            //короче оказалось что апи работает только с товарами сообществ, поэтому предлагается
            //выводить список сообществ пользователя в которых он является админом
            //затем пользователь выбирает сообщество и может выполнять круд операции с товарами выбранного с-ва.
        }
        int numLogo=0;
        private void BuildListBoxGroups()
        {
            //создаем запрос
            WebRequest request = WebRequest.Create(GetQuerryGroups(person.id, person.access_token));
            //получаем ответ в виде json-схемы
            string jsonResponse = GetResponseJson(request, "GET");
            
            JObject groupSearch = JObject.Parse(jsonResponse);
            IList<JToken> results = groupSearch["response"]["items"].Children().ToList();
            //массив десериализованных объектов
            IList<GroupSearchResult> groupSearchResult = new List<GroupSearchResult>();
            foreach(JToken result in results)
            {               
                groupSearchResult.Add(result.ToObject<GroupSearchResult>());
                
            }
            if(groupSearchResult.Count >0)
            {
                             
                foreach (GroupSearchResult group in groupSearchResult)
                {
                    numLogo++;
                    var spForItem = new StackPanel() { Orientation = Orientation.Horizontal };
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(group.photo_100, AppDomain.CurrentDomain.BaseDirectory + "logo" + numLogo +".jpg");                                               
                    }

                    var img = new Image();                    
                    img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "logo" + numLogo + ".jpg", UriKind.Absolute)); ;
                    img.Width = 100;
                    img.Height = 100;
                    //

                    var spItem = new StackPanel() { Orientation = Orientation.Horizontal };
                    spItem.Children.Add(img);
                    var txtBlock = new TextBlock();
                    txtBlock.Text = group.name + "\thttps://vk.com/" + group.screen_name;
                    txtBlock.FontSize = 18;
                    txtBlock.Margin = new Thickness(20, 40, 0, 0);
                    spItem.Children.Add(txtBlock);

                    var item = new ListBoxItem();
                    item.Content = spItem;
                    item.FontSize = 20;
                    item.BorderThickness = new Thickness(1);
                    item.BorderBrush = new SolidColorBrush(Colors.Black);
                    item.Margin = new Thickness(0, 2, 0, 0);
                    item.Tag = group;
                    lbGroupd.Items.Add(item);
                                    
                }
            }

        }
        private string GetQuerryGroups(string user_id, string access_token)
        {
            return string.Format("https://api.vk.com/method/groups.get?user_id={0}&extended=1&filter=admin&access_token={1}&v=5.85", user_id, access_token);
        }
        private string GetResponseJson(WebRequest request, string method)
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
        private string GetQuerryProducts(string group_id, string access_token)
        {
            return string.Format("https://api.vk.com/method/market.get?owner_id=-{0}&access_token={1}&v=5.85", group_id, access_token);
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            e.Cancel = false;
            fw_a.Visibility = Visibility.Visible;            
        }

        private void Window_Closed(object sender, EventArgs e)
        {        
            
            GC.Collect();
            
        }

        private void lbGroupd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            spGroupsProducts.Visibility = Visibility.Visible;
            spGroups.Visibility = Visibility.Collapsed;
            btnBack.IsEnabled = true;
            ListBoxItem selectedItem = (ListBoxItem)lbGroupd.SelectedItem;
            var group = selectedItem.Tag as GroupSearchResult;           
            mainHeader.Content = "Товары сообщества " + group.name;
            mainHeader.Margin = new Thickness(300, 0, 0, 0);
            LoadProducts(group);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            spGroupsProducts.Visibility = Visibility.Collapsed;
            spGroups.Visibility = Visibility.Visible;
            btnBack.IsEnabled = false;
            mainHeader.Content = "Мои сообщества";
            mainHeader.Margin = new Thickness(400, 0, 0, 0);
        }
        private void LoadProducts(GroupSearchResult group)
        {
            //создаем запрос
            WebRequest request = WebRequest.Create(GetQuerryProducts(group.id, person.access_token));
            //получаем ответ в виде json-схемы
            string jsonResponse = GetResponseJson(request, "GET");

            JObject productSearch = JObject.Parse(jsonResponse);
            
            IList<JToken> results = productSearch["response"]["items"].Children().ToList();
            //массив десериализованных объектов
            IList<Product> productSearchResult = new List<Product>();
            foreach (JToken result in results)
            {
                
                productSearchResult.Add(result.ToObject<Product>());
                productSearchResult.Last().cost = result["price"]["text"].ToString();
            }
            if(productSearchResult.Count>0)
            {
                foreach (Product product in productSearchResult)
                {
                    numLogo++;
                    var spForItem = new StackPanel() { Orientation = Orientation.Horizontal };
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(product.thumb_photo, AppDomain.CurrentDomain.BaseDirectory + "logo" + numLogo + ".jpg");
                    }

                    var img = new Image();
                    img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "logo" + numLogo + ".jpg", UriKind.Absolute)); ;
                    img.Width = 100;
                    img.Height = 100;
                    //

                    var spItem = new StackPanel() { Orientation = Orientation.Horizontal };
                    var spDescr = new StackPanel();
                    spItem.Children.Add(img);

                    var txtBlockTitle = new TextBlock();
                    txtBlockTitle.Text = product.title;
                    txtBlockTitle.FontSize = 18;
                    txtBlockTitle.Margin = new Thickness(20, 40, 0, 0);

                    var txtBlockDescr = new TextBlock();
                    txtBlockDescr.Text = product.description;
                    txtBlockDescr.FontSize = 16;
                    txtBlockDescr.Margin = new Thickness(20, 10, 0, 0);

                    var txtBlockCost = new TextBlock();
                    txtBlockCost.Text = product.cost;
                    txtBlockCost.FontSize = 14;
                    txtBlockCost.Margin = new Thickness(20, 10, 0, 0);

                    spDescr.Children.Add(txtBlockTitle);
                    spDescr.Children.Add(txtBlockDescr);
                    spDescr.Children.Add(txtBlockCost);

                    spItem.Children.Add(spDescr);

                    var item = new ListBoxItem();
                    item.Content = spItem;
                    item.FontSize = 20;
                    item.BorderThickness = new Thickness(1);
                    item.BorderBrush = new SolidColorBrush(Colors.Black);
                    item.Margin = new Thickness(0, 2, 0, 0);
                    item.Tag = group;
                    lbGroupProducts.Items.Add(item);

                }
            }
        }
    }
}
