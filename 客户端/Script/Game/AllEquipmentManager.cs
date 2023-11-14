using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEquipmentManager : MonoBehaviour
{
    public static List<Equipment> AllEquipments = new List<Equipment>();
    public static int staticId = 0;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Init()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Equipment e = transform.GetChild(i).GetComponent<Equipment>();
            if (e != null)
            {
                e.id = staticId++;
                AllEquipments.Add(e);
            }
        }
    }
    public static int EquipmentInit(Equipment e)
    {
        int id = staticId++;
        e.id = id;
        AllEquipments.Add(e);
        return id;
    }
}
