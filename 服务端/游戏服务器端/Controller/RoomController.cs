using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Game;
using GameServer.Servers;
namespace GameServer.Controller
{
    class RoomController:BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }
        public string CreateRoom(string data, Client client, Server server)
        {
            string[] tmp = data.Split(',');
            if (Room.FindRoomById(server.GetRoomList(), tmp[1])==null)
            {
                int.TryParse(tmp[0], out int id);
                server.CreateRoom(client,Player.FindPlayerById(server.playerList,id),tmp[1]);
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            
        }
        public string ListRoom(string data, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            foreach(Room room in server.GetRoomList())
            {
                if (room.IsWaitingJoin())
                {
                    sb.Append(room.GetHouseOwnerData()+"|");
                }
            }
            if (sb.Length == 0)
            {
                sb.Append("0");
            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
        public string JoinRoom(string data, Client client, Server server)
        {
            string[] tmp = data.Split(',');
            Room room = Room.FindRoomById(server.roomList, tmp[1]);
            if (room != null)
            {
                if (room.IsWaitingJoin())
                {
                    if (room.AddClient(client, client.player,out string returnData))
                    {
                        return ((int)ReturnCode.Success).ToString() + "|" + returnData;
                    }
                    else
                        return ((int)ReturnCode.Fail).ToString()+"|"+"房间人数已满";
                }
                else
                {
                    return ((int)ReturnCode.Fail).ToString() + "|" + "房间已经开始游戏不得加入";
                }
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString() + "|" + "输入房间id号为空";
            }           
            //if(room == null)
            //{
            //    return ((int)ReturnCode.NotFound).ToString();
            //}
            //else if (room.IsWaitingJoin() == false)
            //{
            //    return ((int)ReturnCode.Fail).ToString();
            //}
            //else
            //{
            //    room.AddClient(client);
            //    string roomData = room.GetRoomData();//"returncode,roletype-id,username,tc,wc|id,username,tc,wc"
            //    room.BroadcastMessage(client, ActionCode.UpdateRoom, roomData);
            //    return ((int)ReturnCode.Success).ToString() + "," + ((int)RoleType.Red).ToString()+ "-" + roomData;
            //}
        }
        public string UpdateRoom(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.UpdateRoom, (int)ReturnCode.Success + "|" + data);
            return null;
        }
        public string QuitRoom(string data, Client client, Server server)
        {
            //bool isHouseOwner = client.IsHouseOwner();
            Room room = client.Room;
            if (true)
            {
                room.Close();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                client.Room.RemoveClient(client,client.player);
                return ((int)ReturnCode.Success).ToString();
            }
        }
    }
}
