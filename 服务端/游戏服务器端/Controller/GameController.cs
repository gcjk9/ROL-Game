using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Game;
using GameServer.Servers;
using GameServer.Tool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameServer.Controller
{
    class GameController:BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }
        public string StartGame(string data, Client client, Server server)
        {
            if (true)
            {
                Room room =  client.Room;
                room.state = RoomState.Battle;
                room.gameMechanics = new GameMechanics(room.playerRoom, server, room);
                string d = ((int)ReturnCode.Success).ToString() + "|" + room.gameMechanics.Init();
                room.BroadcastMessage(client, ActionCode.StartGame, d);
                room.StartTimer();
                return d;
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
        public string Control(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.OControl,(int)ReturnCode.Success+"|"+data);
            return null;
        }
        public string Behavior(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.Behavior, data);
            return null;
        }
        public string SyncPlayer(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.SyncPlayer, ((int)ReturnCode.Success).ToString() + "|" + data);
            //data = room.gameMechanics.SyncPlayer(data);
            return ((int)ReturnCode.Success).ToString() + "|" + "" ;
        }
        public string Notice(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.Notice, (int)ReturnCode.Success + "|" + data);
            return null;
        }
        public string Move(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.Move, data);
            return null;
        }
        public string Shoot(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.Shoot, data);
            return null;
        }
        public string Attack(string data, Client client, Server server)
        {
            int damage = int.Parse(data);
            Room room = client.Room;
            if (room == null) return null;
            room.TakeDamage(damage, client);
            return null;
        }
        public string QuitBattle(string data, Client client, Server server)
        {
            Room room = client.Room;

            if (room != null)
            {
                room.BroadcastMessage(null, ActionCode.QuitBattle, "r");
                room.Close();
            }
            return null;
        }


        public string SyncTransform(string data, Client client, Server server)
        {          
            Room room = client.Room;
            int fromId = 0;
            List<Player> players;
            List<Monster> monsters;

            SyncHelper.Parse(data, out players, out monsters, out fromId);
            Player player = Player.FindPlayerById(players, fromId);
            //room.gameMechanics.players = Player.SetPlayerById(room.gameMechanics.players, fromId, player);



            if (room != null)
            {
                room.BroadcastMessage(null, ActionCode.QuitBattle, "r");
                room.Close();
            }
            return null;
        }
    }
}
