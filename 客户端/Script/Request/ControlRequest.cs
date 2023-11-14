using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class ControlRequest : BaseRequest
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
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Control;
        base.Awake();
    }
    public void Init()
    {      
        cplayer = transform.GetChild(0).GetComponent<CPlayer>();
        playerBase = transform.GetChild(0).GetComponent<PlayerBase>();
        id = cplayer.playerBase.ud.Id.ToString();
    }
    private void Update()
    {
        if (ToResponeMedthod)
        {
            ToResponeMedthod = false;
            ResponeMedthod();
        }              
    }
    public IEnumerator DelaySend(InputTypes inputTypes, string d,float t)
    {
        yield return new WaitForSeconds(t);
        SendRequest(inputTypes, d);
    }
    public void SendRequest(InputTypes inputTypes,string d)
    {
        string data = id +","+ (int)inputTypes+","+ d;
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
