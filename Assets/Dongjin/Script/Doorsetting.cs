using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorsetting : MonoBehaviour
{
    public Transform door;
    public Transform door2;
    private int dooridx;
    private int Randomdooridx;
    public List<GameObject> doorgroup = new List<GameObject>();
    public List<GameObject> doorgroup2 = new List<GameObject>();
    private void Start()
    {
        Adddoor(door, doorgroup);
        Adddoor(door2, doorgroup2);
        dooridx = doorgroup.Count;
        for (int a = doorgroup2.Count-1; 0 <= a; a--)
        {
            Randomdooridx = Random.RandomRange(0, dooridx);
            dooridx--;
            doorgroup[Randomdooridx].GetComponent<Door>().door = doorgroup2[a];
            doorgroup2[a].GetComponent<Door>().door = doorgroup[Randomdooridx];
            doorgroup.RemoveAt(Randomdooridx);
            doorgroup2.RemoveAt(a);
        }
    }
    private void Update()
    {
        
    }
    void Adddoor(Transform group, List<GameObject> doorList)
    {
        foreach (Transform door in group)
        {
            door.gameObject.SetActive(false);
            doorList.Add(door.gameObject);
        }
    }
}
