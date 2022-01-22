using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testItem : MonoBehaviour
{
    public GameObject slotItem;
    public GameObject Interaction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Interaction.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                PotionInventory inven = collision.GetComponent<PotionInventory>();
                for (int i = 0; i < inven.slots.Count; i++)
                {
                    if (inven.slots[i].isEmpty)
                    {
                        Instantiate(slotItem, inven.slots[i].slotObj.transform, false);
                        inven.slots[i].isEmpty = false;
                        Destroy(this.gameObject);
                        break;
                    }
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Interaction.SetActive(false);
        }
    }
}
