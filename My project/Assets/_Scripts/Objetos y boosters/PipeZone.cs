using UnityEngine;

public class PipeZone : MonoBehaviour
{
    [Header("Estado")]
    public bool isActivated = false;

    [Header("Físicas del Tubo")]
    [SerializeField] private float attractionForce = 50f;
    [SerializeField] private float forwardSpeed = 25f; // Impulso constante dentro del tubo
    [SerializeField] private LayerMask pipeLayer;     // Asigna la capa 'Pipe' aquí en el Inspector

    private void OnTriggerStay(Collider other)
    {
        // Si no se ha pisado el activador, no aplica gravedad especial
        if (!isActivated) return;

        if (other.CompareTag("Player"))
        {
            Rigidbody carRb = other.GetComponent<Rigidbody>();

            if (carRb != null)
            {
                RaycastHit hit;

                // Lanzamos el rayo hacia la superficie del tubo usando el LayerMask
                if (Physics.Raycast(other.transform.position, -other.transform.up, out hit, 10f, pipeLayer))
                {
                    // 1. Fuerza de atracción hacia la pared (perpendicular a la normal)
                    Vector3 gravityDirection = -hit.normal;
                    carRb.AddForce(gravityDirection * attractionForce, ForceMode.Acceleration);

                    // 2. Control de velocidad arcade compatible con Unity 2022.3 (usando .velocity)
                    Vector3 targetVelocity = other.transform.forward * forwardSpeed;
                    Vector3 velocityChange = targetVelocity - carRb.velocity;

                    // Mantiene el impulso constante para ignorar los bordes de la malla low-poly
                    carRb.AddForce(velocityChange, ForceMode.VelocityChange);

                    // 3. Alineación del chasis con la curva del tubo
                    Quaternion targetRotation = Quaternion.FromToRotation(other.transform.up, hit.normal) * carRb.rotation;
                    carRb.MoveRotation(Quaternion.Slerp(carRb.rotation, targetRotation, Time.deltaTime * 8f));
                }
            }
        }
    }

    public void ActivatePipe()
    {
        isActivated = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActivated = false;
        }
    }
}