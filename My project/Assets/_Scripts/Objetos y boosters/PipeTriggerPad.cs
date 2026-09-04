using UnityEngine;

public class PipeTriggerPad : MonoBehaviour
{
    // Asignas en el Inspector cuál tubo se desbloquea al pasar por este Pad
    [SerializeField] private PipeZone targetPipe;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetPipe != null)
            {
                targetPipe.ActivatePipe();
                Debug.Log("¡Pad atravesado! Gravedad de tubo activada.");
            }
        }
    }
}