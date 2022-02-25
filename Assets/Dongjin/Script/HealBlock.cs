using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealBlock : MonoBehaviour
{
    [SerializeField]private Text text;
    private bool isCol, use;
    
    private void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.6f, 0));
        text.text = "메디컬머신 사용 (F)";
        text.gameObject.SetActive(false);
        if (isCol == true && Input.GetKey(KeyCode.F)&&use == false)
        {
            GameManager.Instance.stackDamage -= 50;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.4f, 0.4f, 1f);
            use = true;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);
            isCol = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
            isCol = false;
        }
    }
}
