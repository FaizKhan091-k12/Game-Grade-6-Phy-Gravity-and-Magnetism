using UnityEngine;

public class PlanetSelectable : MonoBehaviour
{
    [Header("Planet")]
    [SerializeField] private GameObject outline;
    [SerializeField] private Transform focusPoint;
    [SerializeField] private Transform earthDummy;
    [Header("Camera")]
    [SerializeField] private SolarSystemCameraController cameraController;

    private void Start()
    {
        outline.SetActive(false);
    }

    private void OnMouseEnter()
    {
        outline.SetActive(true);
    }

    private void OnMouseExit()
    {
        outline.SetActive(false);
    }

    private void OnMouseDown()
    {
        outline.SetActive(true);

        cameraController.FocusPlanet(
            focusPoint,
            earthDummy
        );
    }
}