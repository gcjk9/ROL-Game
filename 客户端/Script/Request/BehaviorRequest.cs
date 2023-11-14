using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class BehaviorRequest : BaseRequest
{
    public string id;
    public CPlayer cplayer;
    public PlayerBase playerBase;
    public bool isCanBeControl = true;
    public ControlEventTypes controlTypes;
    // Use this for initialization
    private string result = "";
    private bool ToResponeMedthod = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Control;
        cplayer = GetComponent<CPlayer>();
        playerBase = GetComponent<PlayerBase>();
        id = cplayer.playerBase.ud.Id.ToString();
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
    public void SendRequest(int k, int s)
    {
        string data = id + "," + k.ToString() + "," + s.ToString();
        base.SendRequest(data);
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

    }
}
