
using UnityEngine;
// Thrid person camera behaivour 
public class swarmCamera : MonoBehaviour
{

    public float rotaSpeed = 1;
    public Transform swarmFocus, swarmController;
    float mouseX, mouseY;

    private void Awake()
    {
        // Cursor.lockState = CursorLockMode.Locked;
    }


    void LateUpdate()
    {
        CamRotation();
    }
    void CamRotation()
    {
        mouseX += Input.GetAxis("Mouse X") * rotaSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotaSpeed;
        mouseY = Mathf.Clamp(mouseY, -15, 30);

        transform.LookAt(swarmFocus);

        swarmFocus.rotation = Quaternion.Euler(mouseY, mouseX, 0);          // cam rotation
        swarmController.rotation = Quaternion.Euler(0, mouseX, 0);          // also character rotation
    }





}



