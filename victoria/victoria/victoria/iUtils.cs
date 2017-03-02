using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace victoria
{
    public interface iUtils
    {
        void newBlockingQueue(String QueueName);

        void addToQueue(String QueueName, Action a);

        Action takeFromQueue(String QueueName);

        void startNewThread(String ThreadName, Action a);

        void quickProcessThread<T>(Action<T> a, T s);

        void Sleep(int miliseconds);
        
    }
}
