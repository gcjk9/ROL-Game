using GameServer.Game;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace GameServer.Tool
{
    class SyncHelper
    {
        public static void Parse(string data ,out List<Player> players,out List<Monster> monsters,out int fromId)
        {
            int playersCount = 0;
            int monstersCount = 0;
            int.TryParse(JObject.Parse(data).GetValue("fromId").ToString(), out fromId);
            int.TryParse(JObject.Parse(data).GetValue("playersCount").ToString(), out playersCount);
            int.TryParse(JObject.Parse(data).GetValue("monstersCount").ToString(), out monstersCount);
            JObject jplayers = JObject.Parse(JObject.Parse(data).GetValue("players").ToString());
            JObject jMonsters = JObject.Parse(JObject.Parse(data).GetValue("monsters").ToString());

            players = new List<Player>();
            for (int i = 0; i < playersCount; i++)
            {
                JObject jplayer = JObject.Parse(JObject.Parse(jplayers.ToString()).GetValue("player"+i).ToString());
                if (jplayer != null)
                {
                    string id = jplayer.GetValue("id").ToString();
                    string name = jplayer.GetValue("name").ToString();

                    string HP = jplayer.GetValue("HP").ToString();
                    string V3 = jplayer.GetValue("position").ToString();
                    string[] V3tmp = V3.Split(',');

                    float[] position = new float[3];
                    for (int j = 0; j < 3; j++)
                    {
                        float.TryParse(V3tmp[j], out position[j]);
                    }
                    //Player player = new Player(id, name);
                    //player.SetHP(HP);
                    //player.SetPosition(new Vector3(position[0], position[1], position[2]));
                    //players.Add(player);
                }                
            }
            monsters = new List<Monster>();
            for (int i = 0; i < monstersCount; i++)
            {
                JObject jmonster = JObject.Parse(JObject.Parse(jMonsters.ToString()).GetValue("monster" + i).ToString());
                if (jmonster != null)
                {
                    string id = jmonster.GetValue("id").ToString();
                    //string name = jmonster.GetValue("name").ToString();

                    string HP = jmonster.GetValue("HP").ToString();
                    string V3 = jmonster.GetValue("position").ToString();
                    string[] V3tmp = V3.Split(',');

                    float[] position = new float[3];
                    for (int j = 0; j < 3; j++)
                    {
                        float.TryParse(V3tmp[j], out position[j]);
                    }
                    Monster monster = new Monster(id);
                    monster.SetHP(HP);
                    monster.SetPosition(new Vector3(position[0], position[1], position[2]));
                    monsters.Add(monster);
                }
            }
        }

        public static void Package(out string data, List<Player> players,List<Monster> monsters,int fromId)
        {
            string fromIdtmp = "\"fromId\":\""+fromId+"\"";
            string playersCount= "\"fromId\":\"" + players.Count + "\"";
            string monstersCount= "\"fromId\":\"" + monsters.Count + "\"";

            string playersStr= "\"players\":\"{";
            for (int i=0;i<players.Count; i++)
            {
                if (players[i] != null)
                {
                    string id= "\"id\":\"" + players[i].id + "\"";
                    string name = "\"name\":\"" + players[i].name + "\"";
                    string HP = "\"HP\":\"" + players[i].HP + "\"";
                    string position = "\"position\":\"" + players[i].position.X+","+ players[i].position.Y + "," + players[i].position.Z +"\"";

                    string playerStr = "\"player"+i+"\":\"{" + id + "," + name + "," + HP + "," + position + "}\"";

                    playersStr += playerStr + ",";
                }                
            }
            playersStr += "\"player\":\"End\" }\"";

            string monstersStr = "\"monsters\":\"{";
            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i] != null)
                {
                    string id = "\"id\":\"" + monsters[i].id + "\"";
                    //string name = "\"name\":\"" + monsters[i].name + "\"";
                    string HP = "\"HP\":\"" + monsters[i].HP + "\"";
                    string position = "\"position\":\"" + monsters[i].position.X + "," + monsters[i].position.Y + "," + monsters[i].position.Z + "\"";

                    string monsterStr = "\"player" + i + "\":\"{" + id + "," + "" + "," + HP + "," + position + "}\"";

                    monstersStr += monsterStr + ",";
                }                
            }
            monstersStr += "\"player\":\"End\" }\"";

            data = "{" + fromIdtmp + "," + playersCount + "," + monstersCount + "," + playersStr + "," + monstersStr + "}";
        }
        public static Vector3 SyncMonsterPosition(List<Monster> monsters)
        {
            return new Vector3();
        }
    }
}
