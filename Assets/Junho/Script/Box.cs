using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Box : MonoBehaviour
{
    protected bool isBoxOpen = false;
    protected bool iscollison = false;
    [SerializeField]
    protected GameObject[] particle;

    protected virtual  void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            iscollison = false;
            Debug.Log("³ª°¨");
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            iscollison = true;
            Debug.Log("´êÀ½");
        }
    }
    protected virtual void isParticle()
    {
        if(isBoxOpen == false)
        {
            particle[0].SetActive(true);
        }
        else
        {
            particle[0].SetActive(false);
        }
    }
    protected virtual void Nothing()
    {
        particle[1].SetActive(false);
    }

}
