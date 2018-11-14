using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
namespace vkStoreAPI
{
    static class staticRequestResponse
    {
        public static string GetQuerryGroups(string user_id, string access_token)
        {
            return string.Format("https://api.vk.com/method/groups.get?user_id={0}&extended=1&filter=admin&access_token={1}&v=5.87", user_id, access_token);
        }
        public static string GetResponseJson(WebRequest request)
        {
            string json = "";
            request.Method = "GET";
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
        public static string POSTRequest(string url, NameValueCollection parametrs)
        {
            using (var webClient = new WebClient())
            {
                // Посылаем параметры на сервер
                // Может быть ответ в виде массива байт
                var response = webClient.UploadValues(url, parametrs);
                return Encoding.UTF8.GetString(response, 0, response.Length);
            }
        }
        public static string POSTRequest(string url, string filePath)
        {
            using (var webClient = new WebClient())
            {
                // Посылаем параметры на сервер
                // Может быть ответ в виде массива байт
                var response = webClient.UploadFile(url, filePath);
                return Encoding.UTF8.GetString(response, 0, response.Length);
            }
        }
        public static string GetQuerryProducts(string group_id, string access_token)
        {
            return string.Format("https://api.vk.com/method/market.get?owner_id=-{0}&access_token={1}&v=5.87", group_id, access_token);
        }
        public static string DeleteProduct(string group_id, string access_token, string item_id)
        {
            return string.Format("https://api.vk.com/method/market.delete?owner_id=-{0}&item_id={1}&access_token={2}&v=5.87", group_id, item_id, access_token);
        }
        public static string GetMarketUploadServer(string groupId, string access_token)
        {
            return string.Format("https://api.vk.com/method/photos.getMarketUploadServer?group_id={0}&main_photo=1&access_token={1}&v=5.87", groupId, access_token);
        }
        public static string GetSaveMarketPhoto(string groupId, string access_token, string photo, string server, string hash, string crop_data, string crop_hash)
        {
            return string.Format("https://api.vk.com/method/photos.saveMarketPhoto?group_id={0}&photo={1}&server={2}&hash={3}&crop_data={4}&crop_hash={5}&access_token={6}&v=5.87", groupId, photo, server, hash, crop_data, crop_hash, access_token);
        }
        public static HttpResponseMessage POSTLoadImageToServer(string filePath, string upload_url)
        {
            var httpClient = new HttpClient();
            var form = new MultipartFormDataContent();
            FileStream fs = File.OpenRead(filePath);
            var streamContent = new StreamContent(fs);
            var imageContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            form.Add(imageContent, "file", Path.GetFileName(filePath));

            var response = httpClient.PostAsync(upload_url, form).Result;
            return response;
        }
        public static string GetMarketAdd(string groupId, string access_token, string name, string description, string price, string main_photo_id)
        {
            return string.Format("https://api.vk.com/method/market.add?owner_id=-{0}&name={1}&description={2}&category_id=1&price={3}&main_photo_id={4}&access_token={5}&v=5.87", groupId, name, description, price, main_photo_id, access_token);
        }
    }
}
