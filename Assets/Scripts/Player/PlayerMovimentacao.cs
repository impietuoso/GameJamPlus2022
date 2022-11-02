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
    [SerializeField] private bool canFlip = true;

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
    private bool soltou = false;

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
    public float timeKnockbacked = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        //anim = GetComponent<Animator>();
        gravidadeNormal = rb.gravityScale;
    }

    #region INPUTS
    private void Update() {

        PlayerStatus.instance.playerAnim.SetBool("IsJumping", !Physics2D.OverlapCircle(checkChao.position, 0.1f, camadaChao));
        if (Input.GetAxisRaw("Horizontal") != 0) PlayerStatus.instance.playerAnim.SetBool("IsRunning", true);
        else PlayerStatus.instance.playerAnim.SetBool("IsRunning", false);

        if (atingido) return;

        horizontal = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) {
            AudioManager.instance.PlaySound(PlayerStatus.instance.jumpSound);
            if(contadorDePulos != maximoPulos) {
                contadorDePulos++;
                requisicaoPulo = true;
            }
        }
        if(Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Space)) 
        {
            if(estaNoAr) soltou = true;
            if(contadorDePulos == maximoPulos) requisicaoPulo = false;
        }

        if(Input.GetButtonDown("Dash") || Input.GetKeyDown(KeyCode.L)) if(podeDash) StartCoroutine(Dash());
    }
    
    private void FixedUpdate()
    {
        // Flip
        PlayerFlip();

        // =====================================================================     PULO        =============================================================================================

        // Coyote Efect 
        if (EstaNoChao())
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

        if(contadorTempoPulo < 0) soltou = false;
        if(contadorDePulos == maximoPulos) requisicaoPulo = false;
        if(contadortempoCoyote > 0 && requisicaoPulo)
        {   
            rb.velocity = new Vector2(rb.velocity.x, forcaDoPulo);
            estaNoAr = true;
        }

        if(soltou && estaNoAr) rb.velocity += Vector2.up * Physics2D.gravity.y * 3f * Time.deltaTime;;

        if(contadortempoCoyote < 0 && requisicaoPulo)
        {
            requisicaoPulo = false;
            
            rb.velocity = new Vector2(rb.velocity.x, forcaDoPulo); // double jump
            estaNoAr = true;
        }
        // Aumentando gravidade do pulo
        if (rb.velocity.y < 0) rb.velocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;

        if (estaDashando) return;

        // ================================================================     MOVIMENTAÇÃO        =============================================================================================
        if (podeMover) rb.velocity = new Vector2(horizontal * velocidade, rb.velocity.y);

    }


    #endregion

    #region DASH

    private IEnumerator Dash() {
        AudioManager.instance.PlaySound(PlayerStatus.instance.dashSound);
        podeDash = false;
        estaDashando = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(  transform.localScale.x * forcaDash, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(duracaoDash);
        tr.emitting = false;
        rb.gravityScale = gravidadeNormal;
        estaDashando = false;
        yield return new WaitForSeconds(esperaDash);
        podeDash = true;

    }

    #endregion

    #region MECÂNICAS

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Inimigo" || other.tag == "BalaInimigo")
        {
            float enemyX = other.gameObject.transform.position.x;
            StartCoroutine(KnockBack(enemyX));
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

    #endregion

    #region KNOCKBACK
    private IEnumerator KnockBack(float enemyX)
    {
        MovePlayer(false);
        atingido = true;
        if ( enemyX-transform.position.x > 0) horizontal = -1;
        else horizontal = 1;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce( new Vector2(knockBackX * horizontal, knockBackY), ForceMode2D.Impulse);
        yield return new WaitForSeconds(timeKnockbacked);
        atingido = false;
        MovePlayer(true);
        podeDash = PlayerStatus.instance.DashHabilitado();
        maximoPulos = PlayerStatus.instance.PuloDuploHabilitado();
    }
    #endregion

    #region SUPORTE E VERIFICAÇÕES

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

    private void PlayerFlip()
    {
        if (canFlip)
        {
            if (!olharDireita && horizontal > 0) Virar();
            else if (olharDireita && horizontal < 0) Virar();
        }
    }

    private void MovePlayer(bool canMakeMoviment)
    {
        podeMover = canMakeMoviment;
        GetComponent<PlayerAtirar>().setPodeAtirar(canMakeMoviment);
        canFlip = canMakeMoviment;
    }

    #endregion
}
