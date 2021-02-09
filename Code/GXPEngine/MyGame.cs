using System;
using System.Drawing;
using System.Windows.Forms;
using GXPEngine;
using GXPEngine.OpenGL;

public class MyGame : Game
{
    public static MyGame Instance = null;

    private Level currentLevel = null;

    public MyGame() : base(1280, 720, false)
    {
        GL.glfwSetWindowPos((Screen.PrimaryScreen.Bounds.Width - width) / 2, (Screen.PrimaryScreen.Bounds.Height - height) / 2);
        GL.glfwSetWindowTitle("RKed");
        
        Instance = this;

        currentLevel = new Level();

        AddChild(currentLevel);
    }

    static void Main()
    {
        new MyGame().Start();
    }
}