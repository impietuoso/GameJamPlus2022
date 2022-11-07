using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevador : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float posInicial;
    [SerializeField] private float altura = 10f;
    [SerializeField] private float velocidade = 2f;
    [SerializeField] private bool podeSubir = false;
    [SerializeField] private bool podeColidir = true;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        posInicial = transform.position.y;
    }

    void Update()
    {
        if(transform.position.y >= (posInicial + altura)) {
            rb.velocity = new Vector2(0, 0);
            podeSubir = false;
        }
        if(podeSubir == true) rb.velocity = new Vector2(0, velocidade);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(podeColidir){
            if(other.gameObject.tag == "Player") 
            {
                podeColidir = false;
                podeSubir = true;
            }
        }
    }
}
