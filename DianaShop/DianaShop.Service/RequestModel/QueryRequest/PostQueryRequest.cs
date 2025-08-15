using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel.QueryRequest
{
    public class PostQueryRequest : BaseQuery
    {
        public string? Title { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public DateTime? MinCreatedDate { get; set; }
        public DateTime? MaxCreatedDate { get; set; }
        public DateTime? MinPublishedDate { get; set; }
        public DateTime? MaxPublishedDate { get; set; }
        public PostSort SortBy { get; set; } = PostSort.Default;
        public bool Ascending { get; set; } = true;
        public List<int> TagIds { get; set; } = new List<int>();
    }
    public enum PostSort
    {
        [Display(Name = "Default")]
        Default, // Sort by ID
        [Display(Name = "Title")]
        Title,
        [Display(Name = "Created Date")]
        CreatedDate,
        [Display(Name = "Published Date")]
        PublishedDate,
        [Display(Name = "Updated Date")]
        UpdatedDate
    }
}
