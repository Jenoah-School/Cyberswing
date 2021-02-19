using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Bullet : DamageObject
{
    private bool canMove = false;
    private int moveSpeed = 15;
    private int damage = 40;

    private float lifeTime = 4f;
    private float deathTime = 0f;

    public Bullet(bool _isFlipped) : base("Assets/Sprites/projectile.png", 50)
    {
        if (_isFlipped)
        {
            scaleX = -1;
            moveSpeed = -moveSpeed;
        }

        hitSound = new Sound("Assets/Audio/SoundFX/BulletHit.ogg");
        ResetBullet();
    }

    void Update()
    {
        if (canMove)
        {
            Move(moveSpeed, 0);
            if (Time.time >= deathTime)
            {
                Console.WriteLine("Killing bullet");
                ResetBullet();
            }
        }
    }

    public void StartBullet()
    {
        canMove = true;
        visible = true;
        deathTime = Time.time + (lifeTime * 1000f);
        Console.WriteLine("Starting bullet");
    }

    public void ResetBullet()
    {
        canMove = false;
        SetXY(moveSpeed * 2f, 64);
        visible = false;
    }

    void OnCollision(GameObject other)
    {
        if (canMove && other.GetType() == typeof(Player))
        {
            Player player = (Player)other;
            player.SetHealth(player.GetHealth() - damage);
            hitSound.Play();
            ResetBullet();
        } else if (canMove && other.GetType() == typeof(Bullet) && other.parent != parent)
        {
            deathTime = Time.time + 10f;
        }
    }
}
