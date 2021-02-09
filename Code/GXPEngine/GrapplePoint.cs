using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GrapplePoint : Sprite
{
    public GrapplePoint() : base("Assets/Sprites/square.png")
    {
        SetOrigin(width / 2, height / 2);
        SetColor(231 / 255f, 76 / 255f, 60 / 255f);
    }
}

