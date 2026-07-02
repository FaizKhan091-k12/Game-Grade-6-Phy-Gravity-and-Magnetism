using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitPath : MonoBehaviour
{
    [Header("Orbit")]
    [Min(0.1f)]
    public float radius = 5f;

    [Range(32, 256)]
    public int segments = 128;

    [Header("Appearance")]
    public float lineWidth = 0.03f;
    public Color orbitColor = new Color(1f, 1f, 1f, 0.25f);

    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

        lr.useWorldSpace = false;
        lr.loop = true;

        lr.positionCount = segments;

        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        lr.startColor = orbitColor;
        lr.endColor = orbitColor;

        GenerateOrbit();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (lr == null)
            lr = GetComponent<LineRenderer>();

        if (lr == null)
            return;

        lr.useWorldSpace = false;
        lr.loop = true;

        lr.positionCount = segments;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        lr.startColor = orbitColor;
        lr.endColor = orbitColor;

        GenerateOrbit();
    }
#endif

    private void GenerateOrbit()
    {
        if (lr == null)
            return;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;

            Vector3 point = new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius);

            lr.SetPosition(i, point);
        }
    }
}