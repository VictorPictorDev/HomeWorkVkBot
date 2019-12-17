using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace Bot.Bl.Monitoring
{
    public class DateTimeActionTimer : IMonitoring
    {
        public DateTime Date { get; private set; }
        public int Interval { get; private set; }
        public bool IsRepeat { get; private set; }
        private Timer _timer = new System.Timers.Timer();
        public event EventHandler Tick;
        public DateTimeActionTimer(DateTime date,int interval,bool isrepeat)
        {
            Date = date;
            Interval = interval;
            IsRepeat = isrepeat;
        }
        private void TimerLogic()
        {
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = Interval;
            _timer.Start();
        }
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            if (now.Minute == Date.Minute && now.Hour == Date.Hour)
            {
                Tick?.Invoke(this, new EventArgs());
                if (!IsRepeat)
                {
                    _timer.Stop();
                }
            }
        }
        public void Start()
        {
            TimerLogic();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
