using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fightManager : MonoBehaviour
{
    const float csTurnTime = 0.1f;

    float fightTime = 0.0f;
    float nowTime = 0.0f;
    bool isCalTime = false;  //是否计时
    bool isEnd = false;


    roleProperty role1, role2, role3;
    roleProperty enemy1, enemy2, enemy3;

    // Start is called before the first frame update
    void Start()
    {
        isCalTime = true;
    }

    public void startFight() {
        StartCoroutine(timeTurn());
    }

    IEnumerator timeTurn() {
        float time = 0;
        while (isEnd == false)
        {
            while (time < csTurnTime) // 还需另外设置跳出循环的条件
            {
                time += Time.deltaTime;
                yield return null;
            }

            playOneTurn();

        }
    }
        

    /*
    // Update is called once per frame
    void Update() //不使用Update，改用协程
    {
        if (isCalTime == false) {
            return ;
        }

        nowTime = nowTime + Time.deltaTime;
        if (nowTime > csTurnTime) {
            nowTime = nowTime - csTurnTime;

            isCalTime = false; //暂停计时
            playOneTurn(); //执行一回合
            if (isEnd() == false) //未结束
            {
                isCalTime = true;
            }
        }

        //fightTime = fightTime 
    }

    */

    private void playRoleTurn(roleProperty tmpProperty) {
        if (tmpProperty.hp <= 0)
        {
            return ;
        }


        if (calTime(tmpProperty)) { //是否到攻击时间
            //移动到攻击目标位置，攻击

        }
    }

    private bool calTime(roleProperty tmpProperty) {
        bool res = false;

        tmpProperty.nowTurnTime = tmpProperty.nowTurnTime + csTurnTime;
        if (tmpProperty.nowTurnTime >= tmpProperty.turnTime) {
            tmpProperty.nowTurnTime = 0;
            res = true;
        }

        return res;
    }

    private void calIsEnd() {

        isEnd = true;
    }

    private void playOneTurn() {
        playRoleTurn(role1);  //判断并执行回合，若已死亡的，则不会执行
        playRoleTurn(role2);
        playRoleTurn(role3);
        playRoleTurn(enemy1);
        playRoleTurn(enemy2);
        playRoleTurn(enemy3);
    }
}
