using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : Item
{
    protected override void OnPickup(Collider2D collision)
    {
        Player.ShieldPlayer();
    }
}
