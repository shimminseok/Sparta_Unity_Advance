using System;
using UnityEngine;

public static class ItemEffectFactory
{
    public static IItemEffect CreateItemEffect(ItemSO itemSo)
    {
        return itemSo.ItemType switch
        {
            _                   => null
        };
    }
}