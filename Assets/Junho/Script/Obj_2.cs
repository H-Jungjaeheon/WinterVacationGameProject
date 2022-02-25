using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Obj_2 : Box
{
    public GameObject[] Items;
   
   
    public Slider slider;

    public Sprite Open;

   
    [SerializeField] public GameObject DoPos;
    public int boxIdx;

    public ParticleSystem money;
    private GameObject audioSource;
    public bool BoxDrop;


    // Start is called before the first frame update
    void Start()
    {
        iscollison = false;
        
        particle[0].SetActive(false);
        particle[1].SetActive(false);
        slider.gameObject.SetActive(false);
        if (boxIdx == 0)
        {
            audioSource = GameObject.Find("BoxSounds").gameObject;
        }
        else if (boxIdx == 1)
        {
            audioSource = GameObject.Find("WoodBox").gameObject;
        }
    }

    IEnumerator Cnt()
    {
        yield return new WaitForSeconds(0.01f);
        GameObject.Find("Player").GetComponent<Player>().Chest.RemoveAt(0);
    }
    // Update is called once per frame
    float cnt;
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.6f, 0));
        isParticle();
        slider.value = cnt / 4;
        if (isBoxOpen == false && iscollison)
        {
            slider.gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.F) && GameObject.Find("Player").GetComponent<Player>().IsGrab == false && GameObject.Find("Player").GetComponent<Player>().Chest[0] == gameObject)
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
            StartCoroutine(Cnt());

        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        slider.gameObject.SetActive(true);
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        slider.gameObject.SetActive(false);
    }
   
    public void Drop()
    {
        isBoxOpen = true;
        BoxDrop = true;
        GetComponent<SpriteRenderer>().sprite = Open;
        int ran = Random.Range(0, 3);
        int itemRan = Random.Range(0, 6);

        if (boxIdx == 0)
        {
            audioSource.GetComponent<AudioSource>().Play();
        }
        else if (boxIdx == 1)
        {
            audioSource.GetComponent<AudioSource>().Play();
        }

        switch (boxIdx)
        {
            case 0: // 상자
                switch (ran)
                {
                    case 0:
                        money.GetComponent<ParticleSystem>().Play();
                        GameManager.Instance.Money += 10;
                        //Instantiate(Money,transform.position,Money.transform.rotation).transform.DOLocalMove
                        break;

                    case 1:
                        Instantiate(Items[itemRan], transform.position, Items[itemRan].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);

                        break;
                    case 2:
                        Instantiate(Items[itemRan], transform.position, Items[itemRan].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);

                        break;

                    case 3:
                        money.GetComponent<ParticleSystem>().Play();
                        GameManager.Instance.Money += 10;
                        Instantiate(Items[itemRan], transform.position, Items[itemRan].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
                        break;

                }
                break;
            case 1://드럼통
                switch (ran)
                {
                    case 0:
                        Debug.Log("꽝");
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
                        Instantiate(Items[itemRan], transform.position, Items[itemRan].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);

                        break;

                }
                break;
        }
    }
    
}
