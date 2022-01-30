using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapscript : MonoBehaviour
{
    private GameObject trapspear;
    private void Start()
    {
        trapspear = transform.GetChild(0).gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player"&&trapspear.GetComponent<Spear>().spearon == false)
        {
            trapspear.GetComponent<Spear>().StartCoroutine("spearmove");
        }
    }
}
