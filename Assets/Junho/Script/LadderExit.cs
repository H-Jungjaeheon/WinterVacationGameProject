using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderExit : MonoBehaviour
{
    public Collider2D col;
    public GameObject Interaction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Interaction.SetActive(true);


        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.F))
            {

                Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), col, true);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {


        Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), col, false);
        if (collision.CompareTag("Player"))
        {
            Interaction.SetActive(false);


        }



    }
}
