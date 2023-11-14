using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;
using UnityEngine.UI;

public class CreateRoomRequest : BaseRequest
{
    public SelectRoomPanel selectRoomPanel;
    public RoomPanel roomPanel;
    public Text Result;
    // Use this for initialization
    private string roomId;
    private bool ToResponeMedthod1 = false;//异步处理中控制主线程方法执行
    private bool ToResponeMedthod2 = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        this.requestCode = RequestCode.Room;
        this.actionCode = ActionCode.CreateRoom;
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
        string[] tmp = data.Split(',');
        roomId = tmp[1];
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        Debug.Log("CreateRoomData = " + data);

        int.TryParse(data, out int state);

        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {            
            ToResponeMedthod1 = true;
        }
        if (returnCode == ReturnCode.Fail)
        {
            ToResponeMedthod2 = true;
        }
    }
    public void ResponeMedthod1()
    {
        selectRoomPanel.roomPanel.roomId.text = "房间ID：" + roomId;
        roomPanel.StartGameBut.SetActive(true);
        roomPanel.LeaveGameBut.SetActive(false);
        roomPanel.QuitRoomBut.SetActive(true);
        roomPanel.SelectRoomBut.SetActive(false);
        selectRoomPanel.gameObject.SetActive(false);
    }
    public void ResponeMedthod2()
    {
        Result.text = "该房间id号已经存在！";
    }
}
