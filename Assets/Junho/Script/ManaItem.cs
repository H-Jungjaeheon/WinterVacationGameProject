using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaItem : MonoBehaviour
{
    void Update()
    {
        if (Input.inputString == (transform.parent.GetComponent<Slot>().num + 1).ToString())
        {
            Debug.Log("Mana UP , slotNumbe : " + (transform.parent.GetComponent<Slot>().num + 1));
            GameManager.Instance.curMana += 10; 
            Destroy(this.gameObject);
        }
    }
}
