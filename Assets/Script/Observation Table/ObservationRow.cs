using TMPro;
using UnityEngine;

public class ObservationRow : MonoBehaviour
{
    [SerializeField] private PlanetType planet;

    [SerializeField] private TMP_Text gravityText;
    [SerializeField] private TMP_Text maxHeightText;
    [SerializeField] private TMP_Text hangTimeText;

    [SerializeField] private GameObject correct;
    [SerializeField] private GameObject wrong;

    public PlanetType Planet => planet;

    public void RefreshRow(PlanetMissionData data)
    {
        if (data.dataCollected)
        {
            gravityText.text = data.gravity;

            maxHeightText.text = data.maxHeight.ToString("0.00");

            hangTimeText.text = data.hangTime.ToString("0.00");

            correct.SetActive(true);
            wrong.SetActive(false);
        }
        else
        {
            gravityText.text = "?";

            maxHeightText.text = "?";

            hangTimeText.text = "?";

            correct.SetActive(false);
            wrong.SetActive(true);
        }
    }
}