using DG.Tweening;
using UnityEngine;

public class SolarSystemCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform yawPivot;
    [SerializeField] private Transform pitchPivot;
    [SerializeField] private Camera cam;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float smooth = 10f;

    [Header("Pitch Limits")]
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float zoomSmooth = 10f;
    [SerializeField] private float minZoom = 8f;
    [SerializeField] private float maxZoom = 35f;

    [Header("State")]
    public bool canControl = true;

    private float yaw;
    private float pitch;

    private float targetZoom;
    private float currentZoom;

    private Quaternion targetYawRotation;
    private Quaternion targetPitchRotation;

    void Start()
    {
        SyncState();
    }

    void Update()
    {
        if (!canControl)
            return;

        HandleInput();
        HandleRotation();
        HandleZoom();
    }

    void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;

            // Invert if desired:
            pitch += Input.GetAxis("Mouse Y") * rotationSpeed;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            targetYawRotation = Quaternion.Euler(0f, yaw, 0f);
            targetPitchRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        targetZoom -= Input.mouseScrollDelta.y * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

    void HandleRotation()
    {
        yawPivot.localRotation = Quaternion.Slerp(
            yawPivot.localRotation,
            targetYawRotation,
            smooth * Time.deltaTime);

        pitchPivot.localRotation = Quaternion.Slerp(
            pitchPivot.localRotation,
            targetPitchRotation,
            smooth * Time.deltaTime);
    }

    void HandleZoom()
    {
        currentZoom = Mathf.Lerp(
            currentZoom,
            targetZoom,
            zoomSmooth * Time.deltaTime);

        Vector3 pos = cam.transform.localPosition;
        pos.z = -currentZoom;
        cam.transform.localPosition = pos;
    }

    //========================================================
    // PUBLIC
    //========================================================

    public void EnableControl(bool value)
    {
        canControl = value;
    }

    public void FocusPlanet(Transform focusPoint, Transform planetDummy)
    {
        canControl = false;

        planetDummy.gameObject.SetActive(true);
        planetDummy.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            yawPivot.DOMove(
                focusPoint.position,
                2f)
                .SetEase(Ease.InOutSine));

        seq.Join(
            yawPivot.DORotate(
                focusPoint.rotation.eulerAngles,
                2f)
                .SetEase(Ease.InOutSine));

        seq.AppendInterval(0.15f);

        seq.Append(
            planetDummy.DOScale(
                Vector3.one,
                0.8f)
                .SetEase(Ease.OutBack));

        seq.OnComplete(() =>
        {
            // VERY IMPORTANT
            SyncState();

            canControl = true;
        });
    }

    //========================================================
    // Sync internal controller state with actual transforms
    //========================================================

    private void SyncState()
    {
        yaw = yawPivot.localEulerAngles.y;

        pitch = pitchPivot.localEulerAngles.x;
        if (pitch > 180f)
            pitch -= 360f;

        targetYawRotation = yawPivot.localRotation;
        targetPitchRotation = pitchPivot.localRotation;

        currentZoom = targetZoom = -cam.transform.localPosition.z;
    }
}