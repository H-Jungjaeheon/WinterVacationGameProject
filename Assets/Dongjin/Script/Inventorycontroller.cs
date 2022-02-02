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
    }
    public void Additem(GameObject item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject testsave = items[i];
            if (item == testsave)
            {
                inventorys.transform.GetChild(i).gameObject.GetComponent<ItemButtonScript>().idxup();
                return;
            }
        }
        items.Add(item);
        itembutton = inventorys.transform.GetChild(itemidx).gameObject;
        itemidx++;
    }
}
