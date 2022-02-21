using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorsetting : MonoBehaviour
{
    public Transform door;
    public Transform door2;
    private int dooridx;
    private int Randomdooridx;
    private List<GameObject> doorgroup = new List<GameObject>();
    private List<GameObject> doorgroup2 = new List<GameObject>();
    private void Start()
    {
        Adddoor(door, doorgroup);
        Adddoor(door2, doorgroup2);
        for (int a = doorgroup2.Count-1; 0 <= a; a--)
        {
            dooridx = doorgroup.Count;
            Randomdooridx = Random.Range(0, dooridx);
            //dooridx--;

            doorgroup[Randomdooridx].GetComponent<Door>().door = doorgroup2[0];
            doorgroup2[0].GetComponent<Door>().door = doorgroup[Randomdooridx];
            doorgroup.RemoveAt(Randomdooridx);
            doorgroup2.RemoveAt(0);
        }
    }
    private void Update()
    {
        
    }
    void Adddoor(Transform group, List<GameObject> doorList)
    {
        foreach (Transform door in group)
        {
            //door.gameObject.SetActive(true);
            doorList.Add(door.gameObject);
        }
    }
}
