using UnityEngine;

public class PlataformaVelocidad : MonoBehaviour
{
    [SerializeField] private float boostAmount = 1000f; // Fuerza extra para el torque
    [SerializeField] private float boostDuration = 2f;  // Duración en segundos

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarMovement car = other.GetComponent<CarMovement>();

            if (car != null)
            {
                car.ApplySpeedBoost(boostAmount, boostDuration);
            }
        }
    }
}