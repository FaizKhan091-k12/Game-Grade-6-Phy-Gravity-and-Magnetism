using UnityEngine;

public class DragRotate : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 0.25f;
    [SerializeField] private float damping = 8f;
    [SerializeField] private float maxVelocity = 300f;

    private bool dragging;
    private Vector3 lastPointerPos;

    private float currentVelocity;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.IsChildOf(transform))
                {
                    dragging = true;
                    lastPointerPos = Input.mousePosition;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
            dragging = false;

        if (dragging)
        {
            Vector3 delta = Input.mousePosition - lastPointerPos;

            // Smoothly build rotation speed
            currentVelocity = Mathf.Lerp(currentVelocity,
                                         -delta.x * rotationSpeed * 60f,
                                         Time.deltaTime * 15f);

            currentVelocity = Mathf.Clamp(currentVelocity,
                                          -maxVelocity,
                                           maxVelocity);

            lastPointerPos = Input.mousePosition;
        }
        else
        {
            // Slowly reduce speed after releasing mouse
            currentVelocity = Mathf.Lerp(currentVelocity,
                                         0,
                                         Time.deltaTime * damping);
        }

        transform.Rotate(0, currentVelocity * Time.deltaTime, 0);

#endif

#if UNITY_ANDROID || UNITY_IOS

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.IsChildOf(transform))
                    {
                        dragging = true;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended ||
                touch.phase == TouchPhase.Canceled)
            {
                dragging = false;
            }

            if (dragging && touch.phase == TouchPhase.Moved)
            {
                currentVelocity = Mathf.Lerp(currentVelocity,
                                             -touch.deltaPosition.x * rotationSpeed * 30f,
                                             Time.deltaTime * 15f);

                currentVelocity = Mathf.Clamp(currentVelocity,
                                              -maxVelocity,
                                               maxVelocity);
            }
        }
        else
        {
            currentVelocity = Mathf.Lerp(currentVelocity,
                                         0,
                                         Time.deltaTime * damping);
        }

        transform.Rotate(0, currentVelocity * Time.deltaTime, 0);

#endif
    }
}