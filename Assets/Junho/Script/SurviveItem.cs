using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurviveItem : MonoBehaviour
{
    void Update()
    {
        if (Input.inputString == (transform.parent.GetComponent<Slot>().num + 1).ToString())
        {
            Debug.Log("Survive UP , slotNumbe : " + (transform.parent.GetComponent<Slot>().num + 1));
            GameManager.Instance.curSurvive += 10; 
            Destroy(this.gameObject);
        }
    }
}
