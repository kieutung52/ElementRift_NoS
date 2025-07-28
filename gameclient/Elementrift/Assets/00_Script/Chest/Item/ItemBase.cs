using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] protected string _itemName;
    [SerializeField] protected PlayerController _owner;

    public void Init(Vector3 position)
    {
        this.transform.position = position;
        this._owner = null;
    }

    public abstract void UseItem(PlayerController player);
}
