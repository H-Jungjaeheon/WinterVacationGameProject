using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObj : MonoBehaviour
{
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
            HideText.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideText.SetActive(false);
        }
    }
}
