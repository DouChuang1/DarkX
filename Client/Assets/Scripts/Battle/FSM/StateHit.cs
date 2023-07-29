using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StateHit : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.curAniState = AniState.Hit;
        entity.SetDir(Vector2.zero);
        entity.SetSkillMoveState(false);

        //被攻击 中断自己的攻击流程
        for(int i=0;i<entity.skMoveCBLst.Count;i++)
        {
            int tid = entity.skMoveCBLst[i];
            TimerSvc.Instance.DelTask(tid);
        }
        for (int i = 0; i < entity.skActionCBLst.Count; i++)
        {
            int tid = entity.skActionCBLst[i];
            TimerSvc.Instance.DelTask(tid);
        }
        if(entity.skEndCB!=-1)
        {
            TimerSvc.Instance.DelTask(entity.skEndCB);
            entity.skEndCB = -1;
        }
        
        if(entity.nextSkillID!=0 || entity.comboQue.Count>0)
        {
            entity.nextSkillID = 0;
            entity.comboQue.Clear();
            entity.battleMgr.lastAtkTime = 0;
            entity.battleMgr.comboIndex = 0;
        }
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        
    }

    public void Process(EntityBase entity, params object[] args)
    {
        if(entity.entityType== EntityType.Player)
        {
            entity.canControl = false;
        }
        entity.SetDir(Vector2.zero);
        entity.SetAction(Const.ActionHit);

        if(entity.entityType== EntityType.Player)
        {
            AudioSource charAudio = entity.GetAudio();
            AudioClip audio = ResSvr.instance.LoadAudio("ResAudio/" + Const.assassin_Hit);
            charAudio.clip = audio;
            charAudio.Play();
        }
        TimerSvc.Instance.AddTimeTask((int id) =>
        {
            entity.SetAction(Const.ActionDefault);
        }, (int)(GetHitAniLen(entity) *1000));
    }

    private float GetHitAniLen(EntityBase entity)
    {
        AnimationClip[] clips = entity.ctrl.ani.runtimeAnimatorController.animationClips;
        for(int i=0;i<clips.Length;i++)
        {
            string clipName = clips[i].name;
            if(clipName.Contains("hit")|| clipName.Contains("Hit")|| clipName.Contains("HIT"))
            {
                return clips[i].length;
            }
        }
        return 1;
    }
}