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

    private Vector2 previousPosition = new Vector2();

    //Acceleration speeds
    private float moveAccelerationSpeed = 8f;
    private float grappleAccelerationSpeed = 1f;

    //Jumping and falling
    private float fallSpeed = 4f;
    private float jumpHeight = 10f;

    //Maximum speeds
    private float maxMoveSpeed = 4f;
    private float maxGrappleSpeed = 1.25f;

    //Collision detection
    private Vector2 moveDirection = new Vector2();
    private int collisionCheckDistance = 32;
    private Sprite collisionCheckObject = null;

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

        collisionCheckObject = new Sprite("Assets/Sprites/square.png", false, true);
        collisionCheckObject.SetOrigin(collisionCheckObject.width / 2, collisionCheckObject.height / 2);
        collisionCheckObject.scale = .1f;
        collisionCheckObject.SetColor(52f / 255f, 73f / 255f, 94f / 255f);
        collisionCheckObject.visible = false;



        AddChild(grappleCable);
        AddChild(collisionCheckObject);
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

        if (!IsObstructed())
        {
            x += inputVelocity.x;
            y += inputVelocity.y;

            isGrounded = false;
        }
        else
        {
            isGrounded = true;
            //isGrappling = false;
            //grappleCable.visible = false;

            inputVelocity = new Vector2();
            x = previousPosition.x;
            y = previousPosition.y;
        }

        if (targetGrapplePoint != null)
        {
            grappleCable.height = (int)Vector2.Distance(new Vector2(x, y), new Vector2(targetGrapplePoint.x, targetGrapplePoint.y));
            grappleCable.rotation = Mathf.Atan2(x - targetGrapplePoint.x, targetGrapplePoint.y - y) * 180f / Mathf.PI;
        }

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

    #region Collision

    /// <summary>
    /// Checks if the direction the player is moving in, is blocked by an object
    /// </summary>
    /// <returns>Returns true when path is blocked, otherwise, if path is free, returns false</returns>
    private bool IsObstructed()
    {
        moveDirection = inputVelocity;
        moveDirection = moveDirection.Normalize() * collisionCheckDistance;
        collisionCheckObject.x = moveDirection.x;
        collisionCheckObject.y = moveDirection.y;

        GameObject[] collidedObjects = collisionCheckObject.GetCollisions();
        if(collidedObjects.Length > 0)
        {
            for (int i = 0; i < collidedObjects.Length; i++)
            {
                if(collidedObjects[i] != this)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Backup collision check to see if player is colliding with any object
    /// </summary>
    /// <param name="other">Object that collision is detected with. Gets populated automatically</param>
    public void OnCollision(GameObject other)
    {
        //Console.WriteLine("Colliding with " + other);
        if (other.y > y)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    #endregion

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
