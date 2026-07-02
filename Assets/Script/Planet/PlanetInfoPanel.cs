using TMPro;
using UnityEngine;
using DG.Tweening;
public class PlanetInfoPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private RectTransform panel;
    [Header("Texts")]
    [SerializeField] private TMP_Text planetName;
    [SerializeField] private TMP_Text subTitle;
    [SerializeField] private TMP_Text description;

    [SerializeField] private TMP_Text distance;
    [SerializeField] private TMP_Text gravity;
    [SerializeField] private TMP_Text moons;
    [SerializeField] private TMP_Text averageTemperature;
    [SerializeField] private TMP_Text lengthOfDay;
    [SerializeField] private TMP_Text orbitalPeriod;
    [SerializeField] private TMP_Text funFact;
    bool visible;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void SetData(PlanetInfoData data)
    {
        planetName.text = data.planetName;
        subTitle.text = data.subTitle;
        description.text = data.description;

        distance.text = data.distanceFromSun;
        gravity.text = data.gravity;
        moons.text = data.moons;
        averageTemperature.text = data.averageTemperature;
        lengthOfDay.text = data.lengthOfDay;
        orbitalPeriod.text = data.orbitalPeriod;
        funFact.text = data.funFact;
    }
    public void Hide()
    {
        RectTransform panel = GetComponent<RectTransform>();

        panel.DOKill();

        panel
            .DOLocalRotate(new Vector3(0, -90, 0), .25f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}