using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Threading;
using GameServer.Game;
using Newtonsoft.Json.Linq;

namespace GameServer.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }
    class Room
    {
        private const int MAX_Player = 6;
        private const int MAX_HP = 100;
        private List<Client> clientRoom = new List<Client>();        
        public RoomState state = RoomState.WaitingJoin;
        private Server server;
        public GameMechanics gameMechanics=new GameMechanics();
        public List<Player> playerRoom = new List<Player>();
        public string hostId;
        public string roomId;

        public Room(Server server,Client client, Player player, string roomId)
        {
            this.server = server;
            this.hostId = player.id.ToString();
            this.roomId = roomId;
            client.Room = this;
            player.room = this;
            playerRoom.Add(player);
            clientRoom.Add(client);
        }
        public static Room FindRoomById(List<Room> rooms,string id)
        {
            foreach(Room room in rooms)
            {
                if (room.roomId.Equals(id))
                {
                    return room;
                }
            }
            return null;
        }
        public bool IsWaitingJoin()
        {
            return state == RoomState.WaitingJoin;
        }
        public bool AddClient(Client client,Player player,out string data)
        {
            data = "";
            if (playerRoom.Count >= MAX_Player)
            {
                return false;
            }
            if (Player.FindPlayerById(playerRoom, player.id)!= null)
            {
                //return false;
            }
            client.HP = MAX_HP;
            clientRoom.Add(client);
            playerRoom.Add(player);
            client.Room = this;
            player.room = this;

            data = GetMenberJsonData();
            BroadcastMessage(client, ActionCode.UpdateRoom, data);
            return true;
        }
        public string GetMenberJsonData()
        {
            int i = 0;
            JObject JData = new JObject();
            JData.Add("roomId", roomId);
            JData.Add("hostId", hostId);
            JData.Add("menberCount", playerRoom.Count);
            foreach (Player p in playerRoom)
            {
                JData.Add("menber" + i, p.GetJsonData(true, false).ToString());
                i++;
            }
            return JData.ToString();
        }
        public void RemoveClient(Client client,Player player)
        {
            client.Room = null;
            player.room = null;
            clientRoom.Remove(client);
            playerRoom.Remove(player);

            BroadcastMessage(client, ActionCode.UpdateRoom, GetMenberJsonData());
        }
        public string GetHouseOwnerData()
        {
            return clientRoom[0].GetUserData();
        }
        
        public int GetId()
        {
            if (clientRoom.Count > 0)
            {
                return clientRoom[0].GetUserId();
            }
            return -1;
        }
        public String GetRoomData()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Client client in clientRoom)
            {
                sb.Append(client.GetUserData() + "|");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
        public void BroadcastMessage(Client excludeClient,ActionCode actionCode,string data)
        {
            foreach(Client client in clientRoom)
            {
                if (client != excludeClient)
                {
                    server.SendResponse(client, actionCode, data);
                }
            }
        }
        public bool IsHouseOwner(Client client)
        {
            return client == clientRoom[0];
        }
        public void QuitRoom(Client client)
        {
            if (client == clientRoom[0])
            {
                Close();
            }
            else
            {
                RemoveClient(client, client.player);
            }                
        }
        public void Close()
        {
            foreach(Client client in clientRoom)
            {
                client.Room = null;
            }
            BroadcastMessage(new Client(), ActionCode.QuitRoom, ((int)ReturnCode.Success).ToString());
            server.RemoveRoom(this);
        }
        /// <summary>
        /// 房间开始游戏
        /// </summary>
        public void StartTimer()
        {           
            new Thread(RunTimer).Start();
        }
        private void RunTimer()
        {
            Thread.Sleep(1000);
            for (int i = 3; i > 0; i--)
            {
                BroadcastMessage(null, ActionCode.ShowTimer, i.ToString());
                Thread.Sleep(1000);
            }

            BroadcastMessage(null, ActionCode.StartPlay, "r");
        }
        public void TakeDamage(int damage,Client excludeClient)
        {
            bool isDie = false;
            foreach (Client client in clientRoom)
            {
                if (client != excludeClient)
                {
                    if (client.TakeDamage(damage))
                    {
                        isDie = true;
                    }
                }
            }
            if (isDie == false) return;
            //如果其中一个角色死亡，要结束游戏
            foreach (Client client in clientRoom)
            {
                if (client.IsDie())
                {
                    client.UpdateResult(false);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                }
                else
                {
                    client.UpdateResult(true);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                }
            }
            Close();
        }
    }
}
