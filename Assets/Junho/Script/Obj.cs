using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Obj : MonoBehaviour
{
    public GameObject[] Items;

    public bool isIt;
    bool iscollison;

    public Sprite Open;

    [SerializeField]
    public GameObject[] particle;

    [SerializeField] public GameObject DoPos;

    public int boxIdx;
    [SerializeField] private bool mobspawn;
    [SerializeField] private GameObject mob;
    private GameObject audioSource;
    public ParticleSystem money;
    // Start is called before the first frame update
    void Start()
    {
        money.GetComponent<ParticleSystem>().Stop();
        iscollison = false;
        isIt = true;
        particle[0].SetActive(false);
        particle[1].SetActive(false);
        if (boxIdx == 0)
        {
            audioSource = GameObject.Find("BoxSounds").gameObject;
        }
        else if (boxIdx == 1)
        {
            audioSource = GameObject.Find("WoodBox").gameObject;
        }
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

            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            iscollison = false;
            Debug.Log("釭馬");
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
        if (mobspawn == true)
        {
            Instantiate(mob, this.transform.position, transform.rotation);
        }
        if(boxIdx == 0)
        {
            audioSource.GetComponent<AudioSource>().Play();
        }
        else if(boxIdx == 1)
        {
            audioSource.GetComponent<AudioSource>().Play();
        }
        isIt = false;
        GetComponent<SpriteRenderer>().sprite = Open;
        int ran = Random.Range(0, 3);
        int itemRan = Random.Range(0,6);
        
        switch (boxIdx)
        {
            case 0: // 鼻濠
                switch (ran)
                {
                    case 0:
                        Debug.Log("統");
                        particle[1].SetActive(true);
                        Invoke("Nothing", 4.5f);
                        break;

                    case 1:
                        money.GetComponent<ParticleSystem>().Play();
                        GameManager.Instance.Money += 10;
                        //Instantiate(Money,transform.position,Money.transform.rotation).transform.DOLocalMove
                        break;
                    case 2:
                        money.GetComponent<ParticleSystem>().Play();
                        GameManager.Instance.Money += 10;
                        //Instantiate(Money,transform.position,Money.transform.rotation).transform.DOLocalMove
                        break;

                    case 3:
                        money.GetComponent<ParticleSystem>().Play();
                        GameManager.Instance.Money += 10;
                        Instantiate(Items[itemRan], transform.position, Items[itemRan].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);

                        break;
                }
                break;
            case 1://釭鼠 鼻濠
                switch (ran)
                {
                    case 0:
                        Debug.Log("統");
                        particle[1].SetActive(true);
                        Invoke("Nothing", 4.5f);
                        break;

                    case 1:
                        Debug.Log("統");
                        particle[1].SetActive(true);
                        Invoke("Nothing", 4.5f);
                        break;
                    case 2:
                        money.GetComponent<ParticleSystem>().Play();
                        GameManager.Instance.Money += 10;
                        //Instantiate(Money,transform.position,Money.transform.rotation).transform.DOLocalMove
                        break;

                    case 3:
                        money.GetComponent<ParticleSystem>().Play();
                        GameManager.Instance.Money += 10;
                        //Instantiate(Money,transform.position,Money.transform.rotation).transform.DOLocalMove
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
