using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float velocidade = 20f;
    public int direcao = 0;
    private SpriteRenderer sr;
    public int bulletDuration = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direcao * velocidade, 0);
        if (rb.velocity.x > 0 && sr.flipX) sr.flipX = false;
        if (rb.velocity.x < 0 && !sr.flipX) sr.flipX = true;
    }

    private void OnEnable()
    {
        StartCoroutine(Desabilitar());
    }

    private IEnumerator Desabilitar() 
    {
        yield return new WaitForSeconds(bulletDuration);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.tag);
        gameObject.SetActive(false);
    }
}
