using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vkStoreAPI
{
    class Product
    {
        public string id;
        public string title;
        public string description;
        public string cost; //cost;
        public string thumb_photo;
        public Product(string id, string title, string description, string cost)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.cost = cost;
        }
    }
}
