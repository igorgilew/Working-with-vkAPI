using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vkStoreAPI
{
    class User
    {
        public string id;
        public string access_token;
        public string url;
        public User(string url)
        {
            this.url = url;
            ParseUrl(url);
        }
        private void ParseUrl(string url)
        {
            access_token = (url.Substring(0,url.IndexOf("&"))).Substring(url.IndexOf("=")+1);
            id = url.Substring(url.IndexOf("user_id"), url.IndexOf("&state") - url.IndexOf("user_id"));
            id = id.Substring(id.IndexOf("=")+1);
        }
    }
}
