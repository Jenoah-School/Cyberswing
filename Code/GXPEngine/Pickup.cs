using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Pickup : Sprite
{
    public Pickup(string _fileName) : base(_fileName, true)
    {
        SetOrigin(width / 2, height / 2);
    }

    public virtual void OnCollision(GameObject other)
    {
        if(other.GetType() == typeof(Player))
        {
            Console.WriteLine("No pickup action configured");
            LateDestroy();
        }
    }
}
