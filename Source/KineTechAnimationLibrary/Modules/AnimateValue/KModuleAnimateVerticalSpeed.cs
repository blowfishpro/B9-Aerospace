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
/// Animates based on: 'this.vessel.verticalSpeed'
/// Value is the current vertical speed(away from orbital body)
/// of the part's vessel.
/// </summary>
[KPartModuleConfigurationDocumentationAttribute(
    "\n//Animates based on: 'this.vessel.verticalSpeed'" +
    "\n//Value is the current vertical speed(away from orbital body)" +
    "\n//of the part's vessel.")]
public class KModuleAnimateVerticalSpeed : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        return (float)(this.vessel.verticalSpeed - MinValue) / _Denominator;
    }
}
