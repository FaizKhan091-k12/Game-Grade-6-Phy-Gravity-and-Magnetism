using System.Collections;
using UnityEngine;

public class JumpAnimationController : MonoBehaviour
{
    [Header("Animation Component")]
    [SerializeField] private Animation anim;

    [Header("Animation Clips")]
    [SerializeField] private string idle = "IDLE";
    [SerializeField] private string mercury = "Mercury Jump";
    [SerializeField] private string venus = "Venus";
    [SerializeField] private string earth = "Earth";
    [SerializeField] private string mars = "Mars";
    [SerializeField] private string jupiter = "Jupiter";
    [SerializeField] private string saturn = "Saturn";
    [SerializeField] private string uranus = "Uranus";
    [SerializeField] private string neptune = "Neptune";

    [Header("Animation Speeds")]
    public float mercurySpeed = 1f;
    public float venusSpeed = 1f;
    public float earthSpeed = 1f;
    public float marsSpeed = 1f;
    public float jupiterSpeed = 1f;
    public float saturnSpeed = 1f;
    public float uranusSpeed = 1f;
    public float neptuneSpeed = 1f;

    private bool isJumping;

    void Start()
    {
        anim.Play(idle);
    }

    public void PlayJump(PlanetType planet)
    {
        if (isJumping)
            return;

        StartCoroutine(JumpRoutine(planet));
    }

    IEnumerator JumpRoutine(PlanetType planet)
    {
        isJumping = true;

        string clip = GetClip(planet);
        float speed = GetSpeed(planet);

        anim[clip].speed = speed;

        anim.Play(clip);

        float wait = anim[clip].length / speed;

        yield return new WaitForSeconds(wait);

        anim.Play(idle);

        isJumping = false;
    }

    string GetClip(PlanetType planet)
    {
        switch (planet)
        {
            case PlanetType.Mercury: return mercury;
            case PlanetType.Venus: return venus;
            case PlanetType.Earth: return earth;
            case PlanetType.Mars: return mars;
            case PlanetType.Jupiter: return jupiter;
            case PlanetType.Saturn: return saturn;
            case PlanetType.Uranus: return uranus;
            case PlanetType.Neptune: return neptune;
        }

        return earth;
    }

    float GetSpeed(PlanetType planet)
    {
        switch (planet)
        {
            case PlanetType.Mercury: return mercurySpeed;
            case PlanetType.Venus: return venusSpeed;
            case PlanetType.Earth: return earthSpeed;
            case PlanetType.Mars: return marsSpeed;
            case PlanetType.Jupiter: return jupiterSpeed;
            case PlanetType.Saturn: return saturnSpeed;
            case PlanetType.Uranus: return uranusSpeed;
            case PlanetType.Neptune: return neptuneSpeed;
        }

        return 1f;
    }
}