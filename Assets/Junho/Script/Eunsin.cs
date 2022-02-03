using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eunsin : MonoBehaviour
{
    bool iscollision = false;
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            iscollision = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            iscollision = false;
        }
    }
    void Update()
    {
        if (iscollision == true && Input.GetKey(KeyCode.F))
        {
            Destroy(this.gameObject);            
        }
    }
}
