using System;
using System.Drawing;
using System.Windows.Forms;
using GXPEngine;
using GXPEngine.OpenGL;

public class MyGame : Game
{
    public static MyGame Instance = null;

    public Level currentLevel = null;
    private Sprite background = null;

    public MyGame() : base(1280, 720, false)
    {
        GL.glfwSetWindowPos((Screen.PrimaryScreen.Bounds.Width - width) / 2, (Screen.PrimaryScreen.Bounds.Height - height) / 2);
        GL.glfwSetWindowTitle("RKed");
        
        Instance = this;

        currentLevel = new Level();

        background = new Sprite("Assets/Sprites/square.png", false, false);
        background.SetColor(46f / 255f, 204f / 255f, 113f / 255f);
        background.width = game.width;
        background.height = game.height;

        AddChild(background);
        AddChild(currentLevel);
    }

    static void Main()
    {
        new MyGame().Start();
    }
}