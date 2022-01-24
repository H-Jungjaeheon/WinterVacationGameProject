using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Collider2D col;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), col, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), col, false);

        }
    }
}
