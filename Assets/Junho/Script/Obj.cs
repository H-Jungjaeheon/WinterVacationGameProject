using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Obj : MonoBehaviour
{
    public GameObject Interaction;
    public GameObject[] Items;
    public GameObject Money;

    public bool isIt;
    bool iscollison;

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
    }

    // Update is called once per frame
    void Update()
    {
        isParticle();

        if (isIt==true&&iscollison&&Input.GetKey(KeyCode.F) && GameObject.Find("Player").GetComponent<Player>().IsGrab == false )
        {
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

            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            iscollison = false;
            Debug.Log("釭馬");
            Interaction.gameObject.SetActive(false);
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
            //transform.DOMove(transform.position, 1f).SetEase(Ease.OutBack).OnComplete(() =>
            //{
            //    Items[1].transform.DOMove(DoPos.transform.position, 0.5f).SetEase(Ease.OutBack);
            //});
        
        isIt = false;
        GetComponent<SpriteRenderer>().sprite = Open;
        Interaction.SetActive(false);
        int ran = Random.Range(0, 5);
        int itemRan = Random.Range(0,6);
        
        switch (boxIdx)
        {
            case 0:
                switch (ran)
                {
                    case 4:
                        //Instantiate(Money,transform.position,Money.transform.rotation).transform.DOLocalMove
                        break;

                    case 5:
                        Instantiate(Items[itemRan], transform.position, Items[itemRan].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
                        break;                  
                    default:
                        if (ran < 4)
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
