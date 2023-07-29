using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class SkillMgr :MonoBehaviour
{
    private ResSvr ResSvr;
    public void Init()
    {
        ResSvr = ResSvr.instance;
    }

    public void AttackEffect(EntityBase entity,int skillID)
    {
        SkillCfg skillCfg = ResSvr.GetSkillCfg(skillID);
        if(!skillCfg.isCollide)
        {
            Physics.IgnoreLayerCollision(9, 10);
            TimerSvc.Instance.AddTimeTask((int tid) =>
            {
                Physics.IgnoreLayerCollision(9, 10, false);
            }, skillCfg.skillTime);
        }
        //玩家才自动锁定 怪物不要那么智能
        if(entity.entityType== EntityType.Player)
        {
            if (entity.GetDirInput() == Vector2.zero)
            {
                Vector2 dir = entity.CalcTargetDir();
                if (dir != Vector2.zero)
                {
                    entity.SetAtkRotation(dir);
                }
            }
            else
            {
                entity.SetAtkRotation(entity.GetDirInput(), true);
            }
        }
        entity.SetAction(skillCfg.aniAction);

        entity.SetFX(skillCfg.fx, skillCfg.skillTime);
        CalcSkillMove(entity, skillCfg);
        entity.canControl = false;
        entity.SetDir(Vector2.zero);
        if(!skillCfg.isBreak)
        {
            entity.entityState = EntityState.BatiState;
        }
        entity.skEndCB = TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.Idle();
        }, skillCfg.skillTime);
    }

    public void AttackDamage(EntityBase entity, int skillID)
    {
        SkillCfg skillCfg = ResSvr.GetSkillCfg(skillID);
        List<int> actionLst = skillCfg.skillActionLst;
        int sum = 0;
        for(int i=0;i<actionLst.Count;i++)
        {
            SkillActionCfg skillActionCfg = ResSvr.GetSkillActionCfg(actionLst[i]);
            sum += skillActionCfg.delayTime;
            int index = i;
            if(sum>0)
            {
                int actionId =TimerSvc.Instance.AddTimeTask((int tid) =>
                {
                    SkillAction(entity, skillCfg,index);
                    entity.RemoveActionCB(tid);
                },sum);
                entity.skActionCBLst.Add(actionId);
            }
            else
            {
                SkillAction(entity, skillCfg,index);
            }
        }
    }

    public void SkillAction(EntityBase entity,SkillCfg skillCfg,int index)
    {
        SkillActionCfg skillActionCfg = ResSvr.GetSkillActionCfg(skillCfg.skillActionLst[index]);
        int damage = skillCfg.skillDamageLst[index];
        if (entity.entityType== EntityType.Player)
        {
            List<EntityMonster> monsterLst = entity.battleMgr.GetEntityMonsters();
            for (int i = 0; i < monsterLst.Count; i++)
            {
                EntityMonster em = monsterLst[i];
                if (InRange(entity.GetPos(), em.GetPos(), skillActionCfg.radius) && InAngle(entity.GetTrans(), em.GetPos(), skillActionCfg.angle))
                {
                    CalcDamage(entity, em, skillCfg, damage);
                }
            }
        }
        else if(entity.entityType== EntityType.Monster)
        {
            EntityPlayer player = entity.battleMgr.entityPlayer;
            if (InRange(entity.GetPos(), player.GetPos(), skillActionCfg.radius) && InAngle(entity.GetTrans(), player.GetPos(), skillActionCfg.angle))
            {
                CalcDamage(entity, player, skillCfg, damage);
            }
        }
       
    }

    public void CalcDamage(EntityBase caster, EntityBase target,SkillCfg skillCfg, int damage)
    {
        int dmgSum = damage;
        if(skillCfg.dmgType== DamageType.AD)
        {
            //计算闪避
            int dodgeNum = PETools.RDInt(1, 100);
            if(dodgeNum<=target.Props.dodge)
            {
                PEProtocol.PECommon.Log("闪避Rate:" + dodgeNum + "/" + target.Props.dodge);
                target.SetDodge();
                return;
            }
            //计算属性加成
            dmgSum += caster.Props.ad;
            //计算暴击
            int criticalNum = PETools.RDInt(1, 100);
            if(criticalNum<=caster.Props.critical)
            {
                float criticalRate = 1 + (PETools.RDInt(1, 100) / 100.0f);
                dmgSum = (int)criticalRate * dmgSum;
                PEProtocol.PECommon.Log("暴击Rate:" + criticalNum + "/" + caster.Props.critical);
                target.SetCritical(dmgSum);
            }

            //计算穿甲
            int adddef = (int)((1 - caster.Props.pierce / 100.0f) * target.Props.addef);
            dmgSum -= adddef;
        }
        else if(skillCfg.dmgType== DamageType.AP)
        {
            //计算属性加成
            dmgSum += caster.Props.ap;
            dmgSum -= target.Props.apdef;
        }
        if (dmgSum <= 0)
        {
            dmgSum = 0;
            return;
        }
        target.SetHurt(dmgSum);
        if(target.Hp<dmgSum)
        {
            target.Hp = 0;
            target.Die();
            target.battleMgr.RemoveMonster(target.Name);
        }
        else
        {
            target.Hp -= dmgSum;
            if(target.entityState == EntityState.None && target.GetBreakState())
            {
                target.Hit();
            }
            
        }
            
    }

    public bool InRange(Vector3 from,Vector3 to,float range)
    {
        float dis = Vector3.Distance(from, to);
        if(dis<=range)
        {
            return true;
        }
        return false;
    }

    public bool InAngle(Transform trans,Vector3 to,float angle)
    {
        if(angle==360)
            return true;
        else
        {
            Vector3 start = trans.forward;
            Vector3 dir = (to - trans.position).normalized;
            float ang = Vector3.Angle(start, dir);
            if(ang<=angle/2)
            {
                return true;
            }
            return false;
        }
    }

    public void SkillAttack(EntityBase entity, int skillID)
    {
        AttackDamage(entity, skillID);
        AttackEffect(entity, skillID);
    }

    private void CalcSkillMove(EntityBase entity,SkillCfg skillCfg)
    {
        int sum = 0;
        for (int i = 0; i < skillCfg.skillMoveLst.Count; i++)
        {
            SkillMoveCfg skillMoveCfg = ResSvr.GetSkillMoveCfg(skillCfg.skillMoveLst[i]);
            float speed = skillMoveCfg.moveDis / (skillMoveCfg.moveTime / 1000f);
            sum += skillMoveCfg.delayTime;
            if (sum > 0)
            {
                int moveId =TimerSvc.Instance.AddTimeTask((int tid) =>
                {
                    entity.SetSkillMoveState(true, speed);
                    entity.RemoveMoveCB(tid);
                }, sum);
                entity.skMoveCBLst.Add(moveId);
            }
            else
            {
                entity.SetSkillMoveState(true, speed);
            }
            sum += skillMoveCfg.moveTime;
            int stopId =TimerSvc.Instance.AddTimeTask((int tid) =>
            {
                entity.SetSkillMoveState(false);
                entity.RemoveMoveCB(tid);
            }, sum);
            entity.skMoveCBLst.Add(stopId);
        }
    }
}
