using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


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
        entity.SetAction(skillCfg.aniAction);

        entity.SetFX(skillCfg.fx, skillCfg.skillTime);
        CalcSkillMove(entity, skillCfg);
        entity.canControl = false;
        entity.SetDir(Vector2.zero);
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.Idle();
        }, skillCfg.skillTime);
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
                TimerSvc.Instance.AddTimeTask((int tid) =>
                {
                    entity.SetSkillMoveState(true, speed);
                }, sum);
            }
            else
            {
                entity.SetSkillMoveState(true, speed);
            }
            sum += skillMoveCfg.moveTime;
            TimerSvc.Instance.AddTimeTask((int tid) =>
            {
                entity.SetSkillMoveState(false);
            }, sum);
        }
    }
}
