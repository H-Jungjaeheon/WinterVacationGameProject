using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class trapbutton : MonoBehaviour
{
    public Text text;
    private bool buttonon = false;
    private void Start()
    {
        text.gameObject.SetActive(false);
        text.text = "¹® ¿­±â(F)";
    }

    private void Update()
    {

        if (buttonon == true && Input.GetKeyDown(KeyCode.F))
        {
            transform.GetChild(1).gameObject.transform.DOMoveY(transform.GetChild(1).position.y + 10, 1f).SetEase(Ease.Linear);
            StartCoroutine(Cnt());
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.gameObject.SetActive(true);
            buttonon = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.gameObject.SetActive(false);
            buttonon = false;
        }
    }
    IEnumerator Cnt()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
