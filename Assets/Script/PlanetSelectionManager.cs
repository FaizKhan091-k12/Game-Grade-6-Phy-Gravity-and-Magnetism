using UnityEngine;

public class PlanetSelectionManager : MonoBehaviour
{
    public static PlanetSelectionManager Instance;

    private PlanetSelectable currentPlanet;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SelectPlanet(PlanetSelectable planet)
    {
        if (planet == currentPlanet)
            return;

        // Turn off previous planet completely
        if (currentPlanet != null)
            currentPlanet.Deselect();

        currentPlanet = planet;

        currentPlanet.Select();
    }

    public void ClearSelection()
    {
        if (currentPlanet == null)
            return;

        currentPlanet.Deselect();
        currentPlanet = null;
    }

    public PlanetSelectable CurrentPlanet => currentPlanet;
}