using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    [SerializeField] private TextMeshProUGUI text;
    private int _coinCount;
    public int coinCount 
    { 
        get { return _coinCount; } 
        set { _coinCount = value; UpdateCoinUI(); } 
    }
    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void UpdateCoinUI()
    {
        text.text = "Coins x " + coinCount.ToString();
    }
    public void AddCoin(int amount)
    {
        coinCount += amount;
    }
}
