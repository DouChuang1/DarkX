using System.Linq;
using System.Text;
using UnityEngine;

public class MapMgr : MonoBehaviour
{
    public BattleMgr BattleMgr;
    private int waveIndex = 1;
    public void Init(BattleMgr battleMgr)
    {
        BattleMgr = battleMgr;

        //实例化第一批怪物
        this.BattleMgr.LoadMonsterByWaveID(waveIndex);
    }
}
