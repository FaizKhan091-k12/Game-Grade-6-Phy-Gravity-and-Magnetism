using UnityEngine;

public class OrbitPath : MonoBehaviour
{
    public Material lineMaterial;

    [Min(0.1f)]
    public float radius = 5f;

    [Range(16,256)]
    public int segments = 128;

    public Color orbitColor = new Color(1,1,1,0.25f);

    private void OnRenderObject()
    {
        if (lineMaterial == null)
            return;

        lineMaterial.SetPass(0);

        GL.PushMatrix();

        GL.MultMatrix(transform.localToWorldMatrix);

        GL.Begin(GL.LINE_STRIP);
        GL.Color(orbitColor);

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            GL.Vertex3(x, 0, z);
        }

        GL.End();

        GL.PopMatrix();
    }
}