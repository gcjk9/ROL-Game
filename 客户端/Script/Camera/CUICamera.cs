using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUICamera : MonoBehaviour
{
    public List<Transform> CameraPositions = new List<Transform>();
    public bool isMoving = false;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = CameraPositions[0].position;
        transform.rotation = CameraPositions[0].rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 0.05f);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                isMoving = false;
            }
        }
    }
    public void MoveTo(int i)
    {
            target = CameraPositions[i];
            isMoving = true;
    }
}
