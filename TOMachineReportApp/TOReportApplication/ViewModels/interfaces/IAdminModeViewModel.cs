﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.ViewModels.interfaces
{
    public interface IAdminModeViewModel
    {
        void Dispose();
        Action SearchButtonClickedAction { get; set; }
    }
}