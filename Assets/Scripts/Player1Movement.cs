using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player1Movement : MonoBehaviour {

    public Transform VRCamera;

    private float moveSpeed = 20.0f;

    private CharacterController playerController;

    private Vector3 moveDirection;

    public static Player1Movement player1Movement = null;

    void Awake()
    {
        //Check if instance already exists
        if (player1Movement == null)

            //if not, set instance to this
            player1Movement = this;

        //If instance already exists and it's not this:
        else if (player1Movement != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        Debug.Log("Character spawned");
        playerController = GetComponent<CharacterController>();
	}

    public void IncreaseSpeed(float increaseNumber)
    {
        moveSpeed += increaseNumber;
    }
	
	// Update is called once per frame
	void Update ()
    {
        moveDirection = VRCamera.TransformDirection(Vector3.forward);
        playerController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
