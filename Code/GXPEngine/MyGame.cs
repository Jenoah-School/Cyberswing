using System;
using System.Drawing;
using System.Windows.Forms;
using GXPEngine;
using GXPEngine.OpenGL;

public class MyGame : Game
{
    public static MyGame Instance = null;

    public Level currentLevel = null;
    

    public MyGame() : base(1280, 720, false, false)
    {
        GL.glfwSetWindowPos((Screen.PrimaryScreen.Bounds.Width - width) / 2, (Screen.PrimaryScreen.Bounds.Height - height) / 2);
        GL.glfwSetWindowTitle("Cyber Swing");
        
        Instance = this;

        currentLevel = new Level1();

        AddChild(currentLevel);

        targetFps = 60;
    }

    static void Main()
    {
        new MyGame().Start();
    }

    public void RestartLevel()
    {
        if(currentLevel != null)
        {
            currentLevel.LateDestroy();
            currentLevel.StopAudio();
            currentLevel = null;
        }

        currentLevel = new Level1();
        LateAddChild(currentLevel);
    }
}