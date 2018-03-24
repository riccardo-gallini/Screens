using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Collections.Concurrent;
using System.Threading;

namespace Screens
{

    public class MessageQueue : IDisposable
    {
        private ConcurrentQueue<Message> queue;
        private AutoResetEvent message_waiter;

        public MessageQueue()
        {
            queue = new ConcurrentQueue<Message>();
            message_waiter = new AutoResetEvent(false);
        }

        public bool GetMessage(ref Message msg)
        {

            // can be called only from the application thread
            // TODO: check this is the right thread

            while (true)
            {
                message_waiter.WaitOne();

                if (queue.TryDequeue(out msg))
                    return true;
            }

        }

        public Message PeekMessage()
        {
            Message msg = null;
            queue.TryPeek(out msg);
            return msg;
        }

        public void PostMessage(Message msg)
        {
            queue.Enqueue(msg);
            message_waiter.Set();
        }

        public void PostMessage(WM_MessageType message_type, object par)
        {
            PostMessage(new Message(message_type, par));
        }

        public void Dispose()
        {
            message_waiter.Dispose();
            message_waiter = null;
        }
    }
    
}