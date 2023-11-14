using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    public Text scoreUI;
    public int score = 0;
    public GameObject add;
    public List<GameObject> addList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i=0;i <addList.Count; i++)
        {
            if (Vector3.Distance(addList[i].transform.position, scoreUI.transform.position) < 0.01f)
            {
                int.TryParse(addList[i].name, out int value);
                score += value;
                scoreUI.text = score.ToString();                
                Destroy(addList[i]);
                addList.Remove(addList[i]);
            }
            else
            {
                addList[i].transform.position = Vector3.Lerp(addList[i].transform.position, scoreUI.transform.position, 0.5f);
            }
        }
    }
    public void Add(int value,Vector3 p)
    {
        p= Camera.main.WorldToScreenPoint(p);
        GameObject g= Instantiate(add, p, Quaternion.identity);
        g.name = value.ToString();
        g.GetComponent<Text>().text = value.ToString();
        g.transform.SetParent(transform);
        addList.Add(g);
    }
    public bool TryUse(int value)
    {
        if (score < value)
            return false;

        score -= value;
        return true;
    }
}
