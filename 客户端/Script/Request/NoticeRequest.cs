using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class NoticeRequest : BaseRequest
{
    public string id;
    public CPlayer cplayer;
    public PlayerBase playerBase;
    public List<PlayerBase> opbs = new List<PlayerBase>();
    public bool isCanBeControl = true;
    public ControlEventTypes controlTypes;
    // Use this for initialization
    private string result = "";
    private bool ToResponeMedthod1 = false;//异步处理中控制主线程方法执行
    private bool ToResponeMedthod2 = false;//异步处理中控制主线程方法执行
    private bool ToResponeMedthod3 = false;//异步处理中控制主线程方法执行
    private bool ToResponeMedthod4 = false;//异步处理中控制主线程方法执行
    private bool ToResponeMedthod5 = false;//异步处理中控制主线程方法执行
    private bool ToResponeMedthod6 = false;//异步处理中控制主线程方法执行
    private ReturnData returnData;
    private struct ReturnData
    {
        public int id;
        public NoticeTypes noticeCode;
        public string d;
    }
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Notice;
        base.Awake();
    }
    public void Init()
    {
        cplayer = transform.GetChild(0).GetComponent<CPlayer>();
        playerBase = transform.GetChild(0).GetComponent<PlayerBase>();
        id = cplayer.playerBase.ud.Id.ToString();

        for (int i = 0; i < transform.parent.Find("OControl").childCount; i++)
        {
            opbs.Add(transform.parent.Find("OControl").GetChild(i).GetComponent<PlayerBase>());
        }
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
        if (ToResponeMedthod3)
        {
            ToResponeMedthod3 = false;
            ResponeMedthod3();
        }
        if (ToResponeMedthod4)
        {
            ToResponeMedthod4 = false;
            ResponeMedthod4();
        }
        if (ToResponeMedthod5)
        {
            ToResponeMedthod5 = false;
            ResponeMedthod5();
        }
        if (ToResponeMedthod6)
        {
            ToResponeMedthod6 = false;
            ResponeMedthod6();
        }
    }
    public void SendRequest(NoticeTypes noticeTypes, string d)
    {
        string data = id + "," + (int)noticeTypes + "," + d;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        Debug.Log("NoticeRoomData = " + data);
        string[] tmp0 = data.Split('|');
        int.TryParse(tmp0[0], out int state);

        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {
            string[] tmp1 = tmp0[1].Split(',');
            int.TryParse(tmp1[0], out int id);
            int.TryParse(tmp1[1], out int notice);

            returnData.id = id;
            returnData.noticeCode = (NoticeTypes)notice;
            returnData.d = tmp1[2];

            switch (returnData.noticeCode)
            {
                case NoticeTypes.Pick:
                    ToResponeMedthod1 = true;
                    break;
                case NoticeTypes.Load:
                    ToResponeMedthod2 = true;
                    break;
                case NoticeTypes.LoadMag:
                    ToResponeMedthod3 = true;
                    break;
                case NoticeTypes.Discard:
                    ToResponeMedthod4 = true;
                    break;
                case NoticeTypes.Create:
                    ToResponeMedthod5 = true;
                    break;
            }
        }
    }
    public void ResponeMedthod1()
    {
        foreach (PlayerBase pb in opbs)
        {
            if (returnData.id == pb.ud.Id)
            {
                int.TryParse(returnData.d, out int eid);
                Equipment e = Equipment.FindEquipmentById(eid);
                if (e != null && e.gameObject != null)
                    pb.CanPackEquipment = e.gameObject;
            }
        }
    }
    public void ResponeMedthod2()
    {
        Debug.Log("5-8-1");
        foreach (PlayerBase pb in opbs)
        {
            if (returnData.id == pb.ud.Id)
            {
                string[] tmp = returnData.d.Split('&');
                int.TryParse(tmp[0], out int lid);
                int.TryParse(tmp[1], out int gid);
                Equipment e = Equipment.FindEquipmentById(gid);
                if (e != null && e.gameObject != null)
                    pb.Guns[lid] = e.gameObject;

                Debug.Log("5-8-1" + lid + "|" + gid);
            }
        }
    }
    public void ResponeMedthod3()
    {
        foreach (PlayerBase pb in opbs)
        {
            if (returnData.id == pb.ud.Id)
            {
                string[] tmp = returnData.d.Split('&');
                int.TryParse(tmp[0], out int bulletInGun);
                int.TryParse(tmp[1], out int bulletInMag);

                pb.LoadingGun.GetComponent<Gun>().bulletInGun = bulletInGun;
                pb.LoadingGun.GetComponent<Gun>().bulletInMag = bulletInMag;
            }
        }
    }
    public void ResponeMedthod4()
    {
        foreach (PlayerBase pb in opbs)
        {
            if (returnData.id == pb.ud.Id)
            {
                int.TryParse(returnData.d, out int eid);

                pb.Discard(eid);
            }
        }
    }
    public void ResponeMedthod5()
    {
        GameObject.Find("AirDrop").GetComponent<AirDropPanel>().CreateEquipment(returnData.d);
    }
    public void ResponeMedthod6()
    {

    }
}


