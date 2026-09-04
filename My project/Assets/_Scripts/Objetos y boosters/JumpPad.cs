using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Configuración del Salto")]
    [SerializeField] private float upwardForceMultiplier = 1.5f;   // Fuerza hacia arriba
    [SerializeField] private float forwardForceMultiplier = 1.2f;  // Fuerza hacia adelante

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody carRb = other.GetComponent<Rigidbody>();
            CarMovement car = other.GetComponent<CarMovement>();

            if (carRb != null && car != null)
            {
                // 1. Calculamos la velocidad actual a la que venía rodando
                float currentSpeed = carRb.velocity.magnitude;

                if (currentSpeed < 5f)
                {
                    currentSpeed = car.speed;
                }

                // 2. Calculamos las magnitudes de fuerza según la velocidad de entrada
                float upForce = currentSpeed * upwardForceMultiplier;
                float forwardForce = currentSpeed * forwardForceMultiplier;

                // 3. OBLIGAMOS a la física a cambiar su velocidad vertical de una
                // Esto despega los WheelColliders del suelo inmediatamente sin importar la suspensión
                Vector3 currentVel = carRb.velocity;
                carRb.velocity = new Vector3(currentVel.x, upForce, currentVel.z);

                // 4. Le inyectamos la fuerza hacia adelante para la parábola del salto
                carRb.AddForce(other.transform.forward * forwardForce, ForceMode.Impulse);

                Debug.Log("¡DESPEGUE EXITOSO! Fuerza vertical aplicada: " + upForce);
            }
        }
    }
}