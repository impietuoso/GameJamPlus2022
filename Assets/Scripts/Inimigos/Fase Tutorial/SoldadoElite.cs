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
    [SerializeField] private float distanciaParaAndar = 10f;
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private bool patrulhando = true;
    [SerializeField] private float ponto1 = 0f;
    [SerializeField] private float ponto2 = 0f;
    [Header("Status")]
    [SerializeField] private int vida = 30;
    [Header("Outros")]
    [SerializeField] private float distancia;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform target;
    [SerializeField] private bool olhandoDireita;
    private Rigidbody2D rb;
    public AudioClip hitSound;

    private void Awake() {
        posInicial = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        ponto1 = posInicial+distanciaParaAndar;
        ponto2 = posInicial-distanciaParaAndar;
        olhandoDireita = false;
    }

    private void Update()
    {
        if(target != null) 
        {
            distancia = target.position.x - transform.position.x;
            if(distancia < 0 && olhandoDireita) Virar();
            if(distancia > 0 && !olhandoDireita) Virar();
        }

        if(CheckPlayer()) patrulhando = false;
        else{
            patrulhando = true;
            if( Mathf.Abs(transform.position.x-ponto1) <= 1f && olhandoDireita || Mathf.Abs(transform.position.x-ponto2) <= 1f  && !olhandoDireita) {
                
                Virar();
            }
        }

        if(CheckPlayer() && !atirando) podeAtirar = true;
        else podeAtirar = false;

        if(podeAtirar) StartCoroutine(Atirar());
    }

    private void FixedUpdate() {

        if(patrulhando) rb.velocity = new Vector2(-velocidade, rb.velocity.y);
        else rb.velocity = new Vector2(0, rb.velocity.y);
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
        patrulhando = false;
        podeAtirar = false;
        for (int i = 0; i < 3; i++)
        {
            GameObject bala = ObjectPoolingInimigos.instance.PegarBala();

            if (bala != null) 
            {
                bala.transform.position = posicaoBala.position;
                bala.SetActive(true);
                bala.GetComponent<Bala>().direcao = (int)transform.localScale.x * -1;
            }
            yield return new WaitForSeconds(0.3f);
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
        olhandoDireita = !olhandoDireita;
        velocidade = velocidade * -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
