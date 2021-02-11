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
    private Vector2 velocity = new Vector2();

    private int collisionCheckDistance = 32;
    private Sprite collisionCheckObject = null;

    private float health = 100;
    private float grappleCooldown = 0.75f;
    private float grappleLastUse = 0f;

    private bool isColliding = false;
    private bool isGrappling = false;
    private bool canStickToGrapple = false;
    private bool hookIsPositive = false;

    private int generalHealthDecrementTime = 0;
    private int movementHealthDecrementTime = 0;
    private int timedHealthAmount = 1;

    private Sprite grappleCable = null;

    GrapplePoint targetGrapplePoint = null;

    public Player() : base("Assets/Sprites/square.png")
    {
        SetOrigin(width / 2, height / 2);

        grappleCable = new Sprite("Assets/Sprites/square.png", false, false);
        grappleCable.SetOrigin(grappleCable.width / 2, 0);

        grappleCable.width = 8;
        grappleCable.SetColor(52f / 255f, 152f / 255f, 219f / 255f);
        grappleCable.visible = false;

        collisionCheckObject = new Sprite("Assets/Sprites/square.png", false, true);
        collisionCheckObject.SetOrigin(collisionCheckObject.width / 2, collisionCheckObject.height / 2);
        collisionCheckObject.width = (int)(width / 2f);
        collisionCheckObject.height = (int)(height / 16f);
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

        if (Input.GetMouseButtonDown(0) && Time.time >= grappleLastUse)
        {
            grappleLastUse = Time.time + (grappleCooldown * 1000);
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if (Input.GetMouseButton(0) && isGrappling)
        {
            Grapple();
        }
        else
        {
            Move();
        }

        if (Input.GetMouseButtonUp(1))
        {
            hookIsPositive = !hookIsPositive;
            if (hookIsPositive)
            {
                grappleCable.SetColor(231 / 255f, 76 / 255f, 60 / 255f);
            }
            else
            {
                grappleCable.SetColor(52f / 255f, 152f / 255f, 219f / 255f);
                
            }
        }

        if (!IsObstructed())
        {
            x += inputVelocity.x;
            y += inputVelocity.y;

            isColliding = false;
        }
        else
        {
            isColliding = true;
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

        if (Time.time > generalHealthDecrementTime)
        {
            generalHealthDecrementTime = Time.time + (timedHealthAmount * 1000);
            SetHealth(health - 1);
            UpdateHealthbar();
        }

        if (velocity.Magnitude() != 0)
        {
            if (Time.time > movementHealthDecrementTime)
            {
                SetHealth(health - Time.deltaTime / 1000f);
            }
        }
        else
        {
            movementHealthDecrementTime = Time.time + (timedHealthAmount * 1000);
        }

        velocity = new Vector2(x, y) - previousPosition;
        //Console.WriteLine(velocity);

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

        inputVelocity.x += horizontal * moveAccelerationSpeed * Time.deltaTime / 100f;// / Time.deltaTime;

        if (isColliding)
        {
            inputVelocity.x *= 0.75f;
        }

        if (inputVelocity.x < 0.01f && inputVelocity.x > -0.01f)
        {
            inputVelocity.x = 0f;
        }

        inputVelocity.x = Mathf.Clamp(inputVelocity.x, -maxMoveSpeed, maxMoveSpeed);

        if (Input.GetKeyDown(Key.SPACE) && isColliding)
        {

            inputVelocity.y -= jumpHeight;
        }

        Fall();

        //isGrounded = false;
    }

    /// <summary>
    /// Applies a modified version of gravity to the player
    /// </summary>
    private void Fall()
    {
        inputVelocity.y += -GameBehaviour.gravity / Time.deltaTime;
        inputVelocity.y = Mathf.Clamp(inputVelocity.y, GameBehaviour.gravity, 100f);

        if (inputVelocity.y > 0f && isColliding)
        {
            inputVelocity.y *= (fallSpeed - 1) / Time.deltaTime;
            if (isColliding)
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
            if (grapplePoints[i].HitTestPoint(Input.mouseX, Input.mouseY) && (hookIsPositive != grapplePoints[i].IsPositive()))
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

    /// <summary>
    /// Releases the hook and ends the grapple state
    /// </summary>
    private void StopGrapple()
    {
        isGrappling = false;
        grappleCable.visible = false;
        targetGrapplePoint = null;
    }
    #endregion

    #region Collision

    /// <summary>
    /// Checks if the direction the player is moving in, is blocked by an object
    /// </summary>
    /// <returns>Returns true when path is blocked, otherwise, if path is free, returns false</returns>
    private bool IsObstructed()
    {
        moveDirection = velocity;
        if (moveDirection.Magnitude() < 0.01f)
        {
            moveDirection = inputVelocity;
        }

        moveDirection.Normalize();
        moveDirection.x = Mathf.Round(moveDirection.x);
        moveDirection.y = Mathf.Round(moveDirection.y);

        moveDirection *= collisionCheckDistance;
        collisionCheckObject.x = moveDirection.x;
        collisionCheckObject.y = moveDirection.y;
        collisionCheckObject.rotation = Mathf.Atan2(moveDirection.x, -moveDirection.y) * 180f / Mathf.PI;

        GameObject[] collidedObjects = collisionCheckObject.GetCollisions();
        if(collidedObjects.Length > 0)
        {
            for (int i = 0; i < collidedObjects.Length; i++)
            {

                if (!canStickToGrapple && isGrappling && collidedObjects[i].GetType() == typeof(GrapplePoint))
                {
                    StopGrapple();
                }

                if (collidedObjects[i] != this && collidedObjects[i].GetType() != typeof(Pickup))
                {
                    return true;
                }


            }
        }

        return false;
    }

    private void UpdateHealthbar()
    {
        Level currentLevel = (Level)parent;
        currentLevel.GetHUD().SetHealth(Mathf.Round(health));
    }

    /// <summary>
    /// Backup collision check to see if player is colliding with any object
    /// </summary>
    /// <param name="other">Object that collision is detected with. Gets populated automatically</param>
    public void OnCollision(GameObject other)
    {
        //Console.WriteLine("Colliding with " + other);
        if (other.y > y && other.GetType() != typeof(Pickup))
        {
            isColliding = true;
        }
        else
        {
            isColliding = false;
        }

    }

    #endregion

    #region Getters and Setters
    public void SetHealth(float _health)
    {
        health = _health;
        health = Mathf.Clamp(health, 0, 100);
        UpdateHealthbar();

        if(health <= 0)
        {
            MyGame.Instance.RestartLevel();
        }
    }

    public int GetHealth()
    {
        return (int)health;
    }

    #endregion
}
