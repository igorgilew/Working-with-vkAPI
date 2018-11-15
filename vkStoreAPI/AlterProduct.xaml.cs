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
using System.Net;
using System.Text.RegularExpressions;

namespace vkStoreAPI
{
    /// <summary>
    /// Логика взаимодействия для AlterProduct.xaml
    /// </summary>
    public partial class AlterProduct : Window
    {
        MainWindow fw;
        ListBoxItem lbItem;
        Product product;
        string access_token, groupId;
        public AlterProduct(MainWindow fw, ListBoxItem lbItem, string access_token, string groupId)
        {
            this.fw = fw;
            this.lbItem = lbItem;
            this.access_token = access_token;
            this.groupId = groupId;           
            InitializeComponent();
            FillFields();
        }
        private void FillFields()
        {
            product = lbItem.Tag as Product;
            txtBoxName.Text = product.title;
            rtbDescr.AppendText(product.description);
            txtBoxCost.Text = Regex.Replace(product.cost, @"[^\d]", "", RegexOptions.Compiled);          
        }
        private string GetText(RichTextBox rtb)
        {
            var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text.Trim(new char[] { '\r', '\n' });
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebRequest request = WebRequest.Create(staticRequestResponse.GetMarketEdit(groupId, access_token, txtBoxName.Text, GetText(rtbDescr), txtBoxCost.Text, product.photoID, product.id));
                string response = staticRequestResponse.GetResponseJson(request);
                //MessageBox.Show(response);               
                var spItem = lbItem.Content as StackPanel;
                var spDescr = spItem.Children[1] as StackPanel;
                var txtBlockTitle = spDescr.Children[0] as TextBlock;
                var txtBlockDescr = spDescr.Children[1] as TextBlock;
                var txtBlockCost = spDescr.Children[2] as TextBlock;
                txtBlockTitle.Text = txtBoxName.Text;
                txtBlockDescr.Text = GetText(rtbDescr);
                txtBlockCost.Text = txtBoxCost.Text;
                fw.lbGroupProducts.SelectedItem = lbItem;
            }
            catch
            {
                MessageBox.Show("Fail");

            }
            
        }
    }
}
