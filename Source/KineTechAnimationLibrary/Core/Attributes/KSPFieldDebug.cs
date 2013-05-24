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
/// Used for allowing fields to be visible on the 
/// partmodule when compiled with 'DEBUG' symbol, 
/// but allowing their display state to be different 
/// when compiled without.
/// </summary>
public class KSPFieldDebugAttribute : KSPField
{
    public readonly bool IsGuiActive;

    /// <summary>
    /// Initializes a new instance of 
    /// the KSPFieldDebugAttribute class.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isGuiActive"></param>
    public KSPFieldDebugAttribute(string name, bool isGuiActive = false)
    {
        this.guiActive =
#if DEBUG
                        true;
#else
                        isGuiActive;
#endif
        this.guiName = name;
    }
}

