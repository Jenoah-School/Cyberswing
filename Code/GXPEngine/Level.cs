using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



class Level : GameObject
{
    private Player player = null;

    public Level()
    {
        player = new Player();

        player.SetXY(game.width / 2, game.height / 2);

        AddChild(player);
    }
}
