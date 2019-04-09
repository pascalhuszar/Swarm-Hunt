using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Return the position of the controller
public class controllerPos : MonoBehaviour
{
    public Vector3 prevPos;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        prevPos = rb.transform.position;

    }

    public Vector3 getPos()
    {
        // Update the position 
        if (prevPos != rb.transform.position)
        {
            prevPos = transform.position;
            return prevPos;
        }
        else
        {
            return prevPos;
        }
    }

    private void Update()
    {
        getPos();
    }
}
