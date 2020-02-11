using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XmlSigner.Data.Models
{
    public class DemoData : BaseModel
    {
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string NID { get; set; }
    }
}
