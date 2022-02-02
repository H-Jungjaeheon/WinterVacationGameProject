using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obj_2 : MonoBehaviour
{
    public GameObject Interaction;
    public GameObject[] Items;
    public bool isIt;
    bool iscollison;
    public Slider slider;

    public Sprite Open;

    [SerializeField]
    public GameObject[] particle;
    // Start is called before the first frame update
    void Start()
    {
        iscollison = false;
        isIt = true;
        particle[0].SetActive(false);
        particle[1].SetActive(false);
        

    }

    // Update is called once per frame
    float cnt;
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.6f, 0));
        isParticle();
        slider.value = cnt / 4;
        if (isIt && iscollison)
        {
            slider.gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                cnt += Time.deltaTime;
                Debug.Log(cnt);
            }
            else cnt = 0;
        }
        else
        {
            slider.gameObject.SetActive(false);
            cnt = 0;
        }
        if (cnt >= 4) 
        { 
           cnt = 0;
           Drop();
        }    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�浹");
            if (isIt == true)
            {
                iscollison = true;
                Interaction.SetActive(true);

            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("����");
            iscollison = false;
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
        isIt = false;
        GetComponent<SpriteRenderer>().sprite = Open;
        Interaction.SetActive(false);
        int ran = Random.Range(0, 10);
        if (ran < 7)
        {
            Debug.Log("��");

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
        gameObject.tag = "Untagged";
    }
    void Nothing()
    {
        particle[1].SetActive(false);

    }
}
