using GXPEngine;

public class ElectricNet : DamageObject
{
    SoundChannel buzzChannel = null;
    Player player = null;
    private float buzzDistance = 512f;

    public ElectricNet(int _width = 256) : base("Assets/Sprites/electricNet.png", 100)
    {
        float scaledHeight = (float)height / (float)width;
        width = _width;
        height = (int)(_width * scaledHeight);
        buzzChannel = new Sound("Assets/Audio/SoundFX/ElectricBuzz.ogg", true).Play(false, 0, 0);
        hitSound = new Sound("Assets/Audio/SoundFX/ElectricHit.ogg");
    }

    void Update()
    {
        if (player != null)
        {
            float playerDistance = DistanceTo(player);
            float targetVolume = 1f - (playerDistance / buzzDistance);
            targetVolume = Mathf.Clamp(targetVolume, 0f, 1f);


            buzzChannel.Volume = targetVolume;
        }
        else
        {
            player = ((Level)parent).GetPlayer();
        }
    }

    void OnCollision(GameObject other)
    {
        if (other.GetType() == typeof(Player))
        {
            Player player = (Player)other;
            buzzChannel.Volume = 0;
            player.SetHealth(player.GetHealth() - 100);
            if (!hasPlayedHitSound)
            {
                PlayHitSound();
            }
        }
    }

    public void StopAudio()
    {
        if(buzzChannel != null)
        {
            buzzChannel.Stop();
        }
    }
}
