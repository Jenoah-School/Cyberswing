using GXPEngine;
using GXPEngine.Core;
using System.Drawing;

public class HUD : GameObject
    {

    private Sprite healthBar;
    private Sprite healthBarInfill;
    private float defaultWidth = 128f;
    private int inset = 0;

    private EasyDraw healthText;
    private EasyDraw negativeLightningSprite = null;
    private EasyDraw positiveLightningSprite = null;


    public void DrawHealthbar(Vector2 _position, Vector2 _size, int _inset = 0)
    {
        inset = _inset;

        negativeLightningSprite = new EasyDraw("Assets/Sprites/Lightning_red.png", false);
        negativeLightningSprite.SetOrigin(negativeLightningSprite.width / 2, negativeLightningSprite.height / 2);
        negativeLightningSprite.height = (int)_size.y + 32;
        negativeLightningSprite.width = (int)_size.y + 32;
        negativeLightningSprite.SetXY(_position.x, _position.y + negativeLightningSprite.height / 4);
        LateAddChild(negativeLightningSprite);

        positiveLightningSprite = new EasyDraw("Assets/Sprites/Lighting_blue.png", false);
        positiveLightningSprite.SetOrigin(positiveLightningSprite.width / 2, positiveLightningSprite.height / 2);
        positiveLightningSprite.height = (int)_size.y + 32;
        positiveLightningSprite.width = (int)_size.y + 32;
        positiveLightningSprite.SetXY(_position.x, _position.y + positiveLightningSprite.height / 4);
        LateAddChild(positiveLightningSprite);

        negativeLightningSprite.alpha = 1;

        healthBar = new Sprite("Assets/Sprites/healthbar.png", false, false);
        healthBarInfill = new Sprite("Assets/Sprites/square.png", false, false);

        float aspectRatio = _size.x / _size.y;

        healthBar.width = (int)_size.y * (int)aspectRatio;
        healthBar.height = (int)_size.y;
        //healthBar.SetColor(44f / 255f, 62f / 255f, 80f / 255f);
        healthBar.SetXY(_position.x, _position.y);

        healthBarInfill.SetColor(40f / 255f, 37f / 255f, 48f / 255f);
        healthBarInfill.height = healthBar.height - inset;
        healthBarInfill.width = healthBar.width - inset;
        healthBarInfill.SetXY(healthBar.x, healthBar.y + inset / 2);

        AddChild(healthBar);
        AddChild(healthBarInfill);

        defaultWidth = healthBar.width;

        healthText = new EasyDraw((int)_size.x, (int)_size.y, false);
        //healthText.SetOrigin(healthText.width / 2, healthText.height / 2);
        healthText.SetXY(_position.x, _position.y);
        healthText.TextAlign(CenterMode.Center, CenterMode.Center);
        healthText.Text("100/100", (int)_size.x / 2f, (int)_size.y / 2f);

        AddChild(healthText);
    }

    public void ChangeLightningPolarity(bool isPositive)
    {
        if (isPositive)
        {
            negativeLightningSprite.alpha = 1;
            positiveLightningSprite.alpha = 0;
        }
        else
        {
            negativeLightningSprite.alpha = 0;
            positiveLightningSprite.alpha = 1;
        }
    }
    public void SetHealth(float _value)
    {
        SetFillAmount(_value / 100f);
        healthText.Clear(Color.Empty);
        //healthText.width = (int)(healthBar.width * _value);
        healthText.SetXY(healthBar.x, healthBar.y);
        healthText.SetColor(0, 0, 0);
        healthText.TextAlign(CenterMode.Center, CenterMode.Center);
        healthText.Text($"{_value}/100", healthBar.width * _value / 200, healthBar.height / 2f);
    }

    public void SetFillAmount(float _value)
    {
        _value = 1f - _value;
        healthBarInfill.width = (int)((float)defaultWidth * _value);
        healthBarInfill.x = healthBar.x + healthBar.width - (healthBarInfill.width) - inset;
    }
}

public class Progressbar : EasyDraw
{
    private EasyDraw infill = null;
    private int defaultWidth = 128;

    public Progressbar(Vector2 _position, Vector2 _size, int _inset, string _filename) : base(_filename, false)
    {
        
        float aspectRatio = _size.x / _size.y;
        width = (int)_size.y * (int)aspectRatio;
        height = (int)_size.y;
        defaultWidth = width * 2;


        infill = new EasyDraw("Assets/Sprites/square.png", false);
        infill.width = width * 2;
        infill.height = height * 2;

        AddChild(infill);

        SetXY(_position.x, _position.y);

    }

    public void SetInfillColor(float _r, float _g, float _b)
    {
        //infill.SetColor(_r, _g, _b);
    }

    
}
