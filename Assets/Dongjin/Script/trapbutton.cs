using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapbutton : MonoBehaviour
{
    private GameObject text;
    private bool buttonon = false;
    private void Start()
    {
        text = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (buttonon == true && Input.GetKeyDown(KeyCode.F))
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.SetActive(true);
            buttonon = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.SetActive(false);
            buttonon = false;
        }
    }
}
