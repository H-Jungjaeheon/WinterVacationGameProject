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
    public GameObject particle;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        particle.SetActive(true);
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
            Debug.Log("�浹");
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
            Debug.Log("����");
            Interaction.gameObject.SetActive(false);
        }
    }
    void isParticle()
    {
        if (isIt==true)
        {
            particle.SetActive(true);
        }else particle.SetActive(false);
    }
    public void Drop()
    {
        int ran = Random.Range(0, 10);
        if (ran < 8)
        {
            Debug.Log("��");
            
        }

        else if (ran < 9)
        {
            Instantiate(Items[1], transform.position, Items[1].transform.rotation);
        }
        else if (ran < 10)
        {
            Instantiate(Items[1], transform.position, Items[1].transform.rotation);
        }
    }

}
