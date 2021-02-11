using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class OneTimeAudio : GameObject
{
    private int destroyTime = 0;
    public OneTimeAudio(string _filename, bool _streaming, float _playbackLength)
    {
        Sound tempSound = new Sound(_filename, false, _streaming);
        tempSound.Play();
        destroyTime = Time.time + (int)(_playbackLength * 1000);
    }

    void Update()
    {
        if(Time.time > destroyTime)
        {
            LateDestroy();
        }
    }
}
