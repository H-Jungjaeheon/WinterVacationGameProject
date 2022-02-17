using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEye : MonoBehaviour
{
    [SerializeField] Image Eye;
    [SerializeField] bool IsDestroy;
    [SerializeField] float MaxOne, MaxZero;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetAsFirstSibling();
        IsDestroy = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsDestroy == false)
        {
            StartCoroutine(EyeControll(1.5f));
        }
        else
        {
            StartCoroutine(EyeControll2(1.5f));
        }
    }
    IEnumerator EyeControll(float time)
    {
        Color color = Eye.color;
        while (color.a < MaxOne)
        {
            color.a += Time.deltaTime / time;
            Eye.color = color;
            if (color.a >= MaxOne)
            {
                color.a = MaxOne;
            }
            yield return null;
        }
        yield return new WaitForSeconds(2);
        IsDestroy = true;
    }
    IEnumerator EyeControll2(float time)
    {
        Color color = Eye.color;
        while (color.a > MaxZero)
        {
            color.a -= Time.deltaTime / time;
            Eye.color = color;
            if (color.a <= MaxZero)
            {
                color.a = MaxZero;
            }
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
