using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBossImage : MonoBehaviour
{
    [SerializeField] Image NowImage;
    [SerializeField] Sprite[] Change; //0 - ù��° �̹���, 1-�ι�° �̹��� 2,3,4,5,6 ����° �̹��� ����
    [SerializeField] int RandImgCount;
    [SerializeField] bool IsChange;
    [SerializeField] GameObject FinalBoss;
    // Start is called before the first frame update
    void Start()
    {
        NowImage = GetComponent<Image>();
        RandImgCount = Random.Range(2, 7);
        IsChange = true;
    }

    // Update is called once per frame
    void Update()
    {
        ImageChange();
    }
    void ImageChange()
    {
        if (FinalBoss.GetComponent<BattleFinalBoss>().InstantDeaths == 0)
        {
            IsChange = true;
            NowImage.sprite = Change[0];
        }
        else if (FinalBoss.GetComponent<BattleFinalBoss>().InstantDeaths == 1 || FinalBoss.GetComponent<BattleFinalBoss>().InstantDeaths == 2)
        {
            NowImage.sprite = Change[1];
        }
        else if (FinalBoss.GetComponent<BattleFinalBoss>().InstantDeaths == 3 && IsChange == true)
        {
            IsChange = false;
            NowImage.sprite = Change[RandImgCount];
            RandImgCount = Random.Range(2, 7);
        }
    }
}
