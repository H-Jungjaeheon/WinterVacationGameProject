using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    bool isCollision;
    void Update()
    {
        if (GameManager.Instance.isGetKey == true && Input.GetKey(KeyCode.F))
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.2f, 1f);
            transform.parent.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.1f, 0f, 1f);
            transform.parent.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            GameManager.Instance.isGetKey = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollision = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollision = false;
        }
    }
}
