using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waringroom : MonoBehaviour
{
    private void Update()
    {
         if(transform.childCount == 1)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
        for(int i = 0;i<3;i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }

        }
    }
}
