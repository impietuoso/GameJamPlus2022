using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtirar : MonoBehaviour
{
    [SerializeField] private Transform posicaoTiro;
    [SerializeField] private bool podeAtirar = true;
    [SerializeField] private bool podeEspecial = false;
    public int contadorEspecial = 0;

    private void Update() {
        if(contadorEspecial == 100)
        {
            podeEspecial = true;
        }
    }

    public void Atirar()
    {
        if(podeAtirar)
        {
            podeAtirar = false;
            GameObject bala = PlayerObjectPooling.instance.PegarBala();

            if (bala != null) 
            {
                bala.transform.position = posicaoTiro.position;
                bala.SetActive(true);
                bala.GetComponent<Bala>().direcao = (int)transform.localScale.x;
            }
            StartCoroutine(CoolDown());
        }

    }

     public void AtirarEspecial()
    {
        if(podeEspecial)
        {
            podeEspecial = false;
            contadorEspecial = 0;
            GameObject especial = PlayerObjectPooling.instance.PegarEspecial();

            if (especial != null) 
            {
                especial.transform.position = posicaoTiro.position;
                especial.SetActive(true);
                especial.GetComponent<PlayerEspecial>().direcao = (int)transform.localScale.x;
            }
        }

    }

    private IEnumerator CoolDown() 
    {
        yield return new WaitForSeconds(1f);
        podeAtirar = true;
    }
}
