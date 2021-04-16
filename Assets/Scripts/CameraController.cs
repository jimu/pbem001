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
    public float rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        movementSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : normalSpeed;
        newPosition += transform.up * movementSpeed * Input.GetAxis("Vertical");
        newPosition += transform.right * movementSpeed * Input.GetAxis("Horizontal");

        //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.position = newPosition;

        //newRotation *= Quaternion.Euler(Vector3.forward * rotation); // transform.right * movementSpeed * Input.GetAxis("Horizontal");
        float rotation = Input.GetAxis("Rotation");
        Debug.Log($"Rotation={rotation}");
        transform.rotation *= Quaternion.Euler(Vector3.forward * rotation * rotationSpeed);

    }
}
