using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class AnswerResponseModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int SkinTypeId { get; set; }
        public string SkinType { get; set; } // Instead of ID, return SkinType name
    }
}
