using System.Windows.Forms;
using GXPEngine;
using GXPEngine.OpenGL;

public class MyGame : Game
{
    public static MyGame Instance = null;

    public Level currentLevel = null;

    private Sound backgroundMusicLevel1 = null;
    private Sound backgroundMusicLevel2 = null;

    private SoundChannel backgroundMusicChannel = null;

    private int currentLevelID = -1;


    public MyGame() : base(1280, 720, false, false)
    {
        GL.glfwSetWindowPos((Screen.PrimaryScreen.Bounds.Width - width) / 2, (Screen.PrimaryScreen.Bounds.Height - height) / 2);
        GL.glfwSetWindowTitle("Cyber Swing");

        Instance = this;

        backgroundMusicLevel1 = new Sound("Assets/Audio/Music/BackgroundMusicLevel1.ogg", true);
        backgroundMusicLevel2 = new Sound("Assets/Audio/Music/BackgroundMusicLevel2.ogg", true);

        SwitchLevel(0);

        targetFps = 60;
    }

    static void Main()
    {
        new MyGame().Start();
    }

    public void SwitchLevel(int _id)
    {
        if (currentLevel != null)
        {
            currentLevel.LateDestroy();
            currentLevel.StopAudio();
            currentLevel = null;
        }

        if (backgroundMusicChannel != null && _id != currentLevelID)
        {
            backgroundMusicChannel.Stop();
        }

        switch (_id)
        {
            case 1:
                currentLevel = new Level1();
                if (_id != currentLevelID)
                {
                    backgroundMusicChannel = backgroundMusicLevel1.Play();
                }
                break;
            case 2:
                currentLevel = new Level2();
                if (_id != currentLevelID)
                {
                    backgroundMusicChannel = backgroundMusicLevel2.Play();
                }
                break;
            default:
                currentLevel = new Level1();
                if (_id != currentLevelID)
                {
                    backgroundMusicChannel = backgroundMusicLevel1.Play();
                }
                break;
        }

        currentLevelID = _id;
        LateAddChild(currentLevel);
    }

    public void RestartLevel()
    {
        SwitchLevel(currentLevelID);
    }
}