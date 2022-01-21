using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameObject text;
    private bool opendoor = false;
    private void Start()
    {
        text = transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        if (opendoor == true && Input.GetKeyDown(KeyCode.F))
        {
                Debug.Log("¾À ³Ñ¾î°¨¿ä");
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            text.SetActive(true);
            opendoor = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            text.SetActive(false);
            opendoor = false;
        }
    }
}
