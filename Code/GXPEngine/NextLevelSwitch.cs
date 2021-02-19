using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class NextLevelSwitch : Sprite
{
    private int nextLevelID = 0;

    Sound finishSound = null;

    public NextLevelSwitch(int _nextLevelID, string _musicFileName = "") : base("Assets/Sprites/square.png")
    {
        nextLevelID = _nextLevelID;
        alpha = 0;
        if (!string.IsNullOrEmpty(_musicFileName))
        {
            finishSound = new Sound(_musicFileName);
        }
    }

    void OnCollision(GameObject other)
    {
        if (other.GetType() == typeof(Player))
        {
            if(finishSound != null)
            {
                finishSound.Play();
            }
            MyGame.Instance.SwitchLevel(nextLevelID);
        }
    }
}
