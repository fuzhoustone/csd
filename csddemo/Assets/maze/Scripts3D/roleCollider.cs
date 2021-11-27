using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using stoneState;

public class roleCollider : MonoBehaviour
{
    // Start is called before the first frame update
    private UCharacterController charInstance = null;
    private baseAI selfAI = null;
    private roleProperty selfProperty = null;
    private const string csTagRole = "Role";
    private const string csTagGold = "Gold";

    void Start()
    {
        selfAI = this.transform.GetComponent<baseAI>();
        selfProperty = this.transform.GetComponent<roleProperty>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision) {
        if (collision == null)
            return;

        if ((selfAI == null) || (selfProperty == null))
            return;

        if (collision.collider.tag == csTagRole) //碰到角色
        {
            roleProperty colPro = collision.gameObject.GetComponent<roleProperty>();
            if ((colPro.roleSort != selfProperty.roleSort)
                && (colPro.hp > 0)
                && (selfProperty.hp > 0)
                ) //是敌人，且双方存活
            {
                selfProperty.showUI();
                if (selfAI.enemyObj == null)
                { //进入战斗
                    CsdUIControlMgr.uiMgr().msgNoteTop();
                    selfAI.setEnemyObj(collision.gameObject);
                    selfAI.addEnemyToLst(collision.gameObject);
                    //  selfAI.lookAtEnemy(selfAI.gameObject, selfAI.enemyObj);
                }
                else
                {
                    selfAI.addEnemyToLst(collision.gameObject);
                }
            }
        }
        else if (collision.collider.tag == csTagGold) {
           // Debug.LogWarning("colision gold");
            collision.transform.gameObject.SetActive(false);
            stageMgr.stage().addReward();
        }

    }
}
