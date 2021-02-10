using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class Level : GameObject
{
    private Player player = null;
    private Sprite bottomPlane = null;
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

        bottomPlane = new Sprite("Assets/Sprites/square.png", true, true);
        bottomPlane.SetOrigin(bottomPlane.width / 2, bottomPlane.height / 2);
        bottomPlane.width = game.width;
        bottomPlane.SetXY(game.width / 2, game.height - bottomPlane.height / 2);
        bottomPlane.SetColor(41f / 255f, 128f / 255f, 185f / 255f);

        AddChild(bottomPlane);

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
}
