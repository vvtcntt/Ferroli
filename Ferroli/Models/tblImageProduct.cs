using System;
using System.Collections.Generic;

namespace Ferroli.Models
{
    public partial class tblImageProduct
    {
        public int id { get; set; }
        public Nullable<int> idProduct { get; set; }
        public string Name { get; set; }
        public Nullable<int> Type { get; set; }
        public string Images { get; set; }
    }
}
