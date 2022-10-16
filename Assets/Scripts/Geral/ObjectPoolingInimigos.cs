using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingInimigos : MonoBehaviour
{
    public static ObjectPoolingInimigos instance;

    [Header("Bala")]
    [SerializeField] private GameObject balaInimigoPrefab;
    private List<GameObject> balasPool = new List<GameObject>();
     [SerializeField] private int qtd = 30;

    [Header("Laser")]
     [SerializeField] private GameObject miraLaserPrefab;
    private List<GameObject> miraPool = new List<GameObject>();
     [SerializeField] private int qtdMira = 3;
    [SerializeField] private GameObject laserPrefab;
    private List<GameObject> laserPool = new List<GameObject>();
     [SerializeField] private int qtdLaser = 3;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < qtd; i++) 
        {
            GameObject bala = Instantiate(balaInimigoPrefab);
            bala.SetActive(false);
            balasPool.Add(bala);
        }

        for (int i = 0; i < qtdMira; i++)
        {
            GameObject mira = Instantiate(miraLaserPrefab);
            mira.SetActive(false);
            miraPool.Add(mira);
        }
        for (int i = 0; i < qtdLaser; i++)
        {
            GameObject laser = Instantiate(laserPrefab);
            laser.SetActive(false);
            laserPool.Add(laser);
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

    public GameObject PegarMira()
    {
        for (int i = 0; i < miraPool.Count; i++)
        {
            if (!miraPool[i].activeInHierarchy) return miraPool[i];
        }

        return null;
    }
    public GameObject PegarLaser()
    {
        for (int i = 0; i < laserPool.Count; i++)
        {
            if (!laserPool[i].activeInHierarchy) return laserPool[i];
        }

        return null;
    }
}
