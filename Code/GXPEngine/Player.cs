using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Player : Sprite
{
    public Player() : base("Assets/Sprites/square.png")
    {
        SetOrigin(width / 2, height / 2);
    }
}
