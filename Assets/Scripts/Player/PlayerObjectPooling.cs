using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectPooling : MonoBehaviour
{
    public static PlayerObjectPooling instance;

    [Header("Bala")]
    [SerializeField] private GameObject balaPrefab;
    private List<GameObject> balasPool = new List<GameObject>();
    private int qtd = 20;

    [Header("Especial")]
    [SerializeField] private GameObject EspecialPrefab;
    private List<GameObject> EspecialPool = new List<GameObject>();
    private int qtdEspecial = 1;
   

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < qtd; i++) 
        {
            GameObject bala = Instantiate(balaPrefab);
            bala.SetActive(false);
            balasPool.Add(bala);
        }

        for (int i = 0; i < qtdEspecial; i++)
        {
            GameObject laser = Instantiate(EspecialPrefab);
            laser.SetActive(false);
            EspecialPool.Add(laser);
        }
        
    }

    public GameObject PegarBala() 
    {
        for (int i = 0; i < balasPool.Count; i++) 
        {
            if (!balasPool[i].activeInHierarchy) return balasPool[i];
        }

        return null;
    }

    public GameObject PegarEspecial()
    {
        for (int i = 0; i < EspecialPool.Count; i++)
        {
            if (!EspecialPool[i].activeInHierarchy) return EspecialPool[i];
        }

        return null;
    }
   
}
