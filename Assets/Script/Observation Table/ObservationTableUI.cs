using UnityEngine;

public class ObservationTableUI : MonoBehaviour
{
    public static ObservationTableUI Instance;

    [SerializeField] private ObservationRow[] rows;

    private void Awake()
    {
        Instance = this;
    }

    public void Refresh()
    {
        foreach (ObservationRow row in rows)
        {
            PlanetMissionData data =
                MissionProgressManager.Instance.GetPlanetData(row.Planet);

            row.RefreshRow(data);
        }
    }
}