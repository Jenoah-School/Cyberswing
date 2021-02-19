using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Level2 : Level
{
    Sprite[] blocks = new Sprite[12];

    Pickup[] pickups = new Pickup[3];
    DamageObject[] damageObjects = new DamageObject[3];
    Turret[] turrets = new Turret[2];

    public Level2()
    {
        DrawLevel();
        AddPlayer(256, 256, new Vector2(0 + game.width / 2, 4096 - game.width / 2), new Vector2(0, 1024 - game.height / 2));
        //AddPlayer(2048+196, 384-128, new Vector2(0 + game.width / 2, 4096 - game.width / 2), new Vector2(0, 1024 - game.height / 2));
        AddBackground("Assets/Sprites/background_forest_red.png");
        lowestLevelBarrier = 2048;
    }

    private void DrawLevel()
    {
        //Section 1
        blocks[0] = NewBlock(-128, -256, 128, 1024);
        blocks[1] = NewBlock(0, 384, 256, 512);
        blocks[2] = NewBlock(512, 128, 256, 1024);
        blocks[3] = NewBlock(0, -640, 4096, 512);

        blocks[4] = NewBlock(1280, -640, 512, 1024);
        blocks[5] = NewBlock(1280, 640, 512, 512);

        blocks[6] = NewBlock(1792, -384, 640, 512);
        
        blocks[7] = NewBlock(2048, 384, 384, 512);
        blocks[8] = NewBlock(2560, 640, 256, 512);

        blocks[9] = NewBlock(3200, -192, 896, 256);
        blocks[10] = NewBlock(3328, 384, 768, 512);
        blocks[11] = NewBlock(4096, -64, 128, 768);

        grapplePoints = new GrapplePoint[4];
        grapplePoints[0] = NewGrapplePoint(640, -128, 384, 32, true);
        grapplePoints[1] = NewGrapplePoint(1536, 384, 512, 32, false);
        grapplePoints[2] = NewGrapplePoint(2048, 128, 384, 32, true);
        grapplePoints[3] = NewGrapplePoint(3456, 64, 384, 32, false);

        pickups[0] = NewHealthPickup(896, 256);
        pickups[1] = NewHealthPickup(2240, 260);
        pickups[2] = NewHealthPickup(2688, 576);

        damageObjects[0] = NewElectricNet(1152, 192);
        damageObjects[1] = NewElectricNet(2688, 192);
        damageObjects[2] = NewElectricNet(2944, 224);

        turrets[0] = NewTurret(640, 448);
        turrets[1] = NewTurret(2240, 448);

        NextLevelSwitch nextLevelSwitch = new NextLevelSwitch(0, "Assets/Audio/Music/Level2Complete.ogg");
        nextLevelSwitch.width = 128;
        nextLevelSwitch.height = 128;
        nextLevelSwitch.SetXY(3584, 256);

        AddChild(nextLevelSwitch);


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

        for (int i = 0; i < turrets.Length; i++)
        {
            AddChild(turrets[i]);
        }
    }

    public override void StopAudio()
    {
        for (int i = 0; i < damageObjects.Length; i++)
        {
            ((ElectricNet)damageObjects[i]).StopAudio();
        }
    }

}
