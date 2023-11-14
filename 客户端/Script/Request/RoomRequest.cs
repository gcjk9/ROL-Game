using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class RoomRequest : BaseRequest
{
    public SelectRoomPanel SelectRoomPanel;
    // Use this for initialization
    private bool LoginFinishMedthod = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        this.requestCode = RequestCode.Room;
        base.Awake();
    }
    private void Update()
    {
        
    }
    public void SendRequest(ActionCode actionCode,string data)
    {
        this.actionCode = actionCode;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        Debug.Log("RoomData = " + data);

        string[] tmp = data.Split('|');
        int.TryParse(tmp[0], out int state);

        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {
            
        }


    }
    public void LoginFinish()
    {
        
    }

}
