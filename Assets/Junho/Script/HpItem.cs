using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : MonoBehaviour
{
    void Update()
    {
        if (Input.inputString == (transform.parent.GetComponent<Slot>().num+1).ToString())
        {
            Debug.Log("Hp UP , slotNumbe : "+ (transform.parent.GetComponent<Slot>().num + 1));
            Player.P_Instance.curHp += 10;

            Destroy(this.gameObject);
        }
    }
}
