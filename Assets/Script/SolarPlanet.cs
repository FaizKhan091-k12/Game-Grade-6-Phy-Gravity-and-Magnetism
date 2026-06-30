using UnityEngine;

public class SolarPlanet : MonoBehaviour
{
    [Header("Self Rotation")]
    public Vector3 rotationAxis = Vector3.up;

    public float selfRotationSpeed = 20f;

    void Update()
    {
        transform.Rotate(rotationAxis, selfRotationSpeed * Time.deltaTime, Space.Self);
    }
}