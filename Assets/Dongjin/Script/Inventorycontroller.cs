using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventorycontroller : MonoBehaviour
{
    private bool InventoryOn= false;
    public GameObject inventorys;
    private int itemidx=0;
    private GameObject itembutton;
    private int sibaltest = 0;
    public List<GameObject> items = new List<GameObject>();

    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&InventoryOn == false)
        {
            InventoryOn = true;
            inventorys.gameObject.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.E) && InventoryOn == true)
        {
            InventoryOn = false;
            inventorys.gameObject.SetActive(false);
        }
        settingitem();
    }
    public void Additem(GameObject item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject testsave = items[i];
            if (item == testsave)
            {
                itembutton = inventorys.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject;
                itembutton.GetComponent<ItemButtonScript>().idx++;
                itembutton.GetComponent<ItemButtonScript>().idxup();
                return;
            }
        }
        items.Add(item);
        itembutton = inventorys.transform.GetChild(0).gameObject.transform.GetChild(items.Count -1).gameObject;
        itembutton.GetComponent<ItemButtonScript>().idx++;
        settingitem();
        /*itembutton = inventorys.transform.GetChild(0).gameObject.transform.GetChild(itemidx).gameObject;
        itembutton.GetComponent<ItemButtonScript>().ImageUpdate(item.GetComponent<SpriteRenderer>());
        itembutton.GetComponent<ItemButtonScript>().idxup();*/
    }
    public void settingitem()
    {
        for (int i = 0; i <6; i++)
        {
            itembutton = inventorys.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject;
            if (itembutton.GetComponent<ItemButtonScript>().idx == 0 && i < items.Count)
            {
                itembutton.GetComponent<ItemButtonScript>().idx = inventorys.transform.GetChild(0).gameObject.transform.GetChild(i + 1).gameObject.GetComponent<ItemButtonScript>().idx;
                items.RemoveAt(i);
            }
            itembutton.transform.GetChild(0).GetComponent<Image>().sprite = null;
            itembutton.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            itembutton.GetComponent<ItemButtonScript>().idxup();
        }
        for(int i= 5;i>=items.Count;i--)
        {
            inventorys.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<ItemButtonScript>().idx = 0;
        }
        for (int i = 0; i < items.Count; i++)
        {
            itembutton = inventorys.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject;
            itembutton.GetComponent<ItemButtonScript>().ImageUpdate(items[i].GetComponent<SpriteRenderer>());
        }
    }
}
