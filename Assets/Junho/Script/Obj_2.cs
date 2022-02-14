using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Obj_2 : MonoBehaviour
{
    public GameObject Interaction;
    public GameObject[] Items;
    public bool isIt;
    bool iscollison;
    public Slider slider;

    public Sprite Open;

    [SerializeField]
    public GameObject[] particle;

    [SerializeField] public GameObject DoPos;
    public int boxIdx;
    // Start is called before the first frame update
    void Start()
    {
        iscollison = false;
        isIt = true;
        particle[0].SetActive(false);
        particle[1].SetActive(false);
        slider.gameObject.SetActive(false);

    }

    // Update is called once per frame
    float cnt;
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.6f, 0));
        isParticle();
        slider.value = cnt / 4;
        if (isIt && iscollison)
        {
            slider.gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.F) && GameObject.Find("Player").GetComponent<Player>().IsGrab == false )
            {
                cnt += Time.deltaTime;
            }
            else cnt = 0;
        }
        else
        {
            slider.gameObject.SetActive(false);
            cnt = 0;
        }
        if (cnt >= 4) 
        { 
           cnt = 0;
           Drop();
        }    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("醱給");
            if (isIt == true)
            {
                iscollison = true;
                Interaction.SetActive(true);
                slider.gameObject.SetActive(true);

            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("釭馬");
            iscollison = false;
            Interaction.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);

        }
    }
    void isParticle()
    {
        if (isIt == true)
        {
            particle[0].SetActive(true);
        }
        else
        {
            particle[0].SetActive(false);

        }
    }
    public void Drop()
    {
        isIt = false;
        GetComponent<SpriteRenderer>().sprite = Open;
        Interaction.SetActive(false);
        int ran = Random.Range(0, 5);
        int itemRan = Random.Range(0, 6);
        switch (boxIdx)
        {
            case 0:
                switch (ran)
                {
                    case 5:
                        Instantiate(Items[itemRan], transform.position, Items[itemRan].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
                        break;
                    default:
                        if (ran < 5)
                        {
                            Debug.Log("統");
                            particle[1].SetActive(true);
                            Invoke("Nothing", 4.5f);
                            break;
                        }
                        break;
                }
                break;
            case 1:
                switch (ran)
                {
                    case 5:
                        Instantiate(Items[itemRan], transform.position, Items[itemRan].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
                        break;
                    default:
                        if (ran < 5)
                        {
                            Debug.Log("統");
                            particle[1].SetActive(true);
                            Invoke("Nothing", 4.5f);
                            break;
                        }
                        break;
                }
                break;
        }
    }
    void Nothing()
    {
        particle[1].SetActive(false);

    }
}
