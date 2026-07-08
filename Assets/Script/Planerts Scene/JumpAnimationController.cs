using System.Collections;
using UnityEngine;

public class JumpAnimationController : MonoBehaviour
{
    [System.Serializable]
    public class SpeedProfile
    {
        [Header("Phase End Times (0-1)")]

        [Range(0f, 1f)]
        public float phase1End = 0.5f;

        [Range(0f, 1f)]
        public float phase2End = 0.83f;

        [Header("Phase Speeds")]

        public float phase1Speed = 1f;
        public float phase2Speed = 1f;
        public float phase3Speed = 1f;
    }

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

    [Header("Mercury")]
    public SpeedProfile mercurySpeed = new SpeedProfile();

    [Header("Venus")]
    public SpeedProfile venusSpeed = new SpeedProfile();

    [Header("Earth")]
    public SpeedProfile earthSpeed = new SpeedProfile();

    [Header("Mars")]
    public SpeedProfile marsSpeed = new SpeedProfile();

    [Header("Jupiter")]
    public SpeedProfile jupiterSpeed = new SpeedProfile();

    [Header("Saturn")]
    public SpeedProfile saturnSpeed = new SpeedProfile();

    [Header("Uranus")]
    public SpeedProfile uranusSpeed = new SpeedProfile();

    [Header("Neptune")]
    public SpeedProfile neptuneSpeed = new SpeedProfile();

    private bool isJumping;
    [SerializeField]
    private PlanetJumpData[] jumpDatas;
    private PlanetType currentPlanet;
    PlanetJumpData GetData(PlanetType planet)
    {
        foreach(var d in jumpDatas)
        {
            if(d.planet == planet)
                return d;
        }

        return null;
    }
    private void Start()
    {
        anim.Play(idle);
    }

    public void PlayJump(PlanetType planet)
    {
        if (isJumping)
            return;

        currentPlanet = planet;
        StartCoroutine(JumpRoutine(planet));
    }
    public void ShowHeightMarker()
    {
        PlanetJumpData data = GetData(currentPlanet);

        MaxHeightUI.Instance.ShowHeight(data.maxHeight);
    }
    IEnumerator JumpRoutine(PlanetType planet)
    {
        isJumping = true;
     
        PlanetJumpData data = GetData(planet);
        if (data == null)
        {
           // Debug.LogError("PlanetJumpData is NULL for " + planet);
            yield break;
        }
        //MaxHeightVisualizer.Instance.SaveGroundPoint();
        MissionProgressManager.Instance.RegisterJump(
            planet,
            data.gravity,
            data.maxHeight,
            data.hangTime);
        string clip = GetClip(planet);

        
        anim.Play(clip);

       // Debug.Log("Clip = " + clip);

        AnimationState state = anim[clip];

        if (state == null)
        {
           // Debug.LogError("Animation State NOT FOUND : " + clip);
            yield break;
        }

        SpeedProfile profile = GetProfile(planet);
        if (profile == null)
        {
            Debug.LogError("Speed Profile missing for " + planet);
            yield break;
        }
        while (state.enabled && state.normalizedTime < 1f)
        {
            float t = state.normalizedTime;

            if (t < profile.phase1End)
            {
                state.speed = profile.phase1Speed;
            }
            else if (t < profile.phase2End)
            {
                state.speed = profile.phase2Speed;
            }
            else
            {
                state.speed = profile.phase3Speed;
            }

            yield return null;
        }

        anim.Play(idle);
        MissionProgressManager.Instance.CompleteJump(currentPlanet);
        isJumping = false;
    }

    private string GetClip(PlanetType planet)
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

    private SpeedProfile GetProfile(PlanetType planet)
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

        return earthSpeed;
    }
}