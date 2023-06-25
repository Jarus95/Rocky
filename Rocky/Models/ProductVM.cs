using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocky.Models
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> selectListItems1 { get; set; }
        public IEnumerable<SelectListItem> selectListItems2 { get; set; }
    }
}
