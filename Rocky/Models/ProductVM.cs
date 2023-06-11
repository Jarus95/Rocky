using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocky.Models
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> selectListItems { get; set; }
    }
}
