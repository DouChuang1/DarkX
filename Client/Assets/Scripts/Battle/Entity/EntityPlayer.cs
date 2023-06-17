using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EntityPlayer:EntityBase
{
    public override Vector2 GetDirInput()
    {
        return this.battleMgr.GetDirInput();
    }
}
