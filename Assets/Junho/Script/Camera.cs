using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public Transform Battletarget;
    public Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.IsBattleStart == false)
        {
            transform.position = target.position + offset;
        }
        else
        {
            Invoke("BattleCameraMove", 2f);
        }
    }
    void BattleCameraMove()
    {
        this.transform.position = Battletarget.position + offset;
    }
    //IEnumerator BattleCameraMove()
    //{

    //    yield return null;
    //}
}
