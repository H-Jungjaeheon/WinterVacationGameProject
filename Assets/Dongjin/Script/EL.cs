using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EL : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.stackDamage += 10;
        }
    }
}
