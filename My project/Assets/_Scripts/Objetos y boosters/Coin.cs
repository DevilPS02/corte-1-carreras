using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Le pedimos el inventario al carro que acaba de entrar
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddCoins(1); // Suma 1 moneda
            }
            Destroy(gameObject);
        }
    }
}
