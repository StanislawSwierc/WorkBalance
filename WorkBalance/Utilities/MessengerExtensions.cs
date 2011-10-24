using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace WorkBalance.Utilities
{
    public static class MessengerExtensions
    {
        private static Action<NotificationMessage> CreateAction(string notification, Action action)
        {
            return message =>
            {
                if (message.Notification == notification)
                {
                    action();
                }
            };
        }

        public static void Register(this IMessenger messenger, object recipient, string notification, Action action)
        {
            messenger.Register<NotificationMessage>(recipient, CreateAction(notification, action));
        }

        public static void Unregister(this IMessenger messenger, object recipient, string notification, Action action)
        {
            messenger.Unregister<NotificationMessage>(recipient, CreateAction(notification, action));
        }


        private static Action<NotificationMessage> CreateAction(string notification, Action<MessageBase> action)
        {
            return message =>
            {
                if (message.Notification == notification)
                {
                    action(message);
                }
            };
        }

        public static void Register(this IMessenger messenger, object recipient, string notification, Action<MessageBase> action)
        {
            messenger.Register<NotificationMessage>(recipient, CreateAction(notification, action));
        }

        public static void Unregister(this IMessenger messenger, object recipient, string notification, Action<MessageBase> action)
        {
            messenger.Register<NotificationMessage>(recipient, CreateAction(notification, action));
        }


        private static Action<NotificationMessage<T>> CreateAction<T>(string notification, Action<T> action)
        {
            return message =>
            {
                if (message.Notification == notification)
                {
                    action(message.Content);
                }
            };
        }

        public static void Register<T>(this IMessenger messenger, object recipient, string notification, Action<T> action)
        {
            messenger.Register<NotificationMessage<T>>(recipient, CreateAction(notification, action));
        }

        public static void Unregister<T>(this IMessenger messenger, object recipient, string notification, Action<T> action)
        {
            messenger.Unregister<NotificationMessage<T>>(recipient, CreateAction(notification, action));
        }


        public static void Send(this IMessenger messenger, string notification)
        {
            messenger.Send<NotificationMessage>(new NotificationMessage(notification));
        }
        public static void Send(this IMessenger messenger, string notification, object sender)
        {
            messenger.Send<NotificationMessage>(new NotificationMessage(sender, notification));
        }
        public static void Send(this IMessenger messenger, string notification, object sender, object target)
        {
            messenger.Send<NotificationMessage>(new NotificationMessage(sender, target, notification));
        }
        public static void Send<T>(this IMessenger messenger, string notification, T content)
        {
            messenger.Send<NotificationMessage<T>>(new NotificationMessage<T>(content, notification));
        }
        public static void Send<T>(this IMessenger messenger, string notification, T content, object sender)
        {
            messenger.Send<NotificationMessage<T>>(new NotificationMessage<T>(sender, content, notification));
        }
        public static void Send<T>(this IMessenger messenger, string notification, T content, object sender, object target)
        {
            messenger.Send<NotificationMessage<T>>(new NotificationMessage<T>(sender, target, content, notification));
        }
    }
}
