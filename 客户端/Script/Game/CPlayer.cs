
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventCode;

public class CPlayer : MonoBehaviour
{
    public Camera MainCamera;
    public Camera MapCamera;
    public UIBloodBar BloodBar;
    public BackpackPanel Backpack;
    public EscPanel escPanel;
    public GameObject deadUI;
    public Text Toast;
    private vThirdPersonCamera personCamera;
    private ControlRequest controlRequest;
    private NoticeRequest noticeRequest;
    public PlayerBase playerBase;
    public bool isCanBeControl = false;
    public bool isStartGame = false;
    public bool isAimming = false;

    public int[] keycodeIndex = { 119, 115, 97, 100, 114, 32, 323, 324, 49, 50, 102 };
    private Vector3 tmpPosition = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    public void Init()
    {
        InitializeTpCamera();
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        MapCamera = GameObject.Find("MapCamera").GetComponent<Camera>();
        MapCamera.GetComponent<MapCamera>().target = transform;
        Backpack = GameObject.Find("Backpack").GetComponent<BackpackPanel>();
        BloodBar = GameObject.Find("bloodbar").GetComponent<UIBloodBar>();
        escPanel = GameObject.Find("ESCPanel").GetComponent<EscPanel>();
        Toast = GameObject.Find("Toast").GetComponent<Text>();
        deadUI = GameObject.Find("DeadUI");
        deadUI.SetActive(false);
        controlRequest = transform.parent.GetComponent<ControlRequest>();
        noticeRequest = transform.parent.GetComponent<NoticeRequest>();
        playerBase = GetComponent<PlayerBase>();
        playerBase.cplayer = this;
        isCanBeControl = true;
        isStartGame = true;
    }
    void Start()
    {
        //Init();
    }
    void Update()
    {
        if (isStartGame)
        {
            Controler();
        }
    }
    // Update is called once per frame    
    void Controler()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escPanel.transform.GetChild(0).gameObject.SetActive(isCanBeControl);
            Cursor.visible = isCanBeControl;//隐藏指针
            isCanBeControl = !isCanBeControl;
            //Cursor.lockState = CursorLockMode.None;//锁定指针到视图中心            
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Backpack.transform.GetChild(0).gameObject.SetActive(isCanBeControl);
            Cursor.visible = isCanBeControl;
            isCanBeControl = !isCanBeControl;
        }
        if (isCanBeControl)
        {
            InputCamera();
            if (Vector3.Distance(Input.mousePosition, tmpPosition) > 0.15)
            {
                tmpPosition = Input.mousePosition;
                Vector3 p = MainCamera.transform.position;
                Quaternion r = MainCamera.transform.rotation;
                string data = p.x + ":" + p.y + ":" + p.z + "," + r.x + ":" + r.y + ":" + r.z + ":" + r.w;
                p = playerBase.ToAimTarget.position;
                r = playerBase.ToAimTarget.rotation;
                data = data + "," + p.x + ":" + p.y + ":" + p.z + "," + r.x + ":" + r.y + ":" + r.z + ":" + r.w;
                controlRequest.SendRequest(InputTypes.MouseMove, data);
            }
            foreach (int k in keycodeIndex)
            {
                if (Input.GetKeyDown((KeyCode)k))
                {
                    if ((KeyCode)k == KeyCode.F)
                    {
                        if (playerBase.CanPackEquipment != null)
                            noticeRequest.SendRequest(NoticeTypes.Pick, playerBase.CanPackEquipment.GetComponent<Equipment>().id.ToString());
                        playerBase.Control(k, 1);
                        StartCoroutine(controlRequest.DelaySend(InputTypes.KeyEven, k + ",1", 0.2f));
                        return;
                    }
                    if ((KeyCode)k == KeyCode.Alpha1)
                    {
                        if (playerBase.Guns[0] == null)
                            return;
                        noticeRequest.SendRequest(NoticeTypes.Load, 0.ToString() + "&" + playerBase.Guns[0].GetComponent<Gun>().id);
                        playerBase.Control(k, 1);
                        StartCoroutine(controlRequest.DelaySend(InputTypes.KeyEven, k + ",1", 0.2f));
                        return;
                    }
                    if ((KeyCode)k == KeyCode.Alpha2)
                    {
                        if (playerBase.Guns[1] == null)
                            return;
                        noticeRequest.SendRequest(NoticeTypes.Load, 1.ToString() + "&" + playerBase.Guns[1].GetComponent<Gun>().id);
                        playerBase.Control(k, 1);
                        StartCoroutine(controlRequest.DelaySend(InputTypes.KeyEven, k + ",1", 0.2f));
                        return;
                    }
                    //============================================================
                    playerBase.Control(k, 1);
                    controlRequest.SendRequest(InputTypes.KeyEven, k + ",1");
                    //============================================================
                    if ((KeyCode)k == KeyCode.R)
                    {
                        if (playerBase.LoadingGun == null)
                            return;

                        int bulletInGun = playerBase.LoadingGun.GetComponent<Gun>().bulletInGun;
                        int bulletInMag = playerBase.LoadingGun.GetComponent<Gun>().bulletInMag;
                        noticeRequest.SendRequest(NoticeTypes.LoadMag, bulletInGun.ToString() + "&" + bulletInMag.ToString());
                    }
                }
                if (Input.GetKeyUp((KeyCode)k))
                {
                    playerBase.Control(k, 0);
                    controlRequest.SendRequest(InputTypes.KeyEven, k + ",0");
                }
            }
        }
    }
    void InputCamera()
    {
        if (isCanBeControl)
        {
            if (playerBase == null)
                return;
            float Y = Input.GetAxis("Mouse Y");
            float X = Input.GetAxis("Mouse X");
            if (!playerBase.isAim || playerBase.isMovingAim)
            {
                personCamera.RotateCamera(X, Y);
                playerBase.MoveDirection = MainCamera.transform;
            }
            if (playerBase.isAim && !playerBase.isMovingAim)
            {
                playerBase.ToAimTarget.localPosition = new Vector3(playerBase.ToAimTarget.localPosition.x + X / 5, playerBase.ToAimTarget.localPosition.y + Y / 5, playerBase.ToAimTarget.localPosition.z);
            }
        }
    }
    void InitializeTpCamera()
    {
        //Debug.Log("1-4");
        if (personCamera == null)
        {
            //Debug.Log("1-5");
            personCamera = FindObjectOfType<vThirdPersonCamera>();
            if (personCamera == null)
                return;
            if (personCamera)
            {
                //Debug.Log("1-6");
                personCamera.SetMainTarget(this.transform);
                personCamera.Init();
            }
        }
    }
    public void Discard(int id)
    {
        playerBase.Discard(id);
        noticeRequest.SendRequest(NoticeTypes.Discard, id.ToString());
    }
    public IEnumerator ToastTip(string content, float time)
    {
        Toast.text = content;
        yield return new WaitForSeconds(time);
        Toast.text = "";
    }
}
