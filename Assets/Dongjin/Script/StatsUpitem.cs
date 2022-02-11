using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUpitem : MonoBehaviour
{
    public GameObject Interaction;
    private GameObject managertest;
    public int itemidx;
    void Start()
    {
        managertest = GameObject.Find("GameManager");
        StartCoroutine(cnt());
    }
    IEnumerator cnt()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(3f);
        GetComponent<CapsuleCollider2D>().enabled = true;
        GetComponent<CapsuleCollider2D>().isTrigger = true;

    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Interaction.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().useitem(itemidx);
                gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Interaction.SetActive(false);
        }
    }
}
