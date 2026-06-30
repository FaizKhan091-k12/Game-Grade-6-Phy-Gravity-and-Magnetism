using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class MainMenuController : MonoBehaviour
{
    public bool isTesting;
    [Header("Sound")]
    [SerializeField] private bool isMute;
    [SerializeField] private Transform soundOn;
    [SerializeField] private Transform soundOff;
    [SerializeField] private TextMeshProUGUI soundText;

    [Header("Mission Intro")]
    [SerializeField] private ProceduralImage black_Screen;
    [SerializeField] private ProceduralImage mission_Screen;
    [SerializeField] private ProceduralImage missionBlack_Strip;
    [SerializeField] private TextMeshProUGUI space_Research;
    [SerializeField] private TextMeshProUGUI mission_Text;
    [SerializeField] private Transform objective;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private Transform continue_Btn;

    [Header("Typing")]
    [TextArea(5, 10)]
    [SerializeField] private string missionObjective =
@"Study how gravity changes
astronaut movement
across different planets.";
    private bool keepCursorBlinking = true;
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Typing Audio")] 
    [SerializeField] AudioSource deepSpaceBG;
    [SerializeField] private AudioSource typingAudioSource;
    [SerializeField] private AudioClip keyboardTypingClip;

    [SerializeField] private float typingSoundInterval = 0.06f;

    private float lastTypingSoundTime;
    private void Start()
    {
        if (isTesting) return;
        // Sound
        soundOff.localScale = Vector3.zero;
        soundOn.localScale = Vector3.zero;

        // Hide Intro UI
        black_Screen.color = new Color(1, 1, 1, 0);
        mission_Screen.color = new Color(1, 1, 1, 0);
        space_Research.color = new Color(0, 1, 0.5552924f, 0);
        mission_Text.color = new Color(1, 1, 1, 0);
        objective.localScale = Vector3.zero;

        objectiveText.text = "";

        SoundController();
        black_Screen.gameObject.SetActive(true);
        continue_Btn.gameObject.SetActive(false);
        missionBlack_Strip.gameObject.SetActive(false);
    }

   

    #region SOUND

    public void SoundController()
    {
        isMute = !isMute;

        if (isMute)
        {
            soundOff.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
            soundOn.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack);
            soundText.text = "Sound OFF";
            deepSpaceBG.volume = 0;
        }
        else
        {
            soundOn.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
            soundOff.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack);
            soundText.text = "Sound ON";
            deepSpaceBG.volume = 1;
        }
    }

    #endregion

    #region MISSION INTRO

    /// <summary>
    /// Call this from Start Mission Button
    /// </summary>
    public void PlayMissionIntro()
    {
        black_Screen.raycastTarget = true;
        objectiveText.text = "";
        objective.localScale = Vector3.zero;
        deepSpaceBG.DOFade(0f, 1.5f);
        

        Sequence seq = DOTween.Sequence();
        
        // Fade black screen
        seq.Append(black_Screen.DOFade(1f, 1.5f).OnComplete((() =>
        {
            missionBlack_Strip.gameObject.SetActive(true);
        })));
      
        // Fade mission UI
        seq.Append(space_Research.DOFade(1f, 1f));
        seq.Append(mission_Text.DOFade(1f, 1f));

        seq.Append(mission_Screen.DOFade(1f, 1f));

        // Pop objective panel
        seq.Append(objective.DOScale(Vector3.one, .5f)
            .SetEase(Ease.OutBack));

        seq.AppendInterval(0.2f);

        // Start typing
        seq.AppendCallback(() =>
        {
            StartCoroutine(TypeMission());
        });
    }

    IEnumerator TypeMission()
    {
        objectiveText.text = "";

        // Initial blinking before typing
        for (int i = 0; i < 4; i++)
        {
            objectiveText.text = "|";
            yield return new WaitForSeconds(0.25f);

            objectiveText.text = "";
            yield return new WaitForSeconds(0.25f);
        }

        // Small delay before typing starts
        yield return new WaitForSeconds(0.6f);

        string[] lines = missionObjective.Split('\n');

        string finishedText = "";

        foreach (string line in lines)
        {
            for (int i = 0; i <= line.Length; i++)
            {
                string visible = line.Substring(0, i);
                if (i > 0)
                {
                    PlayTypingSound(line[i - 1]);
                }
                objectiveText.text =
                    finishedText +
                    visible +
                    "<color=#FFFFFF>|</color>";
                
                float delay = typingSpeed;

                if (i > 0)
                {
                    char c = line[i - 1];

                    if (c == ' ')
                        delay = 0.02f;

                    else if (c == ',')
                        delay = 0.15f;

                    else if (c == '.')
                        delay = 0.35f;
                }

                yield return new WaitForSeconds(delay);
            }

            finishedText += line + "\n";

            // Pause before next line
            yield return new WaitForSeconds(0.45f);
        }

        objectiveText.text = finishedText.TrimEnd();
        continue_Btn.gameObject.SetActive(true);
        continue_Btn
            .DOScale(Vector3.one, 0.45f)
            .SetEase(Ease.OutBack);

        // Keep blinking forever
        keepCursorBlinking = true;
        StartCoroutine(BlinkCursor());
    }
    IEnumerator BlinkCursor()
    {
        while (keepCursorBlinking)
        {
            objectiveText.text = missionObjective + "|";
            yield return new WaitForSeconds(0.45f);

            objectiveText.text = missionObjective;
            yield return new WaitForSeconds(0.45f);
        }
    }
    private void PlayTypingSound(char c)
    {
        if (c == ' ' || c == '\n')
            return;

        if (Time.time - lastTypingSoundTime < typingSoundInterval)
            return;

        lastTypingSoundTime = Time.time;

        typingAudioSource.pitch = Random.Range(0.95f, 1.05f);
        typingAudioSource.PlayOneShot(keyboardTypingClip, 0.35f);
    }
    #endregion
}