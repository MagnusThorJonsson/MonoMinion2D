using System;
using System.Collections.Generic;

namespace MonoMinion.Messaging
{
    /// <summary>
    /// Message container struct
    /// </summary>
    public struct Message
    {
        #region Variables & Properties
        /// <summary>
        /// Message type
        /// </summary>
        public string Type;

        /// <summary>
        /// The object that sent the message
        /// </summary>
        public object Sender;

        /// <summary>
        /// The object that is to receive the message
        /// </summary>
        public object Destination;

        /// <summary>
        /// The message payload information
        /// </summary>
        public object Payload;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a message
        /// </summary>
        /// <param name="type">The message type</param>
        /// <param name="sender">The message sender</param>
        /// <param name="destination">The message receiver</param>
        /// <param name="payload">The message data payload</param>
        public Message(string type, object sender, object destination, object payload)
        {
            Type = type;
            Sender = sender;
            Destination = destination;
            Payload = payload;
        }
        #endregion
    }


    // TODO: Make it static?
    /// <summary>
    /// Messaging Manager.
    /// Messaging system main handler, double buffered for your safety.
    /// 
    /// Based on code found on:
    ///     http://astroboid.com/2011/04/messaging-systems-for-games-and-xna.html
    /// </summary>
    public class MessageManager
    {
        #region Variables 
        private List<Message> lastFrameMessages;
        private List<Message> currentFrameMessages;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates and initializes the message manager
        /// </summary>
        public MessageManager()
        {
            lastFrameMessages = new List<Message>();
            currentFrameMessages = new List<Message>();
        }
        #endregion


        #region Main Methods
        /// <summary>
        /// Updates the message manager (call per frame)
        /// </summary>
        public void Update()
        {
            List<Message> t = lastFrameMessages;
            lastFrameMessages = currentFrameMessages;
            currentFrameMessages = t;
            currentFrameMessages.Clear();
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="message">The message to send</param>
        public void Send(Message message)
        {
            currentFrameMessages.Add(message);
        }
        #endregion


        /// <summary>
        /// Searches for messages based on the criteria inputted
        /// </summary>
        /// <param name="criteria">The criteria</param>
        /// <param name="result">The reference to the list object</param>
        public void FindMessage(Predicate<Message> criteria, List<Message> result)
        {
            for (int i = 0; i < lastFrameMessages.Count; i++)
            {
                Message m = lastFrameMessages[i];
                if (criteria.Invoke(m))
                {
                    result.Add(m);
                }
            }
        }

        /// <summary>
        /// Searches for messages by their destination
        /// </summary>
        /// <param name="destination">The destination object</param>
        /// <param name="result">The reference to the list object</param>
        public void FindMessagesByDestination(object destination, List<Message> result)
        {
            for (var i = 0; i < lastFrameMessages.Count; i++)
            {
                var m = lastFrameMessages[i];
                if (m.Destination != null && m.Destination.Equals(destination))
                {
                    result.Add(m);
                }
            }
        }

        /// <summary>
        /// Searches for messages by their sender
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="destination">The destination object</param>
        public void FindMessagesBySender(Object sender, List<Message> result)
        {
            for (var i = 0; i < lastFrameMessages.Count; i++)
            {
                var m = lastFrameMessages[i];
                if (m.Sender.Equals(sender))
                {
                    result.Add(m);
                }
            }
        }


        /// <summary>
        /// Searches for messages by their type
        /// </summary>
        /// <param name="type">The type of message</param>
        /// <param name="result">The reference to the list object</param>
        public void FindMessagesByType(string type, List<Message> result)
        {
            for (var i = 0; i < lastFrameMessages.Count; i++)
            {
                var m = lastFrameMessages[i];
                if (m.Type.Equals(type))
                {
                    result.Add(m);
                }
            }
        }

        /// <summary>
        /// Searches for messages by their type and destination
        /// </summary>
        /// <param name="type">The type of message</param>
        /// <param name="destination">The destination object</param>
        /// <param name="result">The reference to the list object</param>
        public void FindMessagesByTypeAndDestination(string type, object destination, List<Message> result)
        {
            FindMessage(type, destination, lastFrameMessages, result);
        }


        /// <summary>
        /// Searches for messages by their type and sender
        /// </summary>
        /// <param name="type">The type of message</param>
        /// <param name="sender">The sender object</param>
        /// <param name="result">The reference to the list object</param>
        public void FindMessagesByTypeAndSender(string type, object sender, List<Message> result)
        {
            for (int i = 0; i < lastFrameMessages.Count; i++)
            {
                var m = lastFrameMessages[i];
                if (m.Type.Equals(type) && m.Sender.Equals(sender))
                {
                    result.Add(m);
                }
            }
        }

        /// <summary>
        /// Searches for messages in the first frame (hack that should probably not be used
        /// </summary>
        /// <param name="type">The type of message</param>
        /// <param name="destination">The destination object</param>
        /// <param name="result">The reference to the list object</param>
        public void FindFutureMessagesByTypeAndDestination(string type, object destination, List<Message> result)
        {
            FindMessage(type, destination, currentFrameMessages, result);
        }



        #region Static Helpers
        /// <summary>
        /// Searches for messages within the given list
        /// </summary>
        /// <param name="type"></param>
        /// <param name="destination"></param>
        /// <param name="frameMessages"></param>
        /// <param name="result"></param>
        private static void FindMessage(string type, object destination, List<Message> frameMessages, List<Message> result)
        {
            for (var i = 0; i < frameMessages.Count; i++)
            {
                var m = frameMessages[i];
                if (m.Destination != null && m.Type.Equals(type) && m.Destination.Equals(destination))
                {
                    result.Add(m);
                }
            }
        }
        #endregion
    }
}
