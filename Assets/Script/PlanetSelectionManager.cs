using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSelectionManager : MonoBehaviour
{
    public static PlanetSelectionManager Instance;

    [Header("Planet Info")]
    [SerializeField] private PlanetInfoPanel planetInfoPanel;

    [Header("Planet Order")]
    [SerializeField] private PlanetSelectable[] planets;

    [Header("Navigation UI")]
    [SerializeField] private RectTransform previousButton;
    [SerializeField] private RectTransform nextButton;
    [SerializeField] private RectTransform backButton;

    private Button previousBtn;
    private Button nextBtn;
    private Button backBtn;

    private PlanetSelectable currentPlanet;
    private int currentIndex = -1;

    private bool uiVisible;

    //-------------------------------------------------------

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        previousBtn = previousButton.GetComponent<Button>();
        nextBtn = nextButton.GetComponent<Button>();
        backBtn = backButton.GetComponent<Button>();

        previousBtn.onClick.AddListener(PreviousPlanet);
        nextBtn.onClick.AddListener(NextPlanet);
        backBtn.onClick.AddListener(BackToSolarSystem);

        HideUIInstant();

        // if (planetInfoPanel != null)
        //     planetInfoPanel.gameObject.SetActive(false);
    }

    //-------------------------------------------------------
    // Called when clicking a planet
    //-------------------------------------------------------

    public void SelectPlanet(PlanetSelectable planet)
    {
        if (planet == currentPlanet)
            return;

        // Hide current panel immediately
        if (planetInfoPanel != null)
        {
            planetInfoPanel.transform.DOKill();
            planetInfoPanel.gameObject.SetActive(false);
        }

        HideUI();

        if (currentPlanet != null)
            currentPlanet.Deselect();

        currentPlanet = planet;
        currentIndex = System.Array.IndexOf(planets, planet);

        currentPlanet.Select();

        UpdateButtons();
    }

    //-------------------------------------------------------
    // Called by PlanetSelectable AFTER animation finishes
    //-------------------------------------------------------

    public void OnPlanetShown()
    {
        RectTransform panel = planetInfoPanel.GetComponent<RectTransform>();

        // Stop any previous animations
        panel.DOKill();

        // Enable panel first
        planetInfoPanel.Show();
        if (currentPlanet == null || planetInfoPanel == null)
            return;

        if (currentPlanet.Info == null)
        {
            Debug.LogError(currentPlanet.name + " has no PlanetInfoData assigned.");
            return;
        }

      

        // Fill all texts BEFORE animation
        planetInfoPanel.SetData(currentPlanet.Info);

        // Reset transform
        panel.localScale = Vector3.one;
        panel.localRotation = Quaternion.Euler(0f, -90f, 0f);

        Sequence seq = DOTween.Sequence();

        seq.Append(
            panel.DOLocalRotate(Vector3.zero, 0.45f)
                .SetEase(Ease.OutBack)
        );

        seq.OnComplete(() =>
        {
            ShowUI();
        });
    }
  
    //-------------------------------------------------------
    // Previous
    //-------------------------------------------------------

    void PreviousPlanet()
    {
        if (currentIndex <= 0)
            return;

        SwitchPlanet(currentIndex - 1);
    }

    //-------------------------------------------------------
    // Next
    //-------------------------------------------------------

    void NextPlanet()
    {
        if (currentIndex >= planets.Length - 1)
            return;

        SwitchPlanet(currentIndex + 1);
    }

    //-------------------------------------------------------

    void SwitchPlanet(int index)
    {
        if (index < 0 || index >= planets.Length)
            return;

        if (currentPlanet != null)
            currentPlanet.Deselect();

        currentPlanet = planets[index];
        currentIndex = index;

        // Do NOT hide/show UI again
        // PlanetSelectable will call OnPlanetShown()
        currentPlanet.ShowPlanetOnly();

        UpdateButtons();
    }

    //-------------------------------------------------------
    // Back
    //-------------------------------------------------------

    void BackToSolarSystem()
    {
        if (currentPlanet == null)
            return;

        currentPlanet.Deselect();

        currentPlanet = null;
        currentIndex = -1;

        if (planetInfoPanel != null)
        {
            RectTransform panel = planetInfoPanel.GetComponent<RectTransform>();

            panel.DOKill();

            panel
                .DOLocalRotate(new Vector3(0f, -90f, 0f), 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    planetInfoPanel.Hide();
                });
        }

        HideUI();
    }

    //-------------------------------------------------------
    // Buttons
    //-------------------------------------------------------

    void UpdateButtons()
    {
        previousBtn.interactable = currentIndex > 0;
        nextBtn.interactable = currentIndex < planets.Length - 1;
    }

    //-------------------------------------------------------
    // UI
    //-------------------------------------------------------

    void ShowUI()
    {
        if (uiVisible)
            return;

        uiVisible = true;

        previousButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);

        previousButton.localScale = Vector3.zero;
        nextButton.localScale = Vector3.zero;
        backButton.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();

        seq.Append(previousButton.DOScale(1f, 0.30f).SetEase(Ease.OutBack));

        seq.Join(
            nextButton.DOScale(1f, 0.30f)
                .SetEase(Ease.OutBack)
                .SetDelay(0.05f));

        seq.Join(
            backButton.DOScale(1f, 0.30f)
                .SetEase(Ease.OutBack)
                .SetDelay(0.10f));
    }

    //-------------------------------------------------------

    void HideUI()
    {
        if (!uiVisible)
            return;

        uiVisible = false;

        Sequence seq = DOTween.Sequence();

        seq.Append(previousButton.DOScale(0f, 0.20f));
        seq.Join(nextButton.DOScale(0f, 0.20f));
        seq.Join(backButton.DOScale(0f, 0.20f));

        seq.OnComplete(() =>
        {
            previousButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
        });
    }

    //-------------------------------------------------------

    void HideUIInstant()
    {
        uiVisible = false;

        previousButton.localScale = Vector3.zero;
        nextButton.localScale = Vector3.zero;
        backButton.localScale = Vector3.zero;

        previousButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    //-------------------------------------------------------

    public PlanetSelectable CurrentPlanet => currentPlanet;
}