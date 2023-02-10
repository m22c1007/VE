
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityUpdate : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private Transform eyeCamera;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform Plane;
    [SerializeField] private Transform slope;
    [SerializeField] private float angle;

    private Vector3 prevPos;
    private Vector3 cConPos;
    private float offset;
    private float headsetVelocity;
    private float Velocity;
    private float magnitude;
    private float RtVelocity;
    private float RtDVelocity;
    private float RAscendingV;
    private float RDescendingV;

    private float HighRot;
    private float LowRot;
    private float YRotation;
    private float HighPos;
    private float LowPos;
    private float prevHeight;



    // Start is called before the first frame update
    void Start()
    {
        prevPos = eyeCamera.localPosition;
        cConPos = eyeCamera.position;
        RAscendingV = 0.1f;
        RDescendingV = 2.0f;
        RtVelocity = Mathf.Exp((-1) * RAscendingV * angle * Mathf.Deg2Rad);
        RtDVelocity = 1.0f;
        Debug.Log("0:" + RtVelocity);
        Velocity = 0f;
        HighRot = 270.0f;
        LowRot = 90.0f;
        LowPos = 1.0f;
        HighPos = LowPos + 5 * Mathf.Cos(15f * Mathf.Deg2Rad);
        Debug.Log(LowPos);
        Debug.Log(HighPos);
        offset = (parent.position - characterController.transform.position).y;



    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(eyeCamera.position - cConPos);
        if (!characterController.isGrounded)
        {
            characterController.Move(Physics.gravity);
        }

        YRotation = eyeCamera.localEulerAngles.y;
        var heading = eyeCamera.localPosition - prevPos;//•ûŒüƒxƒNƒgƒ‹
        var distance = heading.magnitude;//‹——£

        if ((characterController.transform.position.z > LowPos) && (characterController.transform.position.z < HighPos)){
            if (YRotation <= LowRot || YRotation >= HighRot)//Uphill
            {

                //var heading = eyeCamera.localPosition - prevPos;

                Debug.Log("Yes");
               
                Velocity = headsetVelocity * RtVelocity;

                Debug.Log("Velocity:" + Velocity);
                magnitude = Velocity * Time.deltaTime;
                Debug.Log("magnitude:" + magnitude);
                Debug.Log("distance:" + distance);

                Vector3 pos = parent.position;
                pos.y = characterController.transform.position.y + offset;
                parent.position = pos;
                Debug.Log("pos.y:" + pos.y);
                Vector3 oya = parent.position;
                oya += (heading * (magnitude / distance) - heading);
                parent.position = oya;

            }

            else//Downhill
            {
                Debug.Log("No");
                RtDVelocity = RtDVelocity + (prevHeight - (eyeCamera.position.y)) * RDescendingV;
                Debug.Log(RtDVelocity);
                Velocity = headsetVelocity * RtDVelocity;
                magnitude = Velocity * Time.deltaTime;
                Vector3 pos = parent.position;
                pos.y = characterController.transform.position.y + offset;
                parent.position = pos;
                Vector3 oya = parent.position;
                oya += heading * (magnitude / distance) - heading;
                parent.position = oya;

            }
        }
        else
        {
            Debug.Log("1");
        }


        cConPos = eyeCamera.position;

        headsetVelocity = distance / Time.deltaTime;
        prevPos = eyeCamera.localPosition;
        prevHeight = eyeCamera.position.y;
        Debug.Log("headV:"+ headsetVelocity);
        Debug.Log("prevPos:"+ prevPos);
        Debug.Log("prevHeight:"+ prevHeight);



    }
}


