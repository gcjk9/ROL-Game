using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class QuitRoomRequest : BaseRequest
{
    private RoomPanel roomPanel;
    // Use this for initialization
    private string result = "";
    private bool ToResponeMedthod = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.QuitRoom;
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
    public void SendRequest()
    {        
        base.SendRequest(roomPanel.ud.Id.ToString());
    }

    public override void OnResponse(string data)
    {
        Debug.Log("QuitRoomData = " + data);
        int.TryParse(data, out int state);

        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {
            ToResponeMedthod = true;
        }
    }
    public void ResponeMedthod()
    {
        roomPanel.ClearRoom();
        roomPanel.StartGameBut.SetActive(false);
        roomPanel.LeaveGameBut.SetActive(true);
        roomPanel.QuitRoomBut.SetActive(false);
        roomPanel.SelectRoomBut.SetActive(true);
    }
}
