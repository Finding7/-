using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleApp2
{ 
    public class AlarmEventArgs : EventArgs
    {
        public DateTime AlarmTime { get; }
        public AlarmEventArgs(DateTime alarmTime) => AlarmTime = alarmTime;
    }

    public class AlarmClock
    {
        private readonly Timer _timer;
        private DateTime _alarmTime;

        public DateTime AlarmTime
        {
            get => _alarmTime;
            set
            {
                if (value <= DateTime.Now)
                    throw new ArgumentException("闹钟时间必须是未来的时间");
                _alarmTime = value;
            }
        }

        public delegate void AlarmEventHandler(object sender, AlarmEventArgs e);
        public delegate void TickEventHandler(object sender, EventArgs e);

        public event AlarmEventHandler Alarm;
        public event TickEventHandler Tick;

        public AlarmClock()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Tick?.Invoke(this, EventArgs.Empty);

            if (DateTime.Now >= AlarmTime)
            {
                Alarm?.Invoke(this, new AlarmEventArgs(AlarmTime));
                _timer.Stop(); // 触发后自动停止
            }
        }

        public void Start() => _timer.Start();

        public void SetNewAlarm(DateTime newTime)
        {
            _timer.Stop();
            AlarmTime = newTime;
            _timer.Start();
            Console.WriteLine($"新闹钟已设置: {newTime:HH:mm:ss}");
        }
    }

    class Program
    {
        static void Main()
        {
            var clock = new AlarmClock();

            clock.Tick += (s, e) =>
                Console.WriteLine($"嘀嗒... 当前时间：{DateTime.Now:HH:mm:ss}");

            clock.Alarm += (s, e) =>
                Console.WriteLine($"⏰ 时间到！设定的时间：{e.AlarmTime:HH:mm:ss}");

            while (true)
            {
                Console.WriteLine("\n请输入闹钟时间（格式：HH mm，例如 14 30）（输入exit退出）：");
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) continue;
                if (input.ToLower() == "exit") break;

                try
                {
                    var parts = input.Split();
                    if (parts.Length != 2 ||
                        !int.TryParse(parts[0], out int hour) ||
                        !int.TryParse(parts[1], out int minute))
                    {
                        throw new FormatException("输入格式错误，请使用'小时 分钟'格式");
                    }

                    var now = DateTime.Now;
                    var alarmTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

                    clock.SetNewAlarm(alarmTime);
                    clock.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"错误：{ex.Message}");
                }
            }
        }
    }
}
