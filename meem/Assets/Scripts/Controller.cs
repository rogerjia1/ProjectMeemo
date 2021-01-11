using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    /* Controller includes:
        Player Horizontal Movement, Jump and Double Jump with GroundCheck on FLAT ground
        Animation switcher between idle and running
        Camera that scrolls with the character
    */
    Rigidbody2D player;
    public Transform CameraSideScroll;
    public const string RIGHT = "right";
    public const string LEFT = "left";
    public const string IDLE = "Idle";
    public const string RUNNING = "Running";
    string buttonPressed;
    string currentAnimation;
    private Animator animator;
    public float jumpHeight;
    public int JumpCount = 2;
    public LayerMask groundLayer;
    public bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpHeight = 8;
    }

    // eulerAngles just flips sprite depending on movement direction
    void Update()
    {
        #region HORIZONTAL MOVEMENT AND JUMPING
        if (Input.GetKey(KeyCode.RightArrow))
        {
            buttonPressed = RIGHT;
            transform.Translate(Vector2.right * (Time.deltaTime * 5), Space.World);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            buttonPressed = LEFT;
            transform.Translate(Vector2.left * (Time.deltaTime * 5), Space.World);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            buttonPressed = null;
        }
        if (Input.GetKey(KeyCode.UpArrow) && grounded == true && JumpCount <= 2 && JumpCount > 0)
        {
            player.velocity = new Vector2(player.velocity.x, jumpHeight);
            JumpCount = JumpCount - 1;
        }
        #endregion
        // Camera Side Scroll
        float playerx = player.position.x;
        transform.position = new Vector3(playerx, transform.position.y, -1);
    }
    public void isGrounded()
    {
        // MUST EDIT OVERLAP AREA DEPENDING ON SPRITE SIZE
        grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.64f, transform.position.y - 0.94f), new Vector2(transform.position.x + 0.64f, transform.position.y - 0.94f), groundLayer);
        if (grounded == true && JumpCount <= 0)
        {
            JumpCount = 2;
        }
    }
    private void FixedUpdate()
    {
        isGrounded();
        #region ANIMATION BOOLEANS
        if (buttonPressed == RIGHT || buttonPressed == LEFT)
        {
            animator.SetBool("Running", true);
        }
        else if (buttonPressed == null)
        {
            animator.SetBool("Running", false);
        }
        #endregion
    }
}
