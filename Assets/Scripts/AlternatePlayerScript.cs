using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float yaw;
    public float pitch;
    public float speed;
    public float mouseSensitivity = 2f;
    public Camera playerCamera;
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");

        // Clamp pitch between lookAngle
        pitch = Mathf.Clamp(pitch, -50, 50);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);


        direction = new Vector3(0, 0, 0);
        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            direction += transform.forward * 1 * speed;
        }
        else if (Input.GetKey("s") || Input.GetKey("down"))
        {
            direction += transform.forward * -1 * speed;
        }

        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            direction += transform.right * -1 * speed;
        }
        else if (Input.GetKey("d") || Input.GetKey("right"))
        {
            direction += transform.right * 1 * speed;
        }

        transform.position += direction.normalized * speed * Time.deltaTime;
    }
}
