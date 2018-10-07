﻿using System;
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
            Uri request = new Uri(@"https://oauth.vk.com/authorize?client_id=6713924&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=market&response_type=token&v=5.85&state=123456&revoke=1");            
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
                    browser.Source = request;
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
    }
}