using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class OControlRequest : BaseRequest
{   
    public CPlayer cplayer;
    public List<PlayerBase> pbs = new List<PlayerBase>();
    // Use this for initialization
    private int oid, ok, os;
    private Vector3 ocp,oap;
    private Quaternion ocr,oar;
    private string result = "";
    private bool ToResponeMedthod1 = false;//异步处理中控制主线程方法执行
    private bool ToResponeMedthod2 = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.OControl;
        base.Awake();
    }
    public void Init()
    {
        
        for(int i=0;i<transform.childCount; i++)
        {
            pbs.Add(transform.GetChild(i).GetComponent<PlayerBase>());
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
    }
    public void SendRequest(int k, int s)
    {
        string data =  k.ToString() + "," + s.ToString();
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        //Debug.Log("OControlData = " + data);

        string[] tmp = data.Split('|');
        int.TryParse(tmp[0], out int state);

        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {
            string[] tmp2 = tmp[1].Split(',');
            int.TryParse(tmp2[1], out int inputType);
            int.TryParse(tmp2[0], out oid);
            if ((InputTypes)inputType == InputTypes.KeyEven)
            {
                Debug.Log("30-7");
                int.TryParse(tmp2[2], out ok);
                int.TryParse(tmp2[3], out os);
                ToResponeMedthod1 = true;
            }
            if((InputTypes)inputType == InputTypes.MouseMove)
            {
                //Debug.Log("30-8");
                string[] p = tmp2[2].Split(':');
                string[] r = tmp2[3].Split(':');
                float.TryParse(p[0], out float px);
                float.TryParse(p[1], out float py);
                float.TryParse(p[2], out float pz);
                float.TryParse(r[0], out float rx);
                float.TryParse(r[1], out float ry);
                float.TryParse(r[2], out float rz);
                float.TryParse(r[3], out float rw);

                ocp = new Vector3(px, py, pz);
                ocr = new Quaternion(rx, ry, rz, rw);

                p = tmp2[4].Split(':');
                r = tmp2[5].Split(':');
                float.TryParse(p[0], out  px);
                float.TryParse(p[1], out  py);
                float.TryParse(p[2], out  pz);
                float.TryParse(r[0], out  rx);
                float.TryParse(r[1], out  ry);
                float.TryParse(r[2], out  rz);
                float.TryParse(r[3], out  rw);

                oap = new Vector3(px, py, pz);
                oar = new Quaternion(rx, ry, rz, rw);

                ToResponeMedthod2 = true;
            }           
            //Debug.Log("29-0");
        }
    }
    public void ResponeMedthod1()
    {
        Debug.Log("29-1");
        foreach (PlayerBase pb in pbs)
        {
            if (oid.Equals(pb.ud.Id))
            {
                Debug.Log("29-2"+(KeyCode)ok+"|"+os);
                pb.Control(ok, os);
            }
        }
    }
    public void ResponeMedthod2()
    {
        //Debug.Log("30-0");
        
        foreach (PlayerBase pb in pbs)
        {
            if (oid.Equals(pb.ud.Id))
            {
                //Debug.Log("30-1");
                pb.MoveDirection.SetPositionAndRotation(ocp, ocr);
                pb.ToAimTarget.SetPositionAndRotation(oap, oar);
            }
        }
    }
}
