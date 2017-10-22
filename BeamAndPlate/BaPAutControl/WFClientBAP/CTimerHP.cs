using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TimerHPET
{
    class CTimerHP : IDisposable
    {
        private int mTimerId;
        private TimerEventHandler mHandler;  // NOTE: declare at class scope so garbage collector doesn't release it!!!
     
        //private delegate void TestEventHandler(int tick, TimeSpan span);
        public delegate void TimerEventHandler(int id, int msg, IntPtr user, int dw1, int dw2);

        public event TimerEventHandler Tick;

        public int Interval
        {
            get;
            set;
        }

        private bool _isOn;
        public bool IsOn
        {
            get { return _isOn; }
            set { _isOn = value; }
        }

        public CTimerHP()
        {
            Interval = 1;
            IsOn = false;           
        }

        public void Start()
        {
            if (!IsOn)
            {
                if (timeBeginPeriod(Interval) == 0)
                {
                    mHandler = new TimerEventHandler(TimerCallback);
                    mTimerId = timeSetEvent(Interval, 0, mHandler, IntPtr.Zero, EVENT_TYPE);
                    IsOn = true;
                }
                else
                {
                    Exception e = new Exception("Impossible de créer le Timer Multimedia");                     
                    throw e;
                }
            }
        }

        public void Stop()
        {
            if (IsOn)
            {
                int err = timeKillEvent(mTimerId);
                timeEndPeriod(Interval);
                // Ensure callbacks are drained
                System.Threading.Thread.Sleep(100);
                IsOn = false;
            }
        }
        public void Dispose()
        {
            Stop();       
        }       

        public virtual void TimerCallback(int id, int msg, IntPtr user, int dw1, int dw2)
        {
          if(Tick!=null) Tick(id,msg,user,dw1,dw2);
        }

        private const int TIME_PERIODIC = 1;
        private const int EVENT_TYPE = TIME_PERIODIC;// + 0x100;  // TIME_KILL_SYNCHRONOUS causes a hang ?!
        [DllImport("winmm.dll")]
        private static extern int timeSetEvent(int delay, int resolution, TimerEventHandler handler, IntPtr user, int eventType);
        [DllImport("winmm.dll")]
        private static extern int timeKillEvent(int id);
        [DllImport("winmm.dll")]
        private static extern int timeBeginPeriod(int msec);
        [DllImport("winmm.dll")]
        private static extern int timeEndPeriod(int msec);
    }
    
}
