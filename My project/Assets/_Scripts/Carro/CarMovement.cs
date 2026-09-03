using System.Collections; // Se agrega para poder utilizar las Corrutinas (IEnumerator)
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    // Para almacenar el componente de inputController
    private InputController inputController;

    // Para almacenar el Rigidbody
    private Rigidbody _rb;

    // Variable del Scriptable Object del carro.
    [Header("Values")]
    public CarSo car;

    // Variable para almacenar la velocidad final del carro
    [HideInInspector] public float speed;

    // Variable para controlar si el turbo está activo actualmente
    private bool isBoosting = false;

    // Variable para almacenar el angulo del carro
    private float _steeringAngle;

    // Arreglo para almacenar los colisionadores de llanta
    [Header("Wheels")]
    [SerializeField] private WheelCollider[] _wheelCollider;

    // Arreglo para almacenar las llantas fisicas
    [SerializeField] private Transform[] _wheelTransform;



    // Start is called before the first frame update
    void Start()
    {


        // Se guardan el componente del Rigidbody en la variable
        _rb = GetComponent<Rigidbody>();

        // Cambia el centro de gravedad del carro
        _rb.centerOfMass = new Vector3(0, -0.5f, 0);

        // Guarda la velocidad inicial
        speed = car.speed;


    }

    private void FixedUpdate()
    {
        // Se llaman a los metodos respectivos en Fixed Update, ya que es el indicado para llamar metodos relacionados
        // con fisicas
        Motor();
        Brake();
        Steering();
        UpdateWheels();
    }

    // motorTorque le asigna velocidad a los colliders. Se aplica a cada uno de los 4 y se le multiplica 
    // Por la velocidad
    public void Motor()
    {
        foreach (var wheel in _wheelCollider)
        {
            wheel.motorTorque = InputController.instance.movementVector.y * speed;
        }
    }

    // Para frenar se toma el input del script InputController, el cual valida si el freno se presiona o no
    // Luego si se presiona se le aplica la fuerza de frenado con brakeTorque y si no esta queda en 0
    public void Brake()
    {
        if (InputController.instance.isBraking)
        {
            // Se recorre cada llanta en el grupo de los colisionadores y se le aplica la fuerza de freno
            foreach (var wheel in _wheelCollider)
            {
                wheel.brakeTorque = car.brakeForce;
            }

        }
        else
        {
            // Se recorre cada llanta en el grupo de los colisionadores y la fuerza de freno queda nuevamente en 
            // 
            foreach (var wheel in _wheelCollider)
            {
                wheel.brakeTorque = 0;
            }

        }
    }

    //Método que aplica el giro en las ruedas. Solo se toman las dos delanteras
    public void Steering()
    {
        _steeringAngle = car.angle * InputController.instance.movementVector.x;
        _wheelCollider[2].steerAngle = _steeringAngle;
        _wheelCollider[3].steerAngle = _steeringAngle;
    }

    // Método que permite actualizar las ruedas con el movimiento del collider usando el metido creado abajo.
    public void UpdateWheels()
    {
        for (int i = 0; i < _wheelCollider.Length; i++)
        {
            UpdateSingleWheel(_wheelCollider[i], _wheelTransform[i]);
        }
    }

    // Metodo en donde se toma cada rueda con cada collider y se le asigna a la rueda la posicion y rotacion del collider
    public void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;

        // Se obtiene la posición y rotación del collider.
        wheelCollider.GetWorldPose(out pos, out rot);

        // Se aplican estos datos al transform.
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    // ==========================================
    // SISTEMA DE TURBO (SPEED BOOST)
    // ==========================================

    // Método público que llamará el script de la plataforma (SpeedBoostPlatform)
    public void ApplySpeedBoost(float boostAmount, float boostDuration)
    {
        if (!isBoosting)
        {
            StartCoroutine(BoostRoutine(boostAmount, boostDuration));
        }
    }

    // Corrutina que aumenta la velocidad y luego la devuelve a su valor normal
    private IEnumerator BoostRoutine(float boostAmount, float boostDuration)
    {
        isBoosting = true;

        // Le sumamos el torque extra a la variable speed
        speed += boostAmount;
        Debug.Log("¡TURBO ACTIVADO! Velocidad actual: " + speed);

        // Espera los segundos de duración sin congelar el juego
        yield return new WaitForSeconds(boostDuration);

        // Restaura la velocidad leyendo la base del Scriptable Object
        speed = car.speed;
        isBoosting = false;
        Debug.Log("Turbo finalizado. Velocidad restaurada a: " + speed);
    }
}