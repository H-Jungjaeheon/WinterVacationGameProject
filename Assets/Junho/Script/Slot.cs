using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    PotionInventory inventory;
    public int num;

    private void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<PotionInventory>();
        num = int.Parse(gameObject.name.Substring(gameObject.name.IndexOf("_") + 1));
    }
    private void Update()
    {
        if (transform.childCount <=0)
        {
            inventory.slots[num].isEmpty = true;
        }    
    }
}
