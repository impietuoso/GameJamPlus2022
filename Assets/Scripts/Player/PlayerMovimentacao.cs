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
    [SerializeField] private float horizontal;
    private bool podeMover = true;
    [SerializeField] private bool olharDireita = true;

    [Header("Pulo")]
    public Transform checkChao;
    public LayerMask camadaChao;
    [SerializeField] private int contadorDePulos = 0;
    [SerializeField] private int maximoPulos = 1;
    [SerializeField] private float forcaDoPulo = 8f;
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
    [SerializeField] private bool podeDash = false;
    [SerializeField] private bool estaDashando = false;
    private TrailRenderer tr;
    private float gravidadeNormal;

    [Header("KnockBack")]
    public float knockBackX = 100f;
    public float knockBackY = 10f;
    public float direcaoKnockBack = 0;
    public bool atingido = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        //anim = GetComponent<Animator>();
        gravidadeNormal = rb.gravityScale;
    }

    // =====================================================================     INPUTS        =============================================================================================
    private void Update() {

        PlayerStatus.instance.playerAnim.SetBool("IsJumping", !Physics2D.OverlapCircle(checkChao.position, 0.1f, camadaChao));
        if (Input.GetAxisRaw("Horizontal") != 0)
            PlayerStatus.instance.playerAnim.SetBool("IsRunning", true);
        else
            PlayerStatus.instance.playerAnim.SetBool("IsRunning", false);

        if (atingido) return;

        horizontal = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) {
            AudioManager.instance.PlaySound(PlayerStatus.instance.jumpSound);
            requisicaoPulo = true;
            if(contadorDePulos != maximoPulos) contadorDePulos++;
        }
        if(Input.GetButton("Jump") || Input.GetKey(KeyCode.Space)) apertando = true;
        if(Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Space)) 
        {
            if(contadorDePulos == maximoPulos) requisicaoPulo = false;
            apertando= false;
        }

        if(Input.GetButtonDown("Dash") || Input.GetKeyDown(KeyCode.L)) if(podeDash) StartCoroutine(Dash());
    }

    private void FixedUpdate()
    {
        // não permite virar nem pular nem nada enquanto está no dash
        //if (estaDashando) return;

        // ================================================================     MOVIMENTAÇÃO        =============================================================================================
        //if (podeMover) rb.velocity = new Vector2(horizontal * velocidade, rb.velocity.y);

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

        if(contadorTempoPulo < 0)
        {
            //estaNoAr = false;
            apertando = false;
        } 
        if(contadorDePulos == maximoPulos) requisicaoPulo = false;
        if(contadortempoCoyote > 0 && requisicaoPulo)
        {
            AnimacaoPular();
            
            rb.velocity = new Vector2(rb.velocity.x, forcaDoPulo);
            estaNoAr = true;
            
        }

        if(apertando && estaNoAr)
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaDoPulo);
        }

        if(contadortempoCoyote < 0 && requisicaoPulo)
        {
            requisicaoPulo = false;
            AnimacaoPular();
            rb.velocity = new Vector2(rb.velocity.x, forcaDoPulo);
            estaNoAr = true;
        }
        // Aumentando gravidade do pulo
        if (rb.velocity.y < 0) rb.velocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;


        if (estaDashando) return;

        // ================================================================     MOVIMENTAÇÃO        =============================================================================================
        if (podeMover) rb.velocity = new Vector2(horizontal * velocidade, rb.velocity.y);

    }

    // ===========================================================================      DASH        =============================================================================================

    private IEnumerator Dash() {
        AudioManager.instance.PlaySound(PlayerStatus.instance.dashSound);
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

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Inimigo")
        {
            StartCoroutine(KnockBack());
        }
        if(other.tag == "ArmaduraDash") 
        {
            StartCoroutine(PegarHabilidade()); 
        }
        if(other.tag == "ArmaduraPuloDuplo")
        {
            StartCoroutine(PegarHabilidade());
        } 
    }

    private IEnumerator PegarHabilidade()
    {
        yield return new WaitForSeconds(0.5f);
        if(!podeDash) podeDash = PlayerStatus.instance.DashHabilitado();
        if(maximoPulos == 1) maximoPulos = PlayerStatus.instance.PuloDuploHabilitado();
        AudioManager.instance.PlaySound(PlayerStatus.instance.atkSound);
    }
 
     // ===========================================================================      KNOCKBACK        =============================================================================================
    private IEnumerator KnockBack()
    {
        podeMover = false;
        atingido = true;
        if(olharDireita) horizontal = -1 ;
        else horizontal = 1 ;
        rb.velocity = new Vector2(knockBackX * horizontal, 0 );
        rb.AddForce( new Vector2(0, knockBackY), ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);
        atingido = false;
        podeMover = true;
        podeDash = PlayerStatus.instance.DashHabilitado();
        maximoPulos = PlayerStatus.instance.PuloDuploHabilitado();
    }

    // ===========================================================================      SUPORTE E VERIFICAÇÕES      ============================================================================

    private bool EstaNoChao()
    {
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
   
    private void AnimacaoDash() {
        //PlayerStatus.instance.playerAnim.SetTrigger("Dash");
    }

    private void AnimacaoCorrer() {
        //PlayerStatus.instance.playerAnim.SetBool("IsRunning", true);
    }

    private void AnimacaoPular() {

    }

    private void AnimacaoIdle() {
        //PlayerStatus.instance.playerAnim.SetBool("IsRunning", false);
    }
}
