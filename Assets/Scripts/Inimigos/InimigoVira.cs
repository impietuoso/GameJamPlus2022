using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoVira : MonoBehaviour
{
    [SerializeField] private Transform posicaoBala;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float raio = 5f;
    [SerializeField] private bool podeAtirar = false;
    [SerializeField] private bool atirando = false;
    [SerializeField] private float coolDownTiro = 1.5f;
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

        if(CheckPlayer() && !atirando) 
        {
            podeAtirar = true;
        }
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
        GameObject bala = ObjectPoolingInimigos.instance.PegarBala();

        if (bala != null) 
        {
            bala.transform.position = posicaoBala.position;
            bala.SetActive(true);
            bala.GetComponent<Bala>().direcao = (int)transform.localScale.y * -1;
        }

        yield return new WaitForSeconds(coolDownTiro);
        podeAtirar = true;
        atirando = false;
    }

    
    private void Virar() 
    {
        olharDireita = !olharDireita;
        Vector3 localScale = transform.localScale;
        localScale.y *= -1f;
        transform.localScale = localScale;
    }
}
