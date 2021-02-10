using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class Level : GameObject
{
    private Player player = null;
    private GrapplePoint[] grapplePoints;

    private EasyDraw healthText;

    public Level()
    {
        grapplePoints = new GrapplePoint[2];

        grapplePoints[0] = new GrapplePoint();
        grapplePoints[0].SetXY(game.width / 4, 128);

        grapplePoints[1] = new GrapplePoint();
        grapplePoints[1].SetXY(game.width / 4 * 3, 128);

        AddChild(grapplePoints[0]);
        AddChild(grapplePoints[1]);

        AddBorders();

        player = new Player();

        player.SetXY(game.width / 2, game.height / 2);

        AddChild(player);

        DrawHUD();
    }

    public GrapplePoint[] GetGrapplePoints()
    {
        return grapplePoints;
    }

    public void DrawHUD()
    {
        healthText = new EasyDraw(game.width / 2, game.height / 4, false);
        healthText.Text("Health: " + player.GetHealth(), 8, 32);

        AddChild(healthText);
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

        topBorder.SetColor(41f / 255f, 128f / 255f, 185f / 255f);
        rightBorder.SetColor(41f / 255f, 128f / 255f, 185f / 255f);
        bottomBorder.SetColor(41f / 255f, 128f / 255f, 185f / 255f);
        leftBorder.SetColor(41f / 255f, 128f / 255f, 185f / 255f);

        AddChild(topBorder);
        AddChild(rightBorder);
        AddChild(bottomBorder);
        AddChild(leftBorder);
    }
}
