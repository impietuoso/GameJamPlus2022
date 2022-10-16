using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovimentacao : MonoBehaviour
{
    [Header("Prefab")]
    private Rigidbody2D rb;
    //private Animator anim;
    public GameObject player;
    

    [Header("Movimentação")]
    [SerializeField] private float velocidade = 8f;
    private float horizontal;
    private bool podeMover = true;
    [SerializeField] private bool olharDireita = true;

    [Header("Pulo")]
    public Transform checkChao;
    public LayerMask camadaChao;
    [SerializeField] private int contadorDePulos = 0;
    [SerializeField]  private float forcaDoPulo = 8f;
    [SerializeField] private float tempoCoyote = 0.2f;
    [SerializeField] private float contadortempoCoyote;
    [SerializeField] private float tempoPulo = 0;
    [SerializeField] private float contadorTempoPulo = 0;
    [SerializeField] private bool apertando = false;
    [SerializeField] private bool requisicaoPulo = false;
    [SerializeField] private bool estaNoAr = false;

    [Header("Dash")]
    [SerializeField] private float forcaDash = 24f;
    [SerializeField] private float duracaoDash = 0.5f;
    [SerializeField] private float esperaDash = 2f;
    [SerializeField] private float gravidadeDash;
    [SerializeField] private bool podeDash = true;
    [SerializeField] private bool estaDashando = false;
    private TrailRenderer tr;
    private float gravidadeNormal;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        //anim = GetComponent<Animator>();
        gravidadeNormal = rb.gravityScale;
    }

    // =====================================================================     INPUTS        =============================================================================================
    private void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump")) requisicaoPulo = true;
        if(Input.GetButton("Jump")) apertando = true;
        if(Input.GetButtonUp("Jump")) 
        {
            if(contadorDePulos >= 2) requisicaoPulo = false;
            apertando= false;
        }

        if(Input.GetButtonDown("Dash")) if(podeDash) StartCoroutine(Dash());
    }

    private void FixedUpdate()
    {
        // não permite virar nem pular nem nada enquanto está no dash
        if (estaDashando) return;

        // ================================================================     MOVIMENTAÇÃO        =============================================================================================
        if (podeMover) rb.velocity = new Vector2(horizontal * velocidade, rb.velocity.y);
    
        // Flip
        if (!olharDireita && horizontal > 0) Virar();
        else if (olharDireita && horizontal < 0) Virar();

        // =====================================================================     PULO        =============================================================================================

        // Coyote Efect 
        if(EstaNoChao())
        {
            estaNoAr = false;
            contadorDePulos = 0;
            contadortempoCoyote = tempoCoyote;
            contadorTempoPulo = tempoPulo;
        }else 
        {
            contadortempoCoyote -= Time.deltaTime;
            contadorTempoPulo -= Time.deltaTime;
        }

        if(contadorTempoPulo < 0) estaNoAr = false;

        if(contadortempoCoyote > 0 && requisicaoPulo)
        {
            AnimacaoPular();
            contadorDePulos++;
            rb.velocity = new Vector2(rb.velocity.x, forcaDoPulo);
            estaNoAr = true;
            if(contadorDePulos >= 2) requisicaoPulo = false;
        }

        if(apertando && estaNoAr)
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaDoPulo);
        }

        if(contadortempoCoyote < 0 && requisicaoPulo)
        {
            AnimacaoPular();
            contadorDePulos++;
            rb.velocity = new Vector2(rb.velocity.x, forcaDoPulo);
            estaNoAr = true;
            if(contadorDePulos >= 2) requisicaoPulo = false;
        }
        // Aumentando gravidade do pulo
        if (rb.velocity.y < 0) rb.velocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;

        
    }



    // ===========================================================================      DASH        =============================================================================================

    private IEnumerator Dash() 
    {
        podeDash = false;
        estaDashando = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(  transform.localScale.x * forcaDash, 0f);
        tr.emitting = true;
        AnimacaoDash();
        yield return new WaitForSeconds(duracaoDash);
        tr.emitting = false;
        rb.gravityScale = gravidadeNormal;
        estaDashando = false;
        yield return new WaitForSeconds(esperaDash);
        podeDash = true;

    }

    // ===========================================================================      SUPORTE E VERIFICAÇÕES      ============================================================================

    private bool EstaNoChao()
    {
        if (Physics2D.OverlapCircle(checkChao.position, 0.1f, camadaChao)) {
        } 
        return Physics2D.OverlapCircle(checkChao.position, 0.1f, camadaChao);
    }

    private void Virar() 
    {
        olharDireita = !olharDireita;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    // ===========================================================================        ANIMAÇÕES        =====================================================================================
   

    private void AnimacaoDash() 
    {
        //anim.Play("Dash");// animação do Dash
    }

    private void AnimacaoCorrer()
    {
        //if (podeMover) anim.Play("Run"); // animação de correr
    }

    private void AnimacaoPular()
    {
        //anim.Play("Jump");// animação de pular
    }

    private void AnimacaoIdle() 
    {
        //anim.Play("Idle");// animação idle
    }
}
