using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;
    [SerializeField] private byte vida = 1;
    [SerializeField] private bool puloDuploOn = false;
    [SerializeField] private bool dashOn = false;
    [SerializeField] private bool podeDano = true;
    
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
        dashOn = true;
        vida ++;
        AnimacaoGanharArmadura();
    }
    private void HabilitarPuloDuplo()
    {
        puloDuploOn = true;
        vida ++;
        AnimacaoGanharArmadura();
    }
    private void TomarDano()
    {
        if(podeDano) vida--;
        StartCoroutine(Invulnerabilidade());
        if(dashOn && !puloDuploOn) dashOn = false;
        if(!dashOn && puloDuploOn) puloDuploOn = false;
        if(dashOn && puloDuploOn) puloDuploOn = false;
        AnimacaoTomarDano();
        if(vida == 0) Morte();
    }

    public bool DashHabilitado()
    {
        if(dashOn == true) return true;
        else return false;
    }

    public int PuloDuploHabilitado()
    {
        if(puloDuploOn == true) return 2;
        else return 1;
    }

    private IEnumerator Invulnerabilidade()
    {
        podeDano = false;
        yield return new WaitForSeconds(1.5f);
        podeDano = true;
    }

    private void Morte()
    {
        Debug.Log("Morreu");
        // Desabilitar o player
        // tela de gameOver com Retry
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
