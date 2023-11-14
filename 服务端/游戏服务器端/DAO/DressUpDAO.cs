using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;
namespace GameServer.DAO
{
    class DressUpDAO
    {
        public DressUp GetDressUp(MySqlConnection conn, string id)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from userdressup where id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int Id = reader.GetInt32("id");
                    string head = reader.GetString("head");
                    string face = reader.GetString("face");
                    string body = reader.GetString("body");
                    DressUp dressUp = new DressUp();
                    dressUp.Id = Id;
                    dressUp.Head = head;
                    dressUp.Face = face;
                    dressUp.Body = body;

                    return dressUp;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetDressUp的时候出现异常：" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public bool UpdataDressUp(MySqlConnection conn, string id, string head,string face,string body)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("update userdressup set head = @head , face = @face , body = @body where id="+id, conn);
                cmd.Parameters.AddWithValue("head", head);
                cmd.Parameters.AddWithValue("face", face);
                cmd.Parameters.AddWithValue("body", body);
                //cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("在UpdataDressUp的时候出现异常：" + e);
                return false;
            }
        }
        public void AddDressUp(MySqlConnection conn, string id)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into userdressup set id = @id , head = '0',face = '0',body = '0'", conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddDressUp的时候出现异常：" + e);
            }
        }
    }    
}
