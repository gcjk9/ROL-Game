using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : Equipment
{
    public GunTypes guntype;

    public PlayerBase user;
    public bool Insurance = true;

    public bool isUseSights = false;
    public float injury = 0;//伤害值
    public float shootSpeed = 1;//射速
    public float bulletSpeed = 1000;//子弹发射速度
    public float dispersed = 1;//子弹离散，越小弹孔越密集

    public int bulletInGun = 30;//弹夹的子弹
    public int bulletInMag = 60;//后备的子弹

    public int bulletInGunMax = 0;//弹夹最大容量
    public int bulletInMagMax = 0;//后备最大容量

    public GoodsTypes bulletType;//发射子弹类型

    private long lastOpenFireTime = 0;//上次开火时间
    private float time = 0;

    //public Transform AimPoint;
    public GameObject bullet;
    public Queue bullets=new Queue();
    public Transform Muzzle;
    public Text InGunText;
    public Text InMagText;

    public GameObject fire;
    public Transform target;
    public Transform gun;
    public Transform GunModel;
    public List<Transform> recoilPointTo=new List<Transform>();
    public Transform recoilPointFrom;
    public RenderTexture rt;
    public int recoilToPointRange;
    public bool isInFormPoint=true;

    public bool isOpenFire = false;
    public bool isCanFire = true;

    public AudioClip AFire;
    public AudioClip ALoad;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isCanFire = (DateTime.Now.Ticks - lastOpenFireTime) > (60 * 1000 * 1000 * 10 / shootSpeed);
        if (isOpenFire)
        {
            OpenFire();
        }
        Shake();
        Recoil();
        gun.LookAt(target);
    }
    public void Load(PlayerBase user,int bulletInMag)
    {
        this.user = user;
        this.bulletInMag = bulletInMag;
        UpdataScreen();
        if (isUseSights)
        {
            rt = new RenderTexture(new RenderTextureDescriptor(512, 512));
            Camera sightCamera = transform.Find("Gun").Find("GunModel").Find("sightCamera").GetComponent<Camera>();

            sightCamera.targetTexture = rt;

            transform.Find("Gun").Find("GunModel").Find("sight").Find("view").Find("viewMask").Find("RawImage").GetComponent<RawImage>().texture = rt;

            Debug.Log("Load");
        }
    }
    public void Fire(bool isOpen)
    {
        isOpenFire = isOpen;
    }
    public bool OpenFire()
    {       

        if (bulletInGun > 0 && Insurance)
        {
            if (isCanFire)
            {
                bulletInGun--;
                UpdataScreen();
                lastOpenFireTime = DateTime.Now.Ticks;

                GameObject b = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/"+bulletType.ToString()), Muzzle.position, Muzzle.rotation);                
                b.GetComponent<Rigidbody>().AddForce(Muzzle.transform.up * bulletSpeed, ForceMode.Impulse);
                switch (bulletType)
                {
                    case GoodsTypes.RifleBullet:
                        b.GetComponent<Bullet>().injury = injury;
                        break;
                    case GoodsTypes.ShotgunBullet:
                        b.GetComponent<ShotgunBullet>().injury = injury;
                        b.GetComponent<ShotgunBullet>().Divergency();
                        break;
                    case GoodsTypes.SniperBullet:
                        b.GetComponent<Bullet>().injury = injury;
                        break;
                }
                bullets.Enqueue(b);
                if (bullets.Count > 20)
                {
                    b = (GameObject)bullets.Dequeue();
                    if(b!=null)
                        Destroy(b);
                }
                SoundsPlay(AFire);
                fire.SetActive(true);
                StartCoroutine(Reactivate());
                
            }           
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// 上膛
    /// </summary>
    public bool LoadMag()
    {
        if (user.cplayer != null)//获取背包子弹数量
        {
            BackpackPanel.BackpackInformation bi = user.cplayer.Backpack.GetBackpackGoodsInformation();
            user.cplayer.Backpack.UpdateBackpackGoodsInformation(bulletType, -(bulletInGunMax - bulletInGun));
            //switch (bulletType)
            //{
            //    case GoodsTypes.RifleBullet:
            //        if (user.cplayer.Backpack.UpdateBackpackGoodsInformation(GoodsTypes.RifleBullet, -(bulletInGunMax-bulletInGun)))
            //        {
            //            Debug.Log("LoadMag()Success");
            //        }
            //        else
            //            return false;
            //        break;
            //}            
        }
        Debug.Log(bulletInGun + ":" + bulletInMag);
        if (bulletInGun < bulletInGunMax && bulletInMag != 0)
        {
            if ((bulletInGunMax - bulletInGun) < bulletInMag)
            {
                bulletInMag -= (bulletInGunMax - bulletInGun);
                bulletInGun = bulletInGunMax;
            }
            else
            {
                bulletInGun += bulletInMag;
                bulletInMag = 0;
            }
        }
        SoundsPlay(ALoad);
        UpdataScreen();
        return true;
    }
    public void UpdateBulletCountInMag(int count)
    {
        this.bulletInMag = count;
    }
    /// <summary>
    /// 枪械后坐力
    /// </summary>
    void Recoil()
    {
        if (isOpenFire&&bulletInGun!=0)
        {
            if (isInFormPoint)
            {
                GunModel.localPosition = Vector3.Lerp(GunModel.localPosition, recoilPointTo[recoilToPointRange].localPosition, 0.9f);
                if (Vector3.Distance(GunModel.localPosition, recoilPointTo[recoilToPointRange].localPosition) < 0.005f)
                    isInFormPoint = false;
            }
            else
            {
                GunModel.localPosition = Vector3.Lerp(GunModel.localPosition, recoilPointFrom.localPosition, shootSpeed / 1000f);
                if (Vector3.Distance(GunModel.localPosition, recoilPointFrom.localPosition) < 0.005f)
                {
                    isInFormPoint = true;
                    recoilToPointRange= (int)UnityEngine.Random.Range(0, recoilPointTo.Count);
                }
                    
            }
        }
        else
        {
            if (!isInFormPoint)
            {
                GunModel.localPosition = Vector3.Lerp(GunModel.localPosition, recoilPointFrom.localPosition, shootSpeed / 1000f);
                if (Vector3.Distance(GunModel.localPosition, recoilPointFrom.localPosition) < 0.005f)
                    isInFormPoint = true;
            }
        }
    }
    void Shake()
    {
        //Debug.Log(Mathf.PerlinNoise(time += 0.001f, time += 0.001f));
        if(!((DateTime.Now.Ticks - lastOpenFireTime) > (60 * 1000 * 1000 * 10 / (shootSpeed*5)))&&bulletInGun!=0)
            GunModel.transform.localPosition = Vector3.Lerp(GunModel.transform.localPosition, recoilPointFrom.localPosition*Mathf.PerlinNoise(Mathf.Abs(Mathf.Sin(time+=0.2f)), 0),0.8f);
    }
    IEnumerator Reactivate()
    {
        yield return new WaitForSeconds(0.025f);
        fire.SetActive(false);
        yield return new WaitForSeconds(2000f/shootSpeed);

    }
    public void UpdataScreen()
    {
        InGunText.text = bulletInGun.ToString();
        InMagText.text = bulletInMag.ToString();
    }
    public void SoundsPlay(AudioClip c)
    {
        GetComponent<AudioSource>().clip = c;
        GetComponent<AudioSource>().Play();
    }
}
public enum GunTypes
{
    Rifle=0,
    Shotgun=1,
    Sniper=2,
    Grenade=3
}
public enum RTypes
{
    AKM,
    M416,
    M16A4,
    S12K,
    UMP9
}
