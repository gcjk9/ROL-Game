using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressUpPanel : MonoBehaviour
{
    public GameObject player;

    public GameObject head;
    public GameObject face;
    public GameObject body;

    DressUpData dud=new DressUpData();
    DressUpData tmpDud=new DressUpData();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDressUp(string code)
    {
        string[] tmp = code.Split(':');
        SyncPointInPlayer syncPoint = player.transform.Find("Points").GetComponent<SyncPointInPlayer>();
        switch (tmp[1])
        {
            case "head":
                if (head != null)
                {
                    Destroy(head);
                }
                if (tmp[0].Equals(tmpDud.head))
                    tmp[0] = "0";
                head = Instantiate(Resources.Load<GameObject>("Prefabs/DressUp/" + tmp[0]), player.transform.position, player.transform.rotation);
                head.transform.SetParent(syncPoint.head);
                head.transform.localPosition = syncPoint.head.transform.Find("localPoint").transform.localPosition;
                head.transform.localRotation = syncPoint.head.transform.Find("localPoint").transform.localRotation;
                tmpDud.head = tmp[0];
                break;
            case "face":
                if (face != null)
                {
                    Destroy(face);
                }
                if (tmp[0].Equals(tmpDud.face))
                    tmp[0] = "0";
                face = Instantiate(Resources.Load<GameObject>("Prefabs/DressUp/" + tmp[0]), player.transform.position, player.transform.rotation);
                face.transform.SetParent(syncPoint.head);
                face.transform.localPosition = syncPoint.head.transform.Find("localPoint").transform.localPosition;
                face.transform.localRotation = syncPoint.head.transform.Find("localPoint").transform.localRotation;
                tmpDud.face = tmp[0];
                break;
            case "body":
                if (body != null)
                {
                    Destroy(body);
                }
                if (tmp[0].Equals(tmpDud.body))
                    tmp[0] = "0";
                body = Instantiate(Resources.Load<GameObject>("Prefabs/DressUp/" + tmp[0]), player.transform.position, player.transform.rotation);
                body.transform.SetParent(syncPoint.body);
                body.transform.localPosition = syncPoint.body.transform.Find("localPoint").transform.localPosition;
                body.transform.localRotation = syncPoint.body.transform.Find("localPoint").transform.localRotation;
                tmpDud.body = tmp[0];
                break;
        }
    }
    public static void SetOtherDressUp(string code,GameObject oplayer)
    {
        
        string[] tmp = code.Split(':');
        SyncPointInPlayer syncPoint = oplayer.transform.Find("Points").GetComponent<SyncPointInPlayer>();
        switch (tmp[1])
        {
            case "head":
                GameObject ohead = Instantiate(Resources.Load<GameObject>("Prefabs/DressUp/" + tmp[0]), oplayer.transform.position, oplayer.transform.rotation);
                ohead.transform.SetParent(syncPoint.head);
                ohead.transform.localPosition = syncPoint.body.transform.Find("localPoint").transform.localPosition;
                ohead.transform.localRotation = syncPoint.body.transform.Find("localPoint").transform.localRotation;
                break;
            case "face":
                GameObject oface = Instantiate(Resources.Load<GameObject>("Prefabs/DressUp/" + tmp[0]), oplayer.transform.position, oplayer.transform.rotation);
                oface.transform.SetParent(syncPoint.head);
                oface.transform.localPosition = syncPoint.body.transform.Find("localPoint").transform.localPosition;
                oface.transform.localRotation = syncPoint.body.transform.Find("localPoint").transform.localRotation;
                break;
            case "body":
                GameObject obody = Instantiate(Resources.Load<GameObject>("Prefabs/DressUp/" + tmp[0]), oplayer.transform.position, oplayer.transform.rotation);
                obody.transform.SetParent(syncPoint.body);
                obody.transform.localPosition = syncPoint.body.transform.Find("localPoint").transform.localPosition;
                obody.transform.localRotation = syncPoint.body.transform.Find("localPoint").transform.localRotation;
                break;
        }
    }
    public void Save()
    {
        dud = GameObject.Find("GameFacade").GetComponent<GameFacade>().playerMng.UserData.dressUpData;
        tmpDud.id = dud.id;
        if (tmpDud.head.Equals(""))
            tmpDud.head = dud.head;
        if (tmpDud.face.Equals(""))
            tmpDud.face = dud.face;
        if (tmpDud.body.Equals(""))
            tmpDud.body = dud.body;

        GetComponent<DressUpResquest>().SendRequest(tmpDud.id.ToString(),tmpDud.head,tmpDud.face,tmpDud.body);

        gameObject.SetActive(false);
    }
    public void Cancel()
    {
        UserData ud = GameObject.Find("GameFacade").GetComponent<GameFacade>().playerMng.UserData;
        SetDressUp(ud.dressUpData.head + ":head");
        SetDressUp(ud.dressUpData.face + ":face");
        SetDressUp(ud.dressUpData.body + ":body");

        gameObject.SetActive(false);
    }
}
