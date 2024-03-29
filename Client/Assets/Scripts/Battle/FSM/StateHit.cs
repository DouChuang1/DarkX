﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StateHit : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.curAniState = AniState.Hit;
        entity.RemoveSkillCB();
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
            entity.Idle();
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