using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlanetTravelManager : MonoBehaviour
{
    
    public MainMenuController menuController;
    public static PlanetTravelManager Instance;

    [Header("Containers")]
    [SerializeField] private GameObject solarSystemContainer;
    [SerializeField] private GameObject jumpSceneContainer;

    [Header("Transition")]
    [SerializeField] private Image transitionImage;
    [SerializeField] private float fadeDuration = .6f;

    [Header("Jump Planets")]
    [SerializeField] private JumpPlanet[] jumpPlanets;
    
    [SerializeField] JumpAnimationController jumpController;
    public PlanetType currentPlanet;
    public void OnJumpPressed()
    {
        jumpController.PlayJump(currentPlanet);
    }
    private void Awake()
    {
      if(menuController.isTesting){return;}
        Instance = this;

        transitionImage.color =
            new Color(0,0,0,0);

        transitionImage.gameObject.SetActive(false);

        jumpSceneContainer.SetActive(false);
    }

    //-------------------------------------------------------

    public void Travel()
    {
        PlanetSelectable current =
            PlanetSelectionManager.Instance.CurrentPlanet;

        if(current==null)
            return;

        PlanetType type =
            current.Info.planetType;

        StartTransition(type);
    }

    //-------------------------------------------------------

    void StartTransition(PlanetType type)
    {
        transitionImage.gameObject.SetActive(true);

        transitionImage.DOKill();

        transitionImage.color =
            new Color(0,0,0,0);

        transitionImage
            .DOFade(1,fadeDuration)
            .OnComplete(() =>
            {
                LoadPlanet(type);
            });
    }

    //-------------------------------------------------------

    void LoadPlanet(PlanetType type)
    {
        currentPlanet = type;    // <-- ADD THIS

        solarSystemContainer.SetActive(false);

        jumpSceneContainer.SetActive(true);

        foreach (var p in jumpPlanets)
        {
            bool active = p.planetType == type;

            p.planetObject.SetActive(active);

            if (p.bannerObject != null)
                p.bannerObject.SetActive(active);
        }

        transitionImage
            .DOFade(0, fadeDuration)
            .OnComplete(() =>
            {
                transitionImage.gameObject.SetActive(false);
            });
    }

    //-------------------------------------------------------

    public void BackToSolarSystem()
    {
        transitionImage.gameObject.SetActive(true);

        transitionImage.color =
            new Color(0,0,0,0);

        transitionImage
            .DOFade(1,fadeDuration)
            .OnComplete(() =>
            {
                jumpSceneContainer.SetActive(false);

                solarSystemContainer.SetActive(true);

                transitionImage
                    .DOFade(0,fadeDuration)
                    .OnComplete(() =>
                    {
                        transitionImage.gameObject.SetActive(false);
                    });
            });
    }
}