using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoLaser : MonoBehaviour
{
    [SerializeField] private Transform posicaoLaser;

    [SerializeField] private bool podeAtirar = true;
    [SerializeField] private bool recarregando = false;
    [SerializeField] private float duracaoMira = 1f;
    [SerializeField] private float duracaoLaser = 5f;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float raio = 5f;
    [SerializeField] private bool olharDireita;
    [SerializeField] private Transform target;
    [SerializeField] private float distancia;

    private void Update()
    {
        if(target != null) 
        {
            distancia = target.position.x - transform.position.x;
            if(distancia > 0 && !olharDireita) Virar();
            if(distancia < 0 && olharDireita) Virar();
        }

        if(CheckPlayer()) if (!recarregando) if (podeAtirar) StartCoroutine(AtirarMira());
    }

    private IEnumerator AtirarMira()
    {
        podeAtirar = false;
        GameObject mira = ObjectPoolingInimigos.instance.PegarMira();

        if (mira != null)
        {
            mira.transform.position = new Vector2(posicaoLaser.position.x - mira.transform.localScale.x/2, posicaoLaser.position.y);
            mira.SetActive(true);
        }
        
        yield return new WaitForSeconds(duracaoMira);
        mira.SetActive(false);
        StartCoroutine(AtirarLaser());
        
    }
    private IEnumerator AtirarLaser() 
    {
        GameObject laser = ObjectPoolingInimigos.instance.PegarLaser();

        if (laser != null)
        {
            laser.transform.position = new Vector2(posicaoLaser.position.x - laser.transform.localScale.x/2, posicaoLaser.position.y);
            laser.SetActive(true);
        }

        yield return new WaitForSeconds(duracaoLaser);
        laser.SetActive(false);
        StartCoroutine(Recarregar());
    }
    private IEnumerator Recarregar()
    {
        recarregando = true;
        yield return new WaitForSeconds(5f);
        recarregando = false;
        podeAtirar = true;
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

    private void Virar() 
    {
        olharDireita = !olharDireita;
        Vector3 localScale = transform.localScale;
        localScale.y *= -1f;
        transform.localScale = localScale;
    }
}
