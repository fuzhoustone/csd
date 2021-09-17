using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterCollider : MonoBehaviour
{
    //private GameObject roleInstance = null;
    //private IbaseANI monControl = null;

    private UCharacterController charInstance = null;
    private baseAI selfAI = null;
    private roleProperty selfProperty = null;
    private const string csTagRole = "Role";

    void Start()
    {
        selfAI = this.transform.GetComponent<baseAI>();
        selfProperty = this.transform.GetComponent<roleProperty>();
    }


    private void OnCollisionEnter(Collision collision)
    //    void OnTriggerStay(Collider other)
   // private void OnTriggerEnter(Collider other) //需区分碰撞的对象，限制为敌人才有反应
    {
        
        if (collision == null)
            return;

        // if (charInstance == null)
        // {
        //     charInstance = App.Game.character;
        // }
        if ((selfAI == null) || (selfProperty == null)) 
            return;

        ////以上为容错及过滤其它碰撞///////////
        if (collision.collider.tag == csTagRole) //碰到角色
        {
            roleProperty colPro = collision.gameObject.GetComponent<roleProperty>();
            if ((colPro.roleSort != selfProperty.roleSort)
                && (colPro.hp > 0)
                && (selfProperty.hp > 0)
                ) //是敌人，且双方存活
            {
                selfProperty.showUI(); //显示自己的UI即可
                if (selfAI.enemyObj == null) //当前为空，马上攻击敌人
                {
                    selfAI.setEnemyObj(collision.gameObject);   //设置朝向及攻击目标是相互的
                    selfAI.addEnemyToLst(collision.gameObject);
                    selfAI.lookAtEnemy(selfAI.gameObject, selfAI.enemyObj);
                }
                else {                     //
                    selfAI.addEnemyToLst(collision.gameObject);
                }
            }
        }
        /*
        if (monControl.IsDie() == false)
        {
           
            //判断目标是否存活
            //双方的enemy加上对方, 双方显示血条
      
            if (monControl.IsInAttackState() == false)
            {
                monControl.setToAttack(charInstance.roleInstance.transform.position);
            }
           
        }*/
    }

    void OnTriggerExit(Collider other)
    {
        /*
       if (other == null)
           return;

      if (charInstance == null)
      {
          charInstance = App.Game.character;
      }

              if (monControl == null)
              {
                  monControl = this.transform.GetComponent<IbaseANI>();
              }

              if (monControl.IsDie() == false)
              {
                  if (monControl.IsInAttackState() == true)
                  {
                      monControl.setToStand();
                  }
              }
              */
    }
}
