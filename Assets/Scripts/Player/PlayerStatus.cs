using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Status")]
    public static PlayerStatus instance;
    [SerializeField] private byte vida = 1;
    [SerializeField] private bool puloDuploOn = false;
    [SerializeField] private bool dashOn = false;
    [SerializeField] private bool podeDano = true;
    [SerializeField] private float tempoInvuneral = 2f;

    [Header("Animações")]
    public Animator playerAnim;
    public GameObject GameOverPanel;
    public GameObject TryAgainPanel;
    public AudioClip runSound;
    public AudioClip jumpSound;
    public AudioClip atkSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip dashSound;

    private void Awake() {
        if(instance == null) instance = this;
        playerAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "ArmaduraDash") if(!dashOn) HabilitarDash();
        if(other.tag == "ArmaduraPuloDuplo") if(!puloDuploOn) HabilitarPuloDuplo();
        if(other.tag == "Inimigo" || other.tag == "BalaInimigo") {
            TomarDano();
        }
        if(other.tag == "End") {
             GameManager.instance.ShowCanvasGroup(GameOverPanel.GetComponent<CanvasGroup>());
            Destroy(this.gameObject, 1f);
        }
    }

    public void RunSound() {
        AudioManager.instance.PlaySound(PlayerStatus.instance.runSound);
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
    private void TomarDano() {
        if (podeDano) 
        {
            playerAnim.Play("Damage");
            AudioManager.instance.PlaySound(damageSound);
            vida--;
            StartCoroutine(Invulnerabilidade());
            if(dashOn && !puloDuploOn) dashOn = false;
            if(!dashOn && puloDuploOn) puloDuploOn = false;
            if(dashOn && puloDuploOn) puloDuploOn = false;
            AnimacaoTomarDano();
            if(vida == 0) Morte();
        }
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
        yield return new WaitForSeconds(tempoInvuneral);
        podeDano = true;
    }

    private void Morte()
    {
        playerAnim.Play("Death");
        AudioManager.instance.PlaySound(deathSound);
        GameManager.instance.ShowCanvasGroup(TryAgainPanel.GetComponent<CanvasGroup>());
        Destroy(this.gameObject, 1.5f);
        //Desabilitar o player
        //tela de gameOver com Retry
    }

    private void AnimacaoTomarDano() {
        playerAnim.Play("Damage");
    }
    private void AnimacaoGanharArmadura()
    {
        // animacao de ganhar armadura aqui
    }
    
}
