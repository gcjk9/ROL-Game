using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBloodBar : MonoBehaviour
{
    public Transform blood;
    public Transform toBloodPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        blood.transform.localPosition = Vector3.Lerp(blood.transform.localPosition, toBloodPoint.localPosition, 0.1f);
    }
    public void SetBloodBar(float value)
    {
        float x = -320f + 320f * (value / 100f);
        toBloodPoint.localPosition = new Vector3(x,toBloodPoint.localPosition.y, toBloodPoint.localPosition.z);
    }
}
