using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventCode;

public class AirDropPanel : MonoBehaviour
{
    public List<GameObject> equipments = new List<GameObject>();
    public List<Transform> createPoints = new List<Transform>();
    public GameMode gameMode;
    public NoticeRequest noticeRequest;
    public int lastTimeCreateZombies;
    public int createZombiesTime=30;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
    }
    IEnumerator Init()
    {
        yield return new WaitForSeconds(3f);
        gameMode = GameObject.Find("GameMode").GetComponent<GameMode>();
        noticeRequest = gameMode.transform.Find("Control").GetComponent<NoticeRequest>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Math.Abs(DateTime.Now.Second- lastTimeCreateZombies)> createZombiesTime)
        {
            lastTimeCreateZombies = DateTime.Now.Second;
            AutoCreateZombies();
        }
    }
    public void GetEquipment(int index)
    {
        noticeRequest.SendRequest(NoticeTypes.Create, index.ToString()+",0");
        CreateEquipment(index.ToString() + ",0");
    }
    public void CreateEquipment(string d)
    {
        Transform t;
        GameObject e;
        string[] tmp = d.Split(',');
        int index = int.Parse(tmp[0]);
        int pos = int.Parse(tmp[1]);
        if (index == 0)
        {
            t = createPoints[pos];
            e = Instantiate(equipments[index], t.position, t.rotation);
        }
        else
        {
            t= createPoints[pos];
            e = Instantiate(equipments[index], t.position, t.rotation);
            if (e.GetComponent<Equipment>() != null)
            {
                AllEquipmentManager.EquipmentInit(e.GetComponent<Equipment>());
            }
        }
        
        string[] tmp2 = e.name.Split('(');
        e.name = tmp2[0];
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CPlayer>() != null)
        {
            Cursor.visible = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CPlayer>() != null)
        {
            Cursor.visible = false;
        }
    }
    public void AutoCreateZombies()
    {
        int r = UnityEngine.Random.Range(0, 8);
        noticeRequest.SendRequest(NoticeTypes.Create, "0,"+r);
        CreateEquipment("0," + r);
    }
}
