using UnityEngine;

public class CameraNoClipScript : MonoBehaviour
{
    [SerializeField] Transform referenceTransform;
    [SerializeField] float collisionOffset = 0.3f; //creates buffer around camera to prevent clipping
    [SerializeField] float cameraZoomSpeed = 15f; //How fast the Camera should snap into position if there are no obstacles

    Vector3 defaultPos;
    Vector3 directionNormalized;
    Transform parentTransform;
    float defaultDistance;

    public bool disableNoClip = false;

    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.localPosition;
        directionNormalized = defaultPos.normalized;
        parentTransform = transform.parent;
        defaultDistance = Vector3.Distance(defaultPos, Vector3.zero);
    }

    // LateUpdate is called after Update
    void LateUpdate()
    {
        Vector3 currentPos = defaultPos;

        //if hitching with a car, zoom the camera in just a bit, but lock it so it cant jitter in and out
        if (disableNoClip)
        {
            transform.localPosition = defaultPos + Vector3.forward * 4;
        }
        else
        {
            RaycastHit hit;
            Vector3 dirTmp = parentTransform.TransformPoint(defaultPos) - referenceTransform.position;
            if (Physics.SphereCast(referenceTransform.position, collisionOffset, dirTmp, out hit, defaultDistance) && !hit.transform.CompareTag("Pedestrian"))
            {
                currentPos = (directionNormalized * (hit.distance - collisionOffset));

                transform.localPosition = currentPos;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, cameraZoomSpeed * Time.deltaTime);
            }
        }
    }
}
