using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToRoom : MonoBehaviour
{
    public UserData ud;
    // Start is called before the first frame update
    public void Init()
    {
        GameObject o = GameObject.Find("RoomPanel");
        if (o != null)
        {
            //StartCoroutine(DelayInit(o));
            GameObject gameRoot = GameObject.Find("NewGameRoot");
            if (gameRoot != null)
            {
                gameRoot.name = "";
                Destroy(gameRoot);
            }
            RoomPanel roomPanel = o.GetComponent<RoomPanel>();
            GameObject.Find("Main Camera").GetComponent<CUICamera>().MoveTo(3);
            LoginPanel loginPanel = GameObject.Find("LoginPanel").GetComponent<LoginPanel>();
            loginPanel.roomPanel.gameObject.SetActive(true);
            loginPanel.roomPanel.gameObject.GetComponent<RoomPanel>().Init(ud);
            o.GetComponent<QuitRoomRequest>().SendRequest();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DelayInit(GameObject o)
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("backtoroom");
        GameObject gameRoot= GameObject.Find("NewGameRoot");
        if (gameRoot != null)
        {
            Destroy(gameRoot);
        }
        RoomPanel roomPanel = o.GetComponent<RoomPanel>();
        GameObject.Find("Main Camera").GetComponent<CUICamera>().MoveTo(3);
        LoginPanel loginPanel = GameObject.Find("LoginPanel").GetComponent<LoginPanel>();
        loginPanel.roomPanel.gameObject.SetActive(true);
        loginPanel.roomPanel.gameObject.GetComponent<RoomPanel>().Init(ud);
        Debug.Log("backtoroom2");
    }
}
