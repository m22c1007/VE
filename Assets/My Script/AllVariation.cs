

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllVariation : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private Transform eyeCamera;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform Plane;
    [SerializeField] private Transform slope;
    
    private float angle;
    private Vector3 prevPos;
    private Vector3 cConPos;
    private float YRotation;
    private float SRotation;
    private float LowPos;//UphillLowPos
    private float LowPos2;//DownhillLowPos
    private float HighPos;//UphillHighPos
    private float HighPos2;//DownhillHighPos
    private float LowRot;
    private float HighRot;
    private float Variation = -7.5f;
    private float Variation2 = 7.5f;
    private Vector3 RotateAxis = Vector3.right;

    private float headsetVelocity;
    private float Velocity;
    private float magnitude;
    private float RtVelocity;
    private float RtDVelocity;
    private float RAscendingV;
    private float RDescendingV;
    private float prevHeight;








    // Start is called before the first frame update
    void Start()
    {
        prevPos = eyeCamera.localPosition;
        cConPos = eyeCamera.position;
        LowRot = 90.0f;
        HighRot = 270.0f;
        LowPos = 1.0f;
        HighPos = LowPos + 5 * Mathf.Cos(15f * Mathf.Deg2Rad);
        SRotation = slope.transform.localEulerAngles.x - 360.0f;
        angle = 0.0f;
        RAscendingV = 0.1f;
        RDescendingV = 2.0f;
        RtVelocity = Mathf.Exp((-1) * RAscendingV * 15 * Mathf.Deg2Rad);
        RtDVelocity = 1.0f;
        Velocity = 0f;



    }

    // Update is called once per frame
    void Update()
    {

        HighPos2 = HighPos - 2 * (eyeCamera.localPosition.y * Mathf.Sin(15f * Mathf.Deg2Rad));
        LowPos2 = LowPos + 2 * (eyeCamera.localPosition.y * Mathf.Sin(15f * Mathf.Deg2Rad));
        Debug.Log("1:" + LowPos2);
        Debug.Log("2:" + HighPos2);

        characterController.Move(eyeCamera.position - cConPos);

        YRotation = eyeCamera.localEulerAngles.y;
        var heading = eyeCamera.position - cConPos;
        var distance = heading.magnitude;

        if (YRotation <= LowRot || YRotation > HighRot)//Uphill
        {
            if((eyeCamera.position.z > LowPos) && (eyeCamera.position.z < HighPos2))
            {
                if((Plane.localEulerAngles.x >= (Mathf.Floor(angle))) && (angle > SRotation))
                {
                    Vector3 NewEyeCamera = new Vector3(0f, 0f, LowPos);
                    parent.RotateAround(NewEyeCamera, RotateAxis, Variation * Time.deltaTime);
                    angle += (Variation * Time.deltaTime);
                    if(angle < SRotation)
                    {
                        angle = Mathf.Ceil(angle);
                    }
                }
                Vector3 heading1 = eyeCamera.position - cConPos;
                float distance1 = heading1.magnitude;
                Velocity = headsetVelocity * RtVelocity;
                magnitude = Velocity * Time.deltaTime;
                Vector3 pos = parent.position;
                pos += (heading1 * (magnitude / distance1) - heading1);
                parent.position = pos;
            }

            else
            {
                if((Plane.localEulerAngles.x > angle) && (Mathf.Ceil(angle) >= SRotation))
                {
                    Vector3 NewEyeCamera = new Vector3(0, 0, HighPos);
                    parent.RotateAround(NewEyeCamera, RotateAxis, Variation2 * Time.deltaTime);
                    angle += (Variation2 * Time.deltaTime);
                    if(angle > Plane.transform.localEulerAngles.x)
                    {
                        angle = Mathf.Floor(angle);
                    }
                }
            }
        }
        else//Downhill
        {
            if((eyeCamera.position.z >= LowPos2) && (eyeCamera.position.z <= HighPos))
            {

                if((Plane.transform.localEulerAngles.x >= (Mathf.Floor(angle))) && (angle > SRotation))
                {
                    Vector3 NewEyeCamera = new Vector3(0f, 0f, HighPos);
                    parent.RotateAround(NewEyeCamera, RotateAxis, Variation * Time.deltaTime);
                    angle += (Variation * Time.deltaTime);
                    if(angle < SRotation)
                    {
                        angle = Mathf.Ceil(angle);
                    }
                }
                Vector3 heading2 = eyeCamera.position - cConPos;
                float distance2 = heading2.magnitude;
                RtDVelocity = RtDVelocity + (prevHeight - (eyeCamera.position.y)) * RDescendingV;
                Velocity = headsetVelocity * RtDVelocity;
                magnitude = Velocity * Time.deltaTime;
                Vector3 oya = parent.position;
                oya += (heading2 * (magnitude / distance2) - heading2);
                parent.position = oya;
            }

            else
            {
                if((Plane.transform.localEulerAngles.x > angle) && (Mathf.Ceil(angle) >= SRotation))
                {
                    Vector3 NewEyeCamera = new Vector3(0, 0, LowPos);
                    parent.RotateAround(NewEyeCamera, RotateAxis, Variation2 * Time.deltaTime);
                    angle += (Variation2 * Time.deltaTime);
                    if(angle > Plane.localEulerAngles.x)
                    {
                        angle = Mathf.Floor(angle);
                    }
                }
                if(RtDVelocity > 1.0f)
                {
                    Vector3 heading3 = eyeCamera.position - cConPos;
                    float distance3 = heading3.magnitude;
                    RtDVelocity = RtDVelocity - (1.0f * Time.deltaTime);
                    Velocity = headsetVelocity * RtDVelocity;
                    magnitude = Velocity * Time.deltaTime;
                    Vector3 oya2 = parent.position;
                    oya2 += (heading3 * (magnitude / distance3) - heading3);
                    parent.position = oya2;

                }
            }
        }

        if (!characterController.isGrounded)
        {
            characterController.Move(Physics.gravity);
        }


        cConPos = eyeCamera.position;
        prevPos = eyeCamera.localPosition;

        headsetVelocity = distance / Time.deltaTime;
        prevHeight = eyeCamera.position.y;









    }
}
