using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    public void AddCoins(int amount)
    {
        GameManager.instance.AddCoin(amount);
        Destroy(gameObject);
    }
}
