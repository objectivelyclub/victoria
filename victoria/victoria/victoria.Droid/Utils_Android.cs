using System;
using System.Collections.Generic;

using Xamarin.Forms;
using victoria.Droid;
using System.Threading;
using System.Collections.Concurrent;

[assembly: Dependency(typeof(Utils_Android))]

namespace victoria.Droid
{
    public class Utils_Android : iUtils
    {
        Dictionary<string, System.Timers.Timer> TimerMap = new Dictionary<string, System.Timers.Timer>();
        Dictionary<string, Thread> ThreadMap = new Dictionary<string, Thread>();
        Dictionary<string, Thread> ThreadPoolMap = new Dictionary<string, Thread>();
        Dictionary<string, BlockingCollection<Action>> QueueMap = new Dictionary<string, BlockingCollection<Action>>();

        public Utils_Android()
        {

        }

        public void startNewSelfTerminatingThread<T,G>(Action<T,G> a, T s, G g)
        {
            new Thread(() => a(s,g)).Start();
        }

        public void startNewLoopingThread(string ThreadName, Action a)
        {
            ThreadMap[ThreadName] = new Thread(() =>
            {
                while (true)
                {
                    a();
                }
            });
            ThreadMap[ThreadName].Start();
        }

        public void Sleep(int miliseconds)
        {
            Thread.Sleep(miliseconds);
        }

        public void newBlockingQueue(string QueueName)
        {
            QueueMap.Add(QueueName, new BlockingCollection<Action>());
        }

        public void addToQueue(string QueueName, Action a)
        {
            QueueMap[QueueName].Add(a);
        }

        public Action takeFromQueue(string QueueName)
        {
            return QueueMap[QueueName].Take();
        }

        public void startNewThreadPool(string ThreadPoolName)
        {
            newBlockingQueue(ThreadPoolName);

            ThreadMap[ThreadPoolName] = new Thread(() =>
            {
                while (true)
                {
                    QueueMap[ThreadPoolName].Take()();
                }
            });
            ThreadMap[ThreadPoolName].Start();
        }

        public void addToThreadPool(string ThreadPoolName, Action a)
        {
            addToQueue(ThreadPoolName, a);
        }

        public byte[] fixStupidZXINGByteArray(byte[] b)
        {
            return null;

        }

        public void emptyThreadPool(string ThreadPoolName)
        {
            while (QueueMap[ThreadPoolName].Count>0)
            {
                QueueMap[ThreadPoolName].Take();
            }
        }

        public void newTimer(string TimerName, Action a, int interval)
        {
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Enabled = false;
            timer.AutoReset = false;
            timer.Elapsed += (s, e) => a();
            TimerMap.Add(TimerName, timer);
        }

        public void resetTimer(string TimerName)
        {
            TimerMap[TimerName].Stop();
            TimerMap[TimerName].Start();
        }
    }
}