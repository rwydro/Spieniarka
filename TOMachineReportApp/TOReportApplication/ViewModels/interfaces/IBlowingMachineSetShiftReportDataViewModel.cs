﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.Model;

namespace TOReportApplication.ViewModels.interfaces
{
    public interface IBlowingMachineSetShiftReportDataViewModel
    {
        Action<MaterialTypeMenuModel> OnSendMaterialTypeInfo { get; set; }
    }
}
