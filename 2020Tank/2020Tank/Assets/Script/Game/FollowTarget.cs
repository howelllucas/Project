using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{
    //
    public Transform Target;
    //
    public LayerMask respawnMark;
    //
    public float distance = 10.0f;
    //
    public float height = 5.0f;
    //
    [HideInInspector]
    public Camera cam;
    //
    public Transform camtransform;
    void Start()
    {
        cam = GetComponent<Camera>();
        camtransform = this.transform;
    }

    
    void LateUpdate()
    {
        if (!Target) return;
        Quaternion currentRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        Vector3 pos = Target.position;
        pos -= currentRotation * Vector3.forward * Mathf.Abs(distance);
        pos.y = Target.position.y + Mathf.Abs(height);
        transform.position = pos;

        transform.LookAt(Target);

        transform.position = Target.position - (transform.forward * Mathf.Abs(distance));

    }
    public void HideMask(bool shouldHide)
    {
        if (shouldHide) cam.cullingMask &= respawnMark;
        else cam.cullingMask |= respawnMark;
    }
}
