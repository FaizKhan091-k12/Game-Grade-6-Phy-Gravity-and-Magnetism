using DG.Tweening;
using UnityEngine;

public class PlanetSelectable : MonoBehaviour
{
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

        cameraController.FocusPlanet(focusPoint, planetDummy);

        RefreshVisual();
    }

    public void Deselect()
    {
        isSelected = false;

        if (planetDummy != null)
        {
            planetDummy.gameObject.SetActive(false);
            planetDummy.localScale = Vector3.zero;
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

    public bool IsSelected => isSelected;
}