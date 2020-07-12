﻿using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAcademy : Academy
{
    private PigArea[] areas;

    /// Reset the academy

    public override void AcademyReset()
    {
        if (areas == null)
        {
            areas = GameObject.FindObjectsOfType<PigArea>();
        }

        foreach (PigArea area in areas)
        {

            area.numTruffles = (int)resetParameters["num_truffles"];
            area.numStumps = (int)resetParameters["num_stumps"];
            area.spawnRange = resetParameters["spawn_range"];

            area.ResetArea();
        }
    }
}
