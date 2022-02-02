using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testinventory : MonoBehaviour
{
    public GameObject managertest;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            managertest.GetComponent<Inventorycontroller>().Additem(this.gameObject);
        }
    }
}
