using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;
    [SerializeField] private byte vida = 1;
    [SerializeField] private bool puloDuploOn = false;
    [SerializeField] private bool dashOn = false;
    
    private void Awake() {
        if(instance == null) instance = this;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "ArmaduraDash") if(!dashOn) HabilitarDash();
        if(other.tag == "ArmaduraPuloDuplo") if(!puloDuploOn) HabilitarPuloDuplo();
        if(other.tag == "Inimigo") TomarDano();
    }


    private void HabilitarDash()
    {
        vida ++;
        AnimacaoGanharArmadura();
        dashOn = true;
    }
    private void HabilitarPuloDuplo()
    {
        vida ++;
        AnimacaoGanharArmadura();
        puloDuploOn = true;
    }
    private void TomarDano()
    {
        vida--;
        if(dashOn && !puloDuploOn) dashOn = false;
        if(!dashOn && puloDuploOn) puloDuploOn = false;
        if(dashOn && puloDuploOn) puloDuploOn = false;
        AnimacaoTomarDano();
        if(vida == 0) Morte();
    }

    public bool DashHabilitado()
    {
        StartCoroutine(CoolDown());
        if(dashOn == true) return true;
        else return false;
    }

    public int PuloDuploHabilitado()
    {
        StartCoroutine(CoolDown());
        if(puloDuploOn == true) return 2;
        else return 1;
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private void Morte()
    {
        Debug.Log("Morreu");
    }

    private void AnimacaoTomarDano()
    {
        // animacao de tomar dano aqui
    }
    private void AnimacaoGanharArmadura()
    {
        // animacao de ganhar armadura aqui
    }
    
}
