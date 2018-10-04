using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Castle
{
    public interface IShell
    {
        void Run();
    }
    public class Shell: IShell
    {
        public Shell(MainWindow window)
        {
           this. window = window;
        }

        public virtual MainWindow window { get; set; }
        public void Run()
        {
            window.Show();
        }
    }
}
