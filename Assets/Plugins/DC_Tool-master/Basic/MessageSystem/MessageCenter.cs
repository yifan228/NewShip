using System;
using System.Collections.Generic;

namespace DC_Tool.Basic
{
    public class MessageCenter
    {
        private static Dictionary<Type, Action> messageSubscribers = new Dictionary<Type, Action>();

        public delegate void MessageCallbackWithData<T>(T data) where T : IMessageWithData;
        private static Dictionary<Type, Delegate> messageWithDataSubscribers = new Dictionary<Type, Delegate>();

        public static void RegisterMessage<T>(Action callback) where T : IMessage
        {
            if (!messageSubscribers.ContainsKey(typeof(T)))
            {
                messageSubscribers.Add(typeof(T), callback);
            }
            else
            {
                messageSubscribers[typeof(T)] += callback;
            }
        }

        public static void RegisterMessage<T>(MessageCallbackWithData<T> callback) where T : IMessageWithData
        {
            if (!messageWithDataSubscribers.ContainsKey(typeof(T)))
            {
                messageWithDataSubscribers.Add(typeof(T), callback);
            }
            else
            {
                Delegate currentDelegate;
                if (messageWithDataSubscribers.TryGetValue(typeof(T), out currentDelegate))
                {
                    messageWithDataSubscribers[typeof(T)] = Delegate.Combine(currentDelegate, callback);
                }
            }
        }

        public static void UnregisterMessage<T>(Action callback) where T : IMessage
        {
            if (messageSubscribers.ContainsKey(typeof(T)))
            {
                messageSubscribers[typeof(T)] -= callback;
            }
        }

        public static void UnregisterMessage<T>(MessageCallbackWithData<T> callback) where T : IMessageWithData
        {
            Delegate currentDelegate;
            if (messageWithDataSubscribers.TryGetValue(typeof(T), out currentDelegate))
            {
                messageWithDataSubscribers[typeof(T)] = Delegate.Remove(currentDelegate, callback);
            }
        }

        public static void PostMessage<T>() where T : IMessage
        {
            if (messageSubscribers.ContainsKey(typeof(T)) && messageSubscribers[typeof(T)] != null)
            {
                messageSubscribers[typeof(T)]();
            }
        }

        public static void PostMessage<T>(T postedMessage) where T : IMessageWithData
        {
            if (messageWithDataSubscribers.ContainsKey(typeof(T)) && messageWithDataSubscribers[typeof(T)] != null)
            {
                ((MessageCallbackWithData<T>)messageWithDataSubscribers[typeof(T)])(postedMessage);
            }
        }
    }
}