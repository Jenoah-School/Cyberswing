using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GrapplePoint : Sprite
{ 
    private bool hasPositivePolarity = false;

    public GrapplePoint(bool _hasPositivePolarity = false) : base("Assets/Sprites/square.png")
    {
        hasPositivePolarity = _hasPositivePolarity;
        SetOrigin(width / 2, height / 2);
        if (hasPositivePolarity)
        {
            SetColor(52f / 255f, 152f / 255f, 219f / 255f);
        }
        else
        {
            SetColor(231 / 255f, 76 / 255f, 60 / 255f);
        }
    }

    public bool IsPositive()
    {
        return hasPositivePolarity;
    }
}

