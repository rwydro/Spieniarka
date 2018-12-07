using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class MaterialTypeModel
    {
        public MaterialTypeDto[] Type { get; set; }
    };

    public class MaterialTypeDto
    {
        public string Name { get; set; }
    };
}
