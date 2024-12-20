using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class BaseState <T> : IState{
    protected T blackBoard;

    void IState.Do() { }

    void IState.Enter() { }

    void IState.Exit() { }

    void IState.FixedDo() { }
}
