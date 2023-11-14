using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventCode;

public class SelectRoomPanel : MonoBehaviour
{
    public RoomPanel roomPanel;
    public InputField inputField;
    public Text Result;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void Create()
    {
        if (inputField.text == "")
        {
            Result.text = "房间id号不得为空";
            return;
        }
        if (!int.TryParse(inputField.text, out int i))
        {
            Result.text = "id号不合法，必须为纯数字";
            return;
        }
        GetComponent<CreateRoomRequest>().SendRequest(roomPanel.ud.Id.ToString() + "," + inputField.text);
    }
    public void Join()
    {
        if(inputField.text == "")
        {
            Result.text = "房间id号不得为空";
            return;
        }
        if(!int.TryParse(inputField.text,out int i))
        {
            Result.text = "id号不合法，必须为纯数字";
            return;
        }
        GetComponent<JoinRoomRequest>().SendRequest(roomPanel.ud.Id.ToString() + "," + inputField.text);
    }
}
