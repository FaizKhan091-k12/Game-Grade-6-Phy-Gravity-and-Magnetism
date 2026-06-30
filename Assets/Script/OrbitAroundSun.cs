using UnityEngine;

public class OrbitAroundSun : MonoBehaviour
{
    [Header("Orbit")]

    public Transform orbitCenter;

    public Vector3 orbitAxis = Vector3.up;

    public float orbitSpeed = 10f;

    void Update()
    {
        if (orbitCenter == null)
            return;

        transform.RotateAround(
            orbitCenter.position,
            orbitAxis,
            orbitSpeed * Time.deltaTime);
    }
}