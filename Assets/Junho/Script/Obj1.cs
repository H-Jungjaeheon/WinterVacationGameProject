using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Obj1 : Box
{
    public GameObject[] Items;

    public Sprite Open;


    [SerializeField] public GameObject DoPos;

    public int boxIdx;
    [SerializeField] private bool mobspawn;
    [SerializeField] private GameObject mob;
    private GameObject audioSource;
    public ParticleSystem money;
    public bool BoxDrop;
    // Start is called before the first frame update
    void Start()
    {
        money.GetComponent<ParticleSystem>().Stop();
        iscollison = false;
        isBoxOpen = false;
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

        if (isBoxOpen == false && iscollison && Input.GetKeyDown(KeyCode.F) && GameObject.Find("Player").GetComponent<Player>().IsGrab == false && GameObject.Find("Player").GetComponent<Player>().Chest[0]==gameObject)
        {
            Drop();
            StartCoroutine(Cnt());

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
        if (boxIdx == 0)
        {
            audioSource.GetComponent<AudioSource>().Play();
        }
        else if (boxIdx == 1)
        {
            audioSource.GetComponent<AudioSource>().Play();
        }
        isBoxOpen = true;
        BoxDrop = true;
        GetComponent<SpriteRenderer>().sprite = Open;
        int ran = Random.Range(0, 3);
        int itemRan = Random.Range(0, 6);

        switch (boxIdx)
        {
            case 0: // 惑磊
                switch (ran)
                {
                    case 0:
                        Debug.Log("参");
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
            case 1://唱公 惑磊
                switch (ran)
                {
                    case 0:
                        Debug.Log("参");
                        particle[1].SetActive(true);
                        Invoke("Nothing", 4.5f);
                        break;

                    case 1:
                        Debug.Log("参");
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
    IEnumerator Cnt()
    {
        yield return new WaitForSeconds(0.01f);
        GameObject.Find("Player").GetComponent<Player>().Chest.RemoveAt(0);
    }
}
