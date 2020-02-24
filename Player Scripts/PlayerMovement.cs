using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float Speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public bool _CeilingSpin1;
    public bool _CeilingSpin2;

    public Canvas uiCanvas;
    public Canvas DeathScreen;
    public Canvas WinScreen;
    public Canvas PauseScreen;
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public GameObject ShieldObject;
    public GameObject pickUpEffect;
    public bool isFiringDetection, _gameWon;


    public float Stamina = 100.0f;
    public float MaxStamina = 100.0f;
    public MainMenu MenuScript;

    public float Shield = 100.0f;
    public float MaxShield = 100.0f;
    //---------------------------------------------------------
    private float StaminaRegenTimer = 0.0f;
    private float ShieldRegenTimer = 0.0f;
    //---------------------------------------------------------
    private const float StaminaDecreasePerFrame = 20f;
    private const float StaminaIncreasePerFrame = 10f;
    private const float StaminaTimeToRegen = 1.5f;
    private const float ShieldDecreasePerFrame = 20f;
    private const float ShieldIncreasePerFrame = 10f;
    private const float ShieldTimeToRegen = 0.5f;

    public bool playerAlive;

    public float damage;
    public float MaxHealth;
    public float CurrentHealth;
    public GameObject rearCam;

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        rearCam.SetActive(false);
        MenuScript = GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenu>();
        Time.timeScale = 1;
        _gameWon = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        uiCanvas.gameObject.SetActive(true);
        DeathScreen.gameObject.SetActive(false);
        WinScreen.gameObject.SetActive(false);
        PauseScreen.gameObject.SetActive(false);
        ShieldObject.SetActive(false);
        MaxHealth = 100f;
        CurrentHealth = MaxHealth;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            rearCam.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            rearCam.SetActive(false);
        }

        if (MenuScript.Hard == true)
        {
            SetHard();
        }
        else if (MenuScript.Medium == true)
        {
            SetMedium();
        }
        else if (MenuScript.Easy == true)
        {
            SetEasy();
        }

        bool isAlive = CurrentHealth > 0.0f;
        //damage = Random.Range(10f, 25f);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }

        if (isAlive)
        {
            playerAlive = true;
            if (_gameWon == false)
            {
                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");
                Vector3 move = transform.right * x + transform.forward * z;
                controller.Move(move * Speed * Time.deltaTime);

                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }

                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);

                bool isRunning = Input.GetKey(KeyCode.LeftShift);
                if (isRunning && Stamina > 0)
                {
                    Speed = 16f;
                    Stamina = Mathf.Clamp(Stamina - (StaminaDecreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
                    StaminaRegenTimer = 0.0f;
                }
                else if (Stamina < MaxStamina)
                {
                    Speed = 12f;
                    if (StaminaRegenTimer >= StaminaTimeToRegen)
                        Stamina = Mathf.Clamp(Stamina + (StaminaIncreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
                    else
                        StaminaRegenTimer += Time.deltaTime;
                }

                bool isShielded = Input.GetMouseButton(1);
                if (isShielded && Shield > 0)
                {
                    ShieldObject.SetActive(true);
                    Shield = Mathf.Clamp(Shield - (ShieldDecreasePerFrame * Time.deltaTime), 0.0f, MaxShield);
                    ShieldRegenTimer = 0.0f;
                }
                else if (isShielded && Shield == 0)
                {
                    ShieldObject.SetActive(false);
                }
                else if (!isShielded && Shield < MaxShield)
                {
                    ShieldObject.SetActive(false);
                    if (ShieldRegenTimer >= ShieldTimeToRegen)
                        Shield = Mathf.Clamp(Shield + (ShieldIncreasePerFrame * Time.deltaTime), 0.0f, MaxShield);
                    else
                        ShieldRegenTimer += Time.deltaTime;
                }


                bool isFiring = Input.GetMouseButton(0);
                if (isFiring)
                {
                    if (Time.time > nextFire)
                    {
                        nextFire = Time.time + fireRate;
                        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                    }
                    isFiringDetection = true;
                }
            }
        }

        else if (!isAlive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            uiCanvas.gameObject.SetActive(false);
            DeathScreen.gameObject.SetActive(true);
            playerAlive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
  
    {
        if (other.gameObject.tag == "Trigger")
        {
            other.gameObject.SetActive(false);
            _CeilingSpin1 = true;

        }
        if (other.gameObject.tag == "Object")
        {
            Instantiate(pickUpEffect, transform.position, transform.rotation);
            other.gameObject.SetActive(false);
            _CeilingSpin2 = true;

        }
        if (other.gameObject.tag == "Object2")
        {
            Instantiate(pickUpEffect, transform.position, transform.rotation);
            _gameWon = true;
            CurrentHealth = MaxHealth;
            other.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            uiCanvas.gameObject.SetActive(false);
            WinScreen.gameObject.SetActive(true);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Shot")
        {
            CurrentHealth = CurrentHealth - damage;
        }
    }

    public void SetHard()
    {
        damage = UnityEngine.Random.Range(35f, 50f);

    }
    public void SetMedium()
    {
        damage = UnityEngine.Random.Range(20f, 25f);

    }
    public void SetEasy()
    {
        damage = UnityEngine.Random.Range(15f, 20f);

    }

}
