using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
     private Animator anim; 
     public float upForce = 200f;
     private bool isDead = false;
     private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(GameControl.Instance.gameState == GameControl.GameStatus.PLAYED)
        {
            if (isDead)
            { 
                return;
            }
        
            if (Input.GetKeyDown("space"))
            {
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(new Vector2(0, upForce));
                anim.SetTrigger("Flap");
            }
        } 
    }


    void OnCollisionEnter2D()
    {
        isDead = true;
        rb2d.velocity = Vector2.zero;
        anim.SetTrigger("Die");
        GameControl.Instance.Die();
    }
}
