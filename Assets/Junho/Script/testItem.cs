using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testItem : MonoBehaviour
{
    //public GameObject slotItem;
    private GameObject managertest;
    public int itemidx;
    public Text text;
    public int shopmoney;
    public bool shop;
    bool isCol;
    // Start is called before the first framse update
    void Start()
    {
        text.gameObject.SetActive(false);
        switch (itemidx)
        {
            case 0:
                text.text = "HpÆ÷¼Ç Å‰µæ (F)";
                break;
            case 1:
                text.text = "MpÆ÷¼Ç Å‰µæ (F)";
                break;
            case 2:
                text.text = "SpeedÆ÷¼Ç Å‰µæ (F)";
                break;
            case 3:
                text.text = "ManaBarrierÆ÷¼Ç Å‰µæ (F)";
                break;
            case 4:
                text.text = "HideÆ÷¼Ç Å‰µæ (F)";
                break;
            case 5:
                text.text = "TrapBarrierÆ÷¼Ç Å‰µæ (F)";
                break;
        }
        managertest = GameObject.Find("GameManager");
        StartCoroutine(cnt());
    }
    IEnumerator cnt()
    {
        GetComponent<CapsuleCollider2D>().enabled=false;
        yield return new WaitForSeconds(1.5f);
        GetComponent<CapsuleCollider2D>().enabled = true;
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        
    }
    // Update is called once per frame
    void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.6f, 0));

        if (isCol&&Input.GetKey(KeyCode.F)&& GameObject.Find("Player").GetComponent<Player>().IsGrab == false)
        {
            if(shop == false)
            {
                managertest.GetComponent<Inventorycontroller>().Additem(this.gameObject);
                gameObject.SetActive(false);
            }
            else if(shopmoney <= GameManager.Instance.Money)
            {
                GameManager.Instance.Money -= shopmoney;
                managertest.GetComponent<Inventorycontroller>().Additem(this.gameObject);
                gameObject.SetActive(false);
            }
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (shop != true)
            {
                text.transform.parent.GetComponent<textsetactive>().on = true;
            }
                isCol = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(shop != true)
            {
                text.transform.parent.GetComponent<textsetactive>().on = false;
            }
               isCol = false;
        }
    }
}
