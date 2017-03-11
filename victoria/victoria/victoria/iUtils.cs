using System;
namespace victoria
{
    public interface iUtils
    {
        void newBlockingQueue(String QueueName);

        void addToQueue(String QueueName, Action a);

        Action takeFromQueue(String QueueName);

        void startNewLoopingThread(String ThreadName, Action a);

        void startNewThreadPool(String ThreadName);

        void addToThreadPool(String ThreadName, Action a);

        void emptyThreadPool(String ThreadName);

        void startNewSelfTerminatingThread<T,G>(Action<T,G> a, T s, G g);

        void Sleep(int miliseconds);

        void newTimer(String TimerName,  Action a, int interval);

        void resetTimer(String TimerName);
        
    }
}
