using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopScript : MonoBehaviour
{
    [SerializeField] bool statsshop;
    public Text text;
    public GameObject[] shopitem;
    private GameObject item;
    private int shopmoney;

    private void Start()
    {
        int random;
        text.gameObject.SetActive(false);
        if(statsshop == false)
        {
            random = Random.Range(0, 6);
            item = Instantiate(shopitem[random], transform.position + new Vector3(0, 1f, 0), transform.rotation);
            shopmoney = item.gameObject.GetComponent<testItem>().shopmoney;
            item.gameObject.GetComponent<testItem>().shop = true;
        }
        else
        {
            random = Random.Range(0,7);
            item = Instantiate(shopitem[random], transform.position + new Vector3(0, 1f, 0), transform.rotation);
            shopmoney = item.gameObject.GetComponent<StatsUpitem>().shopmoney;
            item.gameObject.GetComponent<StatsUpitem>().shop = true;
        }
        text.text = "±¸¸Å " + shopmoney +"¿ø (F)";
    }

    void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3f, 0));

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.transform.parent.GetComponent<textsetactive>().on = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.transform.parent.GetComponent<textsetactive>().on =false;
        }
    }

}
