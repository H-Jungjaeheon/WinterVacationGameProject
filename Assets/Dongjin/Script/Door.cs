using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Door : MonoBehaviour
{
    private bool opendoor = false;
    [SerializeField] bool bossdoor,nextdoor,isbigroom;
    public GameObject door;
    private GameObject player;
    public Text text;
    private GameObject stop, audioSource;
    Image FadIn;
    private void Start()
    {
        audioSource = GameObject.Find("DoorSounds").gameObject;
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
            StartCoroutine(DoorCnt());
            player.transform.position = door.transform.position;
            GameManager.Instance.isRoom = true;
            Debug.Log("�� �Ѿ��");
            audioSource.GetComponent<AudioSource>().Play();
            if (nextdoor)
            {
                GameManager.Instance.bosssurvival = true;
                GameManager.Instance.isBossRoom = false;
                GameManager.Instance.isRoom = false;
                GameManager.Instance.Stage++;
            }
            if (bossdoor)
            {
                GameManager.Instance.BossRoomStart = true;
                GameManager.Instance.BossRoom = true;
            }
            else
                StartCoroutine(BattleStartFaidOut(0.1f));
           
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
    IEnumerator DoorCnt()
    {
        GameManager.Instance.isDoor = true;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.isDoor = false;
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
