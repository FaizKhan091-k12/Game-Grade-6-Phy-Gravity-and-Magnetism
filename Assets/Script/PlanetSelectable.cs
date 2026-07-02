using DG.Tweening;
using UnityEngine;

public class PlanetSelectable : MonoBehaviour
{
    [Header("Planet Info")]
    [SerializeField] private PlanetInfoData info;
    public PlanetInfoData Info => info;
    [Header("References")]
    [SerializeField] private GameObject outline;
    [SerializeField] private Transform focusPoint;
    [SerializeField] private Transform planetDummy;
    [SerializeField] private SolarSystemCameraController cameraController;

    private Transform planet;
    private Vector3 originalScale;

    private bool isHovered;
    private bool isSelected;

    private void Awake()
    {
        planet = transform.parent;
        originalScale = planet.localScale;

        outline.SetActive(false);

        if (planetDummy != null)
        {
            planetDummy.gameObject.SetActive(false);
            planetDummy.localScale = Vector3.zero;
        }
    }

    private void OnMouseEnter()
    {
        isHovered = true;
        RefreshVisual();
    }

    private void OnMouseExit()
    {
        isHovered = false;
        RefreshVisual();
    }

    private void OnMouseDown()
    {
        PlanetSelectionManager.Instance.SelectPlanet(this);
    }

    public void Select()
    {
        isSelected = true;

        if (planetDummy != null)
        {
            planetDummy.gameObject.SetActive(true);
            planetDummy.localScale = Vector3.zero;
        }

        cameraController.FocusPlanet(
            focusPoint,
            planetDummy,
            () =>
            {
                PlanetSelectionManager.Instance.OnPlanetShown();
            });

        RefreshVisual();
    }

    public void Deselect()
    {
        isSelected = false;

        if (planetDummy != null)
        {
            planetDummy.DOKill();

            planetDummy
                .DOScale(Vector3.zero, 0.35f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    planetDummy.gameObject.SetActive(false);
                });
        }

        RefreshVisual();
    }

    void RefreshVisual()
    {
        // Outline only while hovering
        outline.SetActive(isHovered);

        Vector3 targetScale =
            isHovered ? originalScale * 1.08f : originalScale;

        planet.DOKill();

        planet.DOScale(targetScale, 0.2f)
            .SetEase(Ease.OutQuad);
    }
    public void ShowPlanetOnly()
    {
        isSelected = true;

        if (planetDummy == null)
            return;

        planetDummy.gameObject.SetActive(true);
        planetDummy.localScale = Vector3.zero;

        planetDummy.DOKill();

        planetDummy
            .DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                PlanetSelectionManager.Instance.OnPlanetShown();
            });

        RefreshVisual();
    }
    public bool IsSelected => isSelected;
}