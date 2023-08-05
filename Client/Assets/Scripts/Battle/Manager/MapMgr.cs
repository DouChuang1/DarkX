using System.Linq;
using System.Text;
using UnityEngine;

public class MapMgr : MonoBehaviour
{
    public BattleMgr BattleMgr;
    private int waveIndex = 1;
    public TriggerData[] triggerArr;
    public void Init(BattleMgr battleMgr)
    {
        BattleMgr = battleMgr;

        //实例化第一批怪物
        this.BattleMgr.LoadMonsterByWaveID(waveIndex);
    }

    public void TriggerMonsterBorn(TriggerData trigger,int waveIndex)
    {
        if(BattleMgr!=null)
        {
            BoxCollider boxCollider = trigger.gameObject.GetComponent<BoxCollider>();
            boxCollider.isTrigger = false;

            BattleMgr.LoadMonsterByWaveID(waveIndex);
            BattleMgr.ActiveCurrentBatchMonsters();
            BattleMgr.triggerCheck = true;
        }
    }

    public bool SetNextTriggerOn()
    {
        waveIndex += 1;
        for(int i=0;i<triggerArr.Length;i++)
        {
            if(triggerArr[i].triggerWave==waveIndex)
            {
                BoxCollider boxCollider = triggerArr[i].gameObject.GetComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                return true;
            }
        }
        return false;
    }
}
