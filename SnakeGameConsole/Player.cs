using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace SnakeGameConsole
{
    [DataContract]
    public class Player
    {
        [DataMember]
        public int MaxScore { get; private set; }

        private readonly DataContractJsonSerializer JsonPlayer = new DataContractJsonSerializer(typeof(Player));
        public Player()
        {
            PlayerUpdate();
        }

        public void PlayerSave()
        {
            if (!Directory.Exists("data")) Directory.CreateDirectory("data"); 
            using (var file = new FileStream("data\\player.json", FileMode.OpenOrCreate))
            {
                JsonPlayer.WriteObject(file, this);
            }
        }
        public void PlayerUpdate()
        {
            if (!Directory.Exists("data")) Directory.CreateDirectory("data");
            using (var file = new FileStream("data\\player.json", FileMode.OpenOrCreate))
            {
                try
                {
                    MaxScore = (JsonPlayer.ReadObject(file) as Player).MaxScore;
                }
                catch(SerializationException) 
                {
                    MaxScore = 0;
                    JsonPlayer.WriteObject(file, this); 
                }
            }
        }
        public void UpdateScore(int score)
        {
            MaxScore = score > MaxScore ? score : MaxScore;
        }
    }
}
