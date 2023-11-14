using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;

public class PlayerBase : MonoBehaviour
{
    public CPlayer cplayer;//玩家输入器
    //public CharacterController controller;
    public float HP = 100;
    public vThirdPersonController cc;
    public vThirdPersonCamera tpCamera;
    public Camera cameraMain;

    public UserData ud;//玩家信息
    public bool isDead = false;

    public float speed = 5;
    public Transform MoveDirection;//移动方向
    public Transform AimTarget;//瞄准目标点
    public Transform ToAimTarget;//瞄准目标控制点
    public Transform DiscardPoint;//丢弃物品位置点
    public bool isUseLocomotion = true;

    public int moveForward = 0;
    public int moveRight = 0;

    public GameObject LoadingGun;//正在使用的枪
    public List<GameObject> Guns = new List<GameObject>();//玩家持有的枪
    public bool isLoad = false;
    public float ShootWeight = 1;

    public bool isAim = false;//是否在瞄准
    public bool isMovingAim = false;//是否正在过渡到瞄准

    public Transform GunPoint1, GunPoint2, GoodsPoint, LoadPoint;//不同状态下枪械位置点

    public GameObject CanPackEquipment;//可以拾起的装备
    public List<GameObject> CanPackEquipments = new List<GameObject>();//可以拾起的装备集，玩家每触碰到一个就加进去

    public BackpackPanel.BackpackInformation backpackInformation = new BackpackPanel.BackpackInformation();//背包基本信息统计表

