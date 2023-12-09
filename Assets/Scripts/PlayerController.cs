using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 6f;
    public float moveSpeed = 5f;
    public float buffer = 0.4f;
    
    public LayerMask groundLayer; // Set this in the inspector to the layer where your platforms are.
    [SerializeField] public Joystick joystick;

    public Animator playerAnimator;
    
    [SerializeField] private AudioSource jumpSound;

    private bool isGrounded = false;
    private Rigidbody2D rb;
    private Collider2D playerCollider;  // Add a reference to the player's collider
    
    // Set these variables to define the camera bounds
    private int currentHighScore = 0;
    private int maxPlatformIndex = -1; // The index of the platform the player is standing on

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        GameManager.Instance.canMoveY = 0;

        currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        HighScoreManager.instance.UpdateScore(currentHighScore);
    }

    void Update()
    {
        // Check if the player is grounded using colliders
        isGrounded = Physics2D.IsTouchingLayers(playerCollider, groundLayer);

        if (isGrounded)
        {
            playerAnimator.SetBool("Landing", false);
            Collider2D platformCollider = GetPlatformColliderUnderPlayer();
            if (platformCollider != null)
            {
                Platform platform = platformCollider.GetComponent<Platform>();
                if (platform != null)
                {
                    int platformIndex = platform.platformIndex;
                    if (platformIndex > maxPlatformIndex)
                    {
                        // Player is on a new platform
                        maxPlatformIndex = platformIndex;
                        PlayerPrefs.SetInt("CurrentScore", maxPlatformIndex);
                        ScoreManager.instance.UpdateScore(maxPlatformIndex);

                        if(platformIndex == 1 || platformIndex == 2)
                        {
                            GameManager.Instance.canMoveY = 1;
                        }
                        
                        if(maxPlatformIndex > currentHighScore)
                        {
                            currentHighScore = maxPlatformIndex;
                            
                            PlayerPrefs.SetInt("HighScore", currentHighScore);
                            HighScoreManager.instance.UpdateScore(currentHighScore);
                        }
                    }
                }
            }
        }

        if ((joystick.Vertical >= 0.3f || Input.GetButtonDown("Jump")) && isGrounded)
        {
            jumpSound.Play();
            rb.velocity = Vector2.up * jumpForce;
            playerAnimator.SetBool("Running", false);
            playerAnimator.SetBool("Jumping", true);
        }

        if(rb.velocity.y < 0 & !isGrounded)
        {
            playerAnimator.SetBool("Jumping", false);
            playerAnimator.SetBool("Landing", true);
        }

        float joystickHorizontalInput = joystick.Horizontal;

        float keyboardHorizontalInput = Input.GetAxis("Horizontal");

        float horizontalInput = joystick.Horizontal != 0f ? joystick.Horizontal : Input.GetAxis("Horizontal");

        if(horizontalInput != 0f)
        {
            if (horizontalInput < 0f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f); // Flip horizontally
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f); // Reset scale if moving right
            }
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            playerAnimator.SetBool("Running", true);
        }
        else{
            rb.velocity = new Vector2(0, rb.velocity.y);
            playerAnimator.SetBool("Running", false);
        }

        // Constrain player position within camera bounds
        float clampedX = Mathf.Clamp(transform.position.x, GameManager.Instance.minX, GameManager.Instance.maxX);
        float clampedY = Mathf.Clamp(transform.position.y, float.NegativeInfinity, GameManager.Instance.maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    Collider2D GetPlatformColliderUnderPlayer()
    {
        // Cast a ray downward from the player to detect the platform
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, buffer+0.2f, groundLayer);
        if (hit.collider != null)
        {
            return hit.collider;
        }
        return null;
    }

    private void OnBecameInvisible()
    {
        if(GameManager.Instance.bgm != null){
            GameManager.Instance.bgm.Stop();
        }
        if(GameManager.Instance.endfx != null){
            GameManager.Instance.endfx.Play();
        }
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}