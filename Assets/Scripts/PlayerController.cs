using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //Private variables, leave unchanged.
    CharacterController controller;
    float movementRotationVel;
    bool running = false;
    float stamina;
    bool superJumping = false;
    float superJumpPower = 0f;
    float verticalVel = 0f;
    float groundCheckRadius = .1f;
    float timeSinceLastSuperJump = 5f;
    bool startSuperJumpCountDown = false;

    //Public variables, change in the inspector. Most values were set by trial and error
    public Transform cam;
    public float walkingSpeed = 10f;
    public float runningSpeed = 20f;
    public float maxStamina = 7f; //Seconds
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float gravityValue = 9.8f;
    public float jumpPower = 1.5f;
    public float superJumpBasePower = 2f;
    public float superJumpMaxPower = 3f;
    public float superJumpFill = .1f; //0.1f increase per second
    public float superJumpCoolDown = 5f; //Seconds
    public float playerTurnSmoothness = .15f; //less value = faster player rotation

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        timeSinceLastSuperJump = superJumpCoolDown; //The character will be allowed to jump as soon as the game starts
        stamina = maxStamina;
    }

    void Update()
    {
        bool grounded =  Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround); //Check if the player is on the ground
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; //Get the H and V input data 

        if(grounded && verticalVel < 0) //Reset the vertical velocity if the character is grounded 
        {
            verticalVel = 0f;
        }

        if(Input.GetKeyDown(KeyCode.Space) && timeSinceLastSuperJump >= superJumpCoolDown) //Only start super jumping if the time since the last supe jump has gone beyond the cool down. The second condition is always true after the game is started.   
        {
            superJumping = true;
            superJumpPower = superJumpBasePower;
            startSuperJumpCountDown = false;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            superJumping = false;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)) //Running
        {
            running = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            running = false;
        }

        if (grounded && !superJumping && superJumpPower != 0f && movement.magnitude >= .1f && running) //If all the conditions are met, the player is going to super jump
        {
            verticalVel += Mathf.Sqrt(superJumpPower * 3.0f * gravityValue); 
            superJumpPower = 0f;
            startSuperJumpCountDown = true; //Start the cool down as soon as the player super jumps.
            timeSinceLastSuperJump = 0f;
        }
        else if (grounded && !superJumping && superJumpPower == 0f) //Always jump when standing still or when not charging the super jump
        {
            verticalVel += Mathf.Sqrt(jumpPower * 3.0f * gravityValue);
        }
        else if (!superJumping && superJumpPower != 0f) //Reset the super jump variables so the player could jump normally. This only happens if the player doesn't run after releasing the super jump button 
        {
            superJumping = false;
            superJumpPower = 0f;
        }

        if(superJumping && grounded) //Charge the super jump if the player is on the ground
        {
            superJumpPower += superJumpFill * Time.deltaTime;
            superJumpPower = Mathf.Clamp(superJumpPower, superJumpBasePower, superJumpMaxPower);
        }

        if (!grounded) //The player is in the air, apply gravity
        {
            verticalVel -= gravityValue * Time.deltaTime;
        }

        if(movement.magnitude >= .1f && !superJumping) //Calculate and move the player
        {
            //First rotate the player to the face movement vector 
            float targetMovementAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float movementAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetMovementAngle, ref movementRotationVel, playerTurnSmoothness);
            transform.rotation = Quaternion.Euler(0f, movementAngle, 0f);

            //Then move the player in the direction that the camera faces 
            Vector3 dir = Quaternion.Euler(0f, targetMovementAngle, 0f) * Vector3.forward;
            controller.Move(dir.normalized * (running && stamina > 0? runningSpeed : walkingSpeed) * Time.deltaTime);

            if(running) //Decrease the stamina of the player if running
            {
                stamina -= Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
            }
        }
        else if (movement.magnitude <= .1f) //If the player is taking a rest, restore his stamina
        {
            stamina += Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
        }
        
        controller.Move(new Vector3(0f, verticalVel, 0f) * Time.deltaTime); //Gravity and jumping movements

        if (startSuperJumpCountDown) //Update the time since the last super jump
        {
            timeSinceLastSuperJump += Time.deltaTime;
        }
    }
}