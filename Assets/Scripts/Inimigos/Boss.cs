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
    // private Rigidbody2D rb;

    // [Header("Dash")]
    // private TrailRenderer tr;
    // [SerializeField] private float forcaDash = 24f;
    // [SerializeField] private float duracaoDash = 0.5f;
    // [SerializeField] private bool podeDash = false;

    // [Header("Escudo")]
    // [SerializeField] private bool escudo = false;
    // [SerializeField] private bool podeEscudo = false;

    // [Header("SubirEDescer")]
    // [SerializeField] private float alturaBaixo = 2f;
    // [SerializeField] private float alturaCima = 12f;
    // [SerializeField] private bool podeSubir = false;
    // [SerializeField] private bool podeDescer = false;

    // [Header("ObjectPooling")]
    // [SerializeField] private GameObject balaBossPrefab;
    // private List<GameObject> balasPool = new List<GameObject>();
    // [SerializeField] private int qtd = 30;

    // [Header("AtirarPadrão1")]
    // [SerializeField] private bool podeMetralhar = false;

    // [Header("AtirarPadrão2")]
    // [SerializeField] private bool podeAtirarPulando = false;

    // [Header("Laser")]
    // [SerializeField] private bool podeLaser = false;
    // [SerializeField] private GameObject miraLaserPrefab;
    // [SerializeField] private GameObject laserPrefab;


    // private void Start()
    // {
    //     for (int i = 0; i < qtd; i++) 
    //     {
    //         GameObject bala = Instantiate(balaBossPrefab);
    //         bala.SetActive(false);
    //         balasPool.Add(bala);
    //     }
    //     rb = GetComponent<Rigidbody2D>();
    //     tr = GetComponent<TrailRenderer>();
    // }
}
