using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class SimpleWallTime
    {
        private const int RunDurationMs = 60 * 1000;

        private ManualResetEvent _stopEvent;
        private List<Task> _activeTasks;

        public void Start()
        {
            if (_stopEvent != null)
            {
                throw new InvalidOperationException("Already running...");
            }

            _stopEvent = new ManualResetEvent(false);
            _activeTasks = new List<Task>
            {
                Task.Factory.StartNew(DoThreadSleep, TaskCreationOptions.LongRunning),
                Task.Factory.StartNew(DoTaskDelay, TaskCreationOptions.LongRunning),
                Task.Factory.StartNew(Do100PercentCPU, TaskCreationOptions.LongRunning),
            };
        }

        public void Stop()
        {
            if (_stopEvent == null)
            {
                throw new InvalidOperationException("Not running...");
            }

            _stopEvent.Set();

            Task.WhenAll(_activeTasks).Wait();

            _stopEvent.Dispose();
            _stopEvent = null;
            _activeTasks = null;
        }

        public void Run()
        {
            StartThreadSleep();
            StartTaskDelay();
            StartThreadCPU();
            Thread.Sleep(RunDurationMs);
        }

        private void DoThreadSleep()
        {
            Console.WriteLine($"Starting {nameof(DoThreadSleep)}.");
            do
            {
                StartThreadSleep();
            }
            while (!_stopEvent.WaitOne(RunDurationMs));

            Console.WriteLine($"Exiting {nameof(DoThreadSleep)}.");
        }

        private void StartThreadSleep()
        {
            var t10 = new Thread(OnSleep10);
            t10.Name = "Sleep10";
            t10.IsBackground = true;

            var t20 = new Thread(OnSleep20);
            t20.Name = "Sleep20";
            t20.IsBackground = true;

            var t30 = new Thread(OnSleep30);
            t30.Name = "Sleep30";
            t30.IsBackground = true;

            var t40 = new Thread(OnSleep40);
            t40.Name = "Sleep40";
            t40.IsBackground = true;

            t10.Start();
            t20.Start();
            t30.Start();
            t40.Start();
        }

        private void OnSleep10(object parameter)
        {
            Thread.Sleep(10 * 1000);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | slept 10s ");
        }

        private void OnSleep20(object parameter)
        {
            Thread.Sleep(20 * 1000);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | slept 20s ");
        }

        private void OnSleep30(object parameter)
        {
            Thread.Sleep(30 * 1000);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | slept 30s ");
        }

        private void OnSleep40(object parameter)
        {
            Thread.Sleep(40 * 1000);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | slept 40s ");
        }

        private void DoTaskDelay()
        {
            Console.WriteLine($"Starting {nameof(DoTaskDelay)}.");

            do
            {
                StartTaskDelay();
            }
            while (!_stopEvent.WaitOne(RunDurationMs));

            Console.WriteLine($"Exiting {nameof(DoTaskDelay)}.");
        }

        private void StartTaskDelay()
        {
            Task.Run(OnDelay10);
            Task.Run(OnDelay20);
            Task.Run(OnDelay30);
            Task.Run(OnDelay40);
        }

        private async Task OnDelay10()
        {
            await Task.Delay(10 * 1000);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | delayed 10s ");
        }

        private async Task OnDelay20()
        {
            await Task.Delay(20 * 1000);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | delayed 20s ");
        }

        private async Task OnDelay30()
        {
            await Task.Delay(30 * 1000);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | delayed 30s ");
        }

        private async Task OnDelay40()
        {
            await Task.Delay(40 * 1000);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | delayed 40s ");
        }

        private void Do100PercentCPU()
        {
            Console.WriteLine($"Starting {nameof(Do100PercentCPU)}.");
            do
            {
                StartThreadCPU();
            }
            while (!_stopEvent.WaitOne(RunDurationMs));

            Console.WriteLine($"Exiting {nameof(Do100PercentCPU)}.");
        }

        private void StartThreadCPU()
        {
            var t1 = new Thread(OnCPU1);
            t1.Name = "CPU1";
            t1.IsBackground = true;

            var t2 = new Thread(OnCPU2);
            t2.Name = "CPU2";
            t2.IsBackground = true;

            var t3 = new Thread(OnCPU3);
            t3.Name = "CPU3";
            t3.IsBackground = true;

            t1.Start();
            t2.Start();
            t3.Start();
        }

        private void OnCPU1(object parameter)
        {
            var sw = Stopwatch.StartNew();
            ComputeSum(1000000);
            sw.Stop();
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | CPU1 worked {sw.Elapsed} ");
        }

        private void OnCPU2(object parameter)
        {
            var sw = Stopwatch.StartNew();
            ComputeSum(1000000);
            sw.Stop();
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | CPU2 worked {sw.Elapsed} ");
        }

        private void OnCPU3(object parameter)
        {
            var sw = Stopwatch.StartNew();
            ComputeSum(1000000);
            sw.Stop();
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3} | CPU3 worked {sw.Elapsed} ");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ComputeSum(long top)
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < RunDurationMs / 2)
            {
                long s = Sum(top);

                // ensure s/Sum are not optimized away
                if (s != top * (top + 1) / 2)
                {
                    Console.WriteLine("it will never happen");
                }

                if ((_stopEvent != null) && _stopEvent.WaitOne(0))
                {
                    break;
                }
            }

            sw.Stop();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private long Sum(long top)
        {
            long s = 0;
            for (int i = 1; i <= top; i++)
            {
                s += i;
            }

            return s;
        }
    }
}
