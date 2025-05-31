using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Gốc xoay ngang
    [SerializeField] private Transform cameraPitchTransform; // Gốc để xoay pitch (con của player)
    [SerializeField] private float mouseSensitivity = 100f;
    // [SerializeField] private float rotationSmoothTime = 0.12f;

    private float yaw = 0f;
    private float pitch = 0f;
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;

    void Start()
    {
        LockCursor(true);
    }

    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            LockCursor(false);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            LockCursor(true);
        }
        if (!Cursor.lockState.Equals(CursorLockMode.Locked))
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -35f, 60f);

        // Xoay player (ngang)
        playerTransform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Xoay camera theo chieu doc (pitch)
        cameraPitchTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void LockCursor(bool shouldLock)
    {
        Cursor.visible = !shouldLock;
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
