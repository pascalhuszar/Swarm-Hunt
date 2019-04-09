using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic movement Controller
public class SwarmMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;

    [SerializeField] private float movementSpeed;

    void Start()
    {

    }
	void Update () {
        PlayerMove();
    }
    private void PlayerMove()
    {
        float horiInput = Input.GetAxis(horizontalInputName) * movementSpeed * Time.deltaTime;
        float vertiInput = Input.GetAxis(verticalInputName) * movementSpeed * Time.deltaTime;

        transform.Translate(horiInput, 0, vertiInput);


    }
}
