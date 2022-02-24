using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Spear : MonoBehaviour
{
    private Vector3 savevector;
    public bool spearon;
    private bool trapped;
    public bool damageOn = false;
    [SerializeField] Sprite[] images;
    private void Update()
    {
        if(trapped == true && damageOn== true && GameManager.Instance.isTrapBarrier == false)
        {
            GameManager.Instance.stackDamage += 10;
            damageOn = false;
        }    
    }
    IEnumerator spearmove()
    {
        spearon = true;
        savevector = this.transform.position;
        gameObject.GetComponent<SpriteRenderer>().sprite = images[1];
        yield return new WaitForSeconds(1.5f);
        damageOn = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = images[2];
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().sprite = images[0];
        damageOn = false;
        transform.position = savevector;
        spearon = false;
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            trapped = true;
        }    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trapped = false;
        }
    }
}
