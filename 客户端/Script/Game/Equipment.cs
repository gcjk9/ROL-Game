using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{   
    public int id=0;
    public EquipmentTypes equipmentTypes;
    public GameObject GoodsUI;
    public bool isBePacked = false;
    // Start is called before the first frame update
    public void Init()
    {

    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public static Equipment FindEquipmentById(int id)
    {
        foreach(Equipment e in AllEquipmentManager.AllEquipments)
        {
            if (e == null || e.gameObject == null)
                continue;
            if (e.id == id)
            {
                return e;
            }
        }
        return null;
    }
}
public enum EquipmentTypes
{
    Gun,
    Goods,
}
public enum GoodsTypes
{
    RifleBullet=0,
    ShotgunBullet=1,
    SniperBullet=2,
    GrenadeBullet=3,
    MedicalBox=4
}
public enum GoodsChineseName
{
    步枪子弹 = 0,
    霰弹枪子弹 = 1,
    狙击枪子弹 = 2,
    榴弹 = 3,
    医疗箱 = 4
}
