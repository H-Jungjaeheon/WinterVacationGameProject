using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class Inventorycontroller : MonoBehaviour
{
    private bool InventoryOn= false;
    public GameObject inventorys;
    private int itemidx=0;
    private GameObject itembutton;
    private int sibaltest = 0;
    public List<GameObject> items = new List<GameObject>();
    private GameObject buttonsave;
    [SerializeField] public GameObject startPos, endPos;
    private void Start()
    {
        inventorys.transform.position = startPos.transform.position;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInventory();
        }
        settingitem();
    }
    public void OnInventory()
    {
        if (InventoryOn == false)
        {
            InventoryOn = true;
            inventorys.transform.DOMove(endPos.transform.position, 1f).SetEase(Ease.OutQuad);
        }
        else if (InventoryOn == true)
        {
            InventoryOn = false;
            inventorys.transform.DOMove(startPos.transform.position, 1f).SetEase(Ease.OutQuad);
        }
    }
    public void Additem(GameObject item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject testsave = items[i];
            if (item.tag == testsave.tag)
            {
                itembutton = inventorys.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject;
                itembutton.GetComponent<ItemButtonScript>().idx++;
                itembutton.GetComponent<ItemButtonScript>().idxset();
                return;
            }
        }
        items.Add(item);
        itembutton = inventorys.transform.GetChild(0).gameObject.transform.GetChild(items.Count -1).gameObject;
        itembutton.GetComponent<ItemButtonScript>().idx++;
        settingitem();
    }
    public void settingitem()
    {
        GameObject buttons = inventorys.transform.GetChild(0).gameObject;
        for (int i = 0; i <6; i++)
        {
            itembutton = buttons.transform.GetChild(i).gameObject;
            if (itembutton.GetComponent<ItemButtonScript>().idx == 0 && i < items.Count)
            {
                itembutton.GetComponent<ItemButtonScript>().idx = buttons.transform.GetChild(i + 1).gameObject.GetComponent<ItemButtonScript>().idx;
                items.RemoveAt(i);
            }
            itembutton.transform.GetChild(0).GetComponent<Image>().sprite = null;
            itembutton.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            itembutton.GetComponent<ItemButtonScript>().idxset();
        }
        for(int i= 5;i>=items.Count;i--)
        {
            inventorys.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<ItemButtonScript>().idx = 0;
        }
        for (int i = 0; i < items.Count; i++)
        {
            itembutton = buttons.transform.GetChild(i).gameObject;
            itembutton.GetComponent<ItemButtonScript>().ImageUpdate(items[i]);
        }
    }
    public void usebutton()
    {
        buttonsave = EventSystem.current.currentSelectedGameObject.gameObject;
        buttonsave.GetComponent<ItemButtonScript>().idx--;
        GameObject.Find("GameManager").GetComponent<GameManager>().useitem(buttonsave.GetComponent<ItemButtonScript>().itemsave);
    }
}
