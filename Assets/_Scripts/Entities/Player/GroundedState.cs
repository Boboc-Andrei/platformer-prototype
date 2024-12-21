using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class GroundedState : CharacterState {

    public override void Do() {
        if(!Character.IsGrounded) {
            IsComplete = true;
        }

    }
}
