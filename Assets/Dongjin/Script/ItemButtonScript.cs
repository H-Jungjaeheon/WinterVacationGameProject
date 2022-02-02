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
    }
    public void idxup()
    {
        idx++;
        Debug.Log(idx);
    }
}
