using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GameBehaviour
{
    public const float gravity = -9.81f;

    public const int tileSize = 128;

    public static float GetHorizontalAxis()
    {
        float horizontalInput = 0f;

        if (Input.GetKey(Key.A))
        {
            horizontalInput -= 1;
        }

        if (Input.GetKey(Key.D))
        {
            horizontalInput += 1;
        }

        return horizontalInput;
    }
}
