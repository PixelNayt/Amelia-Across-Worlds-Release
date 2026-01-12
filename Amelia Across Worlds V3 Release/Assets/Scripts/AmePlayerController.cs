using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmePlayerController : MonoBehaviour
{
    //References & Other
    private CharacterController characterController;
    private float ySpeed; //Come back after jump, keeps track of speed in y-direction
    private float originalStepOffset; //Just a note so I don't break things

    private float? lastGroundedTime; //The question mark means this variable will either have a float value or be null
    private float? jumpButtonPressedTime;

    private Animator animator;

    public GameObject pauseMenu;
    public GameObject optionsScreen;

    public GameObject gameOver;

    public GameObject player;
    public GameObject WaterVolume;


    //Movement
    [SerializeField]
    public float maximumSpeed;

    //Jumping
    [SerializeField]
    public float jumpSpeed;

    [SerializeField]
    public float jumpButtonGracePeriod;

    //Rotation and Look
    [SerializeField]
    public float rotationSpeed;

    [SerializeField]
    private Transform cameraTransform;


    void Start()
    {
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    void Update()
    {
        //UNDERWATER EFFECT
        //Activates our WaterVolume Effects in the Hierarchy Layers in the Unity Editor
        //If player position is y=24 or below the camera gets the post processing water effects we made
        if(player.transform.position.y <= 24)
        {
            WaterVolume.SetActive(true);
        }
        else
        {
            WaterVolume.SetActive(false);
        }

        //CURSOR STATE
        if (Time.timeScale == 0f)
        {
            //Cursor is unlocked and visible is shown and interaction with pause menu is possible
            //cuz the only time Time flow is set to 0 is when game is "paused"
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            //When player resumes (Time is back to normal), the game cursor goes back into a locked state and cursor is no longer visible
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        //MOVEMENT
        //Variables that store the axes
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Vector3 movementDirection uses the axes based on Input of the Player (e.g., WASD)
        //Input magnitude is speed
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        //If left shift or right shift key is pressed, ame uses her zoom powah - SPEED!!!
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputMagnitude *= 2;
        }

        //Access Animator parameter and change animation based on inputMagnitude or speed
        animator.SetFloat("Input Magnitude", inputMagnitude);

        float speed = inputMagnitude * maximumSpeed;

        //Move Camera based on movement direction
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;

        movementDirection.Normalize();



        //JUMPING
        ySpeed += Physics.gravity.y * Time.deltaTime;

        //Checkers
        //Character will ONLY jump if on the ground, but also give leeway or delay when jumping in the air for ease of life
        if (characterController.isGrounded)
        {
            //Time.time is the number of seconds since the game has started
            lastGroundedTime = Time.time; 
        }

        if (Input.GetButtonDown("Jump"))
        {
            //Check if jump is pressed
            jumpButtonPressedTime = Time.time;

        }

        //JUMP ACTION
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            //If character is on the ground make ySpeed = 0 so that we prevent ySpeed from decreasing,
            //leading to instant big gravity pulls, -0.5 is used for ground detection 
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                //Set checkers back to null, so we can't jump to infinity
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }

        } 
        else
        {
            characterController.stepOffset = 0;
        }


        //ACTUAL MOVEMENT
        //Moves the player by translating the input to the player transform
        Vector3 velocity = movementDirection * speed;
        velocity = AdjustVelocityToSlope(velocity);
        velocity.y += ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        //Check if character is not moving, if moving we want Amelia/Player to rotate and face the direction of movement
        if (movementDirection != Vector3.zero)
        {
            //Quaternion at this simplest form is used to store the rotation value
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            //Transform the rotation of the player model to look at the direction of movement smoothly
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        //This method is used to prevent bouncing off extreme slopes, done by detecting the slope of the ground by using a raycast or rays
        //Rays are like lines emitted from a source in a direction, here the ray source is the position of the character and direction is down
        var ray = new Ray(transform.position, Vector3.down);

        //Using physics raycast we check for collissions, RaycastHit will get the info and find the direction the ground is facing
        //0.5f is used so we only detect things close to the bottom of the player
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.5f))
        {
            //If there is a collision, we redirect the player normally
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }

    //DETECT ENEMY COLLISSION & GAME OVER
    private void OnCollisionEnter(Collision collision)
    {
        //We added a new tag to the Enemy called literally "Enemy", if our player collides with anything named that tag activate this script
        if (collision.gameObject.tag == "Enemy")
        {
            gameOver.SetActive(true);
            Time.timeScale = 0;
        }
    }
    
}
