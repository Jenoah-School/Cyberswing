using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PrototypeLevel : Level
{
    public PrototypeLevel()
    {
        AddBorders();

        grapplePoints = new GrapplePoint[2];

        grapplePoints[0] = new GrapplePoint(256, 32);
        grapplePoints[0].SetXY(game.width / 8, game.height / 4 * 0.55f);

        grapplePoints[1] = new GrapplePoint(256, 32, true);
        grapplePoints[1].SetXY(game.width / 8 * 7, game.height / 4 * 0.85f);

        AddChild(grapplePoints[0]);
        AddChild(grapplePoints[1]);

        player = new Player(.3f);

        player.SetXY(game.width / 2, game.height / 4 * 2f);

        AddChild(player);

        camera = new CameraFollow(player, new Vector2(0, 2048), new Vector2(0, 720));
        AddChild(camera);

        hud = new HUD();

        hud.DrawHealthbar(new Vector2(64, 8) - new Vector2(game.width / 2, game.height / 2), new Vector2(384, 32));
        //hud.DrawHookCharge(new Vector2(game.width - 416, 16), new Vector2(384, 32), 12);

        //hud.SetHookCharge(1f);

        camera.AddChild(hud);

        HealthPickup healthPickup = new HealthPickup(25);
        healthPickup.SetXY(game.width / 8 * 7, game.height / 2);

        AddChild(healthPickup);

        player.SetHealth(100);

        AddBackground("Assets/Sprites/Background.png");
    }

    private void AddBorders()
    {
        Sprite topBorder = null;
        Sprite rightBorder = null;
        Sprite bottomBorder = null;
        Sprite leftBorder = null;

        topBorder = new Sprite("Assets/Sprites/square.png", true, true);
        rightBorder = new Sprite("Assets/Sprites/square.png", true, true);
        bottomBorder = new Sprite("Assets/Sprites/square.png", true, true);
        leftBorder = new Sprite("Assets/Sprites/square.png", true, true);

        topBorder.SetOrigin(topBorder.width / 2, topBorder.height / 2);
        rightBorder.SetOrigin(rightBorder.width / 2, rightBorder.height / 2);
        bottomBorder.SetOrigin(bottomBorder.width / 2, bottomBorder.height / 2);
        leftBorder.SetOrigin(leftBorder.width / 2, leftBorder.height / 2);

        topBorder.width = game.width;
        bottomBorder.width = game.width;
        rightBorder.width = 16;
        leftBorder.width = 16;

        topBorder.height = 16;
        rightBorder.height = game.height;
        leftBorder.height = game.height;

        topBorder.SetXY(game.width / 2, topBorder.height / 2);
        rightBorder.SetXY(game.width - rightBorder.width / 2, game.height / 2);
        bottomBorder.SetXY(game.width / 2, game.height - bottomBorder.height / 2);
        leftBorder.SetXY(leftBorder.width / 2, game.height / 2);

        topBorder.SetColor(52f / 255f, 73f / 255f, 94f / 255f);
        rightBorder.SetColor(52f / 255f, 73f / 255f, 94f / 255f);
        bottomBorder.SetColor(52f / 255f, 73f / 255f, 94f / 255f);
        leftBorder.SetColor(52f / 255f, 73f / 255f, 94f / 255f);

        AddChild(topBorder);
        AddChild(rightBorder);
        AddChild(bottomBorder);
        AddChild(leftBorder);

        Sprite blockTopLeft = null;
        blockTopLeft = new Sprite("Assets/Sprites/square.png", true, true);
        blockTopLeft.SetOrigin(blockTopLeft.width / 2, blockTopLeft.height / 2);
        blockTopLeft.width = game.width / 4;
        blockTopLeft.height = game.width / 12;
        blockTopLeft.SetXY(blockTopLeft.width / 2, blockTopLeft.height / 2);
        blockTopLeft.SetColor(52f / 255f, 73f / 255f, 94f / 255f);
        AddChild(blockTopLeft);

        Sprite blockBottomRight = null;
        blockBottomRight = new Sprite("Assets/Sprites/square.png", true, true);
        blockBottomRight.SetOrigin(blockBottomRight.width / 2, blockBottomRight.height / 2);
        blockBottomRight.width = game.width / 4;
        blockBottomRight.height = game.width / 4;
        blockBottomRight.SetXY(game.width - blockBottomRight.width / 2, game.height - topBorder.height);
        blockBottomRight.SetColor(52f / 255f, 73f / 255f, 94f / 255f);
        AddChild(blockBottomRight);

        Sprite blockTopRight = null;
        blockTopRight = new Sprite("Assets/Sprites/square.png", true, true);
        blockTopRight.SetOrigin(blockTopRight.width / 2, blockTopRight.height / 2);
        blockTopRight.width = game.width / 4;
        blockTopRight.height = game.width / 8;
        blockTopRight.SetXY(game.width - blockTopRight.width / 2, blockTopRight.height / 2);
        blockTopRight.SetColor(52f / 255f, 73f / 255f, 94f / 255f);
        AddChild(blockTopRight);
    }
}
