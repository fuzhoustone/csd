using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseAI : MonoBehaviour
{
    public delegate void StateActEnd();

    public event StateActEnd EventStandEnd;
    public event StateActEnd EventAttackEnd;
    public event StateActEnd EventDieStart;
    public event StateActEnd EventDieEnd;

    public const string csStand = "stand";
    public const string csRun = "run";
    public const string csAttack = "attack1";
    public const string csAttack2 = "attack2";
    public const string csDie = "die";

    public virtual void stateStandEnd() {
        if(EventStandEnd != null)
            EventStandEnd();
    }

    public virtual void stateAttackEnd()
    {
        if(EventAttackEnd != null)
            EventAttackEnd();
    }

    public virtual void stateDieStart()
    {
        if(EventDieStart != null)
            EventDieStart();
    }

    public virtual void stateDieEnd()
    {
        if(EventDieEnd != null)
            EventDieEnd();
    }
}
