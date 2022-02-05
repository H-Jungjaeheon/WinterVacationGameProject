using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonScript : MonoBehaviour
{
    public int idx = 0;
    private void Update()
    {
        
    }
    public void ImageUpdate(SpriteRenderer itemimage)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = itemimage.sprite;
        transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
    public void idxup()
    {
        if (idx == 0)
        {
            this.transform.GetChild(1).GetComponent<Text>().text = "";
        }
        else
        {
            this.transform.GetChild(1).GetComponent<Text>().text = "X" + idx.ToString();

        }
    }
}
