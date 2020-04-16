using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public Camera cam;
    public Transform P1;
    public Transform P2;
    public Transform P3;
    public Transform P4;

    // How many units should we keep from the players
    public float zoomFactor = 1.5f;
    public float followTimeDelta = 0.8f;
    public float ShiftCameraUpwards;



    private float maxX;
    private float minX;
    private float maxY;
    private float minY;

    float[] xPositions;
    float[] yPositions;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get positions of players
        float[] xPositions = { P1.position.x, P2.position.x ,P3.position.x, P4.position.x };
        float[] yPositions = { P1.position.y, P2.position.y, P3.position.y, P4.position.y };

        // Find max and min X and Y values
        maxX = Mathf.Max(xPositions);
        minX = Mathf.Min(xPositions);
        maxY = Mathf.Max(yPositions);
        minY = Mathf.Max(yPositions);

        // Pass these values in for the player distances
        FixedCameraFollowSmooth(new Vector2(minX, minY), new Vector2(maxX, maxY));
    }

    // Follow Two Transforms with a Fixed-Orientation Camera
    public void FixedCameraFollowSmooth(Vector2 t1, Vector2 t2)
    {
        // Midpoint we're after
        Vector3 midpoint = (t1 + t2) / 2f;

        // Distance between objects
        Vector3 UpwardShift = new Vector3(0, ShiftCameraUpwards, 0);
        float distance = (t1 - t2).magnitude;

        // Move camera a certain distance
        Vector3 cameraDestination = midpoint - (cam.transform.forward * distance * zoomFactor) + UpwardShift;

        // Adjust ortho size if we're using one of those
        if (cam.orthographic)
        {
            // The camera's forward vector is irrelevant, only this size will matter
            cam.orthographicSize = distance;
        }
        // You specified to use MoveTowards instead of Slerp
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, followTimeDelta);

        // Snap when close enough to prevent annoying slerp behavior
        if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
            cam.transform.position = cameraDestination;
    }
}
