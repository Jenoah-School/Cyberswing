using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HUD : GameObject
    {

    private Progressbar healthBar;
    private EasyDraw healthText;
    private Progressbar hookCharge;

    public void DrawHealthbar(Vector2 _position, Vector2 _size, int _inset = 0)
    {
        EasyDraw lightningSprite = new EasyDraw("Assets/Sprites/Lightning.png", false);
        lightningSprite.SetOrigin(lightningSprite.width / 2, lightningSprite.height / 2);
        lightningSprite.height = (int)_size.y + _inset;
        lightningSprite.width = (int)_size.y + _inset;
        lightningSprite.SetXY(_position.x - lightningSprite.width / 2, _position.y + lightningSprite.height / 2);
        AddChild(lightningSprite);

        

        healthBar = new Progressbar(_position, _size, _inset);

        healthBar.SetColor(44f / 255f, 62f / 255f, 80f / 255f);
        healthBar.SetInfillColor(241f / 255f, 196f / 255f, 15f / 255f);

        AddChild(healthBar);

        healthText = new EasyDraw((int)_size.x, (int)_size.y, false);
        //healthText.SetOrigin(healthText.width / 2, healthText.height / 2);
        healthText.SetXY(_position.x, _position.y);
        healthText.SetColor(0, 0, 0);
        healthText.TextAlign(CenterMode.Center, CenterMode.Center);
        healthText.Text("100/100", (int)_size.x / 2f, (int)_size.y / 2f);

        AddChild(healthText);
    }

    public void DrawHookCharge(Vector2 _position, Vector2 _size, int _inset = 16)
    {
        hookCharge = new Progressbar(_position, _size, _inset);

        hookCharge.SetColor(44f / 255f, 62f / 255f, 80f / 255f);
        hookCharge.SetInfillColor(155f / 255f, 89f / 255f, 182f / 255f);

        AddChild(hookCharge);
    }

    public void SetHealth(float _value)
    {
        healthBar.SetFillAmount(_value / 100f);
        healthText.Clear(Color.Empty);
        //healthText.width = (int)(healthBar.width * _value);
        healthText.SetXY(healthBar.x, healthBar.y);
        healthText.SetColor(0, 0, 0);
        healthText.TextAlign(CenterMode.Center, CenterMode.Center);
        healthText.Text($"{_value}/100", healthBar.width * _value / 200, healthBar.height / 2f);
    }

    public void SetHookCharge(float _value)
    {
        hookCharge.SetFillAmount(_value / 1f);
    }
}

public class Progressbar : EasyDraw
{
    private EasyDraw infill = null;
    private int defaultWidth = 128;

    public Progressbar(Vector2 _position, Vector2 _size, int _inset) : base((int)_size.x, (int)_size.y, false)
    {
        defaultWidth = (int)_size.x;

        infill = new EasyDraw((int)_size.x, (int)_size.y, false);

        SetXY(_position.x, _position.y);

        Rect((int)_size.x / 2, (int)_size.y / 2, (int)_size.x, (int)_size.y);
        infill.Rect((int)_size.x / 2, (int)_size.y / 2, (int)_size.x - _inset, (int)_size.y - _inset);

        AddChild(infill);
    }

    public void SetInfillColor(float _r, float _g, float _b)
    {
        infill.SetColor(_r, _g, _b);
    }

    public void SetFillAmount(float _value)
    {
        infill.width = (int)((float)defaultWidth * _value);
    }
}
