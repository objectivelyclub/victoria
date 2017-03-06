using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;
using victoria.Droid;
using System.Threading;
using System.Collections.Concurrent;
using ZXing.Common;

[assembly: Dependency(typeof(Utils_Android))]

namespace victoria.Droid
{
    public class Utils_Android : iUtils
    {
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
    }
}