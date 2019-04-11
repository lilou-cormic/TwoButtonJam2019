using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPickup : Item
{
    protected override void OnPickup(Collider2D collision)
    {
        Player.PowerPlayer();
    }
}
