using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressUpData : MonoBehaviour
{
    public int id;
    public string head;
    public string face;
    public string body;

    public DressUpData()
    {
        head = "";
        face = "";
        body = "";
    }
    public DressUpData(string data)
    {
        JObject JData = JObject.Parse(data);
        int.TryParse(JData.GetValue("id").ToString(), out id);
        head = JData.GetValue("head").ToString();
        face = JData.GetValue("face").ToString();
        body = JData.GetValue("body").ToString();
    }
}
