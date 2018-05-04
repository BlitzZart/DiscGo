using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankTile : Sector {
    public Color InColor;

    public AFacility Slot;

    protected override void DoOnEnter(Collider coll) {
        base.DoOnEnter(coll);
        Color c = spriteRenderer.color;
        spriteRenderer.color = new Color(InColor.r, InColor.g, InColor.b, c.a);
    }
    protected override void DoOnExit(Collider coll) {
        base.DoOnExit(coll);
        spriteRenderer.color = sectorColor;
    }
}
