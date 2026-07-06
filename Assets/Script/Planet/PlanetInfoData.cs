using UnityEngine;

[System.Serializable]
public class PlanetInfoData
{
   
    public PlanetType planetType;
    public string planetName;
    public string subTitle;

    [TextArea(2,5)]
    public string description;

    public string distanceFromSun;
    public string gravity;
    public string moons;
    public string averageTemperature;
    public string lengthOfDay;
    public string orbitalPeriod;

    [TextArea(2,5)]
    public string funFact;
}