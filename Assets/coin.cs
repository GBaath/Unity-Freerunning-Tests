using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    public float rotSpeed;
    public void AddCoins(int amount)
    {
        GameManager.instance.AddCoin(amount);
        Destroy(gameObject);
    }
    private void Update()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + rotSpeed * Time.deltaTime, transform.localEulerAngles.z);
    }
}
