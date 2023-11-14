using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class DressUpResquest : BaseRequest
{
    public DressUpPanel dressUpPanel;
    // Use this for initialization
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.DressUp;
        base.Awake();
    }
    public void SendRequest(string id, string head, string face, string body)
    {
        JObject JData = new JObject();
        JData.Add("id", JToken.FromObject(id));
        JData.Add("head", JToken.FromObject(head));
        JData.Add("face", JToken.FromObject(face));
        JData.Add("body", JToken.FromObject(body));
        string data = JData.ToString();
        base.SendRequest(data);
    }
    public override void OnResponse(string data)
    {
        Debug.Log("DressupData=" + data);
        JObject JData = JObject.Parse(data);
        int state = 0;
        int.TryParse(JData.GetValue("state").ToString(), out state);


        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {
            facade.playerMng.UserData.dressUpData = new DressUpData(data);

        }
    }
}
