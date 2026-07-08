using System.Collections;
using TMPro;
using UnityEngine;

public class MaxHeightUI : MonoBehaviour
{
    public static MaxHeightUI Instance;

    [SerializeField] private TMP_Text maxHeightText;

    [SerializeField] private float countDuration = 1f;

    Coroutine routine;

    private void Awake()
    {
        Instance = this;

        maxHeightText.text = "";
    }

    //----------------------------------------------------

    public void ShowHeight(float targetHeight)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(AnimateHeight(targetHeight));
    }

    //----------------------------------------------------

    IEnumerator AnimateHeight(float target)
    {
        float timer = 0;

        while (timer < countDuration)
        {
            timer += Time.deltaTime;

            float value = Mathf.Lerp(
                0,
                target,
                timer / countDuration);

            maxHeightText.text =
                $"MAX HEIGHT\n{value:0.00} m";

            yield return null;
        }

        maxHeightText.text =
            $"MAX HEIGHT\n{target:0.00} m";
    }

    //----------------------------------------------------

    public void ResetHeight()
    {
        if (routine != null)
            StopCoroutine(routine);

        maxHeightText.text = "";
    }
}