using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class HealthPickup : Pickup
{
    private int healthPoints = 10;

    public HealthPickup(int _healthPoints) : base("Assets/Sprites/square.png")
    {
        healthPoints = _healthPoints;
        SetColor(241f / 255f, 196f / 255f, 15f / 255f);
    }

    public override void OnCollision(GameObject other)
    {
        Player player = null;
        if (other.GetType() == typeof(Player))
        {
            player = (Player)other;
            player.SetHealth(player.GetHealth() + healthPoints);
            Console.WriteLine($"Added {healthPoints} to players health");
            parent.LateAddChild(new OneTimeAudio("Assets/Audio/SoundFX/Health.ogg", false, 1.5f));
            
            LateDestroy();
        }
    }
}
