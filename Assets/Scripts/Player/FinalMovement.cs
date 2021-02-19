using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMovement : MonoBehaviour
{
    // Movement
    public float speed = 10f;
    public float sprint = 0.4f;
    public float rotationSpeed = 2.0f;
    private float yVelocity = 0f;
    public float jumpSpeed = 15.0f;
    public float gravity = 30.0f;


    // Health bar
    public int maxHealth = 3;
    public int currentHealth;
    public HealthBar healthBar;

    // Controller
    private CharacterController controller;

    // Animator
    public Animator anim;


    private float currentCameraHeadRotation = 0;
    private float maxCameraHeadRotation = 80.0f;
    private float minCameraHeadRotation = -80.0f;
    public Transform followTarget;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        playAnimations();

        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.Rotate(Vector3.up, mouseInput.x * rotationSpeed);
        // followTarget.Rotate(Vector3.right, mouseInput.y * rotationSpeed);

        currentCameraHeadRotation = Mathf.Clamp(currentCameraHeadRotation + mouseInput.y * rotationSpeed, minCameraHeadRotation, maxCameraHeadRotation);
        followTarget.localRotation = Quaternion.identity;
        followTarget.Rotate(Vector3.left, currentCameraHeadRotation);



        jump();

        // Movement
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        yVelocity -= gravity * Time.deltaTime;
        controller.Move(transform.TransformDirection(input.normalized * speed * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime));

        if (Input.GetKey(KeyCode.LeftShift) && StaminaBar.instance.currentStamina > 0.1f)
        {
            controller.Move(transform.TransformDirection(input.normalized * speed * sprint * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime));
            StaminaBar.instance.UseStamina(0.05f);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && StaminaBar.instance.currentStamina <= 0f)
        {
            controller.Move(transform.TransformDirection(input.normalized * speed * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime));
        }
    }

    void playAnimations()
    {
        if (Input.GetKey("w"))
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (Input.GetKey("a"))
        {
            anim.SetBool("isStrafeL", true);
        }
        else
        {
            anim.SetBool("isStrafeL", false);
        }

        if (Input.GetKey("d"))
        {
            anim.SetBool("isStrafeR", true);
        }
        else
        {
            anim.SetBool("isStrafeR", false);
        }

        if (Input.GetKey("s"))
        {
            anim.SetBool("isWalkingBackW", true);
        }
        else
        {
            anim.SetBool("isWalkingBackW", false);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("isRolling", true);
        }
        else
        {
            anim.SetBool("isRolling", false);
        }
    }

    void jump()
    {
        // Prevents the isGrounded bug/error where sometimes you can't jump!
        if (controller.isGrounded)
        {
            yVelocity = -controller.stepOffset / Time.deltaTime;

            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
                anim.SetBool("isJumping", true);
            }
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;
            anim.SetBool("isJumping", false);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 1)
        {
            DeathScreen();
            reEnableMouseCursor();
        }
    }

    // Deals damage to player based upon colliding with an object that has "Sword" tag.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sword" || collision.gameObject.tag == "EnemyProjectile")
        {
            TakeDamage(1);
            healthBar.SetHealth(currentHealth);
        }
    }

    public void DeathScreen()
    {
        // Remember to update this number as more scenes are created
        SceneManager.LoadScene(6);
    }

    void reEnableMouseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
