using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.OpenXR.NativeTypes;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(XROrigin))]
public class RoomscaleFix : MonoBehaviour
{
    private CharacterController cc;
    private XROrigin xROrigin;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        xROrigin = GetComponent<XROrigin>();
    }

    void FixedUpdate()
    {
        cc.height = xROrigin.CameraInOriginSpaceHeight + 0.15f;

        var centerPoint = transform.InverseTransformPoint(xROrigin.Camera.transform.position);
        cc.center = new Vector3(centerPoint.x, cc.height / 2 + cc.skinWidth, centerPoint.z);

        cc.Move(new Vector3(0.001f, -0.001f, 0.001f));
        cc.Move(new Vector3(-0.001f, -0.001f, -0.001f));
    }
}
