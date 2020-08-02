using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerAcademy : Academy
{
    private TigerArea[] areas;


    public override void AcademyReset()
    {
        if (areas == null)
        {
            areas = GameObject.FindObjectsOfType<TigerArea>();
        }

        foreach (TigerArea area in areas)
        {

            area.numTruffles = (int)resetParameters["num_truffles"];
            area.numStumps = (int)resetParameters["num_stumps"];
            area.spawnRange = resetParameters["spawn_range"];

            area.ResetArea();
        }
    }
}
