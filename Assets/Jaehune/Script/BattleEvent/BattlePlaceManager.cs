using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlaceManager : MonoBehaviour
{
    [SerializeField] Image[] BattlePlaceImage; //���� ���۽� ���� ��� ���� �̹���
    [SerializeField] Text[] BattlePlaceText, SBattlePlaceText; //���� ���۽� ���� ��� ���� �ؽ�Ʈ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Main Camera").GetComponent<CameraMove>().BossBattleStart == false)
        {
            if (GameManager.Instance.IsBattlePlace == true)
            {
                StartCoroutine(BattlePlaceFaidIn(1));
            }
            else
            {
                StartCoroutine(BattlePlaceFaidOut(1));
            }
        }
        else
        {
            if (GameManager.Instance.IsBattlePlace == true)
            {
                StartCoroutine(BossBattlePlaceFaidIn(1));
            }
            else
            {
                StartCoroutine(BossBattlePlaceFaidOut(1));
            }
        }
    }
    IEnumerator BattlePlaceFaidIn(float FaidTime)
    {
        Color color = BattlePlaceImage[GameManager.Instance.Stage - 1].color;
        Color color2 = BattlePlaceText[GameManager.Instance.Stage - 1].color;
        Color color3 = SBattlePlaceText[GameManager.Instance.Stage - 1].color;
        while (color.a < 1f && color2.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            color2.a += Time.deltaTime / FaidTime;
            color3.a += Time.deltaTime / FaidTime;
            BattlePlaceImage[GameManager.Instance.Stage - 1].color = color;
            BattlePlaceText[GameManager.Instance.Stage - 1].color = color2;
            SBattlePlaceText[GameManager.Instance.Stage - 1].color = color3;
            if (color.a >= 1f && color2.a >= 1f)
            {
                color.a = 1f;
                color2.a = 1f;
                color3.a = 1f;
            }
            yield return null;
        }
    }
    IEnumerator BattlePlaceFaidOut(float FaidTime)
    {
        Color color = BattlePlaceImage[GameManager.Instance.Stage - 1].color;
        Color color2 = BattlePlaceText[GameManager.Instance.Stage - 1].color;
        Color color3 = SBattlePlaceText[GameManager.Instance.Stage - 1].color;
        while (color.a > 0 && color2.a > 0)
        {
            color.a -= Time.deltaTime / FaidTime;
            color2.a -= Time.deltaTime / FaidTime;
            color3.a -= Time.deltaTime / FaidTime;
            BattlePlaceImage[GameManager.Instance.Stage - 1].color = color;
            BattlePlaceText[GameManager.Instance.Stage - 1].color = color2;
            SBattlePlaceText[GameManager.Instance.Stage - 1].color = color3;
            if (color.a <= 0f && color2.a <= 0f)
            {
                color.a = 0f;
                color2.a = 0f;
                color3.a = 0f;
            }
            yield return null;
        }
    }
    IEnumerator BossBattlePlaceFaidOut(float FaidTime)
    {
        Color color = BattlePlaceImage[GameManager.Instance.Stage + 2].color;
        Color color2 = BattlePlaceText[GameManager.Instance.Stage + 2].color;
        Color color3 = SBattlePlaceText[GameManager.Instance.Stage + 2].color;
        while (color.a > 0 && color2.a > 0)
        {
            color.a -= Time.deltaTime / FaidTime;
            color2.a -= Time.deltaTime / FaidTime;
            color3.a -= Time.deltaTime / FaidTime;
            BattlePlaceImage[GameManager.Instance.Stage + 2].color = color;
            BattlePlaceText[GameManager.Instance.Stage + 2].color = color2;
            SBattlePlaceText[GameManager.Instance.Stage + 2].color = color3;
            if (color.a <= 0f && color2.a <= 0f)
            {
                color.a = 0f;
                color2.a = 0f;
                color3.a = 0f;
            }
            yield return null;
        }
    }
    IEnumerator BossBattlePlaceFaidIn(float FaidTime)
    {
        Color color = BattlePlaceImage[GameManager.Instance.Stage + 2].color;
        Color color2 = BattlePlaceText[GameManager.Instance.Stage + 2].color;
        Color color3 = SBattlePlaceText[GameManager.Instance.Stage + 2].color;
        while (color.a < 1f && color2.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            color2.a += Time.deltaTime / FaidTime;
            color3.a += Time.deltaTime / FaidTime;
            BattlePlaceImage[GameManager.Instance.Stage + 2].color = color;
            BattlePlaceText[GameManager.Instance.Stage + 2].color = color2;
            SBattlePlaceText[GameManager.Instance.Stage + 2].color = color3;
            if (color.a >= 1f && color2.a >= 1f)
            {
                color.a = 1f;
                color2.a = 1f;
                color3.a = 1f;
            }
            yield return null;
        }

    }
}
