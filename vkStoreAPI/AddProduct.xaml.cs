using Microsoft.Win32;
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
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;

using System.IO;
using System.Net.Http.Headers;

namespace vkStoreAPI
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        string access_token, groupId;
        MainWindow fw;
        public AddProduct(string access_token, string groupId, MainWindow fw)
        {
            this.access_token = access_token;
            this.groupId = groupId;
            this.fw = fw;
            InitializeComponent();
        }
        
        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Картинки(*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                
                var bmImage = new BitmapImage(new Uri(myDialog.FileName));           
                imgPhoto.Source = bmImage;
                imgPhoto.Tag = myDialog.FileName;                
            }
        }
        private string GetMarketUploadSerer()
        {
            WebRequest request = WebRequest.Create(staticRequestResponse.GetMarketUploadServer(groupId, access_token));
            //получаем ответ в виде json-схемы
            return staticRequestResponse.GetResponseJson(request);           
        }
        private string GetText(RichTextBox rtb)
        {
            var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text.Trim(new char[] { '\r', '\n' });
        }
        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            //получаем адрес сервера
            try
            {
                string upload_url = GetMarketUploadSerer();
                JObject result = JObject.Parse(upload_url);
                upload_url = result["response"]["upload_url"].ToString();

                string filePath = imgPhoto.Tag.ToString();
                //загружаем фотку
                var response = staticRequestResponse.POSTLoadImageToServer(filePath, upload_url);
                var str = await response.Content.ReadAsStringAsync();
                //сохраняем фотку на сервере
                //MessageBox.Show(str);
                JObject responseJSON = JObject.Parse(str);
                WebRequest request = WebRequest.Create(staticRequestResponse.GetSaveMarketPhoto(groupId, access_token, responseJSON["photo"].ToString(), responseJSON["server"].ToString(), responseJSON["hash"].ToString(), responseJSON["crop_data"].ToString(), responseJSON["crop_hash"].ToString()));
                JObject photoJSON = JObject.Parse(staticRequestResponse.GetResponseJson(request));
                JArray pa = JArray.Parse(photoJSON["response"].ToString());
                JObject pj = JObject.Parse(pa[0].ToString());

                //MessageBox.Show(pj["id"].ToString());
                //сначала проверить достается ли айдишник фотки, потом попробовать запилить товар запросом
                request = WebRequest.Create(staticRequestResponse.GetMarketAdd(groupId, access_token, txtBoxName.Text, GetText(rtbDescr), txtBoxCost.Text, pj["id"].ToString()));
                JObject res = JObject.Parse(staticRequestResponse.GetResponseJson(request));
                //MessageBox.Show(res["response"]["market_item_id"].ToString());

                var product = new Product(res["response"]["market_item_id"].ToString(), txtBoxName.Text, GetText(rtbDescr), txtBoxCost.Text);

                var spItem = new StackPanel() { Orientation = Orientation.Horizontal };
                var spDescr = new StackPanel();

                var img = new Image();
                img.Source = imgPhoto.Source;
                img.Width = 100;
                img.Height = 100;

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
                //spItem.Children.Add(btnDel);

                var item = new ListBoxItem();
                item.Content = spItem;
                item.FontSize = 20;
                item.BorderThickness = new Thickness(1);
                item.BorderBrush = new SolidColorBrush(Colors.Black);
                item.Margin = new Thickness(0, 2, 0, 0);
                item.Tag = product;
                fw.lbGroupProducts.Items.Add(item);
                MessageBox.Show("Товар добавлен");
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так. Добавление не выполнено(");
            }
            
        }
    }
}
