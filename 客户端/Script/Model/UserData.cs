using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;


public class UserData
{
    
    public UserData(int id,string username)
    {
        this.Id = id;
        this.Username = username;
    }
    public void SetDressUp(DressUpData d)
    {
        dressUpData = d;
    }
   
    public int Id { get; set; }
    public string Username { get; set; }
    public DressUpData dressUpData { get; set; }

    public string birthPointId { get; set; }
}
