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
/// Animated based on: 'this.vessel.staticPressure'.
/// Value is the static pressure of the atomosphere
/// at the current position of this part's vessel.
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Animated based on: 'this.vessel.staticPressure'." +
"\n//Value is the static pressure of the atomosphere" +
"\n//at the current position of this part's vessel.")]
public class KModuleAnimateStaticPressure : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        return (float)(this.vessel.staticPressure - MinValue) / _Denominator;
    }
}