    public int loadingGunId = -1, gun1Id = -1, gun2Id = -1;
    // Start is called before the first frame update
    public void Init()
    {
        if (GetComponent<CPlayer>() != null)
        {
            cameraMain = Camera.main;
        }
        else
        {
            cameraMain = MoveDirection.gameObject.AddComponent<Camera>();
            cameraMain.depth = -1;
        }

    }
    void Start()
    {
        cplayer = GetComponent<CPlayer>();
        //controller = GetComponent<CharacterController>();
        cc = GetComponent<vThirdPersonController>();
        if (cc != null)
            cc.Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cc.UpdateMotor();               // updates the ThirdPersonMotor methods       
        cc.ControlRotationType();       // handle the controller rotation type

        if (isUseLocomotion)
        {
            cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
        }
        OPlayerSync();
        AimAnim();
    }
    void Update()
    {
        Behavior();
        cc.UpdateAnimator();            // updates the Animator Parameters
    }
    public void OnAnimatorMove()
    {
        cc.ControlAnimatorRootMotion(); // handle root motion animations 
    }
    public void Behavior()
    {
        CameraInput();
        cc.animator.SetFloat("ShootWeight", isLoad ? 1 : 0, .1f, Time.deltaTime);
        AimTarget.transform.position = Vector3.Lerp(AimTarget.transform.position, ToAimTarget.transform.position, 0.5f);
        if (isAim && !isMovingAim)
        {
            cc.input.x = 0;
            cc.input.z = 0;
            return;
        }
        cc.input.x = moveRight;
        cc.input.z = moveForward;

        //Debug.Log(AimTarget.transform.position);
        PackDetection();
    }
    public void Control(int k, int s)
    {
        switch ((KeyCode)k)
        {
            case KeyCode.W:
                if (s == 0)
                    moveForward = 0;
                if (s == 1)
                    moveForward = 1;
                break;
            case KeyCode.S:
                if (s == 0)
                    moveForward = 0;
                if (s == 1)
                    moveForward = -1;
                break;
            case KeyCode.D:
                if (s == 0)
                    moveRight = 0;
                if (s == 1)
                    moveRight = 1;
                break;
            case KeyCode.A:
                if (s == 0)
                    moveRight = 0;
                if (s == 1)
                    moveRight = -1;
                break;
            case KeyCode.Space:
                if (s == 1 && JumpConditions())
                    Debug.Log("jump");
                cc.Jump();
                break;
            case KeyCode.R:
                if (s == 1)
                    LoadMag();
                break;
            case KeyCode.Alpha1:
                if (s == 1)
                    LoadGun(Guns[0]);
                break;
            case KeyCode.Alpha2:
                if (s == 1)
                    LoadGun(Guns[1]);
                break;
            case KeyCode.Mouse0:
                if (s == 0)
                    Fire(false);
                if (s == 1)
                    Fire(true);
                break;
            case KeyCode.Mouse1:
                if (s == 0)
                    isAim = true;
                cameraMain.transform.SetParent(MoveDirection);
                if (s == 1)
                {
                    isAim = false;
                    StartCoroutine(AimTurnBoay());
                }

                isMovingAim = true;

                AimTarget.transform.localPosition = new Vector3(0, 1.4f, 5);
                ToAimTarget.transform.localPosition = new Vector3(0, 1.4f, 5);
                break;
            case KeyCode.F:
                if (s == 1)
                    if (CanPackEquipment != null)
                        Pack(CanPackEquipment.GetComponent<Equipment>().id);
                break;

        }
    }
    protected virtual void CameraInput()
    {
        cc.UpdateMoveDirection(MoveDirection);

        //var Y = Input.GetAxis("Mouse Y");
        //var X = Input.GetAxis("Mouse X");

        //tpCamera.RotateCamera(X, Y);
    }
    protected bool JumpConditions()
    {
        return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
    }
    public void AimAnim()
    {


        if (GetComponent<CPlayer>() == null)
            return;

        if (isMovingAim)
        {
            if (isAim)
            {
                cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, MoveDirection.position, 0.3f);
                cameraMain.transform.rotation = Quaternion.Lerp(cameraMain.transform.rotation, MoveDirection.rotation, 0.3f);
                if (Vector3.Distance(cameraMain.transform.position, MoveDirection.position) < 0.01f)
                {
                    isAim = false;
                    isMovingAim = false;
                    cameraMain.transform.SetParent(MoveDirection);
                    AimTarget.transform.localPosition = new Vector3(0, 1.4f, 5);
                    ToAimTarget.transform.localPosition = new Vector3(0, 1.4f, 5);
                }
            }
            else
            {
                if (LoadingGun == null)
                    return;
                cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, LoadingGun.transform.Find("Gun").transform.Find("ShootCamera").position, 0.3f);
                cameraMain.transform.rotation = Quaternion.Lerp(cameraMain.transform.rotation, LoadingGun.transform.Find("Gun").transform.Find("ShootCamera").rotation, 0.3f);
                if (Vector3.Distance(cameraMain.transform.position, LoadingGun.transform.Find("Gun").transform.Find("ShootCamera").position) < 0.01f)
                {
                    isAim = true;
                    isMovingAim = false;
                    cameraMain.transform.SetParent(LoadingGun.transform.Find("Gun").transform.Find("ShootCamera"));
                    AimTarget.transform.localPosition = new Vector3(0, 1.4f, 5);
                    ToAimTarget.transform.localPosition = new Vector3(0, 1.4f, 5);
                }
            }
        }
        else
        {
            if (isAim)
            {

            }
        }
    }
    IEnumerator AimTurnBoay()
    {
        moveForward = 1;
        yield return new WaitForSeconds(0.2f);
        moveForward = 0;
    }
    public void Fire(bool isOpen)
    {
        if (LoadingGun != null)
            LoadingGun.GetComponent<Gun>().Fire(isOpen);
    }
    public void LoadMag()
    {
        if (LoadingGun != null)
            LoadingGun.GetComponent<Gun>().LoadMag();
    }
    public void LoadGun(GameObject Gun)
    {
        Debug.Log("5-7-11");
        if (Gun == null)
            return;
        Debug.Log("5-7-12");
        if (Gun.Equals(LoadingGun))
        {
            UnLoadGun();
            return;
        }
        Debug.Log("5-7-13");

        if (LoadingGun != null)
        {
            UnLoadGun();
        }
        isLoad = true;
        LoadingGun = Gun;
        LoadingGun.transform.SetParent(LoadPoint);
        LoadingGun.transform.localPosition = Vector3.zero;
        LoadingGun.transform.localRotation = Quaternion.identity;

    }
    public void UnLoadGun()
    {
        Debug.Log("5-7-14");
        if (LoadingGun == null)
            return;
        Debug.Log("5-7-15");
        isLoad = false;
        Transform GunPoint = GunPoint1.transform.childCount == 0 ? GunPoint1 : GunPoint2;
        LoadingGun.transform.SetParent(GunPoint);
        LoadingGun.transform.localPosition = Vector3.zero;
        LoadingGun.transform.localRotation = Quaternion.identity;
        LoadingGun = null;
        Debug.Log("5-7-16");
    }
    public void Pack(int equipmentId)
    {
        Debug.Log("5-7-3");
        Equipment e = Equipment.FindEquipmentById(equipmentId);

        if (e != null)
        {
            if (!e.isBePacked)
            {
                Debug.Log("5-7-2");
                if (cplayer != null)
                {
                    if (cplayer.Backpack.IsFull(e.equipmentTypes))
                    {
                        Debug.Log("5-7-1");
                        FullTip();
                        return;
                    }
                    cplayer.Backpack.Add(e.equipmentTypes, e.gameObject);
                    backpackInformation = cplayer.Backpack.GetBackpackGoodsInformation();
                }
                else
                {

                }
                e.isBePacked = true;
                Debug.Log("5-7-4" + e.equipmentTypes);
                switch (e.equipmentTypes)
                {
                    case EquipmentTypes.Gun:
                        Transform GunPoint = GunPoint1.transform.childCount == 0 ? GunPoint1 : GunPoint2;
                        if (GunPoint1.transform.childCount == 0)
                        {
                            GunPoint = GunPoint1;
                            Guns[0] = e.gameObject;
                        }
                        else
                        {
                            GunPoint = GunPoint2;
                            Guns[1] = e.gameObject;
                        }
                        e.transform.SetParent(GunPoint);
                        e.transform.localPosition = Vector3.zero;
                        e.transform.localRotation = Quaternion.identity;
                        e.GetComponent<Collider>().enabled = false;
                        Destroy(e.GetComponent<Rigidbody>());
                        Debug.Log("5-7-8");
                        //初始化枪械信息
                        switch (e.GetComponent<Gun>().bulletType)
                        {
                            case GoodsTypes.RifleBullet:
                                e.GetComponent<Gun>().Load(this, backpackInformation.RifleBulletCount);
                                break;
                            case GoodsTypes.ShotgunBullet:
                                e.GetComponent<Gun>().Load(this, backpackInformation.ShotgunBulletCount);
                                break;
                            case GoodsTypes.SniperBullet:
                                e.GetComponent<Gun>().Load(this, backpackInformation.SniperBulletCount);
                                break;
                            case GoodsTypes.GrenadeBullet:
                                e.GetComponent<Gun>().Load(this, backpackInformation.GrenadeBulletCount);
                                break;
                        }
                        Debug.Log("5-7-5");
                        break;
                    case EquipmentTypes.Goods:
                        e.transform.SetParent(GoodsPoint);
                        e.transform.localPosition = Vector3.zero;
                        e.transform.localRotation = Quaternion.identity;
                        e.GetComponent<MeshRenderer>().enabled = false;
                        e.GetComponent<Collider>().enabled = false;
                        Destroy(e.GetComponent<Rigidbody>());
                        break;
                }
            }
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (layerIndex == 0)
        {
            cc.animator.SetLookAtPosition(AimTarget.position);
            cc.animator.SetLookAtWeight(1f, 1f, 1f, 1f, 1f);
        }
        if (LoadingGun == null || !isLoad)
            return;
        Transform leftHandle = LoadingGun.transform.Find("Gun").transform.Find("GunModel").transform.Find("leftHandle");
        Transform rightHandle = LoadingGun.transform.Find("Gun").transform.Find("GunModel").transform.Find("rightHandle");
        ShootWeight = cc.animator.GetFloat("ShootWeight");
        if (layerIndex == 1)
        {
            if (leftHandle != null)
            {
                cc.animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandle.transform.position);
                cc.animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandle.transform.rotation);
                cc.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ShootWeight);
                cc.animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ShootWeight);
            }

            if (rightHandle != null)
            {
                cc.animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandle.transform.position);
                cc.animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandle.transform.rotation);
                cc.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ShootWeight);
                cc.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ShootWeight);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("equipment"))
        {
            CPlayer cp = GetComponent<CPlayer>();
            if (cp != null || true)
            {
                CanPackEquipments.Add(other.gameObject);
            }
            //Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("equipment"))
        {
            CPlayer cp = GetComponent<CPlayer>();
            if (cp != null || true)
            {
                CanPackEquipments.Remove(other.gameObject);
            }
            //Destroy(other.gameObject);
        }
    }
    /// <summary>
    /// 拾起物品检测
    /// </summary>
    public void PackDetection()
    {
        if (GetComponent<CPlayer>() == null)
            return;
        if (CanPackEquipments.Count > 0)
        {
            for (int i = 0; i < CanPackEquipments.Count; i++)
            {
                Equipment e = CanPackEquipments[i].GetComponent<Equipment>();
                if (!e.isBePacked)
                {
                    if (CanPackEquipment == null)
                        CanPackEquipment = CanPackEquipments[i];
                    if (CanPackEquipment.Equals(CanPackEquipments[i]))
                    {
                        return;
                    }
                    else
                    {
                        CanPackEquipment = CanPackEquipments[i];
                        PackTip();
                    }
                }
                else
                {
                    CanPackEquipments.Remove(e.gameObject);
                }
            }
        }
    }
    /// <summary>
    /// 物品拾起提示
    /// </summary>
    public void PackTip()
    {
        if (cplayer != null)
            StartCoroutine(cplayer.ToastTip("按下 F 捡起" + CanPackEquipment.name, 5f));
    }
    public void FullTip()
    {
        if (cplayer != null)
            StartCoroutine(cplayer.ToastTip("背包已满", 5f));
    }
    public void Discard(int id)
    {
        Equipment e = Equipment.FindEquipmentById(id);
        if (e != null)
        {
            e.isBePacked = false;
            e.GetComponent<Collider>().enabled = true;
            e.gameObject.AddComponent<Rigidbody>();
            e.transform.SetParent(DiscardPoint);
            e.transform.localPosition = Vector3.zero;
            e.transform.localRotation = Quaternion.identity;
            Debug.Log("5-9-1:" + e.transform.parent.name + "|" + e.name + "|" + e.id);
            if (e.equipmentTypes == EquipmentTypes.Goods)
            {
                e.GetComponent<MeshRenderer>().enabled = true;
            }
            Debug.Log("5-31-1");
            if (e.equipmentTypes == EquipmentTypes.Gun)
            {
                Debug.Log("5-31-1.5");
                Debug.Log("5-31-1.6");
                if (true)
                {
                    Debug.Log("5-31-2");
                    if (LoadingGun.GetComponent<Gun>().id == id)
                    {
                        UnLoadGun();
                        Debug.Log("5-31-3");
                    }
                }

                for (int i = 0; i < Guns.Count; i++)
                {
                    Debug.Log("5-31-4");
                    if (Guns[i] == null)
                        continue;
                    if (Guns[i].GetComponent<Gun>().id == id)
                    {
                        Debug.Log("5-31-5");
                        Guns[i] = null;
                    }
                }
            }
            CanPackEquipments.Remove(e.gameObject);
            e.transform.SetParent(GameObject.Find("AllEquipments").transform);
        }
    }
    public void BeAttacked(float injury)
    {
        HP -= injury;
        Debug.Log("被丧尸攻击到");

        CPlayer cp = GetComponent<CPlayer>();
        if (cp != null)
        {
            cp.BloodBar.SetBloodBar(HP);
        }
        if (HP <= 0)
        {
            Debug.Log("你死了");
            if (cp != null&&!isDead)
            {
                isDead = true;
                cp.isCanBeControl = false;
                cp.isStartGame = false;
                cp.deadUI.SetActive(true);
                NoticeRequest nr = transform.parent.GetComponent<NoticeRequest>();
                nr.SendRequest(EventCode.NoticeTypes.Dead, "");
                GetComponent<Animator>().CrossFadeInFixedTime("DeadAnim", 0.1f);
                Cursor.visible = true;
            }
        }
    }
    public void Treatment()
    {
        HP = 100;
        CPlayer cp = GetComponent<CPlayer>();
        if (cp != null)
        {
            cp.BloodBar.SetBloodBar(HP);
        }
    }
    /// <summary>
    /// 修复第三人称中其他玩家拿错枪的问题
    /// </summary>
    public void OPlayerSync()
    {
        if (GetComponent<CPlayer>() != null)
            return;


        if (gun1Id != -1)
        {
            if (Guns[0] == null)
            {
                Pack(gun1Id);
            }
            else
            {
                if (Guns[0].GetComponent<Gun>().id == gun1Id)
                {

                }
                else
                {
                    Discard(Guns[0].GetComponent<Gun>().id);
                    Pack(gun1Id);
                }
            }
        }
        else
        {
            if (Guns[0] != null)
            {
                Discard(Guns[0].GetComponent<Gun>().id);
            }
        }
        if (gun2Id != -1)
        {
            if (Guns[1] == null)
            {
                Pack(gun2Id);
            }
            else
            {
                if (Guns[1].GetComponent<Gun>().id == gun2Id)
                {

                }
                else
                {
                    Discard(Guns[1].GetComponent<Gun>().id);
                    Pack(gun2Id);
                }
            }
        }
        else
        {
            if (Guns[1] != null)
            {
                Discard(Guns[1].GetComponent<Gun>().id);
            }
        }
        if (loadingGunId != -1)
        {
            GameObject g = Equipment.FindEquipmentById(loadingGunId).gameObject;
            if (LoadingGun == null)
            {
                LoadGun(g);
            }
            else
            {
                if (LoadingGun.Equals(g))
                {

                }
                else
                {
                    LoadGun(g);
                }
            }
        }
        else
        {
            if (LoadingGun != null)
            {
                UnLoadGun();
            }
        }
    }
}
