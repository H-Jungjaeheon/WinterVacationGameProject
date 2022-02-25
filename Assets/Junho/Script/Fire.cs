using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private bool isPlayer = false;
    private bool getOut = false;
    private float cnt;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer)
        {
            GameManager.Instance.isBurns = true;
        }
        if (getOut)
        {
            cnt += Time.deltaTime;
            if (cnt>=5f)
            {
                GameManager.Instance.isBurns = false;
                cnt = 0;
                getOut = false;
            }
        }
    }
    IEnumerator IsFire()
    {
        GameManager.Instance.stackDamage += 3;
        yield return new WaitForSeconds(1f);
        if (isPlayer)
        {
            StartCoroutine(IsFire());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&GameManager.Instance.isTrapBarrier==false)
        {
            isPlayer = true;
            GameManager.Instance.isBurns = true;
            if (isPlayer)
            {
                StartCoroutine(IsFire());
                StartCoroutine(IsBurns());
            }
        } 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.Instance.isTrapBarrier == false)
        {
            cnt = 0; 
            isPlayer = false;
            getOut = true;
        }
    }
    IEnumerator IsBurns()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.stackDamage += 0.5f;
        if (GameManager.Instance.isBurns)
        {
            StartCoroutine(IsBurns());
        }
    }
}
