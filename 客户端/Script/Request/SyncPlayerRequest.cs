using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;
using UnityEngine.SceneManagement;

public class SyncPlayerRequest : BaseRequest
{
    private GameMode gameMode;
    // Use this for initialization
    private string result = "";
    private JObject JData;
    private bool ToResponeMedthod = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.SyncPlayer;
        gameMode = GetComponent<GameMode>();
        base.Awake();
    }
    private void Update()
    {
        if (ToResponeMedthod)
        {
            ToResponeMedthod = false;
            ResponeMedthod();
        }
        SyncPalyer();
    }
    public void SendRequest(string data)
    {        
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        Debug.Log("SyncPlayerData = " + data);
        string[] tmp = data.Split('|');
        int.TryParse(tmp[0], out int state);
        //Debug.Log("1 - 0");
        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {
            if (tmp[1] == "")
                return;

            JData = JObject.Parse(tmp[1]);
            ToResponeMedthod = true;
        }
        if (returnCode == ReturnCode.Fail)
        {

        }
    }
    public void ResponeMedthod()
    {
        foreach(GameObject player in gameMode.players)
        {
            string id= player.GetComponent<PlayerBase>().ud.Id.ToString();
            string localId = gameMode.ud.Id.ToString();
            if (localId.Equals(id))
                continue;
            if(JData.TryGetValue(id,out JToken Jtmp))
            {
                JObject JPlayer = JObject.Parse(Jtmp.ToString());
                JPlayer.TryGetValue("velocity", out JToken tmpV);
                JPlayer.TryGetValue("position", out JToken tmpP);
                JPlayer.TryGetValue("rotation", out JToken tmpR);
                JPlayer.TryGetValue("gun", out JToken tmpG);
                string[] v = tmpV.ToString().Split('&');
                string[] p = tmpP.ToString().Split('&');
                string[] r = tmpR.ToString().Split('&');
                string[] g = tmpG.ToString().Split('&');
                float.TryParse(v[0], out float vx);
                float.TryParse(v[1], out float vy);
                float.TryParse(v[2], out float vz);
                float.TryParse(p[0], out float px);
                float.TryParse(p[1], out float py);
                float.TryParse(p[2], out float pz);
                float.TryParse(r[0], out float rx);
                float.TryParse(r[1], out float ry);
                float.TryParse(r[2], out float rz);
                float.TryParse(r[3], out float rw);
                int.TryParse(g[0], out int g0);
                int.TryParse(g[1], out int g1);
                int.TryParse(g[2], out int g2);
                //Vector3 syncPosition = new Vector3(px, py, pz);
                //Quaternion syncRotation = new Quaternion(rx, ry, rz, rw);

                player.transform.position = new Vector3(px, py, pz);
                player.transform.rotation = new Quaternion(rx, ry, rz, rw);
                //player.GetComponent<PlayerBase>().cc.isControlVelocity = true;
                PlayerBase pb = player.GetComponent<PlayerBase>();
                pb.cc.velocity = new Vector3(vx, vy, vz);
                pb.loadingGunId = g0;
                pb.gun1Id = g1;
                pb.gun2Id = g2;
                break;
            }
        }
    }
    void SyncPalyer()
    {

    }
}
