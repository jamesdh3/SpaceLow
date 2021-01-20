using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicMovement : MonoBehaviour
{
    public float speed = 10f;
    public float sprint = 0.4f;
    public float rotationSpeed = 2.0f;
    private float yVelocity = 0f;
    public float jumpSpeed = 15.0f;
    public float gravity = 30.0f;

    public Transform cameraHead;
    public float maxCameraHeadRotation = 80.0f;
    public float minCameraHeadRotation = -80.0f;
    private float currentCameraHeadRotation = 0;

    private CharacterController controller;

    public int maxHealth = 3;
    public int currentHealth;
    public HealthBar healthBar;

    public Animator anim;


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



        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.Rotate(Vector3.up, mouseInput.x * rotationSpeed);
        currentCameraHeadRotation = Mathf.Clamp(currentCameraHeadRotation + mouseInput.y * rotationSpeed, minCameraHeadRotation, maxCameraHeadRotation);

        cameraHead.localRotation = Quaternion.identity;
        cameraHead.Rotate(Vector3.left, currentCameraHeadRotation);

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        yVelocity -= gravity * Time.deltaTime;
        gameObject.GetComponent<CharacterController>().Move(transform.TransformDirection(input.normalized * speed * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime));

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift) && StaminaBar.instance.currentStamina > 0.1f)
        {
            gameObject.GetComponent<CharacterController>().Move(transform.TransformDirection(input.normalized * speed * sprint * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime));
            StaminaBar.instance.UseStamina(0.05f);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && StaminaBar.instance.currentStamina <= 0f)
        {
            gameObject.GetComponent<CharacterController>().Move(transform.TransformDirection(input.normalized * speed * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime));
        }


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

            // re-enable mouse cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
        SceneManager.LoadScene(3);
    }
}
