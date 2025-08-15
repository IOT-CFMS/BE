using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel.QueryRequest
{
    public class SkincareRoutineQueryRequest : BaseQuery
    {
        public string? Name { get; set; }
        public List<int> SkinTypeIds { get; set; } = new List<int>();
    }
}
