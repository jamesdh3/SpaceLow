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

    // Controls turning of the camera with the mouse
    private float currentCameraHeadRotation = 0, maxCameraHeadRotation = 80.0f, minCameraHeadRotation = -80.0f;
    public Transform followTarget;

    // Change color when hit
    private Renderer rend;
    private Color red = Color.red;
    private Color white = Color.white;
    private Color green = Color.green;
    private Transform player;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("PlayerSkinRender").transform;
        rend = player.GetComponent<Renderer>();
    }

    void Update()
    {
        playerCameraMouseStrafe();
        jump();

        // Movement
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        yVelocity -= gravity * Time.deltaTime;
        controller.Move(transform.TransformDirection(input.normalized * speed * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime));
        // Sprint
        if (Input.GetKey(KeyCode.LeftShift) && controller.velocity.magnitude > 0.1f && StaminaBar.instance.currentStamina > 0.1f)
        {
            controller.Move(transform.TransformDirection(input.normalized * speed * sprint * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime));
            StaminaBar.instance.UseStamina(0.05f);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && StaminaBar.instance.currentStamina <= 0f)
        {
            controller.Move(transform.TransformDirection(input.normalized * speed * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime));
        }
    }

    private void LateUpdate()
    {
        flashGreen();
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

    void playerCameraMouseStrafe()
    {
        // Control left and right turning of camera/player with mouse
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.Rotate(Vector3.up, mouseInput.x * rotationSpeed);

        // Control up and down turning of camera with mouse
        currentCameraHeadRotation = Mathf.Clamp(currentCameraHeadRotation + mouseInput.y * rotationSpeed, minCameraHeadRotation, maxCameraHeadRotation);
        followTarget.localRotation = Quaternion.identity;
        followTarget.Rotate(Vector3.left, currentCameraHeadRotation);
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

    void OnCollisionEnter(Collision collision)
    {
        // Take Damage Sound Effect
        TakeDamageSound scriptDamageSound = GameObject.FindObjectOfType(typeof(TakeDamageSound)) as TakeDamageSound;

        if (collision.gameObject.tag == "Sword" || collision.gameObject.tag == "EnemyProjectile")
        {
            TakeDamage(1);
            scriptDamageSound.TakeDmgSoundEffect();
            StartCoroutine(FlashRedWhenHit());

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
    
    IEnumerator FlashRedWhenHit()
    {
        rend.material.color = red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = white;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = white;
    }

    IEnumerator FlashGreenWhenLowStamina()
    {
        rend.material.color = white;
        
        yield return new WaitForSeconds(0.09f);
        rend.material.color = green;
        yield return new WaitForSeconds(0.09f);
        rend.material.color = white;

    }

    void flashGreen()
    {
        if (Input.GetKey(KeyCode.LeftShift) && StaminaBar.instance.currentStamina <= 0.2f)
        {
            StartCoroutine(FlashGreenWhenLowStamina());
        }
    }

}
