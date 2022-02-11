using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonScript : MonoBehaviour
{
    public int idx = 0;
    public int itemsave;
    private bool Saveitem;
    private void Update()
    {
        
    }
    public void ImageUpdate(GameObject item)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        itemsave = item.GetComponent<testItem>().itemidx;
    }
    public void idxset()
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
