﻿using System;
using System.Messaging;

namespace communicationModule
{
    public class MethodsClass
    {
        public void SendMessage(string msg)
        {
            using (MessageQueue queue = new MessageQueue())
            {
                queue.Path = @".\private$\Queue";
                if (!MessageQueue.Exists(queue.Path))
                {
                    MessageQueue.Create(queue.Path);
                }
                System.Messaging.Message mymessage = new System.Messaging.Message();
                mymessage.Body = msg;
                queue.Send(mymessage);
            }
        }

        public string ReceiveMessage()
        {
            using (MessageQueue queue = new MessageQueue())
            {
                queue.Path = @".\private$\Queue";
                if (!MessageQueue.Exists(queue.Path))
                {
                    MessageQueue.Create(queue.Path);
                    return " ";
                }
                System.Messaging.Message mymessage = new System.Messaging.Message();
<<<<<<< HEAD
                mymessage = queue.Receive(new TimeSpan(0, 0, 5));
=======
                mymessage = queue.Receive();
>>>>>>> feature/ServerAPI-005.OrdersManagementController
                mymessage.Formatter = new XmlMessageFormatter(new String[] { "System.String, mscorlib" });
                string msg = mymessage.Body.ToString();
                return msg;
            }
        }
    }
}
