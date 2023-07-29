using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EntityPlayer : EntityBase
{
    public EntityPlayer()
    {
        entityType = EntityType.Player;
    }

    public override Vector2 GetDirInput()
    {
        return this.battleMgr.GetDirInput();
    }

    public override Vector2 CalcTargetDir()
    {
        EntityMonster monster = FindCloseTarget();
        if(monster!=null)
        {
            Vector3 target = monster.GetPos();
            Vector3 self = GetPos();
            Vector2 dir = new Vector2(target.x - self.x, target.z - self.z);
            return dir.normalized;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public EntityMonster FindCloseTarget()
    {
        List<EntityMonster> lst = battleMgr.GetEntityMonsters();
        if (lst == null || lst.Count==0)
        {
            return null;
        }
        Vector3 self = GetPos();
        EntityMonster targetMonster = null;

        float dis = 0;
        for(int i=0;i<lst.Count;i++)
        {
            Vector3 target = lst[i].GetPos();
            if(i==0)
            {
                dis = Vector3.Distance(self, target);
                targetMonster = lst[0];
            }
            else
            {
                float calcDis = Vector3.Distance(self, target);
                if(dis>calcDis)
                {
                    dis = calcDis;
                    targetMonster = lst[i];
                }
            }
        }
        return targetMonster;
    }

    public override void SetHpValue(int oldValue, int newValue)
    {
        BattleSys.instance.PlayerCtrlWnd.SetSelfHP(newValue);
    }

    public override void SetDodge()
    {
        GameRoot.Instance.dynamicWnd.SetSelfDodge();
    }
}
