using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUpitem : MonoBehaviour
{
    public Text text;
    private GameObject managertest;
    public int itemidx;
    public int shopmoney;
    public bool shop;
    bool isCol;
    void Start()
    {
        text.gameObject.SetActive(false);
        text.text = gameObject.name + "Å‰µæ (F)";
        managertest = GameObject.Find("GameManager");
        StartCoroutine(cnt());
    }
    IEnumerator cnt()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().isTrigger = true;

    }
    // Update is called once per frame
    void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.6f, 0));
        if (isCol && Input.GetKey(KeyCode.F) && GameObject.Find("Player").GetComponent<Player>().IsGrab == false)
        {
            if (shop == false)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().useitem(itemidx);
                gameObject.SetActive(false);
            }
            else if (shopmoney <= GameManager.Instance.Money)
            {
                GameManager.Instance.Money -= shopmoney;
                GameObject.Find("GameManager").GetComponent<GameManager>().useitem(itemidx);
                gameObject.SetActive(false);
            }
        }
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(shop != true)
                text.gameObject.SetActive(true);
            isCol = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(shop !=true)
                text.gameObject.SetActive(false);
            isCol = false;
        }
    }
}
