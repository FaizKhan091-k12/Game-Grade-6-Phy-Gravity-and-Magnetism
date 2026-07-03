using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
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
    [SerializeField] private Button travelButton;

    void PlayAnimation()
    {
        float t = 0f;

        AnimateItem(
            planetName.GetComponent<RectTransform>(),
            t += .05f);

        AnimateItem(
            subTitle.GetComponent<RectTransform>(),
            t += .05f);

        AnimateItem(
            description.GetComponent<RectTransform>(),
            t += .08f);

        AnimateItem(
            distance.transform.parent.GetComponent<RectTransform>(),
            t += .06f);

        AnimateItem(
            gravity.transform.parent.GetComponent<RectTransform>(),
            t += .05f);

        AnimateItem(
            moons.transform.parent.GetComponent<RectTransform>(),
            t += .05f);

        AnimateItem(
            averageTemperature.transform.parent.GetComponent<RectTransform>(),
            t += .05f);

        AnimateItem(
            lengthOfDay.transform.parent.GetComponent<RectTransform>(),
            t += .05f);

        AnimateItem(
            orbitalPeriod.transform.parent.GetComponent<RectTransform>(),
            t += .05f);

        AnimateItem(
            funFact.transform.parent.GetComponent<RectTransform>(),
            t += .08f);

        AnimateItem(
            travelButton.GetComponent<RectTransform>(),
            t += .10f);
    }
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
        PlayAnimation();
    }
    public void Hide()
    {
        RectTransform panel = GetComponent<RectTransform>();

        // Kill any previous tweens on this object
        DOTween.Kill(panel);

        panel.DOLocalRotate(new Vector3(0, -90, 0), 0.25f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                panel.localRotation = Quaternion.Euler(0, -90, 0);
                gameObject.SetActive(false);
            });
    }
    void AnimateItem(RectTransform rect, float delay)
    {
        CanvasGroup cg = rect.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = rect.gameObject.AddComponent<CanvasGroup>();

        rect.localScale = Vector3.one * .8f;
        cg.alpha = 0;

        Sequence s = DOTween.Sequence();

        s.AppendInterval(delay);

        s.Append(rect.DOScale(1f, .25f).SetEase(Ease.OutBack));
        s.Join(cg.DOFade(1f, .25f));
    }
    public void Show()
    {
        RectTransform panel = GetComponent<RectTransform>();

        DOTween.Kill(panel);

        gameObject.SetActive(true);
    }
}