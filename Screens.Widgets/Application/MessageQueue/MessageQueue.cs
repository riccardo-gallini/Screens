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
        private Application owningApp;

        public MessageQueue(Application app)
        {
            owningApp = app;
            queue = new ConcurrentQueue<Message>();
            message_waiter = new AutoResetEvent(false);
        }

        public bool GetMessage(ref Message msg)
        {
            if (Application.Current != owningApp)
            {
                throw new ThreadStateException("<DEBUG> message loop not running on this thread!");
            }

            while (true)
            {

                if (queue.TryDequeue(out msg)) return true;

                message_waiter.WaitOne();
            
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