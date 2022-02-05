using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Spear : MonoBehaviour
{
    private Vector3 savevector;
    public bool spearon;
    private bool trapped;
    public bool damageOn = false;
    private void Update()
    {
        if(trapped == true && damageOn==true)
        {
            GameManager.Instance.stackDamage += 10;
            damageOn = false;
        }    
    }
    IEnumerator spearmove()
    {
        spearon = true;
        savevector = this.transform.position;
        transform.position += new Vector3(0, 0.1f, 0);
        yield return new WaitForSeconds(1.5f);
        damageOn = true;
        transform.position += new Vector3(0, 0.2f, 0);
        yield return new WaitForSeconds(1);
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
