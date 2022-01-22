using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    protected virtual void Start()
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

        transform.Translate(new Vector2(x, 0) * Time.deltaTime * speed);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            speed = 0;
        }
    }
}
