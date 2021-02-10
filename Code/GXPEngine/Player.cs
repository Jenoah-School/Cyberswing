using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Player : Sprite
{
    private bool canMove = true;

    private Vector2 inputVelocity = new Vector2();

    private Vector2 globalVelocity = new Vector2();
    private Vector2 previousPosition = new Vector2();

    //Speeds and heights
    private float moveAccelerationSpeed = 8f;
    private float grappleAccelerationSpeed = 1f;

    private float fallSpeed = 4f;
    private float jumpHeight = 10f;

    private float maxMoveSpeed = 4f;
    private float maxGrappleSpeed = 1.25f;

    private float health = 100f;

    private bool isGrounded = false;
    private bool isGrappling = false;

    private Sprite grappleCable = null;

    GrapplePoint targetGrapplePoint = null;

    public Player() : base("Assets/Sprites/square.png")
    {
        SetOrigin(width / 2, height / 2);

        grappleCable = new Sprite("Assets/Sprites/square.png", false, false);
        grappleCable.SetOrigin(grappleCable.width / 2, 0);

        grappleCable.width = 8;
        grappleCable.SetColor(.1f, .1f, .1f);
        grappleCable.visible = false;

        AddChild(grappleCable);
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isGrappling = false;
            grappleCable.visible = false;
            targetGrapplePoint = null;
        }

        if (Input.GetMouseButton(0) && isGrappling)
        {
            Grapple();
        }
        else
        {
            Move();
        }

        x += inputVelocity.x;
        y += inputVelocity.y;

        if (targetGrapplePoint != null)
        {
            grappleCable.height = (int)Vector2.Distance(new Vector2(x, y), new Vector2(targetGrapplePoint.x, targetGrapplePoint.y));
            grappleCable.rotation = Mathf.Atan2(x - targetGrapplePoint.x, targetGrapplePoint.y - y) * 180f / Mathf.PI;
        }

        globalVelocity = new Vector2(x, y) - previousPosition;

        previousPosition.x = x;
        previousPosition.y = y;
    }

    #region Movement
    /// <summary>
    /// Moves the player to given input direction
    /// </summary>
    private void Move()
    {
        float horizontal = GameBehaviour.GetHorizontalAxis();

        inputVelocity.x += horizontal * moveAccelerationSpeed;// / Time.deltaTime;

        if (isGrounded)
        {
            inputVelocity.x *= 0.75f;
        }

        if (inputVelocity.x < 0.01f && inputVelocity.x > -0.01f)
        {
            inputVelocity.x = 0f;
        }

        inputVelocity.x = Mathf.Clamp(inputVelocity.x, -maxMoveSpeed, maxMoveSpeed);

        if (Input.GetKeyDown(Key.SPACE) && isGrounded)
        {

            inputVelocity.y -= jumpHeight;
        }

        Fall();

        isGrounded = false;
    }

    /// <summary>
    /// Applies a modified version of gravity to the player
    /// </summary>
    private void Fall()
    {
        inputVelocity.y += -GameBehaviour.gravity / Time.deltaTime;
        inputVelocity.y = Mathf.Clamp(inputVelocity.y, GameBehaviour.gravity, 100f);

        if (inputVelocity.y > 0f && isGrounded)
        {
            inputVelocity.y *= (fallSpeed - 1) / Time.deltaTime;
            if (isGrounded)
            {
                inputVelocity.y = 0f;
            }
        }
    }
    #endregion

    #region Grapple
    /// <summary>
    /// Checks if there is a grapple point at the mouse position and sets it. Also starts displaying cable
    /// </summary>
    private void StartGrapple()
    {
        targetGrapplePoint = null;
        GrapplePoint[] grapplePoints = MyGame.Instance.currentLevel.GetGrapplePoints();

        for (int i = 0; i < grapplePoints.Length; i++)
        {
            if (grapplePoints[i].HitTestPoint(Input.mouseX, Input.mouseY))
            {
                targetGrapplePoint = grapplePoints[i];
                break;
            }
        }

        if (targetGrapplePoint == null)
        {
            Console.WriteLine("No grapple point found");
            return;
        }


        isGrappling = true;
        grappleCable.visible = true;
        //MyGame.Instance.AddChild(new ParticleSystem(20, new Vector2(Input.mouseX, Input.mouseY)));
    }


    /// <summary>
    /// Attaches the grapple to designated spot if it is a valid one and sets velocity to it
    /// </summary>
    private void Grapple()
    {
        if (targetGrapplePoint == null)
        {
            return;
        }
        Vector2 grappleDirection = new Vector2(targetGrapplePoint.x, targetGrapplePoint.y) - new Vector2(x, y);
        grappleDirection.Normalize();
        inputVelocity += grappleDirection * grappleAccelerationSpeed;
        if (grappleDirection.Magnitude() >= maxGrappleSpeed)
        {
            inputVelocity = grappleDirection.Normalize() * maxGrappleSpeed;
        }
    }
    #endregion

    public void OnCollision(GameObject other)
    {
        //Console.WriteLine("Colliding with " + other);
        if (other.y > y)
        {
            isGrounded = true;
            isGrappling = false;
            grappleCable.visible = false;

            x = previousPosition.x;
            y = previousPosition.y;
        }
        else
        {
            isGrounded = false;
        }

    }

    #region Getters and Setters
    public void SetHealth(float _health)
    {
        health = _health;
    }

    public float GetHealth()
    {
        return health;
    }

    #endregion
}
