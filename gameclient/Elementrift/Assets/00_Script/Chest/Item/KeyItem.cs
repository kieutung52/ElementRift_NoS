using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : ItemBase
{
    
    public override void UseItem(PlayerController player)
    {
        if (_owner == null)
        {
            this._owner = player;
            GateManager.Instance.AccessKeyRequired(this._owner);
        }
    }
}
