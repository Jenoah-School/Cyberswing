using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Level1 : Level
{
    Sprite[] blocks = new Sprite[9];
    
    Pickup[] pickups = new Pickup[3];
    DamageObject[] damageObjects = new DamageObject[4];

    public Level1()
    {
        DrawLevel();
        AddPlayer(256, 256, new Vector2(0 + game.width / 2, 3712), new Vector2(0, 720 - game.height / 2));
        //AddPlayer(1792 + 128, 64, new Vector2(0 + game.width / 2, 3712), new Vector2(0, 720 - game.height / 2));
        AddBackground("Assets/Sprites/background-forest.png");
        //camera.scale = 2f;
        //Add background music V1 Slow
    }

    private void DrawLevel()
    {
        

        //Section 1
        blocks[0] = NewBlock(-128, 0, 128, 512);
        blocks[1] = NewBlock(0, 512, 512, 512);
        blocks[2] = NewBlock(512, 700, 512, 512);
        blocks[3] = NewBlock(1024, 512, 512, 512);
        blocks[4] = NewBlock(0, -480, 1536, 512);

        //Section 2
        blocks[5] = NewBlock(1536, -572, 4096, 512);
        blocks[6] = NewBlock(1792, 324, 256, 512);
        blocks[7] = NewBlock(2560, 324, 256, 512);
        blocks[8] = NewBlock(4096, 324, 256, 512);

        grapplePoints = new GrapplePoint[5];
        grapplePoints[0] = NewGrapplePoint(768, 30, 384, 32, false);
        grapplePoints[1] = NewGrapplePoint(1920, -60, 384, 32, true);
        grapplePoints[2] = NewGrapplePoint(2688, -60, 384, 32, false);
        grapplePoints[3] = NewGrapplePoint(3456, -60, 384, 32, true);
        grapplePoints[4] = NewGrapplePoint(4224, -60, 384, 32, false);

        pickups[0] = NewHealthPickup(1280, 448);
        pickups[1] = NewHealthPickup(1920, 64 + 196);
        pickups[2] = NewHealthPickup(2688, 64 + 196);

        damageObjects[0] = NewElectricNet(2304, 452);
        damageObjects[1] = NewElectricNet(3072, 452);
        damageObjects[2] = NewElectricNet(3456, 452);
        damageObjects[3] = NewElectricNet(3840, 452);


        //Adding all created objects to level
        for (int i = 0; i < blocks.Length; i++)
        {
            AddChild(blocks[i]);
        }

        for (int i = 0; i < grapplePoints.Length; i++)
        {
            AddChild(grapplePoints[i]);
        }

        for (int i = 0; i < pickups.Length; i++)
        {
            AddChild(pickups[i]);
        }

        for (int i = 0; i < damageObjects.Length; i++)
        {
            AddChild(damageObjects[i]);
        }
    }

    public override void StopAudio()
    {
        for (int i = 0; i < damageObjects.Length; i++)
        {
            ((ElectricNet)damageObjects[i]).StopAudio();
        }

        Console.WriteLine("Audio stopped");
    }
    
}
