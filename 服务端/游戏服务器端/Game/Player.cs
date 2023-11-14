using GameServer.Model;
using GameServer.Servers;
using GameServer.Tool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace GameServer.Game
{
    class Player
    {
        public int id = 0;
        public string name;
        public bool isLive = true;
        public float HP = 100;
        public int score=0;
        public Client client;
        public Room room;
        public DressUp dressup;

        public Vector3 velocity=new Vector3();
        public Vector3 position = new Vector3();
        public Quaternion rotation = new Quaternion();

        public List<Monster> localMonsters=new List<Monster>();

        public Vector3 coord =new Vector3();

        public Player(Client client,string id,string name,DressUp dressup)
        {
            this.client = client;
            int.TryParse(id, out this.id);
            this.name = name;
            this.dressup = dressup;
        }
        /// <summary>
        /// 获取json数据，所带bool为要添加的信息
        /// </summary>
        /// <param name="isDressUp"></param>
        /// <param name="isGame"></param>
        /// <returns></returns>
        public JObject GetJsonData(bool isDressUp,bool isGame)
        {
            JObject JPlayer = new JObject();

                JPlayer.Add("id", JToken.FromObject(id));
                JPlayer.Add("name", JToken.FromObject(name));
            if (isDressUp)
            {
                JPlayer.Add("dressUp", JToken.FromObject(dressup.GetJsonPackage().ToString()));
            }
            if(isGame)
            {
                JPlayer.Add("isLive", JToken.FromObject(isLive));
                JPlayer.Add("HP", JToken.FromObject(HP));
            }
            return JPlayer;
        }
        public void SetHP(string HP)
        {
            float.TryParse(HP, out this.HP);
        }
        public void SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }
        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }
        public void SetRotation(Quaternion rotation)
        {
            this.rotation = rotation;
        }
        public static Player FindPlayerById(List<Player> players,int id)
        {
            foreach(Player player in players)
            {
                if (id == player.id)
                {
                    return player;
                }
            }
            return null;
        }
        public static List<Player> SetPlayerById(List<Player> players, int id,Player player)
        {
            for(int i = 0; i < players.Count; i++)
            {
                if (id == players[i].id)
                {
                    players[i] = player;
                }
            }
            return players;
        }
    }
}
