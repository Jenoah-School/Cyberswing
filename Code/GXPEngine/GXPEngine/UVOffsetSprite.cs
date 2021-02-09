using System;
using GXPEngine;

public class UVOffsetSprite : Sprite
{
    protected float _offsetX = 0;
    protected float _offsetY = 0;

    public UVOffsetSprite(string filename) : base(filename)
    {
        texture.wrap = true;
    }

    public void SetOffset(float x, float y)
    {
        _offsetX = x;
        _offsetY = y;
        SetUVs();
    }

    public void AddOffset(float x, float y)
    {
        _offsetX += x;
        _offsetY += y;
        SetUVs();
    }

    protected override void SetUVs()
    {
        float left = _mirrorX ? 1.0f : 0.0f;
        float right = _mirrorX ? 0.0f : 1.0f;
        float top = _mirrorY ? 1.0f : 0.0f;
        float bottom = _mirrorY ? 0.0f : 1.0f;

        left += _offsetX;
        right += _offsetX;
        top += _offsetY;
        bottom += _offsetY;

        _uvs = new float[8] { left, top, right, top, right, bottom, left, bottom };
    }
}