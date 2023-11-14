using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BackpackPanel : MonoBehaviour
{
    public bool isShow = true;
    public CPlayer cplayer;
    public int gunCapacity = 2;
    public int goodsCapacity = 12;
    public List<GameObject> gunList = new List<GameObject>();
    public List<GameObject> goodsList = new List<GameObject>();
    public List<GameObject> gunUIList = new List<GameObject>();
    public List<GameObject> goodsUIList = new List<GameObject>();
    public int[] goodsPosition = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };//记录物品在背包中的位置，-1为空，保存物品id
    public int[] gunsPosition = { -1, -1 };
    public Transform selectPoint, unSelectPoint, backpackPoints;
    public BackpackInformation backpackInformation = new BackpackInformation();
    public class BackpackInformation
    {
        public int RifleBulletCount = 0;
        public int ShotgunBulletCount = 0;
        public int SniperBulletCount = 0;
        public int GrenadeBulletCount = 0;
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {

    }
    public void Init(CPlayer cp)
    {
        Debug.Log("5-7-1");
        cplayer = cp;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("UsingBackpack:");
    }
    /// <summary>
    /// 检查背包是否有剩余空间
    /// </summary>
    /// <param name="equipmentTypes"></param>
    /// <returns></returns>
    public bool IsFull(EquipmentTypes equipmentTypes)
    {
        switch (equipmentTypes)
        {
            case EquipmentTypes.Gun:
                return gunList.Count >= gunCapacity;
            case EquipmentTypes.Goods:
                return goodsList.Count >= goodsCapacity;
        }
        return true;
    }
    /// <summary>
    /// 添加物品
    /// </summary>
    /// <param name="equipmentTypes"></param>
    /// <param name="equipment"></param>
    /// <returns></returns>
    public bool Add(EquipmentTypes equipmentTypes, GameObject equipment)
    {
        switch (equipmentTypes)
        {
            case EquipmentTypes.Gun:
                if (gunList.Count >= gunCapacity)
                    return false;
                for (int i = 0; i < gunsPosition.Length; i++)
                {
                    if (gunsPosition[i] == -1)
                    {
                        gunList.Add(equipment);
                        gunsPosition[i] = equipment.GetComponent<Equipment>().id;
                        InstantiantionGoodsUI(equipmentTypes, equipment, i);

                        break;
                    }
                }
                break;
            case EquipmentTypes.Goods:
                if (goodsList.Count >= goodsCapacity)
                    return false;
                for (int i = 0; i < goodsPosition.Length; i++)
                {
                    if (goodsPosition[i] == -1)
                    {
                        goodsList.Add(equipment);
                        goodsPosition[i] = equipment.GetComponent<Equipment>().id;
                        InstantiantionGoodsUI(equipmentTypes, equipment, i);

                        //Goods g = equipment.GetComponent<Goods>();
                        //UpdateBackpackGoodsInformation(g.goodsTypes, g.count);

                        //更新枪械屏幕信息
                        UpdateAllGunsScreen();

                        break;
                    }
                }
                break;
        }
        return true;
    }
    /// <summary>
    /// 删除物品
    /// </summary>
    /// <param name="equipmentTypes"></param>
    /// <param name="equipment"></param>
    /// <returns></returns>
    public bool Delete(EquipmentTypes equipmentTypes, GameObject equipment)
    {
        switch (equipmentTypes)
        {
            case EquipmentTypes.Gun:
                gunList.Remove(equipment);
                for (int i = 0; i < gunsPosition.Length; i++)
                {
                    if (gunsPosition[i] == equipment.GetComponent<Equipment>().id)
                    {
                        gunsPosition[i] = -1;
                        gunUIList.Remove(equipment.GetComponent<Equipment>().GoodsUI);
                        gunList.Remove(equipment);
                        Destroy(equipment.GetComponent<Equipment>().GoodsUI);
                        break;
                    }
                }
                for (int i = 0; i < gunList.Count; i++)
                {
                    cplayer.playerBase.Guns[i] = gunList[i];
                }
                break;
            case EquipmentTypes.Goods:
                goodsList.Remove(equipment);
                Goods goods = equipment.GetComponent<Goods>();
                for (int i = 0; i < goodsPosition.Length; i++)
                {
                    if (goodsPosition[i] == goods.id)
                    {

                        goodsPosition[i] = -1;
                        goodsUIList.Remove(goods.GoodsUI);
                        goodsList.Remove(equipment);
                        Destroy(goods.GoodsUI);
                        break;
                    }
                }
                break;
        }
        return true;
    }
    /// <summary>
    /// 更新物品的位置信息（拖拽物品）
    /// </summary>
    /// <param name="equipmentTypes"></param>
    /// <param name="equipment"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public bool Updata(EquipmentTypes equipmentTypes, GameObject equipment, int from, int to)
    {
        switch (equipmentTypes)
        {
            case EquipmentTypes.Gun:
                if (gunsPosition[to] == -1)
                {
                    gunsPosition[to] = equipment.GetComponent<Equipment>().id;
                    gunsPosition[from] = -1;
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case EquipmentTypes.Goods:
                if (goodsPosition[to] == -1)
                {
                    goodsPosition[to] = equipment.GetComponent<Equipment>().id;
                    goodsPosition[from] = -1;
                    return true;
                }
                else
                {
                    return false;
                }
                break;
        }
        return true;
    }
    /// <summary>
    /// 合并物体
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public bool CombineGoods(int from, int to)
    {
        UIGoods ugf = FindGoodsUIByPosition(from);
        UIGoods ugt = FindGoodsUIByPosition(to);

        if (ugf == null || ugt == null || ugf.id == ugt.id)
            return false;

        Goods gf = ugf.equipment.GetComponent<Goods>();
        Goods gt = ugt.equipment.GetComponent<Goods>();
        if (gf.goodsTypes == gt.goodsTypes)
        {
            if ((gf.count + gt.count) <= gt.countMax)
            {

                gt.count += gf.count;
                Delete(gf.equipmentTypes, ugf.equipment);
            }
            else
            {
                gf.count -= (gt.countMax - gt.count);
                gt.count = gt.countMax;
            }
            Debug.Log(goodsUIList.Count);
            ugf.count = gf.count;
            ugt.count = gt.count;
            ugf.UpdateCountUI();
            ugt.UpdateCountUI();
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 检查背包中某个位置是否为空
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool FindPositionIsNull(int index)
    {
        return goodsPosition[index] == -1;
    }
    /// <summary>
    /// 获取背包资源信息,统计各个物品数量
    /// </summary>
    /// <returns></returns>
    public BackpackInformation GetBackpackGoodsInformation()
    {
        backpackInformation = new BackpackInformation();
        foreach (GameObject equipment in goodsList)
        {
            if (equipment.GetComponent<Goods>() != null)
            {
                Goods g = equipment.GetComponent<Goods>();
                if (g.goodsTypes == GoodsTypes.RifleBullet)
                {
                    backpackInformation.RifleBulletCount += g.count;
                }
                if (g.goodsTypes == GoodsTypes.ShotgunBullet)
                {
                    backpackInformation.ShotgunBulletCount += g.count;
                }
                if (g.goodsTypes == GoodsTypes.SniperBullet)
                {
                    backpackInformation.SniperBulletCount += g.count;
                }
                if (g.goodsTypes == GoodsTypes.GrenadeBullet)
                {
                    backpackInformation.GrenadeBulletCount += g.count;
                }
            }
        }
        return backpackInformation;
    }
    /// <summary>
    /// 设置背包资源，通过+delta/-delta 来控制某种物品数量，会适当删除部分多余的物品空盒
    /// </summary>
    /// <param name="goodsTypes"></param>
    /// <param name="delta"></param>
    public bool UpdateBackpackGoodsInformation(GoodsTypes goodsTypes, int delta)
    {

        if (backpackInformation.RifleBulletCount <= 0)
            return false;

        for (int i = 0; i < goodsUIList.Count; i++)
        {
            UIGoods ug = goodsUIList[i].GetComponent<UIGoods>();
            Goods g = ug.equipment.GetComponent<Goods>();
            if (ug != null && g.goodsTypes == goodsTypes)
            {
                UpdateAllGunsScreen();
                ug.count += delta;
                g.count = ug.count;
                ug.UpdateCountUI();
                if (ug.count > 0)
                {
                    return true;
                }
                if (ug.count == 0)
                {
                    Delete(ug.equipmentTypes, ug.equipment);
                    return true;
                }
                if (ug.count < 0)
                {
                    Delete(ug.equipmentTypes, ug.equipment);
                    delta = ug.count;
                    continue;
                }
            }

        }
        return false;

    }
    /// <summary>
    /// 生成物品UI
    /// </summary>
    /// <param name="equipmentTypes"></param>
    /// <param name="equipment"></param>
    /// <param name="p"></param>
    void InstantiantionGoodsUI(EquipmentTypes equipmentTypes, GameObject equipment, int p)
    {
        Transform tmp = backpackPoints.Find(equipmentTypes.ToString() + "Point:" + p);
        GameObject Item =Resources.Load<GameObject>("Prefabs/UI/Backpack/" + equipmentTypes.ToString() + "UI/"+equipment.gameObject.name);
        Debug.Log("Prefabs / UI / Backpack / " + equipmentTypes.ToString() + "UI / "+equipment.gameObject.name);
        if (Item == null)
        {
            Item = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Backpack/" + equipmentTypes.ToString() + "UI/"+equipmentTypes.ToString() + "UI"));
        }
        Item= Instantiate(Item);
        Item.transform.Find("Button").Find("Text").GetComponent<Text>().text = equipment.name;
        Debug.Log("tmp=" + tmp.localPosition);
        Item.transform.SetParent(unSelectPoint);
        Item.transform.localPosition = tmp.localPosition;
        Item.transform.localRotation = tmp.localRotation;
        equipment.GetComponent<Equipment>().GoodsUI = Item;
        Item.GetComponent<UIGoods>().Init(equipment.GetComponent<Equipment>().id, this, equipment, selectPoint, unSelectPoint, tmp);
        switch (equipmentTypes)
        {
            case EquipmentTypes.Gun:
                gunUIList.Add(Item);
                break;
            case EquipmentTypes.Goods:
                goodsUIList.Add(Item);
                break;
        }
        Debug.Log("5-7-10");
    }
    public void Use(int id)
    {
        Equipment e = Equipment.FindEquipmentById(id);
        if (e != null)
        {
            if (e.GetComponent<Goods>() != null)
            {
                if (e.GetComponent<Goods>().goodsTypes == GoodsTypes.MedicalBox)
                {
                    Delete(e.equipmentTypes, e.gameObject);
                    cplayer.playerBase.Treatment();
                }
                else
                {
                    return;
                }
            }
            Delete(e.equipmentTypes, e.gameObject);
        }
    }
    public void Discard(int id)
    {
        Equipment e = Equipment.FindEquipmentById(id);
        if (e != null)
        {
            Delete(e.equipmentTypes, e.gameObject);
        }
        cplayer.Discard(id);
    }
    /// <summary>
    /// 关闭所有物品UI的二级面板
    /// </summary>
    public void CloseAllItemPanel()
    {
        foreach (GameObject item in gunUIList)
        {
            if (item == null)
                continue;
            if (item.GetComponent<UIGoods>().isOpenPanel)
            {
                item.GetComponent<UIGoods>().ClosePanel();
            }
        }
        foreach (GameObject item in goodsUIList)
        {
            if (item == null)
                continue;
            if (item.GetComponent<UIGoods>().isOpenPanel)
            {
                item.GetComponent<UIGoods>().ClosePanel();
            }
        }
    }
    /// <summary>
    /// 通过背包中的位置获取物体
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public UIGoods FindGoodsUIByPosition(int p)
    {
        foreach (GameObject g in goodsUIList)
        {
            UIGoods ui = g.GetComponent<UIGoods>();
            if (ui.positionInBackpack == p)
            {
                return ui;
            }
        }
        return null;
    }
    public UIGoods FindGoodsUIById(int id)
    {
        foreach (GameObject g in goodsUIList)
        {
            UIGoods ui = g.GetComponent<UIGoods>();
            if (ui.id == id)
            {
                return ui;
            }
        }
        return null;
    }
    /// <summary>
    /// 跟新所有枪支的子弹ui显示数目
    /// </summary>
    public void UpdateAllGunsScreen()
    {
        foreach (GameObject g in gunList)
        {
            if (g == null)
            {
                continue;
            }
            Gun gun = g.GetComponent<Gun>();
            Debug.Log("Testlog=" + GetBackpackGoodsInformation().ShotgunBulletCount);
            switch (gun.guntype)
            {
                case GunTypes.Rifle:
                    gun.Load(cplayer.playerBase, GetBackpackGoodsInformation().RifleBulletCount);
                    break;
                case GunTypes.Shotgun:
                    gun.Load(cplayer.playerBase, GetBackpackGoodsInformation().ShotgunBulletCount);
                    break;
                case GunTypes.Sniper:
                    gun.Load(cplayer.playerBase, GetBackpackGoodsInformation().SniperBulletCount);
                    break;
                case GunTypes.Grenade:
                    gun.Load(cplayer.playerBase, GetBackpackGoodsInformation().GrenadeBulletCount);
                    break;
            }
        }
    }
}
