using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObj : MonoBehaviour
{
    bool isCol;
    public GameObject HideText;
    // Start is called before the first frame update
    private void Awake()
    {
        HideText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HideText.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2f, 0));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCol = true;
            HideText.SetActive(true);
            if (GameManager.Instance.IsBattleStart == false)
            {
                if (GameObject.Find("Player").GetComponent<Player>().isHide == true && Input.GetKey(KeyCode.F) && isCol)
                {
                    GameObject.Find("Player").transform.position = transform.position;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCol=false;
            HideText.SetActive(false);
        }
    }
}
