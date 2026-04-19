using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASHOP.DAL.Models
{
    public class CategoryTranslation
    {   public int Id { get; set; }
        public string Name { get; set; }
        public string lang { get; set; } = "en";
        public int CategoryId { get; set; }
         public Category category { get; set; }
    }
}
