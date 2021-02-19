using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Menu : Level
{
    Sprite logo;
    Sprite background;

    Button playButton;
    Button quitButton;

    public Menu()
    {
        background = new Sprite("Assets/Sprites/background_menu.jpg", false, false);
        background.width = game.width;
        background.height = game.height;

        logo = new Sprite("Assets/Sprites/logo.png", false, false);
        logo.SetOrigin(logo.width / 2, logo.height / 2);
        float aspectRatio = logo.width / logo.height;
        logo.height = game.height / 6;
        logo.width = (int)(game.height / 6 * aspectRatio);
        logo.SetXY(game.width / 2, game.height / 6);

        playButton = new Button("Play", true);
        playButton.SetXY(game.width / 2, game.height / 6 * 3);
        playButton.scale = 0.8f;
        playButton.SetButtonAction(new Action(() => StartGame()));

        quitButton = new Button("Quit", true);
        quitButton.SetXY(game.width / 2, game.height / 6 * 4.5f);
        quitButton.scale = 0.8f;
        quitButton.SetButtonAction(new Action(() => QuitGame()));

        AddChild(background);
        AddChild(logo);
        AddChild(playButton);
        AddChild(quitButton);


    }

    void Update()
    {

    }

    public void StartGame()
    {
        MyGame.Instance.SwitchLevel(1);
    }

    public void QuitGame()
    {
        game.Destroy();
    }
}
