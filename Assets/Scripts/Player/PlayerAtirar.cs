using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtirar : MonoBehaviour
{
    [SerializeField] private Transform posicaoTiro;
    [SerializeField] private bool podeAtirar = true;
    [SerializeField] private bool podeEspecial = false;
    public int contadorEspecial = 0;
    [SerializeField] private float coolDown = 1f;

    private void Update() {

        if(/*input.GetButton("Fire1") ||*/ Input.GetKey(KeyCode.K))Atirar();

        if(contadorEspecial == 100)
        {
            podeEspecial = true;
        }
    }

    private void Atirar()
    {
        if(podeAtirar)
        {
            PlayerStatus.instance.playerAnim.SetFloat("Blend", 1f);
            AudioManager.instance.PlaySound(PlayerStatus.instance.atkSound);
            podeAtirar = false;
            GameObject bala = PlayerObjectPooling.instance.PegarBala();

            if (bala != null) 
            {
                bala.transform.position = posicaoTiro.position;
                bala.SetActive(true);
                bala.GetComponent<Bala>().direcao = (int)transform.localScale.x;
            }
            StopCoroutine(CoolDown());
            StartCoroutine(CoolDown());
        }

    }

     private void AtirarEspecial()
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
        yield return new WaitForSeconds(coolDown);
        podeAtirar = true;
        yield return new WaitForSeconds(coolDown);
        PlayerStatus.instance.playerAnim.SetFloat("Blend", 0);
    }
}
