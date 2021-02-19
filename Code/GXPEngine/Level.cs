using GXPEngine;
using GXPEngine.Core;
using System;


public class Level : GameObject
{
    protected Player player = null;
    protected CameraFollow camera = null;
    protected GrapplePoint[] grapplePoints;
    private UVOffsetSprite background = null;

    private Vector2 cameraVelocity = new Vector2();
    private Vector2 previousCameraPosition = new Vector2();

    private float backgroundScrollSpeed = .00085f;
    protected int lowestLevelBarrier = 1024;

    protected HUD hud;

    public GrapplePoint[] GetGrapplePoints()
    {
        return grapplePoints;
    }

    public HUD GetHUD()
    {
        return hud;
    }


    public GameObject GetCamera()
    {
        return camera;
    }

    protected void AddPlayer(int _xPos, int _yPos, Vector2 _boundriesX, Vector2 _boundriesY, int _startHealth = 100)
    {
        player = new Player(.3f, new Vector2(_xPos, _yPos));

        AddChild(player);

        camera = new CameraFollow(player, _boundriesX, _boundriesY);
        AddChild(camera);

        hud = new HUD();

        hud.DrawHealthbar(new Vector2(64, 32) - new Vector2(game.width / 2, game.height / 2), new Vector2(384, 32), 9);

        camera.AddChild(hud);

        player.SetHealth(_startHealth);
    }

    protected void Update()
    {
        cameraVelocity.x = camera.x - previousCameraPosition.x;
        cameraVelocity.y = camera.y - previousCameraPosition.y;

        background.x = camera.x;
        background.y = camera.y;
        background.AddOffset(cameraVelocity.x * backgroundScrollSpeed, 0);

        previousCameraPosition.x = camera.x;
        previousCameraPosition.y = camera.y;

        background.width = (int)(game.width * camera.scale);
        background.height = (int)(game.height * camera.scale);
        //background.SetXY(game.width / 2f, game.height / 2f);

        if (player.y > lowestLevelBarrier)
        {
            MyGame.Instance.RestartLevel();
        }
    }

    protected Pickup NewHealthPickup(int _xPos, int _yPos, int _health = 25)
    {
        HealthPickup healthPickup = new HealthPickup(25);
        healthPickup.SetXY(_xPos, _yPos);

        return healthPickup;
    }

    protected Sprite NewBlock(int _xPos, int _yPos, int _width, int _height, bool _doDebug = false)
    {
        Sprite block = new Sprite("Assets/Sprites/Blocks/main.png", true, true);

        block.width = _width;
        block.height = _height;

        float rows = (float)_width / GameBehaviour.tileSize;
        float columns = (float)_height / GameBehaviour.tileSize;

        Vector2 blockScale = new Vector2(GameBehaviour.tileSize, GameBehaviour.tileSize);
        blockScale = block.InverseTransformDirection(blockScale.x, blockScale.y);

        if (_doDebug)
        {
            Console.WriteLine($"{_width}, {_height}: Rows = {rows} and columns = {columns}");
            Console.WriteLine($"Block size: {blockScale.x} x {blockScale.y}");
        }

        Random randBlock = new Random(1);
        int randomBlockIndex = 1;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Sprite tempBlock = null;
                if (j == 0)
                {
                    if (i == 0)
                    {
                        tempBlock = new Sprite("Assets/Sprites/Blocks/corner_left.png", true, false);
                    }
                    else if (i == rows - 1)
                    {
                        tempBlock = new Sprite("Assets/Sprites/Blocks/corner_right.png", true, false);
                    }
                    else
                    {
                        randomBlockIndex = randBlock.Next(1, 3);
                        tempBlock = new Sprite("Assets/Sprites/Blocks/floor" + randomBlockIndex + ".png", true, false);
                    }
                }else if(j == columns - 1)
                {
                        randomBlockIndex = randBlock.Next(1, 3);
                        tempBlock = new Sprite("Assets/Sprites/Blocks/ceiling" + randomBlockIndex + ".png", true, false);
                }
                else
                {
                    if (i == 0)
                    {
                        tempBlock = new Sprite("Assets/Sprites/Blocks/edge_left.png", true, false);
                    } else if (i == rows - 1)
                    {
                        tempBlock = new Sprite("Assets/Sprites/Blocks/edge_right.png", true, false);
                    }
                    else
                    {
                        tempBlock = new Sprite("Assets/Sprites/Blocks/main.png", true, false);
                    }
                }
                tempBlock.width = Mathf.Ceiling(blockScale.x);
                tempBlock.height = Mathf.Ceiling(blockScale.y);
                tempBlock.x = i * blockScale.x;
                tempBlock.y = j * blockScale.y;

                block.AddChild(tempBlock);
            }
        }

        block.SetXY(_xPos, _yPos);

        return block;
    }

    protected GrapplePoint NewGrapplePoint(int _xPos, int _yPos, int _width, int _height, bool _isPositive)
    {
        GrapplePoint grapplePoint = new GrapplePoint(_width, _height, _isPositive);
        grapplePoint.SetXY(_xPos, _yPos);

        return grapplePoint;
    }

    protected ElectricNet NewElectricNet(int _xPos, int _yPos)
    {
        ElectricNet electricNet = new ElectricNet();
        electricNet.SetXY(_xPos, _yPos);

        return electricNet;
    }

    protected Turret NewTurret(int _xPos, int _yPos)
    {
        Turret turret = new Turret(4f);
        turret.SetXY(_xPos, _yPos);

        return turret;
    }

    protected void AddBackground(string _backgroundFileName)
    {
        background = new UVOffsetSprite(_backgroundFileName, false, false);
        background.SetOrigin(background.width / 2, background.height / 2);
        background.width = (int)(game.width * camera.scale);
        background.height = (int)(game.height * camera.scale);
        background.SetXY(-game.width / 2, -game.height / 2);

        LateAddChildAt(background, 0);
    }

    public Player GetPlayer()
    {
        return player;
    }

    public virtual void StopAudio()
    {
        Console.WriteLine("No audio to stop");
    }
}
