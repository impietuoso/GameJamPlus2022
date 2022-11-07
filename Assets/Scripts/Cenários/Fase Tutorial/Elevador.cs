using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevador : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Transform alvo;
    [SerializeField] private float altura = 10f;
    [SerializeField] private float velocidade = 2f;
    [SerializeField] private bool podeSubir = false;
    [SerializeField] private bool podeColidir = true;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        alvo.position = new Vector2(transform.position.x, transform.position.y + altura); 
    }

    void Update()
    {
        if(transform.position == alvo.position) {
            rb.velocity = new Vector2(0, 0);
            podeSubir = false;
        }
        if(podeSubir) rb.velocity = new Vector2(0, velocidade);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(podeColidir) if(other.gameObject.tag == "Player") podeSubir = true;
    }
}
