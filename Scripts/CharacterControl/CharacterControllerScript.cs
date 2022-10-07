using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterControllerScript : MonoBehaviour
{
    // PLAYER STILL SNAPPING TO ODD POSITIONS WHEN HITCHIKING
    // ADD CAMERA ZOOM CHANGE BASED ON SPEED


    CharacterController characterControllerRef;
    [SerializeField] GameObject viewModelRef;
    [SerializeField] CameraNoClipScript camNoClipScriptRef;

    [SerializeField] float viewModelRotSpeed = 20f;

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
    [SerializeField] float boostDecayRate = 0.35f; //rate that the boost speed will reduce

    bool isJumping = false;
    [SerializeField] float jumpSpeed = 5f;
    float jumpDecay = 0.2f;
    float jumpTimer = 1f;
    float gravitySpeed = -9.81f;
    float gravityAccelleration = 0f;

    GameObject hitcHikerCar;
    Vector3 hitchHikerCarOffset;

    Vector3 previousPos;

    bool controlsEnabled = true;

    int playerhealth = 100;
    [SerializeField] GameObject playerRagdoll;
    [SerializeField] GameObject deathUI;
    [SerializeField] GameObject gameplayUI;

    [SerializeField] GameObject drownedCharacterModel;

    void Start()
    {
        characterControllerRef = GetComponent<CharacterController>();
        currentSpeedBoost = 0f;
        turnRot = transform.rotation.y;
        rollRot = 45f;
    }


    void Update()
    {
        if (controlsEnabled)
        {
            if (currentSpeedBoost > 0)
            {
                //if the player has much boost, the camera noClip is disabled and the camera distance is lock fairly close. This prevents camera rubberbanding at high speeds.
                if (currentSpeedBoost > 10f)
                {
                    camNoClipScriptRef.disableNoClip = true;
                }
                else
                {
                    camNoClipScriptRef.disableNoClip = false;
                }
                SpeedScreenDistortion();
            }



            if (!isHitchHiking)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && carInRange)
                {
                    isHitchHiking = true;
                    hitchHikerCarOffset = hitcHikerCar.transform.position - transform.position;
                    previousPos = transform.position;
                }
                MovementControls();
            }
            else
            {

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    transform.position = hitcHikerCar.transform.TransformPoint(hitchHikerCarOffset);// >>> should it be -hitchHikerCarOffset
                    currentSpeedBoost = (transform.position - previousPos).magnitude * 1.1f / Time.deltaTime;//boost when you let go of a car is actually slightly faster than the car
                }
                else
                {
                    isHitchHiking = false;
                }
                previousPos = transform.position;
            }

            BoostDecay();
        } 
        
        if (controlsEnabled && playerhealth <= 0)
        {
            Wasted();
        }

        if(transform.position.y <= -1f)
        {
            Drowned();
        }
    }



    void LateUpdate()
    {
        if (controlsEnabled)
        {
            CameraControls();
        }
    }




    void MovementControls()
    {
        //regular movement
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 moveMag = Vector3.Normalize(moveDir) * normalMoveSpeed + Vector3.Normalize(moveDir) * currentSpeedBoost;

        moveMag = transform.TransformDirection(moveMag);

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
                moveMag.y += jumpSpeed * jumpDecay;
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
                moveMag.y += jumpSpeed * jumpDecay;
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
            moveMag.y = gravitySpeed * gravityAccelleration;
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
                moveMag.y = -0.2f;
            }
        }

        characterControllerRef.Move(moveMag * Time.deltaTime);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //rotates player model in the direction they're moving
            viewModelRef.transform.rotation = Quaternion.Slerp(viewModelRef.transform.rotation, Quaternion.LookRotation(moveMag), viewModelRotSpeed * Time.deltaTime);
        }
    }

    void CameraControls()
    {
        //adds any changes in the mouse position to the rotation
        turnRot += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        rollRot += -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        rollRot = Mathf.Clamp(rollRot, maxCamClampAngle, minCamClampAngle); //limits the camera's vertical rotation to a defined range

        transform.rotation = Quaternion.Euler(0f, turnRot, 0f); //turns the character based on the new rotation
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
            carInRange = true;
            hitcHikerCar = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car") && !isHitchHiking)
        {
            carInRange = false;
            hitcHikerCar = null;
        }
    }

    void SpeedScreenDistortion()
    {
        //add screen blur around edges relative to boost speed
        //change fov relative to boost speed
    }



    public void ModifyHealth(int healthChangeAmount)
    {
        playerhealth += healthChangeAmount;
    }


    void Wasted()
    {
        controlsEnabled = false;
        viewModelRef.SetActive(false);
        cameraRef.SetActive(false);
        playerRagdoll.SetActive(true);
        Time.timeScale = 0.3f;
        deathUI.SetActive(true);
        gameplayUI.SetActive(false);

        Invoke("ReloadCurrentScene", 2f);
    }


    void Drowned()
    {
        //if player falls below water level:
        controlsEnabled = false;
        viewModelRef.SetActive(false);
        cameraRef.SetActive(false);
        drownedCharacterModel.SetActive(true);
        drownedCharacterModel.transform.position = new Vector3(drownedCharacterModel.transform.position.x, -0.5f, drownedCharacterModel.transform.position.z);
        Time.timeScale = 0.3f;
        deathUI.SetActive(true);
        gameplayUI.SetActive(false);

        Invoke("ReloadCurrentScene", 2f);
    }


    void ReloadCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
}