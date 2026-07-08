using DG.Tweening;
using UnityEngine;



[System.Serializable]
public class PlanetMissionData
{
    public PlanetType planet;

    public int jumpCount;

    public bool dataCollected;

    public string gravity;
    public bool jumpCompleted;
    public float maxHeight;

    public float hangTime;
}
public class MissionProgressManager : MonoBehaviour
{
    public static MissionProgressManager Instance;

    [SerializeField]
    private PlanetMissionData[] planets;

    [SerializeField] private Transform returnHome;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void RegisterJump(
        PlanetType planet,
        string gravity,
        float maxHeight,
        float hangTime)
    {
        PlanetMissionData data = GetPlanetData(planet);

        if (data == null)
            return;

        data.jumpCount++;

        if (!data.dataCollected)
        {
            data.dataCollected = true;

            data.gravity = gravity;

            data.maxHeight = maxHeight;

            data.hangTime = hangTime;

          //  Debug.Log($"Collected data for {planet}");
        }

        // Refresh the UI
        ObservationTableUI.Instance.Refresh();

      //  Debug.Log($"{planet} Jump Count : {data.jumpCount}");
    }   
    public PlanetMissionData GetPlanetData(PlanetType planet)
    {
        foreach (PlanetMissionData data in planets)
        {
            if (data.planet == planet)
                return data;
        }

        return null;
    }
    
    public void PrintMissionData()
    {
        Debug.Log("========== Mission Data ==========");

        foreach (PlanetMissionData data in planets)
        {
            Debug.Log(
                $"{data.planet} | " +
                $"Jumps : {data.jumpCount} | " +
                $"Collected : {data.dataCollected} | " +
                $"Height : {data.maxHeight} | " +
                $"Hang Time : {data.hangTime}"
            );
        }

       // Debug.Log("==================================");
    }
    public void CompleteJump(PlanetType planet)
    {
      // Debug.Log("CompleteJump Called");

        PlanetMissionData data = GetPlanetData(planet);

        if (data == null)
        {
          //  Debug.LogError("Planet data is NULL");
            return;
        }

        data.jumpCompleted = true;
//
        //Debug.Log($"{planet} Jump Completed");

     //   Debug.Log("Calling CheckMissionComplete...");

        CheckMissionComplete();

   // Debug.Log("Finished CheckMissionComplete");
    }
    private void CheckMissionComplete()
    {
        foreach (PlanetMissionData data in planets)
        {
            if (!data.jumpCompleted)
                return;
        }

        returnHome.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        Debug.Log("MISSION COMPLETE!");

        // Medal
        // Certificate
        // Complete Panel
    }
    
}