using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;
    [SerializeField] private int vida = 1;
    [SerializeField] private bool puloDuploOn = false;
    [SerializeField] private bool dashOn = false;
    
    private void Awake() {
        if(instance == null) instance = this;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "ArmaduraDash") if(!dashOn) StartCoroutine(HabilitarDash());
        if(other.tag == "ArmaduraPuloDuplo") if(!puloDuploOn) StartCoroutine(HabilitarPuloDuplo());
        if(other.tag == "Inimigo") StartCoroutine(TomarDano());
    }

    private IEnumerator HabilitarDash()
    {
        vida++;
        GanharArmadura();
        dashOn = true;
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator HabilitarPuloDuplo()
    {
        vida++;
        GanharArmadura();
        puloDuploOn = true;
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator TomarDano()
    {
        vida--;
        AnimacaoTomarDano();
        if(vida == 0) Morte();
        yield return new WaitForSeconds(1f);
    }

    public bool DashHabilitado()
    {
        if(dashOn = true) return true;
        else return false;
    }

    public int PuloDuploHabilitado()
    {
        if(puloDuploOn = true) return 2;
        else return 1;
    }

    private void Morte()
    {
        Debug.Log("Morreu");
    }

    private void AnimacaoTomarDano()
    {
        // animacao de tomar dano aqui
    }
    private void GanharArmadura()
    {
        // animacao de ganhar armadura aqui
    }
    
}
