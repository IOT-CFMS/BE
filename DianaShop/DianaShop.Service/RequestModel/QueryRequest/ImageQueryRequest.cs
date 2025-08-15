using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel.QueryRequest
{
    public class ImageQueryRequest : BaseQuery
    {
        public string? ImageUrl { get; set; }
    }
}
