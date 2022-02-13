using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Door : MonoBehaviour
{
    private bool opendoor = false;
    public GameObject door;
    private GameObject player;
    public Text text;
    private GameObject stop;
    Image FadIn;
    private void Start()
    {
        FadIn = GameObject.Find("GameManager").GetComponent<GameManager>().FadIn;
        player = GameObject.Find("Player");
        text.gameObject.SetActive(false);
        stop = GameObject.Find("stop").gameObject;
    }
   
    private void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2.5f, 0));
        if (opendoor == true && Input.GetKeyDown(KeyCode.F) &&GameObject.Find("Player").GetComponent<Player>().IsGrab == false)
        {
            player.transform.position = door.transform.position;
            GameManager.Instance.isRoom = true;
            Debug.Log("¾À ³Ñ¾î°¨¿ä");
            StartCoroutine(BattleStartFaidOut(0.01f));


        }
    }
    IEnumerator BattleStartFaidOut(float FaidTime)
    {
        Color color = FadIn.color;
        while (color.a < 1f )
        {
            color.a += Time.deltaTime / FaidTime;
            FadIn.color = color;
            if (color.a >= 1f )
            {
                color.a = 1f;
            }
            yield return null;
        }
        StartCoroutine(BattleStartFaidIn(1f));

    }
    IEnumerator BattleStartFaidIn(float FaidTime)
    {
        Color color = FadIn.color;
        while (color.a > 0f )
        {
            color.a -= Time.deltaTime / FaidTime;
            FadIn.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
            }
            yield return null;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            text.gameObject.SetActive(true);
            opendoor = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            text.gameObject.SetActive(false);
            opendoor = false;
        }
    }
}
