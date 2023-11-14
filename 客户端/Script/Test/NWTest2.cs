using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class NWTest2 : BaseRequest
{
    private UserData ud;
    public GameMode gameMode;
    public PlayerBase p1, p2;
    public bool isCanBeControl = true;
    public ControlEventTypes controlTypes;
    // Use this for initialization
    private string result = "";
    private bool ToResponeMedthod = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        base.Awake();
    }
    private void Start()
    {
        StartCoroutine(DelaySendTest());
    }
    /// <summary>
    /// 测试快速登录，匹配房间
    /// </summary>
    /// <returns></returns>
    IEnumerator DelaySendTest()
    {

        SendRequest("1,1");
        yield return new WaitForSeconds(0.3f);


        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        SendRequest("2,1");
        yield return new WaitForSeconds(0.3f);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            p1.ud = new UserData(2, "1");
            p2.ud = new UserData(1, "123");
            gameMode.TestInit();
        }
        if (ToResponeMedthod)
        {
            ToResponeMedthod = false;
            ResponeMedthod();
        }
    }
    public void SendRequest(int k, int s)
    {
        string data = ud.Id.ToString() + "," + k.ToString() + "," + s.ToString();
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {

    }
    public void ResponeMedthod()
    {

    }
}

