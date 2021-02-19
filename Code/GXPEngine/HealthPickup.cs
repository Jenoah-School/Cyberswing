using GXPEngine;
using System;

public class HealthPickup : Pickup
{
    private int healthPoints = 10;

    public HealthPickup(int _healthPoints) : base("Assets/Sprites/healthPickup.png")
    {
        healthPoints = _healthPoints;
        width = 64;
        height = 64;
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
