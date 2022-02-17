using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waringroom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 0;i<3;i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
