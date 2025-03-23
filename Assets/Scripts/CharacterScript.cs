using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CharacterScript : MonoBehaviour
{
    private Animator animator;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.5f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;
    private MoveState prevMoveState = MoveState.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveState moveState = (MoveState) animator.GetInteger("MoveState");

        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            if (moveState == MoveState.Jump)
            {
                moveState = MoveState.JumpFinish;
            }
        }
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float sprintValue = sprintAction.ReadValue<float>();

        if (sprintValue > 0 && moveValue.magnitude > 0)
        {
            if (GameState.stamina > 0.01f)
            {
                GameState.stamina -= Time.deltaTime;
            }
            else
            {
                sprintValue = 0;
            }
        }
        else
        {
			if (GameState.stamina < GameState.maxStamina)
			{
				GameState.stamina += Time.deltaTime;
                if (GameState.stamina > GameState.maxStamina)
                    GameState.stamina = GameState.maxStamina;
			}
		}

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        if(cameraForward != Vector3.zero)
        {
            cameraForward.Normalize();
        }
        Vector3 moveStep = playerSpeed * Time.deltaTime * (1 + 2 * sprintValue) * (
            moveValue.x * Camera.main.transform.right +
            moveValue.y * cameraForward
        );
        if (!IsJumping(moveState))
        {
            if (moveStep.magnitude > 0f)
            {
                moveState = Mathf.Abs(moveValue.x) > Mathf.Abs(moveValue.y)
                ? (sprintValue != 0 ? MoveState.SideRun : MoveState.SideWalk)
                : (sprintValue != 0 ? MoveState.Run : MoveState.Walk);

                this.transform.forward = cameraForward;
            }
            else
            {
                moveState = MoveState.Idle;
            }
        }
        characterController.Move(moveStep);
        // Makes the player jump
        if (jumpAction.ReadValue<float>() > 0 && groundedPlayer)
        {
            moveState = MoveState.JumpStart;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
        if(moveState != prevMoveState)
        {
            animator.SetInteger("MoveState", (int)moveState);
            prevMoveState = moveState;
        }
        
    }

    private bool IsJumping(MoveState moveState)
    {
        return moveState == MoveState.JumpStart
                || moveState == MoveState.Jump
                || moveState == MoveState.JumpFinish;
    }

    private void OnJumpStartAnimationEnds()
    {
        animator.SetInteger("MoveState", (int)MoveState.Jump);
    }
    private void OnJumpFinishAnimationEnds()
    {
        animator.SetInteger("MoveState", (int)MoveState.Idle);
    }
}

enum MoveState
{
    Idle = 1,
    Walk = 2,
    SideWalk = 3,
    Run = 4,
    SideRun = 5,
    JumpStart = 6,
    Jump = 7,
    JumpFinish = 8,
}

/*
  jumpAction(6)            grounded(8)
     |                         |
     |    time(7)              |
 JumpStart ---> Jump (repeat) --- JumpFinish --> Idle
 */
