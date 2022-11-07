using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldadoElite : MonoBehaviour
{
    [Header("Tiro")]
    [SerializeField] private Transform posicaoBala;
    [SerializeField] private bool podeAtirar = false;
    [SerializeField] private bool atirando = false;
    [SerializeField] private float recarrega = 3f;
    [SerializeField] private float raio = 5f;
    [Header("Patrulha")]
    [SerializeField] private float posInicial;
    [SerializeField] private float distanciaAndar = 10f;
    [SerializeField] private float velocidade = 5f;
    [Header("Status")]
    [SerializeField] private int vida = 30;
    [Header("Outros")]
    [SerializeField] private float distancia;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform target;
    private Rigidbody2D rb;
    public AudioClip hitSound;

    private void Awake() {
        posInicial = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(target != null) 
        {
            distancia = target.position.x - transform.position.x;
            if(distancia > 0 && velocidade < 0) Virar();
            if(distancia < 0 && velocidade > 0) Virar();
        }

        if(!podeAtirar) rb.velocity = new Vector2(velocidade, 0);
        else rb.velocity = new Vector2(0, 0);

        if(transform.position.x >= (posInicial + distanciaAndar) && velocidade > 0) Virar();
        else if(transform.position.x <= (posInicial - distanciaAndar) && velocidade < 0) Virar();   

        if(CheckPlayer() && !atirando) podeAtirar = true;
        else podeAtirar = false;

        if(podeAtirar) StartCoroutine(Atirar());
    }

    private bool CheckPlayer()
    {
        Collider2D colisaoAlvo = Physics2D.OverlapCircle(transform.position, raio, playerLayer);
        if(colisaoAlvo != null) target = colisaoAlvo.transform;
        return Physics2D.OverlapCircle(transform.position, raio, playerLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, raio);
    }
    

    private IEnumerator Atirar() 
    {
        atirando = true;
        podeAtirar = false;
        for (int i = 0; i < 3; i++)
        {
            GameObject bala = ObjectPoolingInimigos.instance.PegarBala();

            if (bala != null) 
            {
                bala.transform.position = posicaoBala.position;
                bala.SetActive(true);
                bala.GetComponent<Bala>().direcao = (int)transform.localScale.y;
            }
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(recarrega);
        podeAtirar = true;
        atirando = false;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BalaPlayer") {
            vida--;
            AudioManager.instance.PlaySound(hitSound);
        }
        
        if(vida < 1) gameObject.SetActive(false);
    }

    private void Virar() 
    {
        velocidade = velocidade * -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
