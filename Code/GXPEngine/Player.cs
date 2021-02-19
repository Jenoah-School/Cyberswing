using GXPEngine;
using GXPEngine.Core;
using System;

public class Player : AnimationSprite
{
    private bool canMove = true;

    private Vector2 inputVelocity = new Vector2();

    private Vector2 previousPosition = new Vector2();

    //Acceleration speeds
    private float moveAccelerationSpeed = 8f;
    private float grappleAccelerationSpeed = .75f;

    //Jumping and falling
    private float fallSpeed = 4f;
    private float jumpHeight = 10f;

    //Maximum speeds
    private float maxMoveSpeed = 4f;

    //Collision detection
    private Vector2 moveDirection = new Vector2();
    private Vector2 velocity = new Vector2();

    private int collisionCheckDistance = 32;
    private Sprite collisionCheckObject = null;

    private float health = 100;
    private float grappleCooldown = 0.75f;
    private float grappleLastUse = 0f;
    private float restartTime = 0f;

    private bool isColliding = false;
    private bool isGrappling = false;
    private bool canStickToGrapple = false;
    private bool hookIsPositive = false;
    private bool isFlipped = false;
    private bool hasDied = false;

    private int generalHealthDecrementTime = 0;
    private int movementHealthDecrementTime = 0;
    private int timedHealthAmount = 1;

    private float localScale = 1f;

    private Sprite blueCable = null;
    private Sprite redCable = null;
    private Sprite grappleCable = null;
    private Vector2 grapplePos = new Vector2();
    private AnimationSprite visors = null;

    GrapplePoint targetGrapplePoint = null;

    public Player(float _scale = 1, Vector2 _startPosition = new Vector2()) : base("Assets/Sprites/Player/Walk.png", 4, 2)
    {
        _animationDelay = 5;
        localScale = _scale;

        SetOrigin(width / 2, height / 2);

        visors = new AnimationSprite("Assets/Sprites/Player/Walk_visors.png", 4, 2, -1, false, false);
        //visors.SetOrigin(width / 2, height / 2);
        visors.width = width;
        visors.height = height;
        visors.SetXY(-width / 2, -height / 2);

        collisionCheckDistance = width / 2;

        redCable = new Sprite("Assets/Sprites/chain_red_long.png", false, false);
        blueCable = new Sprite("Assets/Sprites/chain_blue_long.png", false, false);

        redCable.SetOrigin(redCable.width / 2, 0);
        blueCable.SetOrigin(blueCable.width / 2, 0);

        redCable.width = width / 16;
        blueCable.width = width / 16;

        redCable.visible = false;
        blueCable.visible = false;

        grappleCable = blueCable;

        collisionCheckObject = new Sprite("Assets/Sprites/lightning.png", false, true);
        collisionCheckObject.SetOrigin(collisionCheckObject.width / 2, collisionCheckObject.height / 2);
        collisionCheckObject.width = (int)(width / 2f);
        collisionCheckObject.height = (int)(height / 16f);
        collisionCheckObject.SetColor(52f / 255f, 73f / 255f, 94f / 255f);
        collisionCheckObject.visible = false;

        LateAddChild(visors);
        AddChild(redCable);
        AddChild(blueCable);
        AddChild(grappleCable);
        AddChild(collisionCheckObject);

        scale = localScale;
        x = _startPosition.x;
        y = _startPosition.y;
        previousPosition = _startPosition;
    }

    void Update()
    {
        if (hasDied)
        {
            inputVelocity.y += -GameBehaviour.gravity / Time.deltaTime;
            inputVelocity.y = Mathf.Clamp(inputVelocity.y, GameBehaviour.gravity, 100f);

            if (inputVelocity.y > 0)
            {
                inputVelocity.y *= (fallSpeed - 1) / Time.deltaTime;
            }
            

            y += inputVelocity.y;
            if (Time.time >= restartTime)
            {
                MyGame.Instance.RestartLevel();
            }
            return;
        }
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
            StopGrapple();
            hookIsPositive = !hookIsPositive;
            if (hookIsPositive)
            {
                grappleCable = redCable;
                visors.alpha = 0;
            }
            else
            {
                grappleCable = blueCable;
                visors.alpha = 1;
            }

            ((Level)parent).GetHUD().ChangeLightningPolarity(hookIsPositive);
        }

        //MoveAndCollide(GetCollisions());
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
        if (velocity.Magnitude() > 0.1f)
        {
            if (velocity.x < 0 && !isFlipped)
            {
                isFlipped = true;
                scaleX = -localScale;
            }
            else if (velocity.x > 0 && isFlipped)
            {
                isFlipped = false;
                scaleX = localScale;
            }

            if (!isGrappling && Mathf.Abs(velocity.y) < 2f)
            {
                Animate();
                visors.SetFrame(currentFrame);
            }
        }
        else
        {
            SetFrame(0);
            visors.SetFrame(0);
        }

