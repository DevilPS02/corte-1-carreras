using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int TotalCoins { get; private set; }

    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        Debug.Log("Monedas en el inventario: " + TotalCoins);
    }
}