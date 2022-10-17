using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    /*
        Dash    -> força para ir exatamente até o canto do mapa e voltar
        Escudo  -> dano = 0;
        Tiros   -> 2 padrões -> subir tiro descer tiro ... ou ficar embaixo e atirar para caralho
        Lazer   -> laser em cima ou em baixo

        Tiros -> Tiros Pulando -> Dash indo -> Dash Voltando -> Escudo -> Carregando laser -> Subir ou descer -> Laser -> Vunerável -> Tiros 
    
        altura
    */
    [Header("BOSS")]
    private Rigidbody2D rb;
    [SerializeField] private float tempoEntreGolpes = 1f;
    [SerializeField] private float vida = 100f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float raio = 15f;
    [SerializeField] private bool olharDireita;
    [SerializeField] private Transform target;
    [SerializeField] private float distancia;
    [SerializeField] private float tempoInicio = 10f;
    [SerializeField] private bool podeVuneravel = false;
    [SerializeField] private float tempoVuneravel = 5f;

    [Header("Dash")]
    private TrailRenderer tr;
    [SerializeField] private float forcaDash = 24f;
    [SerializeField] private float duracaoDash = 0.5f;
    [SerializeField] private bool podeDash = false;

    [Header("Escudo")]
    [SerializeField] private bool podeEscudo = false;
    [SerializeField] private float tempoEscudo = 10f;
    [SerializeField] private float multiplicadorDano = 0f;

    [Header("SubirEDescer")]
    [SerializeField] private float alturaBaixo = 2f;
    [SerializeField] private float alturaCima = 12f;
    [SerializeField] private bool podeSubir = false;
    [SerializeField] private bool podeDescer = false;
    [SerializeField] private float velocidade = 10f;

    [Header("ObjectPooling")]
    [SerializeField] private GameObject balaBossPrefab;
    private List<GameObject> balasBossPool = new List<GameObject>();
    [SerializeField] private int qtd = 30;
    [SerializeField] private Transform posicaoBala;

    [Header("AtirarPadrão1")]
    [SerializeField] private bool podeMetralhar = false;
    [SerializeField] private float tempoMetralhar = 0.3f;

    [Header("AtirarPadrão2")]
    [SerializeField] private bool podeAtirarPulando = false;
    [SerializeField] private float tempoAtirarPulando = 0.5f;

    [Header("Laser")]
    [SerializeField] private bool podeLaser = false;
    [SerializeField] private GameObject miraLaserPrefab;
    private GameObject mira;
    [SerializeField] private GameObject laserPrefab;
    private GameObject laser;
    [SerializeField] private float duracaoMira = 4f;
    [SerializeField] private float duracaoLaser = 7.5f;


    private void Awake() {
        for (int i = 0; i < qtd; i++) 
        {
            GameObject bala = Instantiate(balaBossPrefab);
            bala.SetActive(false);
            balasBossPool.Add(bala);
        }
        mira = Instantiate(miraLaserPrefab);
        mira.SetActive(false);
        laser = Instantiate(laserPrefab);
        laser.SetActive(false);
    }   
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }
         // ================================================================================ UPDATE ============================================================== 
    private void Update() {

        
        if(target != null) 
        {
            distancia = target.position.x - transform.position.x;
            if(distancia > 0 && !olharDireita) Virar();
            if(distancia < 0 && olharDireita) Virar();
        }

        if(podeSubir)
        {
            rb.velocity = new Vector2( 0,  velocidade);
            if(transform.position.y >= alturaCima) podeSubir = false;
        }
        if(podeDescer)
        {
            rb.velocity = new Vector2( 0,  -velocidade);
            if(transform.position.y <= alturaBaixo) podeDescer = false;
        }

       // if(Input.GetKeyDown(KeyCode.M)) podeMetralhar = true;

        if(CheckPlayer()) StartCoroutine(IniciarBoss());
        if(podeMetralhar) StartCoroutine(AtaqueMetralhar());
        if(podeAtirarPulando) StartCoroutine(AtaquePulando());
        if(podeDash) StartCoroutine(Dash());
        if(podeEscudo) StartCoroutine(Escudo());
        if(podeLaser) StartCoroutine(Laser());
        if(podeVuneravel) StartCoroutine(Vuneravel());
    }
     // ================================================================================ PEGAR BALA ============================================================== 
     private GameObject PegarBala() 
    {
        for (int i = 0; i < balasBossPool.Count; i++) 
        {
            if (!balasBossPool[i].activeInHierarchy) 
            {
                return balasBossPool[i];
            }
        }
        
        return null;
    }
        // ================================================================================ METRALHAR ============================================================== 
     private IEnumerator AtaqueMetralhar() 
     {
        podeMetralhar = false;
        for(int i = 0; i < (balasBossPool.Count - 5); i++)
        {
            GameObject bala = PegarBala();
            if (bala != null) 
            {
                bala.transform.position = posicaoBala.position;
                bala.SetActive(true);
                bala.GetComponent<Bala>().direcao = (int)transform.localScale.x * -1;
            }
            yield return new WaitForSeconds(tempoMetralhar);
        }

        yield return new WaitForSeconds(tempoEntreGolpes);
        podeAtirarPulando = true;
     }
         // ================================================================================ ATACAR PULANDO ============================================================== 
     private IEnumerator AtaquePulando() 
     {
        podeAtirarPulando = false;
        GameObject bala = PegarBala();

        for(int i = 0; i < (balasBossPool.Count - 5); i++)
        {
            if (bala != null) 
            {
                bala.transform.position = posicaoBala.position;
                bala.SetActive(true);
                bala.GetComponent<Bala>().direcao = (int)transform.localScale.x * -1;
            }
            yield return new WaitForSeconds(0.1f);
            SubirEDescer();
            yield return new WaitForSeconds(tempoAtirarPulando);
            
        }
        yield return new WaitForSeconds(tempoEntreGolpes);
        podeDash = true;
     }

      // ================================================================================ DASH ============================================================== 
     private IEnumerator Dash() 
     {
        podeDash = false;
        rb.velocity = new Vector2(  transform.localScale.y * -forcaDash, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(duracaoDash);
        rb.velocity = new Vector2(  0f , 0f);
        tr.emitting = false;
        rb.velocity = new Vector2(  transform.localScale.y * forcaDash, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(duracaoDash);
        rb.velocity = new Vector2(  0f , 0f);
        tr.emitting = false;
        yield return new WaitForSeconds(tempoEntreGolpes);
        podeEscudo = true;
     }
      // ================================================================================ ESCUDO ============================================================== 
     private IEnumerator Escudo() 
     {
        podeEscudo = false;
        multiplicadorDano = 0;
        Debug.Log("ESCUDO");
        yield return new WaitForSeconds(tempoEscudo);
        Debug.Log("cabou o escudo");
        multiplicadorDano = 1;
        yield return new WaitForSeconds(tempoEntreGolpes);
        podeLaser = true;
     }
      // ================================================================================ LASER ============================================================== 
     private IEnumerator Laser() 
     {
        podeLaser = false;
        if (mira != null)
        {
            //mira.transform.localScale = new Vector2(mira.transform.localScale.x * 1.5f, mira.transform.localScale.y);
            mira.transform.position = new Vector2(posicaoBala.position.x - mira.transform.localScale.x/2, posicaoBala.position.y);
            mira.SetActive(true);
        } 
        yield return new WaitForSeconds(duracaoMira);
        mira.SetActive(false);
         
        if (laser != null)
        {
            //laser.transform.localScale = new Vector2(laser.transform.localScale.x * 1.5f, laser.transform.localScale.y * 1.5f);
            laser.transform.position = new Vector2(posicaoBala.position.x - laser.transform.localScale.x/2, posicaoBala.position.y);
            laser.SetActive(true);
        }
        yield return new WaitForSeconds(duracaoLaser);
        laser.SetActive(false);
        yield return new WaitForSeconds(tempoEntreGolpes);
        podeVuneravel = true;
     }
      // ================================================================================ VULNERÁVEL ============================================================== 
     private IEnumerator Vuneravel() 
     {
        podeVuneravel = false;
        multiplicadorDano = 2f;
        Debug.Log("vuneravel");
        yield return new WaitForSeconds(tempoVuneravel);
        Debug.Log("voltou ao normal");
        multiplicadorDano = 1f;
        yield return new WaitForSeconds(tempoEntreGolpes);
        podeMetralhar = true;
     }
        // ================================================================================ INICIAR BOSS ============================================================== 
     private IEnumerator IniciarBoss()
     {
        yield return new WaitForSeconds(tempoInicio);
        multiplicadorDano = 1f;
        podeMetralhar = true;
     }
         // ================================================================================ SUBIR E DESCER ============================================================== 
     public void SubirEDescer()
     {
        if(transform.position.y >= alturaCima) 
        {
            podeDescer = true;
        }
        if(transform.position.y <= alturaBaixo) 
        {
            podeSubir = true;
        }
     }

    // ================================================================================ RECEBER DANO ============================================================== 
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "BalaPlayer") vida -= multiplicadorDano;
        if(vida < 1) gameObject.SetActive(false);
    }

    // ================================================================================ VIRAR ============================================================== 
    private void Virar() 
    {
        olharDireita = !olharDireita;
        Vector3 localScale = transform.localScale;
        localScale.y *= -1f;
        transform.localScale = localScale;
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
}
