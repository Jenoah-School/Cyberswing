using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GrapplePoint : Sprite
{ 
    private bool hasPositivePolarity = false;
    private Sprite grappleSprite = null;

    public GrapplePoint(int _width, int _height, bool _hasPositivePolarity = false) : base("Assets/Sprites/grappleNegative.png")
    {
        hasPositivePolarity = _hasPositivePolarity;

        if (hasPositivePolarity)
        {
            grappleSprite = new Sprite("Assets/Sprites/grapplePositive.png", false);
            grappleSprite.SetXY(-width / 2, 0);
            AddChild(grappleSprite);
        }

        SetOrigin(width / 2, 0);
        width = _width;
        height = _height;

        
    }

    public Vector2 GetGrapplePosition(float _xPos)
    {
        return new Vector2(_xPos, y + height / 2);
    }

    public bool CheckHitbox(float _xPos, float _yPos)
    {
        return HitTestPoint(_xPos, _yPos);
    }

    public bool IsPositive()
    {
        return hasPositivePolarity;
    }
}

