using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMovement : MonoBehaviour {

    public Transform VRCamera;

    float moveSpeed = 20.0f;

    private CharacterController playerController;

    private Vector3 moveDirection;

	// Use this for initialization
	void Start ()
    {
        Debug.Log("Character spawned");
        playerController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        moveDirection = VRCamera.TransformDirection(Vector3.forward);
        playerController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
