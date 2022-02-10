using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDamageText : MonoBehaviour
{
    public float moveSpeed, damage;
    public Text text;
    [SerializeField] bool IsUp = false;

    // Start is called before the first frame update
    void Start()
    {
        text.text = damage.ToString();
        StartCoroutine("DamageText", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsUp == true)
        {
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        }
    }
    IEnumerator DamageText(float Times)
    {
        yield return new WaitForSeconds(0.4f);
        IsUp = true;
        Color color = text.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / Times;
            text.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
            }
            yield return null;
        }
        Destroy(this.gameObject);
        yield return null;
    }
}


