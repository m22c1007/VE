using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightScript : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private Transform eyeCamera;
    [SerializeField] private CharacterController characterController;

    private Vector3 prevPos;
    private float offset;

    private void Start()
    {
        prevPos = eyeCamera.position;
        offset = (parent.position - characterController.transform.position).y;
    }

    private void Update()
    {
        characterController.Move(eyeCamera.position - prevPos);
        prevPos = eyeCamera.position;

        if (!characterController.isGrounded)
        {
            characterController.Move(Physics.gravity);
        }
        //Debug.Log("test");
        Vector3 pos = parent.position;
        pos.y = characterController.transform.position.y + offset;
        parent.position = pos;
    }
}