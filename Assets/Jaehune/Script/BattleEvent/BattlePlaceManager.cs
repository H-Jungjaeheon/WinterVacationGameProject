using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlaceManager : MonoBehaviour
{
    [SerializeField] Image[] BattlePlaceImage; //전투 시작시 띄우는 장소 연출 이미지
    [SerializeField] Text[] BattlePlaceText; //전투 시작시 띄우는 장소 연출 텍스트
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsBattlePlace == true)
        {
            StartCoroutine("BattlePlaceFaidIn", 1);
        }
        else
        {
            StartCoroutine("BattlePlaceFaidOut", 1);
        }
    }
    IEnumerator BattlePlaceFaidIn(float FaidTime)
    {
        Color color = BattlePlaceImage[GameManager.Instance.Stage - 1].color;
        Color color2 = BattlePlaceText[GameManager.Instance.Stage - 1].color;
        while (color.a < 1f && color2.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            color2.a += Time.deltaTime / FaidTime;
            BattlePlaceImage[GameManager.Instance.Stage - 1].color = color;
            BattlePlaceText[GameManager.Instance.Stage - 1].color = color2;
            if (color.a >= 1f && color2.a >= 1f)
            {
                color.a = 1f;
                color2.a = 1f;
            }
            yield return null;
        }

    }
    IEnumerator BattlePlaceFaidOut(float FaidTime)
    {
        Color color = BattlePlaceImage[GameManager.Instance.Stage - 1].color;
        Color color2 = BattlePlaceText[GameManager.Instance.Stage - 1].color;
        while (color.a > 0 && color2.a > 0)
        {
            color.a -= Time.deltaTime / FaidTime;
            color2.a -= Time.deltaTime / FaidTime;
            BattlePlaceImage[GameManager.Instance.Stage - 1].color = color;
            BattlePlaceText[GameManager.Instance.Stage - 1].color = color2;
            if (color.a <= 0f && color2.a <= 0f)
            {
                color.a = 0f;
                color2.a = 0f;
            }
            yield return null;
        }
    }
}
