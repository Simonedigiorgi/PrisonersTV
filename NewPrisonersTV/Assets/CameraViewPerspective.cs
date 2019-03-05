using UnityEngine;

public class CameraViewPerspective : MonoBehaviour 
{
    public Transform[] targets; 
    public Vector3 offset;
    public float smoothTime;
    public float minZoom;
    public float maxZoom;
    public float zoomLimiter;

    Transform thisTransform;
    Vector3 centerPoint;
    Vector3 velocity;
    Camera cam;
    Camera childCam;
    Bounds bounds;
    float greatestDistance;
    float newZoom;

    private void Start()
    {
        cam = GetComponent<Camera>();
        thisTransform = this.transform;
        childCam = thisTransform.GetChild(0).GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (targets == null || targets.Length == 0)
            return;
        Move();
        Zoom();
        
    }

    void Move()
    {
        centerPoint = GetCenterPoint();
        thisTransform.position = Vector3.SmoothDamp(thisTransform.position, centerPoint + offset, ref velocity, smoothTime);
    }

    void Zoom()
    {
        newZoom = Mathf.Lerp(maxZoom, minZoom, greatestDistance/zoomLimiter);
        childCam.fieldOfView = cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime); 
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Length == 1)
        {
            return targets[0].position;
        }

        bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        greatestDistance = bounds.size.x;
        return bounds.center;
    }
}