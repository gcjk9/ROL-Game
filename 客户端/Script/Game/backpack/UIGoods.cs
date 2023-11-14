using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGoods : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    public int count;
    public Text countUI;
    public EquipmentTypes equipmentTypes;
    public BackpackPanel backpackPanel;
    public int positionInBackpack;
    public GameObject equipment;
    public Transform selectedPoint;
    public Transform unSelectedPoint;
    public Transform targetPoint;
    public Transform panel;
    public Transform from, to;
    public bool isDrag = false;
    public bool isAdsorb = false;
    public bool isMouseAbove = false;
    public bool isOpenPanel = false;
    public bool isMovingPanel = false;
    // Start is called before the first frame update
    public void Init(int id, BackpackPanel backpackPanel, GameObject equipment, Transform selectedPoint, Transform unSelectedPoint, Transform targetPoint)
    {
        this.id = id;
        this.backpackPanel = backpackPanel;
        this.equipment = equipment;
        this.selectedPoint = selectedPoint;
        this.unSelectedPoint = unSelectedPoint;
        this.targetPoint = targetPoint;
        if (equipment.GetComponent<Equipment>().equipmentTypes == EquipmentTypes.Goods)
        {
            this.count = equipment.GetComponent<Goods>().count;
            equipmentTypes = EquipmentTypes.Goods;
        }
        else
        {
            this.count = 1;
            equipmentTypes = EquipmentTypes.Gun;
        }
        Debug.Log("5-7-8");
        string[] tmp = targetPoint.name.Split(':');
        int.TryParse(tmp[1], out positionInBackpack);
        Debug.Log("5-7-9");
        UpdateCountUI();
    }
    void Start()
    {

    }
    void Update()
    {
        if (backpackPanel.isShow)
        {
            if (equipmentTypes == EquipmentTypes.Gun)
            {
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                isDrag = true;
                isAdsorb = false;
            }
            if (Input.GetMouseButtonUp(1))
            {
                isDrag = false;
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMovingPanel)
        {
            if (isOpenPanel)
            {
                panel.position = Vector3.Lerp(panel.position, from.position, 0.5f);
                if (Vector3.Distance(panel.position, from.position) < 0.1f)
                {
                    isOpenPanel = false;
                    isMovingPanel = false;
                }
            }
            else
            {
                panel.position = Vector3.Lerp(panel.position, to.position, 0.5f);
                if (Vector3.Distance(panel.position, to.position) < 0.1f)
                {
                    isOpenPanel = true;
                    isMovingPanel = false;
                }
            }
        }
        if (isDrag && isMouseAbove)
        {
            transform.position = Input.mousePosition;
            transform.SetParent(selectedPoint);
        }
        else
        {
            if (!isAdsorb)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPoint.localPosition, 0.3f);
                if (Vector3.Distance(transform.localPosition, targetPoint.localPosition) < 0.01f)
                {
                    isAdsorb = true;
                    transform.SetParent(unSelectedPoint);

                    string[] tmp = targetPoint.name.Split(':');
                    int.TryParse(tmp[1], out int index);

                    backpackPanel.Updata(equipmentTypes, equipment, positionInBackpack, index);
                    positionInBackpack = index;
                }
            }
            else
            {

            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        string[] tmp = collision.gameObject.name.Split(':');
        if (tmp[0].Equals(equipmentTypes.ToString() + "Point"))
        {
            int.TryParse(tmp[1], out int id);
            if (backpackPanel.FindPositionIsNull(id))
            {
                targetPoint = collision.transform;
            }
            if (backpackPanel.CombineGoods(positionInBackpack, id))
            {
                Debug.Log("CombineSuccess");
            }
        }
    }
    public void MovePanel()
    {
        backpackPanel.CloseAllItemPanel();
        transform.SetParent(selectedPoint);
        isMovingPanel = true;
    }
    public void ClosePanel()
    {
        if (isOpenPanel)
        {
            isMovingPanel = true;
        }
    }
    public void UpdateCountUI()
    {
        countUI.text = count.ToString();
    }
    public void Use()
    {
        //isMovingPanel = true;
        backpackPanel.Use(id);
        Destroy(gameObject);
    }
    public void Discard()
    {
        backpackPanel.Discard(id);
        Destroy(gameObject);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseAbove = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseAbove = false;
    }
    public static UIGoods FindUIById(int i, List<GameObject> list)
    {
        foreach (GameObject g in list)
        {
            UIGoods u = g.GetComponent<UIGoods>();
            if (u != null)
            {
                if (u.id == i)
                {
                    return u;
                }
            }
        }
        return null;
    }
}
