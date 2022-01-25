using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurviveGas : MonoBehaviour
{
    public bool isDamage = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SurviveDamage();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¥Í¿Ω");
            isDamage = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("æ»¥Í¿Ω");
            isDamage = true;
        }
    }
    void SurviveDamage()
    {
        if (isDamage == true)
        {
            GameManager.Instance.curSurvive += 10;

        }
        
    }
}
