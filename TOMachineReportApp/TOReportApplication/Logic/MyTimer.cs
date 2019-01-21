using System;
using System.Timers;

namespace TOReportApplication.Logic
{
    public class MyTimer
    {
        private Timer timer;
        public event EventHandler Elapsed;

        public void SetTimer()
        { 
            timer = new Timer(90*1000);
            timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
        }

        public void ResetTimer()
        {
            if (timer.Enabled)
            {
                timer.Stop();
            }

            timer.Start();  
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (Elapsed != null)
                Elapsed(sender, e);
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
