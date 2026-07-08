using TMPro;
using UnityEngine;

public class MaxHeightVisualizer : MonoBehaviour
{
    public static MaxHeightVisualizer Instance;

    [Header("References")]
    [SerializeField] private Transform maxHeightPoint;

    // Empty GameObject placed beside the astronaut
    [SerializeField] private Transform lineAnchor;

    [Header("Prefabs")]
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private TMP_Text heightTextPrefab;

    [Header("Text Offset")]
    [SerializeField] private Vector3 textOffset = new Vector3(0.15f, 0.25f, 0);

    private Vector3 groundPosition;

    private LineRenderer currentLine;
    private TMP_Text currentText;

    private void Awake()
    {
        Instance = this;
    }

    //----------------------------------------------------

    public void SaveGroundPoint()
    {
        groundPosition = maxHeightPoint.position;
    }

    //----------------------------------------------------

    public void Show(float height)
    {
        Clear();

        currentLine = Instantiate(linePrefab);

        currentLine.positionCount = 2;

        // Bottom of line
        Vector3 start = lineAnchor.position;
        start.y = groundPosition.y;

        // Top of line
        Vector3 end = lineAnchor.position;
        end.y = maxHeightPoint.position.y;

        currentLine.SetPosition(0, start);
        currentLine.SetPosition(1, end);

        //------------------------------------------------

        currentText = Instantiate(heightTextPrefab);

        currentText.text =
            $"Max Height\n{height:0.00} m";

        currentText.transform.position =
            end + textOffset;
    }

    //----------------------------------------------------

    public void Clear()
    {
        if (currentLine != null)
            Destroy(currentLine.gameObject);

        if (currentText != null)
            Destroy(currentText.gameObject);
    }
}