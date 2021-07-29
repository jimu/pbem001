using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;

    public Vector3 newPosition;
    public Quaternion newRotation;

    public float rotationSpeed = 1f;

    public Transform cameraTransform;
    public Vector3 zoomAmount;
    public Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;

    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
        HandleZoom();
    }

    void HandleMovementInput()
    {
        if (Input.mouseScrollDelta.y != 0)
            newZoom += Input.mouseScrollDelta.y * zoomAmount;

        movementSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : normalSpeed;
        newPosition += transform.up * movementSpeed * Input.GetAxis("Vertical");
        newPosition += transform.right * movementSpeed * Input.GetAxis("Horizontal");

        //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.position = newPosition;

        //newRotation *= Quaternion.Euler(Vector3.forward * rotation); // transform.right * movementSpeed * Input.GetAxis("Horizontal");
        float rotationDelta = Input.GetAxis("Rotation");

        newRotation *= Quaternion.Euler(Vector3.forward * rotationDelta * rotationSpeed);

        // Debug.Log($"Rotation={rotation} Zoom={zoom}");

        //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.position = newPosition;
        transform.rotation = newRotation;

    }

    void HandleZoomOld()
    {
        float zoom = Input.GetAxis("Zoom");
        newZoom += zoom * zoomAmount;
        cameraTransform.localPosition = newZoom;
    }

    void HandleZoom()
    {
        float zoomDelta = Input.GetAxis("Zoom");

        if (zoomDelta != 0f)
        {        
            newZoom += zoomDelta * zoomAmount;
            cameraTransform.localPosition = newZoom;
            /*
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (plane.Raycast(ray, out entry))
            {
                Vector3 desiredPosition = ray.GetPoint(entry);
                Debug.Log($"DesiredPosition: {desiredPosition} zoomDelta:{zoomDelta}");
                float distance = Vector3.Distance(desiredPosition, transform.position);
                Vector3 direction = Vector3.Normalize(desiredPosition - transform.position) * (distance * zoomDelta);

                //direction = Quaternion.AngleAxis(-90, Vector3.forward) * direction;

                cameraTransform.localPosition += direction;
            }
            */
        }/*
        else
        {
            Vector3 forward = cameraTransform.TransformDirection(Vector3.forward) * 10;
            Debug.DrawRay(transform.position, forward, Color.green);
            Debug.Log($"Drawing ray at {transform.position}");
        }
        //newZoom += zoomDelta * zoomAmount;
        //cameraTransform.localPosition = newZoom;
        */
    }

    private void OnDrawGizmosx()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(cameraTransform.position, 1);
        Gizmos.color = Color.red;
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward) * 10;
        Gizmos.DrawSphere(forward, 2);

        Gizmos.color = Color.green;
        forward = cameraTransform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawSphere(forward, 2);
    }

    private void OnGUI()
    {
        
    }




    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
                dragStartPosition = ray.GetPoint(entry);
        }
        else if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.forward * (-difference.x / 5f));
        }
    }
}
