using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Obj : MonoBehaviour
{
    public static Obj Instance { get; private set; }
    public GameObject Interaction;
    public GameObject[] Items;
    public bool isIt = true;

    [SerializeField]
    public GameObject[] particle;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        particle[0].SetActive(false);
        particle[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        isParticle();

        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Ãæµ¹");
            if (isIt == true)
            {
                Interaction.SetActive(true);

            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("³ª°¨");
            Interaction.gameObject.SetActive(false);
        }
    }
    void isParticle()
    {
        if (isIt == true)
        {
            particle[0].SetActive(true);
        }
        else
        {
            particle[0].SetActive(false);

        }
    }
    public void Drop()
    {
        int ran = Random.Range(0, 10);
        if (ran < 7)
        {
            Debug.Log("²Î");

            //Instantiate(Items[2], transform.position, Items[1].transform.rotation);

            particle[1].SetActive(true);
            Invoke("Nothing", 4.5f);

        }
        else if (ran < 8)
        {
            Instantiate(Items[2], transform.position, Items[2].transform.rotation);
        }

        else if (ran < 9)
        {
            Instantiate(Items[0], transform.position, Items[1].transform.rotation);
        }
        else if (ran < 10)
        {
            Instantiate(Items[1], transform.position, Items[1].transform.rotation);
        }
    }
    void Nothing()
    {
        particle[1].SetActive(false);

    }
}
