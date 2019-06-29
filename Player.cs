using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour

{
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    public Animator animator;
    public bool attack;

    //  Stats
    public int curHealth;
    public int maxHealth = 100;


    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    private GameMaster gm;



    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);

        curHealth = maxHealth;
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }




    void Update()
    {



        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        Vector3 characterScale = transform.localScale; ;
        if (Input.GetAxis("Horizontal") < 0)
        {

            characterScale.x = -0.1798278f;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = 0.1798278f;
        }

        transform.localScale = characterScale;

        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));

        HandleInput();

        HandleAttack();

        ResetValues();

        if(curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
        if(curHealth <= 0)
        {
            Die();
        }


    }

    void Die()
    {
        //  Restart Game
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Damage(int dmg)
    {
        curHealth -= dmg;
        gameObject.GetComponent<Animation>().Play("RedFlash");
    }
    


    private void HandleAttack()
    {
        if (attack)
        {
            animator.SetTrigger("Attack");
        }
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            attack = true;
        }
    }

    private void ResetValues()
    {
        attack = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Coin"))
        {
            Destroy(col.gameObject);
            gm.gems += 1;        }
    }
}    