using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DamageObject : Sprite
{
    int damage = 5;
    protected Sound hitSound;
    protected bool hasPlayedHitSound = false;

    public DamageObject(string _fileName, int _damage) : base(_fileName)
    {
        SetOrigin(width / 2, height / 2);
        damage = _damage;
    }

    void OnCollision(GameObject other)
    {
        Console.WriteLine(other);
        if(other.GetType() == typeof(Player))
        {
            Player player = (Player)other;
            player.SetHealth(player.GetHealth() - damage);
        }
    }

    protected void PlayHitSound()
    {
        if(hitSound != null)
        {
            hitSound.Play();
            hasPlayedHitSound = true;
        }
    }
}
