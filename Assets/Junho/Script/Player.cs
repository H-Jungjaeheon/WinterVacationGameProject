using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : PlayerControl
{
   
    [SerializeField]
    private State health;
    [SerializeField]
    private State survive;

    private float initHealth = 100;
    private float initSurvive = 100;

    
    // Start is called before the first frame update
   
    protected override void Start()
    {
        health.Initialize(initHealth, initHealth);
        survive.Initialize(initSurvive, initSurvive);

        base.Start();
    }
    // Update is called once per frame
    void Update()
    {
        health.MyCurrentValue -=1;
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
      
    }
}
