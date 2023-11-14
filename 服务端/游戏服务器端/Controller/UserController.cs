using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.DAO;
using GameServer.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GameServer.Game;

namespace GameServer.Controller
{
    class UserController:BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private DressUpDAO dressUpDAO = new DressUpDAO();
        private ResultDAO resultDAO = new ResultDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }
        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user =  userDAO.VerifyUser(client.MySQLConn, strs[0], strs[1]);
            if (user == null)
            {
                //Enum.GetName(typeof(ReturnCode), ReturnCode.Fail);
                JObject JLoginRespone = new JObject();

                JLoginRespone.Add("state", JToken.FromObject(ReturnCode.NotFound));
                JLoginRespone.Add("return", "账号密码错误！");
                return JLoginRespone.ToString();
            }
            else
            {
                //Result res = resultDAO.GetResultByUserid(client.MySQLConn, user.Id);
                //client.SetUserData(user, res);
                //return  string.Format("{0},{1},{2},{3}", ((int)ReturnCode.Success).ToString(), user.Username, res.TotalCount, res.WinCount);

                JObject JLoginRespone = new JObject();
                if (Player.FindPlayerById(server.playerList, user.Id) != null)
                {
                    JLoginRespone.Add("state", JToken.FromObject(ReturnCode.NotFound));
                    JLoginRespone.Add("return", "账号不能重复登陆！");
                    return JLoginRespone.ToString();
                }
                
                DressUp dressUp = dressUpDAO.GetDressUp(client.MySQLConn, user.Id.ToString());
                Player player = new Player(client, user.Id.ToString(), user.Username, dressUp);
                client.player = player;
                server.playerList.Add(player);
                
                JLoginRespone.Add("state", JToken.FromObject(ReturnCode.Success));
                JLoginRespone.Add("id", JToken.FromObject(user.Id.ToString()));
                JLoginRespone.Add("username", JToken.FromObject(user.Username));
                JLoginRespone.Add("dressup", JToken.FromObject(dressUp.GetJsonPackage().ToString()));

                return JLoginRespone.ToString();
               
            }
        }
        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0];string password = strs[1];
            bool res = userDAO.GetUserByUsername(client.MySQLConn,username);
            if (res)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            userDAO.AddUser(client.MySQLConn, username, password);

            if(userDAO.GetUserByUsername(client.MySQLConn, username))
            {
                User user = userDAO.VerifyUser(client.MySQLConn, username, password);
                dressUpDAO.AddDressUp(client.MySQLConn, user.Id.ToString());
            }           
            return ((int)ReturnCode.Success).ToString();
        }
        /// <summary>
        /// 将装扮信息写入数据库
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string DressUp(string data, Client client, Server server)
        {
            JObject JData = JObject.Parse(data);
            string id = JData.GetValue("id").ToString();
            string head = JData.GetValue("head").ToString();
            string face = JData.GetValue("face").ToString();
            string body = JData.GetValue("body").ToString();


            if (dressUpDAO.UpdataDressUp(client.MySQLConn,id,head,face,body))
            {
                int.TryParse(id, out int ID);
                Player player= Player.FindPlayerById(server.playerList, ID);
                DressUp dressUp = new DressUp();
                dressUp.Id = ID;
                dressUp.Head = head;
                dressUp.Face = face;
                dressUp.Body = body;
                player.dressup = dressUp;
                JData.Add("state", JToken.FromObject(ReturnCode.Success));
                if (client.player.room != null)
                {
                    client.player.room.BroadcastMessage(client, ActionCode.UpdateRoom, client.player.room.GetMenberJsonData());
                }
                return JData.ToString();
            }
            else
            {
                JData.Add("state", JToken.FromObject(ReturnCode.Fail));
                return JData.ToString();
            }
        }
    }
}
