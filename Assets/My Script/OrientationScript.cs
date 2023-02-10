


 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLocomotion : MonoBehaviour
{
    [SerializeField] private Transform parent;   
    [SerializeField] private Transform eyeCamera;
    [SerializeField] private Transform Plane;
    [SerializeField] private Transform slope;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float SlopeAngle;


    private Vector3 prevPos;
    private float offset;
    private float HighPos;
    private float LowPos;
    private float HighPos2;
    private float LowPos2;
    private float HighRot;
    private float LowRot;
    private float Variation = -7.5f;
    private float Variation2 = 7.5f;
    private float angle = 0.0f;
    private float SRotation;
    private float YRotation;
    private float YCamera;
    private float ZCamera;
    private Vector3 RotateAxis = Vector3.right;
    private Vector3 test = Vector3.zero;




    // Start is called before the first frame update
    void Start()
    {
        prevPos = eyeCamera.position;
        offset = (this.transform.position - characterController.transform.position).y;
        LowPos = 1.0f;
        HighPos2 = LowPos + 5 * Mathf.Cos(SlopeAngle * Mathf.Deg2Rad);
        SRotation = slope.transform.localEulerAngles.x - 360.0f;
        HighRot = 270.0f;
        LowRot = 90.0f;
        Debug.Log("1:" + SRotation);
        Debug.Log("2:" + SlopeAngle);




    }

    /*void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.tag == "slope")
        {
            if(!(parent.rotation == hit.transform.rotation))
            {
                Debug.Log("test");
                parent.rotation = hit.transform.rotation;
            }
            

        }
        else if (hit.gameObject.tag == "plane")
        {
                        
            if(!(parent.rotation == hit.transform.rotation))
            {
                parent.rotation = hit.transform.rotation;
            }
            
        }
    }*/
        

    // Update is called once per frame
    void Update()
    {

        characterController.Move(eyeCamera.position - prevPos);
        prevPos = eyeCamera.position;
        if (!characterController.isGrounded)
        {
            characterController.Move(Physics.gravity);
        }
        /*if((eyeCamera.localPosition.z < LowPos) || (eyeCamera.localPosition.z > HighPos))
        {
            Vector3 pos = this.transform.position;
            pos.y = characterController.transform.position.y + offset;
            this.transform.position = pos;
        }*/

        //Debug.Log("1:" + eyeCamera.position.y);
        //Debug.Log("2:" + eyeCamera.localPosition.y);

    }  

    void FixedUpdate()
    {
        Debug.Log(eyeCamera.position.z);
        //Debug.Log(eyeCamera.localPosition.y);
        HighPos = LowPos + 5 * Mathf.Cos(SlopeAngle * Mathf.Deg2Rad) - eyeCamera.localPosition.y * Mathf.Sin(SlopeAngle * Mathf.Deg2Rad);
        LowPos2 = LowPos - eyeCamera.localPosition.y * Mathf.Sin(SlopeAngle * Mathf.Deg2Rad);
        //Debug.Log(HighPos);
        YRotation = eyeCamera.localEulerAngles.y;
        YCamera = 5 * Mathf.Sin(SlopeAngle * Mathf.Deg2Rad) + eyeCamera.localPosition.y * Mathf.Cos(SlopeAngle * Mathf.Deg2Rad);
        ZCamera = eyeCamera.position.z + eyeCamera.localPosition.y * Mathf.Sin(SlopeAngle * Mathf.Deg2Rad);
        //Debug.Log(YRotation);


        if (YRotation <= LowRot || YRotation > HighRot)//Uphill
        {
            //Debug.Log("Yes");
            if ((eyeCamera.localPosition.z >= LowPos2) && (eyeCamera.localPosition.z < HighPos))//â‚Ì‰º
            {
                //Debug.Log("if");
                //Debug.Log("plane::" + Plane.transform.localEulerAngles.x);
                //Debug.Log("slope::" + SRotation);
                if ((Plane.transform.localEulerAngles.x >= (Mathf.Floor(angle))) && (angle > SRotation))
                {
                    //Debug.Log("AIDA");
                    Vector3 NewEyeCamera = new Vector3(0f, 0f, LowPos);
                    parent.RotateAround(NewEyeCamera, RotateAxis, Variation * Time.deltaTime);
                    angle += (Variation * Time.deltaTime);
                    //Debug.Log("parent:" + parent.position);
                    //Debug.Log("chara:" + characterController.transform.position);
                    //Debug.Log("Eye:" + eyeCamera.position);
                    if (angle < SRotation)
                    {
                        angle = Mathf.Ceil(angle);
                    }
                    //Debug.Log(angle);
                }
            }

            else
            {
                //Debug.Log("else");
                //Debug.Log("plane::" + Plane.transform.localEulerAngles.x);
                //Debug.Log("slope::" + SRotation);
                //Debug.Log(angle);
                if ((Plane.transform.localEulerAngles.x > angle) && (Mathf.Ceil(angle) >= SRotation))
                {
                    //Debug.Log("No1");
                    Vector3 NewEyeCamera = new Vector3(0, 0, HighPos2);
                    parent.RotateAround(NewEyeCamera, RotateAxis, Variation2 * Time.deltaTime);
                    angle += (Variation2 * Time.deltaTime);
                    if (angle > Plane.transform.localEulerAngles.x)
                    {
                        angle = Mathf.Floor(angle);
                    }
                }
            }

        }
        else
        {
            //Debug.Log("No");
            if ((eyeCamera.localPosition.z >= LowPos) && (eyeCamera.localPosition.z <= HighPos2))
            {
                //Debug.Log("if");
                //Debug.Log("plane::" + Plane.transform.localEulerAngles.x);
                //Debug.Log("slope::" + SRotation);
                if ((Plane.transform.localEulerAngles.x >= (Mathf.Floor(angle))) && (angle > SRotation))
                {
                    //Debug.Log("AIDA");
                    //Debug.Log(angle);
                    Vector3 NewEyeCamera = new Vector3(0f, 0f, HighPos);
                    parent.RotateAround(NewEyeCamera, RotateAxis, Variation * Time.deltaTime);
                    angle += (Variation * Time.deltaTime);
                    Debug.Log("New..." + parent.rotation);
                    if (angle < SRotation)
                    {
                        angle = Mathf.Ceil(angle);
                    }
                }
            }

            else
            {
                //Debug.Log("else");
                //Debug.Log("plane::" + Plane.transform.localEulerAngles.x);
                //Debug.Log("slope::" + SRotation);
                //Debug.Log(angle);
                if ((Plane.transform.localEulerAngles.x > angle) && (Mathf.Ceil(angle) >= SRotation))
                {
                    //Debug.Log("No1");
                    Vector3 NewEyeCamera = new Vector3(0, 0, LowPos2);
                    parent.RotateAround(NewEyeCamera, RotateAxis, Variation2 * Time.deltaTime);
                    angle += (Variation2 * Time.deltaTime);
                    if (angle > Plane.transform.localEulerAngles.x)
                    {
                        angle = Mathf.Floor(angle);
                    }
                }
            }

        }
    }
}
