using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TOReportApplication.Logic
{
    public static class MessageBoxHelper
    {
        public static void ShowMessageBox(string info, MessageBoxIcon icon)
        {
            MessageBox.Show(info, "Error", MessageBoxButtons.OK,
                icon);
        }
    }
}
