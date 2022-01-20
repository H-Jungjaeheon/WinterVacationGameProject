using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : MonoBehaviour
{
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
            Debug.Log("충돌");
            Interaction.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("나감");
            Interaction.gameObject.SetActive(false);
        }
    }
    

}
