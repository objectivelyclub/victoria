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

[assembly: Dependency(typeof(Utils_Android))]

namespace victoria.Droid
{
    public class Utils_Android : iUtils
    {
        Dictionary<string, Thread> ThreadMap = new Dictionary<string, Thread>();
        Dictionary<string, BlockingCollection<Action>> QueueMap = new Dictionary<string, BlockingCollection<Action>>();

        public Utils_Android()
        {

        }

        public void quickProcessThread<T>(Action<T> a, T s)
        {
            new Thread(() => a(s)).Start();
        }

        public void startNewThread(string ThreadName, Action a)
        {
            ThreadMap[ThreadName] = new Thread(() => a());
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
    }
}