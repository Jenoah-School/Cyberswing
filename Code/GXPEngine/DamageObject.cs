using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DamageObject : Sprite
{
    int damage = 5;
    bool isColliding = false;

    public DamageObject(string _fileName, int _damage) : base(_fileName)
    {
        SetOrigin(width / 2, height / 2);
        damage = _damage;
    }

    void OnCollision(GameObject other)
    {
        if(other.GetType() == typeof(Player))
        {
            isColliding = true;
            Player player = (Player)other;
            player.SetHealth(player.GetHealth() - damage);
        }
    }
}
