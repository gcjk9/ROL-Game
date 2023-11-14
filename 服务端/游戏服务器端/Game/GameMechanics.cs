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
    class GameMechanics
    {
        public List<Player> players = new List<Player>();
        public List<Monster> monsters = new List<Monster>();
        public int birthPointCount = 8;
        public int progress = 0;
        public Server server;
        public Room room;

        private List<int> birthPoints = new List<int>();

        public GameMechanics()
        {

        }
        public GameMechanics(List<Player> players,Server server,Room room)
        {
            this.players = players;
            this.server = server;
            this.room = room;
        }
        public string Init()
        {
            JObject JData = new JObject();

            foreach(Player player in players)
            {
                while (true)
                {
                    Random ran = new Random();
                    int r = ran.Next(0, birthPointCount);
                    if (!birthPoints.Contains(r))
                    {
                        birthPoints.Add(r);
                        JData.Add(player.id.ToString(), r);
                        break;
                    }
                }
            }
            return JData.ToString();
        }
        public string SyncPlayer(string data)
        {
            //同步玩家位置到服务器
            JObject JData = JObject.Parse(data);
            int.TryParse(JData.GetValue("id").ToString(), out int id);
            JData.TryGetValue("velocity", out JToken tmpV);
            JData.TryGetValue("position", out JToken tmpP);
            JData.TryGetValue("rotation", out JToken tmpR);
            string[] v = tmpV.ToString().Split('&');
            string[] p = tmpP.ToString().Split('&');
            string[] r = tmpR.ToString().Split('&');
            float.TryParse(v[0], out float vx);
            float.TryParse(v[1], out float vy);
            float.TryParse(v[2], out float vz);
            float.TryParse(p[0], out float px);
            float.TryParse(p[1], out float py);
            float.TryParse(p[2], out float pz);
            float.TryParse(r[0], out float rx);
            float.TryParse(r[1], out float ry);
            float.TryParse(r[2], out float rz);
            float.TryParse(r[3], out float rw);

            Player player = Player.FindPlayerById(players, id);

            player.SetPosition(new Vector3(px, py, pz));
            player.SetRotation(new Quaternion(rx, ry, rz, rw));

            JData = new JObject();
            foreach(Player P in room.playerRoom)
            {
                JObject JPlayer = new JObject();
                JPlayer.Add("velocity", P.velocity.X + "&" + P.velocity.Y + "&" + P.velocity.Z);
                JPlayer.Add("position", P.position.X + "&" + P.position.Y + "&" + P.position.Z);
                JPlayer.Add("rotation", P.rotation.x + "&" + P.rotation.y+ "&" + P.rotation.z + "&" + P.rotation.w);

                JData.Add(P.id.ToString(), JPlayer.ToString());
            }
            return JData.ToString();
        }
        /// <summary>
        /// 调用一次，关卡+1
        /// </summary>
        ///         
        public void GameProgress()
        {
            progress++;
            if (progress == 1)
            {
                
            }
        }
    }
}
