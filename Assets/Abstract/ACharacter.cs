using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Abstract
{
    //uint _currentAnimationState = STATE_IDLE;

    abstract class ACharacter
    {
        virtual public void Attaquer() { }

        virtual public void changeState(uint state)
        {
        }
    }
}
