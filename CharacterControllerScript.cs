using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    CharacterController characterControllerRef;

    bool isHitchHiking = false;//this will be set to true when the player is hanging on to a vehicle
    public bool carInRange = false;

    [SerializeField] GameObject cameraArmRef; //reference to the camera arm that is a part of the character controller
    [SerializeField] GameObject cameraRef; //reference to the camera itself
    [SerializeField] float rotationSpeed = 300f; //controls the character turning and also the vertical rotation of the camera

    float minCamClampAngle = 75f;
    float maxCamClampAngle = -10f;

    float turnRot;
    float rollRot;

    [SerializeField] float normalMoveSpeed; //players skating speed
    float currentSpeedBoost; //when the player lets go of a moving vehicle, this will be set to the speed of the vehicle and will then decay over time
    [SerializeField] float boostDecayRate = 1; //rate that the boost speed will reduce

    bool isJumping = false;
    [SerializeField] float jumpSpeed = 5f;
    float jumpDecay = 1f;
    float jumpTimer = 1f;
    float gravitySpeed = -9.81f;
    float gravityAccelleration = 0f;

    GameObject hitcHikerCar;
    Vector3 hitchHikerCarOffset;

    void Start()
    {
        characterControllerRef = GetComponent<CharacterController>();
        currentSpeedBoost = 0f;
        turnRot = transform.rotation.y;
        rollRot = 45f;
        // Hide and lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        if (!isHitchHiking)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && carInRange)
            {
                isHitchHiking = true;
                hitchHikerCarOffset = hitcHikerCar.transform.position - transform.position;
            }
            MovementControls();
        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                transform.position = hitcHikerCar.transform.position /* add offset and fix rotation*/;
            }
            else
            {
                isHitchHiking = false;
                currentSpeedBoost = hitcHikerCar.GetComponent<Rigidbody>().velocity.magnitude;
            }
        }


        BoostDecay();
    }

    void LateUpdate()
    {
        CameraControls();
    }
            
    void MovementControls()
    {
        //regular movement
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal") * normalMoveSpeed + currentSpeedBoost, 0f, Input.GetAxis("Vertical") * normalMoveSpeed + currentSpeedBoost);

        moveDir = transform.TransformDirection(moveDir);

        if (isJumping)
        {
            //if the player is grounded (has landed) the jump exits immidiately
            //while the jump button is held, they will keep going up (till the jump timer ends), otherwise, if they release the jump button, the jump timer will decrease at a faster pace (facilites a minimum jump value).
            //There is also a falloff of the jump speed
            //At present, horizontal input is not affected by jumping, allowing for hectic mid-air manouvering
            if (characterControllerRef.isGrounded)
            {
                jumpTimer = 1f;
                jumpDecay = 1f;
                isJumping = false;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                moveDir.y += jumpSpeed * jumpDecay;
                jumpTimer -= Time.deltaTime;
                jumpDecay *= 0.95f;
                if (jumpTimer <= 0)
                {
                    jumpTimer = 1f;
                    jumpDecay = 1f;
                    isJumping = false;
                }
            }
            else
            {
                moveDir.y += jumpSpeed * jumpDecay;
                jumpTimer -= 4 * Time.deltaTime;
                jumpDecay *= 0.95f;
                if (jumpTimer <= 0)
                {
                    jumpTimer = 1f;
                    jumpDecay = 1f;
                    isJumping = false;
                }
            }

        }
        else if (!characterControllerRef.isGrounded)
        {
            gravityAccelleration += 1f * Time.deltaTime; //multiply Time.deltaTime by a higher value to increase the initial gravity fall pace
            moveDir.y = gravitySpeed * gravityAccelleration;
        }
        else
        {
            gravityAccelleration = 0;
            //check for jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
            }
            else if (!isJumping)
            {
                // applies a nominal amount of gravity when the player is grounded to ensure they don't drift above the ground accidentally preventing jumps
                moveDir.y = -0.01f;
            }
        }

        characterControllerRef.Move(moveDir * Time.deltaTime);
    }

    void CameraControls()
    {
        //adds any changes in the mouse position to the rotation
        turnRot += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        rollRot += -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        rollRot = Mathf.Clamp(rollRot, maxCamClampAngle, minCamClampAngle); //limits the camera's vertical rotation to a defined range

        if (!isHitchHiking)
        {
            //if the character is holding onto a car, they cannot turn, their turning will be linked to the car 
            transform.rotation = Quaternion.Euler(0f, turnRot, 0f); //turns the character based on the new rotation
        }
        cameraArmRef.transform.localRotation = Quaternion.Euler(rollRot, 0f, 0f); //rotates the camera vertically
    }

    void BoostDecay()
    {
        if (currentSpeedBoost < 0)
        {
            currentSpeedBoost = 0;//speed boost can't drop below 0
        }
        else if (currentSpeedBoost > 0)
        {
            currentSpeedBoost -= boostDecayRate * Time.deltaTime;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && !isHitchHiking)
        {
            print("Can Grab");
            carInRange = true;
            hitcHikerCar = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Car") && !isHitchHiking)
        {
            print("Can't Grab :(");
            carInRange = false;
            hitcHikerCar = null;
        }
    }
}