        if (targetGrapplePoint != null)
        {
            grappleCable.height = (int)(Vector2.Distance(new Vector2(x, y), new Vector2(grapplePos.x, grapplePos.y)) / localScale);
            if (isFlipped)
            {
                grappleCable.rotation = Mathf.Atan2(grapplePos.x - x, grapplePos.y - y) * 180f / Mathf.PI;
            }
            else
            {
                grappleCable.rotation = Mathf.Atan2(x - grapplePos.x, grapplePos.y - y) * 180f / Mathf.PI;
            }
        }

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
        Level parentLevel = (Level)parent;

        Vector2 globalMousePos = (parentLevel.GetCamera().TransformPoint(Input.mouseX, Input.mouseY) - new Vector2(game.width / 2, game.height / 2) * parentLevel.GetCamera().scale);

        for (int i = 0; i < grapplePoints.Length; i++)
        {
            if (grapplePoints[i].CheckHitbox(globalMousePos.x, globalMousePos.y) && (hookIsPositive != grapplePoints[i].IsPositive()))
            {
                grapplePos = grapplePoints[i].GetGrapplePosition(globalMousePos.x);
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
        SetFrame(4);
        visors.SetFrame(4);
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
        Vector2 grappleDirection = grapplePos - new Vector2(x, y);

        grappleDirection.Normalize();
        inputVelocity += grappleDirection * grappleAccelerationSpeed;
        //if (!(inputVelocity.Magnitude() >= maxGrappleSpeed))
        //{
        //    inputVelocity = grappleDirection.Normalize() * maxGrappleSpeed;
        //    Console.WriteLine($"Grapple speed = {inputVelocity.Magnitude()}");
        //}


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
        if (moveDirection.Magnitude() < 0.001f)
        {
            moveDirection = inputVelocity;
        }

        //moveDirection.x = Mathf.Round(moveDirection.x);
        //moveDirection.y = Mathf.Round(moveDirection.y);

        moveDirection.Normalize();
        if (isFlipped)
        {
            moveDirection.x = -moveDirection.x;
        }


        moveDirection *= collisionCheckDistance;
        collisionCheckObject.x = moveDirection.x;
        collisionCheckObject.y = moveDirection.y;

        collisionCheckObject.rotation = Mathf.Atan2(moveDirection.x, -moveDirection.y) * 180f / Mathf.PI;


        GameObject[] collidedObjects = collisionCheckObject.GetCollisions();
        if (collidedObjects.Length > 0)
        {
            for (int i = 0; i < collidedObjects.Length; i++)
            {

                if (!canStickToGrapple && isGrappling && collidedObjects[i] as GrapplePoint != null)
                {
                    StopGrapple();
                }

                if (collidedObjects[i] != this && collidedObjects[i].GetType() != typeof(HealthPickup) && collidedObjects[i].GetType() != typeof(ElectricNet) && collidedObjects[i].GetType() != typeof(NextLevelSwitch))
                {
                    //Console.WriteLine($"Colliding with: {collidedObjects[i]}");
                    ResolveCollision(collidedObjects[i] as Sprite);
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
        if (hasDied)
        {
            return;
        }

        //Console.WriteLine("Colliding with " + other);
        if (other.y > y && other.GetType() != typeof(HealthPickup) && other != collisionCheckObject)
        {
            //ResolveCollision(other as Sprite);
            isColliding = true;
        }
        else
        {
            isColliding = false;
        }

    }

    void ResolveCollision(Sprite collider)
    {
        if (inputVelocity.x < 0) x = collider.x + collider.width;
        if (inputVelocity.x > 0) x = collider.x - width;
        if (inputVelocity.y < 0) y = collider.y + collider.height;
        if (inputVelocity.y > 0) y = collider.y - height;
    }
    #endregion

    #region Health
    public void SetHealth(float _health)
    {
        health = _health;
        health = Mathf.Clamp(health, 0, 100);
        UpdateHealthbar();

        if (health <= 0 && !hasDied)
        {
            Die();
        }
    }

    public int GetHealth()
    {
        return (int)health;
    }

    private void UpdateHealthbar()
    {
        Level currentLevel = (Level)parent;
        currentLevel.GetHUD().SetHealth(Mathf.Round(health));
    }

    public void Die()
    {
        canMove = false;
        hasDied = true;
        inputVelocity = new Vector2();
        inputVelocity.y -= jumpHeight;
        fallSpeed *= -GameBehaviour.gravity / 2.025f;
        restartTime = Time.time + 1500f;
        SetFrame(4);
    }

    #endregion

    public Vector2 GetVelocity()
    {
        return velocity;
    }
}
