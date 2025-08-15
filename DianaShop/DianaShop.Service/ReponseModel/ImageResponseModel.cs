using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class ImageResponseModel
    {
        public int Id { get; set; }
        public string Base64Image { get; set; }
        public string ContentType { get; set; }
        public bool IsDeleted { get; set; }
        public List<int> PostIds { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
