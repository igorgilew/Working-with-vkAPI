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
using System.Windows.Shapes;
using System.IO;

namespace vkStoreAPI
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            Uri request = new Uri(@"https://oauth.vk.com/authorize?client_id=6713924&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=market,group,photos&response_type=token&v=5.85&state=123456&revoke=1");            
            browser.Source = request;
            
            browser.LoadCompleted += (sender, e) =>
            {
                var str = browser.Source.ToString();
                if(str.Contains(@"#access_token"))
                {
                    var formMain = new MainWindow(str, this);
                    formMain.Owner = this;
                    this.Visibility = Visibility.Collapsed;                  
                    formMain.ShowDialog();
                    //удаление скачанных картинок
                    var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                    var files = dir.GetFiles("logo*.jpg");
                    foreach (var file in files)
                    {
                        File.Delete(file.FullName);
                    }

                    //browser.Source = request;
                    browser.Navigate(request);
                }
                else
                {
                    if(!str.Equals(request.ToString()) && !str.Equals("http://vk.com/"))
                    {
                        //browser.Navigate(@"https://oauth.vk.com/authorize?client_id=6713924&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=market&response_type=token&v=5.85&state=123456&revoke=1");
                        browser.Navigate(request);
                    }
                    else
                    {
                        return;
                    }                   
                }
                
            };
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GC.Collect();
            //File.Delete(AppDomain.CurrentDomain.BaseDirectory + "logo" + 1 + ".jpg");
            //либо через регулярку удалять все файлы либо сделать из в отдельной папке и удалять всю папку

            
        }
    }
}
