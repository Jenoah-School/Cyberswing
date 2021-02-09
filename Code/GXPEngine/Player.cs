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
    private Vector2 moveVelocity = new Vector2();
    private Vector2 globalVelocity = new Vector2();
    private Vector2 grappleVelocity = new Vector2();

    private Vector2 localVelocity = new Vector2();
    private Vector2 previousPosition = new Vector2();

    //Speeds and heights
    private float fallSpeed = 4f;
    private float moveSpeed = 8f;
    private float jumpHeight = 10f;
    private float maxMoveSpeed = 4f;

    private bool isGrounded = false;
    private bool isGrappling = false;

    private Sprite grappleCable = null;

    public Player() : base("Assets/Sprites/square.png")
    {
        SetOrigin(width / 2, height / 2);

        grappleCable = new Sprite("Assets/Sprites/square.png", false, false);
        grappleCable.SetOrigin(grappleCable.width / 2, 0);

        grappleCable.width = 8;
        grappleCable.height = (int)Vector2.Distance(new Vector2(x, y), new Vector2(Input.mouseX, Input.mouseY));
        grappleCable.rotation = Mathf.Atan2(x - Input.mouseX, Input.mouseY - y) * 180f / Mathf.PI;
        grappleCable.SetColor(.1f, .1f, .1f);
        grappleCable.visible = false;

        AddChild(grappleCable);
    }

    void Update()
    {
        //This is a temporary key...
        if (!canMove)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Grapple();
        }

        Move();

        if (Input.GetMouseButtonUp(0) || isGrounded)
        {
            grappleVelocity = new Vector2();
            isGrappling = false;
            grappleCable.visible = false;
        }

        grappleCable.height = (int)Vector2.Distance(new Vector2(x, y), new Vector2(Input.mouseX, Input.mouseY));
        grappleCable.rotation = Mathf.Atan2(x - Input.mouseX, Input.mouseY - y) * 180f / Mathf.PI;

        localVelocity = isGrappling ? grappleVelocity : moveVelocity;

        x += localVelocity.x;
        y -= localVelocity.y;

        

        globalVelocity = new Vector2(x, y) - previousPosition;

        previousPosition.x = x;
        previousPosition.y = y;
    }

    /// <summary>
    /// Moves the player to given input direction
    /// </summary>
    private void Move()
    {
        float horizontal = GameBehaviour.GetHorizontalAxis();

        moveVelocity.x = horizontal * moveSpeed;// / Time.deltaTime;


        if (Input.GetKeyDown(Key.SPACE) && isGrounded)
        {
            moveVelocity.y += jumpHeight;
        }

        moveVelocity.x = Mathf.Clamp(moveVelocity.x, -maxMoveSpeed, maxMoveSpeed);

        Fall();
        //x += moveVelocity.x;
        //y -= moveVelocity.y;

        //Console.WriteLine(localVelocity);
        isGrounded = false;
    }

    /// <summary>
    /// Applies a modified version of gravity to the player
    /// </summary>
    private void Fall()
    {
        moveVelocity.y += GameBehaviour.gravity / Time.deltaTime;
        moveVelocity.y = Mathf.Clamp(moveVelocity.y, GameBehaviour.gravity, 100f);

        if (moveVelocity.y < 0f && isGrounded)
        {
            moveVelocity.y *= (fallSpeed - 1) / Time.deltaTime;
            if (isGrounded)
            {
                moveVelocity.y = 0f;
            }
        }
    }


    /// <summary>
    /// Attaches the grapple to designated spot if it is a valid one and sets velocity to it
    /// </summary>
    private void Grapple()
    {
        GrapplePoint targetGrapplePoint = null;
        GrapplePoint[] grapplePoints = MyGame.Instance.currentLevel.GetGrapplePoints();

        for (int i = 0; i < grapplePoints.Length; i++)
        {
            if (grapplePoints[i].HitTestPoint(Input.mouseX, Input.mouseY))
            {
                targetGrapplePoint = grapplePoints[i];
                break;
            }
        }

        if(targetGrapplePoint == null)
        {
            Console.WriteLine("No grapple point found");
            return;
        }

        
        Vector2 grappleDirection = new Vector2(targetGrapplePoint.x, targetGrapplePoint.y) - new Vector2(x, -y);
        grappleDirection = grappleDirection.Normalize();
        Console.WriteLine($"Direction to target grapple is: {grappleDirection}");
        grappleVelocity = grappleDirection * jumpHeight;
        isGrappling = true;
        grappleCable.visible = true;
        //MyGame.Instance.AddChild(new ParticleSystem(20, new Vector2(Input.mouseX, Input.mouseY)));
    }

    public void OnCollision(GameObject other)
    {
        //Console.WriteLine("Colliding with " + other);
        if (other.y > y)
        {
            isGrounded = true;
            isGrappling = false;
            grappleCable.visible = false;
        }
        else
        {
            isGrounded = false;
        }
        
    }
}
