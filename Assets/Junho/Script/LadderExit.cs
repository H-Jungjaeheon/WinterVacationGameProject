using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderExit : MonoBehaviour
{
    public Collider2D col;
    public GameObject Interaction;
    public Sprite up;
    public bool isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        isPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer&&Input.GetKey(KeyCode.F))
        {
            GameManager.Instance.is2F = true;
            Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<Collider2D>(), col, true);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayer=true;
            Interaction.SetActive(true);

        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        if (Input.GetKey(KeyCode.F))
    //        {
    //            GameManager.Instance.is2F = true;
    //            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), col, true);
    //        }

    //    }
    //}
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("d");
        Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), col, false);
        
        if (collision.CompareTag("Player"))
        {
            Interaction.SetActive(false);

            isPlayer = false;
            GameManager.Instance.is2F = false;
        }



    }
}
