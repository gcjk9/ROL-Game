using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : Equipment
{
    public int count;
    public int countMax;
    public GoodsTypes goodsTypes;
    private void Start()
    {
        Init();
    }
}

