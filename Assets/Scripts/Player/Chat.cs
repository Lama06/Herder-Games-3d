using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class Chat : MonoBehaviour
    {
        [SerializeField] private float MaxMessageTime;
        
        private readonly List<Message> Messages = new();

        public void SendChatMessage(string msg)
        {
            Messages.Add(new Message {Text = msg});
        }

        public void SendChatMessage(Lehrer.Lehrer sender, string msg)
        {
            SendChatMessage($"[{sender.GetName()}] {msg}");
        }

        public void SendChatMessageDurchsage(string durchsage)
        {
            SendChatMessage($"[Durchsage] {durchsage}");
        }

        public IEnumerable<string> ChatMessages
        {
            get
            {
                var strings = new List<string>();
                foreach (var message in Messages)
                {
                    strings.Add(message.Text);
                }

                return strings;
            }
        }

        private void Update()
        {
            for (var i = 0; i < Messages.Count; i++)
            {
                var msg = Messages[i];
                msg.Update();

                if (msg.ShouldBeRemoved(MaxMessageTime))
                {
                    Messages.RemoveAt(i);
                }
            }
        }

        private class Message
        {
            public float TimeSinceSent;
            public string Text;

            public void Update()
            {
                TimeSinceSent += Time.deltaTime;
            }

            public bool ShouldBeRemoved(float maxTime)
            {
                return TimeSinceSent >= maxTime;
            }
        }
    }
}