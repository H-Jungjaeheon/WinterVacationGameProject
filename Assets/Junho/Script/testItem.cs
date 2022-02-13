using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testItem : MonoBehaviour
{
    //public GameObject slotItem;
    public GameObject Interaction;
    private GameObject managertest;
    public int itemidx;
    public Text text;
    bool isCol;
    // Start is called before the first framse update
    void Start()
    {
        text.gameObject.SetActive(false);
        text.text = gameObject.name + "ŉ�� (F)";
        managertest = GameObject.Find("GameManager");
        StartCoroutine(cnt());
    }
    IEnumerator cnt()
    {
        GetComponent<CapsuleCollider2D>().enabled=false;
        yield return new WaitForSeconds(3f);
        GetComponent<CapsuleCollider2D>().enabled = true;
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        
    }
    // Update is called once per frame
    void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.6f, 0));

        if (isCol&&Input.GetKey(KeyCode.F)&& GameObject.Find("Player").GetComponent<Player>().IsGrab == false)
        {
            managertest.GetComponent<Inventorycontroller>().Additem(this.gameObject);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);
            Interaction.SetActive(true);
            isCol = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
            Interaction.SetActive(false);
            isCol = false;
        }
    }
}
