using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionInventory : MonoBehaviour
{
    public List<PotionSlotData> slots = new List<PotionSlotData>();
    private int maxSlot = 3;
    public GameObject slotPrefab;
    private void Start()
    {
        GameObject slotPanel = GameObject.Find("Panel");
        for (int i = 0; i < maxSlot; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotPanel.transform, false);
            go.name = "Slot_" + i;
            PotionSlotData slot = new PotionSlotData();
            slot.isEmpty = true;
            slot.slotObj = go;
            slots.Add(slot);
        }
    }
}
