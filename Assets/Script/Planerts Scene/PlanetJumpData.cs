using UnityEngine;

[CreateAssetMenu(menuName="Planet/Jump Data")]
public class PlanetJumpData : ScriptableObject
{
    public PlanetType planet;
    public string gravity;
    public float maxHeight;

    public float hangTime;
}