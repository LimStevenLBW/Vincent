using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    //Player Owned Gameobjects
    public Stats playerStats;
    public PlayerCanvas playerCanvas;
    public WeaponController weaponController;
    private Animator animator;

    public PlayerAudio playerAudio;
    public ViewModel viewModel;
    public Attack attack;
    public Projectile fireballPrefab;
    public Rigidbody body;
    public Camera playerCamera;

    //Camera Values
    public Transform rotatorJoint;
    public float yaw;
    public float pitch;
    public float mouseSensitivity;

    //Player Movement
    public float gravityBoost;
    private Vector3 direction;
    private bool isGrounded;
    private bool isMoving;
    private bool isDashing;

    private Coroutine DashCoroutine;
    private Coroutine AttackCoroutine;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            playerCamera.enabled = false;

            gameObject.layer = LayerMask.NameToLayer("Default");
            foreach(Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }

        }

        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas").GetComponent<PlayerCanvas>();
        playerCanvas.SetHealthBar(playerStats.GetHealth());
        playerCanvas.UpdateGoldCounterText(playerStats.GetGold());
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;


        if (playerStats.GetHealth() <= 0)
        {
            playerCanvas.ShowGameOverScreen();

            return;
        }

        DebugConsoleFocus();
        CameraControl();
        Move();
        Jump();
        Attack();
    }

    //For Reclaiming the mouse to check console window
    void DebugConsoleFocus()
    {
        if(Input.GetKeyDown("t")) Cursor.lockState = CursorLockMode.None;
    }

    void FixedUpdate()
    {
        body.AddForce(Vector3.down * gravityBoost);
    }

    void Move()
    {
        direction = new Vector3(0, 0, 0);

        //Resets the isMoving boolean
        isMoving = false;

        if (Input.GetKey("w"))
        {
            direction += transform.forward;
        }
        if (Input.GetKey("s"))
        {
            direction += transform.forward * -1;
        }
        if (Input.GetKey("a"))
        {
            direction += transform.right * -1;
        }
        if (Input.GetKey("d"))
        {
            direction += transform.right;
        }

        //Checks if we are moving in any direction
        if (direction.x != 0 || direction.y != 0 || direction.z != 0) isMoving = true;
        animator.SetBool("isRunning", isMoving);

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && !isDashing)
        {

            if (DashCoroutine != null) StopCoroutine(DashCoroutine);

            isDashing = true;
            DashCoroutine = StartCoroutine(Dashing(direction));
            //StartCoroutine(Rolling(direction));
        }


        if (!isDashing) transform.position += direction.normalized * playerStats.GetSpeed() * Time.deltaTime;

    }
    IEnumerator Rolling(Vector3 direction)
    {
        /*
        float time = 0;
        while(time < 1)
        {
            transform.position = Vector3.Slerp(transform.position, transform.position + direction, time);

            time += Time.deltaTime * speed * 5;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        */

        // Vector3 rotation = transform.rotation.eulerAngles;
        //rotation.y += 90;
        //rotatorJoint.localRotation = Quaternion.Euler(rotation);


        for (int i = 0; i < 360; i++)
        {
            rotatorJoint.Rotate(direction.normalized, Space.Self);
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator Dashing(Vector3 direction)
    {
        viewModel.SetRunningIdle();

        //Dashing
        body.AddForce(direction.normalized * 1500);
        yield return new WaitForSeconds(0.3f);

        //Slow down the character
        body.velocity *= 0.5f;
        body.angularVelocity *= 0.5f;

        //Regular movement becomes available
        isDashing = false;


        viewModel.SetIdle();
        //Cooldown 
        yield return new WaitForSeconds(0.2f);

    }


    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerAudio.PlayAudio("jump");
            isGrounded = false;
            body.velocity += new Vector3(0, playerStats.GetJumpSpeed(), 0);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0)) //LEFT CLICK
        {

            if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);

            AttackCoroutine = StartCoroutine(Attacking());

        }
        else if (Input.GetMouseButtonDown(1)) //RIGHT CLICK
        {

            Vector3 cameraForward = playerCamera.transform.forward;

            Vector3 spawnPos = transform.position + cameraForward * 2;
            spawnPos.y += 1f; //Raise the projectile a little bit off the ground


            Projectile projectile = Instantiate(fireballPrefab, spawnPos, Quaternion.identity); //SPAWN THE PROJECTILE

            playerAudio.PlayAudio("fireball"); //Play the sound effect
            Rigidbody pBody = projectile.GetComponent<Rigidbody>(); //Get the rigidbody
            pBody.AddForce(cameraForward * projectile.GetForce()); //Apply the force
        }

    }

    IEnumerator Attacking()
    {
        viewModel.PlayAttackAnim();
        playerAudio.PlayAudio("attack");
        attack.Activate(playerStats.GetPower() + weaponController.weapon.attackPower);
        yield return new WaitForSeconds(0.3f);
        attack.Deactivate();
    }

    void CameraControl()
    {
        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");

        // Clamp pitch between lookAngle
        pitch = Mathf.Clamp(pitch, -80, 80);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();

            if (zombie.GetIsActive())
            {
                playerStats.TakeDamage(zombie.zombieStats.GetPower());
                playerAudio.PlayAudio("tookDamage");
                playerCanvas.SetHealthBar(playerStats.GetHealth());
            }
        }

        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

    }

    public bool GetGroundedState()
    {
        return isGrounded;
    }

    public bool GetIsMovingState()
    {
        return isMoving;
    }

    public void ResetHealth()
    {
        playerStats.ResetHealth();
        playerCanvas.SetHealthBar(playerStats.GetHealth());
    }

    public void AddGold(int gold)
    {
        playerStats.AddGold(gold);
        playerCanvas.UpdateGoldCounterText(playerStats.GetGold());
    }
    public int GetGold()
    {
        return playerStats.GetGold();
    }

    public void SetGold(int gold)
    {
        playerStats.SetGold(gold);
        playerCanvas.UpdateGoldCounterText(playerStats.GetGold());
    }

    public void ShowShop()
    {
        playerCanvas.ShowShop();
    }

    public void HideShop()
    {
        playerCanvas.HideShop();
    }
}
