using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class SensorManager : MonoBehaviour
{
    public GameObject objeto;
    public float moveSpeed = 30f;
    public float rotationSpeed = 100f;
   
    private void OnEnable()
    {
        EnableSensors();
    }

    private void EnableSensors()
    {
        if (Accelerometer.current != null && !Accelerometer.current.enabled)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }

        if (Gyroscope.current != null && !Gyroscope.current.enabled)
        {
            InputSystem.EnableDevice(Gyroscope.current);
        }
    }

    private void Update()
    {
        if (Accelerometer.current != null)
        {
            if (!Accelerometer.current.enabled)
            {
                InputSystem.EnableDevice(Accelerometer.current);
            }

            Vector3 accel = Accelerometer.current.acceleration.ReadValue();
            Debug.Log($"Accel: {accel}");

            // Desplazamiento lateral (Eje X) usando el acelerómetro
            if (objeto != null)
            {
                Vector3 movement = new Vector3(accel.x, 0, 0) * moveSpeed * Time.deltaTime;
                objeto.transform.Translate(movement, Space.World);
            }
        }
        else 
        {
            Debug.LogWarning("Acelerometro nulo");
        }

        if (Gyroscope.current != null)
        {
            if (!Gyroscope.current.enabled)
            {
                InputSystem.EnableDevice(Gyroscope.current);
            }

            Vector3 gyro = Gyroscope.current.angularVelocity.ReadValue();
            Debug.Log($"Gyro: {gyro}");

            // Rotación angular usando la velocidad angular del giroscopio
            if (objeto != null)
            {
                Vector3 rotation = new Vector3(-gyro.x, -gyro.y, gyro.z) * rotationSpeed * Time.deltaTime;
                objeto.transform.Rotate(rotation, Space.Self);
            }
        }
        else
        {
            Debug.LogWarning("Giroscopio nulo");
        }
    }
}
