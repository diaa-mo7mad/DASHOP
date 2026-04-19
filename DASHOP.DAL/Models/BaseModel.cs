using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASHOP.DAL.Models
{
    public class BaseModel
    {

        public int Id { get; set; }

        public Status status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
