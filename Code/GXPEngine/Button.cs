using GXPEngine;
using System;
using System.Drawing;

class Button : Sprite
{
    //Backend
    private Action buttonAction = null;
    private bool isClickable = true;
    public bool isEnabled = true;

    //Visuals
    private string buttonText = "";
    private string disabledText = "Disabled";
    private EasyDraw text = null;

    public Button(string _buttonText, bool _isClickable = true, string _backgroundImage = "Assets/Sprites/Button.png") : base(_backgroundImage, true, true)
    {
        isClickable = _isClickable;
        alpha = 0;

        SetOrigin(width / 2, height / 2);
        buttonText = _buttonText;
        text = new EasyDraw(width, height, false);
        text.TextSize(96);
        text.SetColor(1f, 99f / 255f, 99f / 255f);
        text.SetOrigin(width / 2, height / 2);
        text.TextAlign(CenterMode.Center, CenterMode.Center);
        text.Text(buttonText, width / 2, height / 2);

        AddChild(text);
    }

    private void Update()
    {
        if (isClickable)
        {
            if (Input.GetMouseButtonUp(0) && IsPressed())
            {
                ExecuteButtonAction();
            }
        }
    }

    /// <summary>
    /// Checks whether or not the mouse is within the sprite of the button
    /// </summary>
    /// <returns>If player hovers the button sprite</returns>
    public bool IsPressed()
    {
        return HitTestPoint(Input.mouseX, Input.mouseY);
    }

    /// <summary>
    /// Sets the action that should be exectuted when the button is triggered
    /// </summary>
    /// <param name="_buttonAction">The action that the button should trigger</param>
    public void SetButtonAction(Action _buttonAction)
    {
        buttonAction = _buttonAction;
    }

    /// <summary>
    /// Executes the given button action
    /// </summary>
    public void ExecuteButtonAction()
    {
        if (!isEnabled)
        {
            return;
        }

        Console.WriteLine("Executing button action");
        if (buttonAction != null)
        {
            buttonAction.Invoke();
        }
    }

    /// <summary>
    /// Disables the possibility to execute the button action and changes the button text
    /// </summary>
    public void Disable()
    {
        isEnabled = false;
        text.Clear(Color.Empty);
        text.SetOrigin(width / 2, height / 2);
        text.TextAlign(CenterMode.Center, CenterMode.Center);
        text.Text(disabledText, width / 2, height / 2);
    }

    /// <summary>
    /// Sets the text of the button message upon disable
    /// </summary>
    /// <param name="_disabledText">The text upon disabling</param>
    public void SetDisabledText(string _disabledText)
    {
        disabledText = _disabledText;
    }
}
