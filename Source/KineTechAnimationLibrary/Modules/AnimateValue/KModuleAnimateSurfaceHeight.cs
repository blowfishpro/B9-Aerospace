/* * * * * * * * * * * * * * * */
/* Author: Dustin 'Kine' Judson
/*
/* This work is licensed under the 
/* Creative Commons Attribution-ShareAlike 3.0 Unported License. 
/*
/* To view a copy of this license, visit: 
/* http://creativecommons.org/licenses/by-sa/3.0/
/* 
/* A the following must be included with 
/* any and all formats using this work or any
/* derivatives of this work:
/* 
/* "Kine-Tech Animation Library by Dustin 'Kine' Judson is viewable at: https://github.com/KineMorto/KineTechAnimationLibrary"
/*
/* * * * * * * * * * * * * * * */

/// <summary>
/// Value is the current height of the vessel
/// above the surface of the planet. Not to be 
/// confused with altitude which is the height 
/// above sea-level.
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Value is the current height of the vessel " +
"\n//above the surface of the planet. Not to be " +
"\n//confused with altitude which is the height " +
"\n//above sea-level.")]
public class KModuleAnimateSurfaceHeight : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        Vector3d radial = 
            this.vessel.transform.position - this.vessel.mainBody.transform.position;

        return (float)((this.vessel.mainBody.pqsController
            .GetSurfaceHeight(radial.normalized) - MinValue) / _Denominator);
    }
}
