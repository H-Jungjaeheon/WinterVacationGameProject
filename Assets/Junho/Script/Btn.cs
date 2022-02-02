using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour
{
    bool isCollider;
    public bool isOn;
    private void Awake()
    {
        isOn = false;
    }
    
    private void Update()
    {
        if (isCollider==true&&Input.GetKey(KeyCode.F))
        {
            isOn = true;
        }
        if (isOn)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else GetComponent<SpriteRenderer>().color = Color.red;


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollider = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollider = false;
        }
        
    }
    
}
