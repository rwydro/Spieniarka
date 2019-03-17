using System;
using System.Timers;
using TOReportApplication.Logic.Enums;
using TOReportApplication.ViewModels.interfaces;

namespace TOReportApplication.Logic
{
    public class MyTimer
    {
        private Timer timer;
        public event EventHandler Elapsed;

        private void SetTimer()
        { 
            timer = new Timer(20*1000);
            timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
        }

        public void SetTimerAction(TimerActionEnum action)
        {
            switch (action)
            {
                case TimerActionEnum.Reset:
                    this.ResetTimer();
                    break;
                case TimerActionEnum.Start:
                    this.timer.Stop();        
                    break;
                case TimerActionEnum.Stop:
                    this.timer.Stop();
                    break;
                case TimerActionEnum.Set:
                    SetTimer();
                    break;                   
                default:
                    return;
            }
        }

        private void ResetTimer()
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
