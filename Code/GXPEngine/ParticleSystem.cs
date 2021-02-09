using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ParticleSystem : GameObject
{
    private Sprite[] particles = null;
    private float lifeTime = 0.5f;
    private int startSize = 8;
    private float startSpeed = 1f;

    public ParticleSystem(int availableParticles, Vector2 position)
    {
        SetXY(position.x, position.y);
        float rotationAngle = 360f / availableParticles;
        particles = new Sprite[availableParticles];
        for (int i = 0; i < availableParticles; i++)
        {
            particles[i] = new Sprite("Assets/Sprites/square.png", false, false);
            particles[i].SetOrigin(particles[i].width / 2, particles[i].height / 2);
            particles[i].rotation = rotationAngle * (i + 1);
            AddChild(particles[i]);
        }

        Simulate();
    }

    private async void Simulate()
    {
        float elapsedTime = 0f;
        float normalizedTime = 0f;
        int particleAmount = particles.Length;

        while (elapsedTime < lifeTime)
        {
            elapsedTime += (float)Time.deltaTime / 1000f;
            normalizedTime = elapsedTime / lifeTime;

            for (int i = 0; i < particleAmount; i++)
            {
                particles[i].Move(0, startSpeed * (normalizedTime - 1) * 2);
                particles[i].width = (int)(startSize * (float)(1f - normalizedTime));
                particles[i].height = (int)(startSize * (float)(1f - normalizedTime));
            }
            await Task.Delay(Time.deltaTime);
        }

        for (int i = 0; i < particleAmount; i++)
        {
            particles[i].visible = false;
        }

        LateDestroy();
    }
}
