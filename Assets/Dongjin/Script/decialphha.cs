using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decialphha : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
