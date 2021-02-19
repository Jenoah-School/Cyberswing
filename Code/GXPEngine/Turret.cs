using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Turret : AnimationSprite
{
    private float _bulletTime = 4f;
    private float nextBulletTime = 0;
    
    
    private Bullet[] bullets = new Bullet[2];
    private Sound shootSound = null;

    private bool isClosed = false;
    private float closingTime = 0;
    private float openTime = 1f;

    public Turret(float _bulletDelay) : base("Assets/Sprites/turret.png", 2, 1)
    {
        SetOrigin(width / 2, height / 2);

        width = GameBehaviour.tileSize;
        height = GameBehaviour.tileSize;

        shootSound = new Sound("Assets/Audio/SoundFX/TurretShoot.ogg");

        SetFrame(1);
        nextBulletTime = Time.time + (_bulletTime * 1000);
        for (int i = 0; i < 2; i++)
        {
            bullets[i] = new Bullet(i % 2 == 0 || i == 0);
            AddChild(bullets[i]);
        }
    }

    void Update()
    {
        if(Time.time >= nextBulletTime)
        {
            if (isClosed)
            {
                isClosed = false;
                SetFrame(0);
                closingTime = Time.time + (openTime * 1000f);
            }

            Shoot();
            nextBulletTime = Time.time + (_bulletTime * 1000);
        }

        if(!isClosed && Time.time >= closingTime)
        {
            isClosed = true;
            SetFrame(1);
        }
    }

    private void Shoot()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].ResetBullet();
            bullets[i].StartBullet();
        }

        shootSound.Play();
    }
}
