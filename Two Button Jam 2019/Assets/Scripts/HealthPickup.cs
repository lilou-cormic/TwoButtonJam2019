using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Item
{
    protected override void OnPickup(Collider2D collision)
    {
        Player.HealPlayer(1);
    }
}
