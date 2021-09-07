using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetStateMachine : MonoBehaviour
{
    public void HitJudge() {
        //判断死亡还是受击
    }

    private void defenseState() {
        //播放一次受击动画
        //同时播放扣血UI
        
    }

    private void dieState() {
        //播放死亡动画
        //同时播放扣血UI
    }


}
