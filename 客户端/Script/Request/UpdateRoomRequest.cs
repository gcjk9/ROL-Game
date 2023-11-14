using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class UpdateRoomRequest : BaseRequest
{
    private RoomPanel roomPanel;
    // Use this for initialization
    private string data = "";
    private bool ToResponeMedthod = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.UpdateRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }
    private void Update()
    {
        if (ToResponeMedthod)
        {
            ToResponeMedthod = false;
            ResponeMedthod();
        }
    }
    public void SendRequest(string data)
    {        
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        Debug.Log("UpdateRoomData = " + data);

        this.data = data;
        ToResponeMedthod = true;
    }
    public void ResponeMedthod()
    {
        roomPanel.UpdateRoom(data);
    }
}
