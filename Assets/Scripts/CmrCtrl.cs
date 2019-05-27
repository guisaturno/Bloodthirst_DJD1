using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmrCtrl : MonoBehaviour
{
    // Camera modes
    public enum Mode
    {
        Feedback,
        Linear
    }

    public Mode         mode; // Camera mode
    public Transform    character; // Target
    public float        camaraSpeed = 0.1f; // CmrSpeed - ajustable
    public bool         inbounds = false;
    public Rect         bounds;
    new Camera          camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        // Camera position on z - following player
        Vector3 caPos = character.position;

        switch (mode)
        {
            // Feedback loop
            case Mode.Feedback:
                Vector3 delta = caPos - transform.position;
                caPos = transform.position + delta * camaraSpeed * Time.fixedDeltaTime;
                break;
            // Linear
            case Mode.Linear:
                caPos = Vector3.MoveTowards
                    (transform.position, caPos, camaraSpeed * Time.fixedDeltaTime);
                break;
        }

        if (inbounds)
        {
            float y = camera.orthographicSize; // Y size
            float x = y * camera.aspect; // X size

            if (caPos.x < (bounds.xMin + x)) // Case cmr's x is smaller than rect's x
                caPos.x = bounds.xMin + x;         
            if (caPos.x > (bounds.xMax - x)) // Cmr's x is bigger than rect's x
                caPos.x = bounds.xMax - x;

            if (caPos.y < (bounds.yMin + y)) // Cmr's y is smaller than rect's y
                caPos.y = bounds.yMin + y;
            if (caPos.y > (bounds.yMax - y)) // Cmr's y is bigger than rect's y
                caPos.y = bounds.yMax - y;

            // *we may use Clamp
        }

        // So the camara is at te right Z position
        caPos.z = transform.position.z;

        // Camera assigned to character position
        transform.position = caPos;
    }

    // Draws referende bounds
    private void OnDrawGizmos()
    {
        if (inbounds)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bounds.center, new Vector3(bounds.width, bounds.height, 0.0f));
        }
    }
}
