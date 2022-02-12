using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Obj_3 : MonoBehaviour
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
        slider.value = cnt / 100;
        if (isIt && iscollison)
        {
            slider.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) && GameObject.Find("Player").GetComponent<Player>().IsGrab == false )
            {
                cnt += 5;
            }
           
        }
        else
        {
            slider.gameObject.SetActive(false);
            cnt = 0;
        }
        if (cnt >= 100)
        {
            cnt = 0;
            Drop();
        }
        if (cnt > 0)
        { 
            cnt -= Time.deltaTime*10;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Ãæµ¹");
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
            Debug.Log("³ª°¨");
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
        int ran = Random.Range(0, 13);
        if (ran < 7)
        {
            Debug.Log("²Î");
            particle[1].SetActive(true);
            Invoke("Nothing", 4.5f);

        }
        else if (ran < 8)
        {

            Instantiate(Items[0], transform.position, Items[0].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);

        }
        else if (ran < 9)
        {
            Instantiate(Items[1], transform.position, Items[1].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
        }
        else if (ran < 10)
        {
            Instantiate(Items[2], transform.position, Items[2].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
        }
        else if (ran < 11)
        {
            Instantiate(Items[3], transform.position, Items[3].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
        }
        else if (ran < 12)
        {
            Instantiate(Items[4], transform.position, Items[4].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
        }
        else if (ran < 13)
        {
            Instantiate(Items[5], transform.position, Items[5].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
        }
        else if (ran < 14)
        {
            Instantiate(Items[6], transform.position, Items[6].transform.rotation).transform.DOLocalMoveY(DoPos.transform.position.y, 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
        }
    }
    void Nothing()
    {
        particle[1].SetActive(false);

    }
}
