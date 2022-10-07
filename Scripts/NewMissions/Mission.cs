using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
    int reward;
    int p, d;
    string pickup, dropoff, item;
    Transform pickt, dropt;

    public Mission(int reward, string itemName, string pick, string drop, int p, int d, Transform pickt, Transform dropt)
    {
        this.p = p;
        this.d = d;
        this.item = itemName;
        this.pickup = pick;
        this.dropoff = drop;
        this.pickt = pickt;
        this.dropt = dropt;
    }

    public Transform GetPickT()
    {
        return this.pickt;
    }
    public Transform GetDropT()
    {
        return this.dropt;
    }
    public int PickUpIndex()
    {
        return this.p;
    }

    public int GetReward()
    {
        return this.reward;
    }

    public string GetItem()
    {
        return this.item;
    }

    public string GetPickUp()
    {
        return pickup;
    }
    public string GetDropOff()
    {
        return dropoff;
    }
}
