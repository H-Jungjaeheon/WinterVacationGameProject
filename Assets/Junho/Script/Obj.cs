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
    void isParticle()
    {
        if (isIt==true)
        {
            particle.SetActive(true);
        }else particle.SetActive(false);
    }
    

}
