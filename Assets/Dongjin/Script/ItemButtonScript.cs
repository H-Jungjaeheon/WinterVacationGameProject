using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonScript : MonoBehaviour
{
    private int idx;
    public void ImageUpdate(SpriteRenderer itemimage)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = itemimage.sprite;
        transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
    public void idxup()
    {
        idx++;
        this.transform.GetChild(1).GetComponent<Text>().text = "X" + idx.ToString();
    }
}
