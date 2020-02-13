using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Logic.Enums
{

    public enum DataContextEnum
    {
        [Description("Forma")]
        FormViewModel,
        [Description("Spieniarka")]
        BlowingMachineViewModel,
        [Description("Spieniarka Ciągła")]
        ContinuousBlowingMachineViewModel,
        [Description("historia Bloków")]
        BlockHistoryViewModel
    }

}
