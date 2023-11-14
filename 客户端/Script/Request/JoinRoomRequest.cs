using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;
using UnityEngine.UI;

public class JoinRoomRequest : BaseRequest
{
    public SelectRoomPanel selectRoomPanel;
    public RoomPanel roomPanel;
    public Text Result;
    // Use this for initialization
    private string roomData;
    private bool ToResponeMedthod1 = false;//异步处理中控制主线程方法执行
    private bool ToResponeMedthod2 = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        this.requestCode = RequestCode.Room;
        this.actionCode = ActionCode.JoinRoom;
        base.Awake();
    }
    private void Update()
    {
        if (ToResponeMedthod1)
        {
            ToResponeMedthod1 = false;
            ResponeMedthod1();
        }
        if (ToResponeMedthod2)
        {
            ToResponeMedthod2 = false;
            ResponeMedthod2();
        }
    }
    public void SendRequest(string data)
    {        
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        Debug.Log("JoinRoomData = " + data);

        string[] tmp = data.Split('|');
        int.TryParse(tmp[0], out int state);

        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {
            roomData = tmp[1];
            ToResponeMedthod1 = true;
        }
        if(returnCode == ReturnCode.Fail)
        {
            roomData = tmp[1];
            ToResponeMedthod2 = true;
        }
    }
    public void ResponeMedthod1()
    {
        selectRoomPanel.roomPanel.GetComponent<RoomPanel>().UpdateRoom(roomData);
        roomPanel.StartGameBut.SetActive(true);
        roomPanel.LeaveGameBut.SetActive(false);
        roomPanel.QuitRoomBut.SetActive(true);
        roomPanel.SelectRoomBut.SetActive(false);
        selectRoomPanel.gameObject.SetActive(false);
    }
    public void ResponeMedthod2()
    {
        Result.text = roomData;
    }
}
