using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class invenUi : MonoBehaviour
{
    private GameObject buttonsave;
    public void buttonclick()
    {
        buttonsave = EventSystem.current.currentSelectedGameObject.gameObject;
        transform.position = new Vector2(buttonsave.transform.position.x+ 230, buttonsave.transform.position.y);
    }
    public void Use()
    {
        buttonsave.GetComponent<ItemButtonScript>().idx--;
    }
    public void Enrollment()
    {
        
    }
}
