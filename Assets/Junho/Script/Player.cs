using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Jump");
       
        transform.Translate(new Vector2(x, y) * Time.deltaTime * speed);
    }
    void InputManager()
    {
        if (Input.GetKey(KeyCode.F))
        {
            //¿¿æ÷
        }
    }
}
