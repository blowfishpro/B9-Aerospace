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
using UnityEngine;

/// <summary>
/// Provides logic for animation based on a value calculated every frame.
/// </summary>
public abstract class KModuleAnimateValue : KModuleAnimate
{
    #region Constants

    /// <summary>
    /// Format for error logging.
    /// </summary>
    private const string ERROR_FORMAT = "\n[{0}]\n\tMaxVelocity:{1}\n\tMinVelocity:{2}\n\tAnimationName:{3}\n\tMessage:{4}";

    #endregion


    #region KSPFields

    #region Configuration Defined
    /// <summary>
    /// The minimum value at which the animation will begin interpolating.
    /// </summary>
    [KPartModuleFieldConfigurationDocumentation(
        "0", 
        "The minimum value at which the animation will begin interpolating.")]
    [KSPFieldDebug("Min")]
    public float MinValue = 0;

    /// <summary>
    /// The animation frame for the minimum value.
    /// </summary>
    [KPartModuleFieldConfigurationDocumentation(
        "0.0", 
        "The animation frame for the minimum value.")]
    [KSPFieldDebug("MinFrame")]
    public float MinFrame = 0;

    /// <summary>
    /// The value at which the animation will complete its interpolation. 
    /// </summary>
    [KPartModuleFieldConfigurationDocumentation(
        "1", 
        "The value at which the animation will complete its interpolation.")]
    [KSPFieldDebug("Max")]
    public float MaxValue = 1;

    /// <summary>
    /// The animation frame for the maximum value.
    /// </summary>
    [KPartModuleFieldConfigurationDocumentation(
        "1.0", 
        "The animation frame for the maximum value.")]
    [KSPFieldDebug("MaxFrame")]
    public float MaxFrame = 1;

    /// <summary>
    /// When FALSE, once the animation has reached the maximum value it
    /// becomes locked and can not play at times other than 1.0f.
    /// </summary>
    [KPartModuleFieldConfigurationDocumentation(
        "True", 
        "When set to False, once the animation has reached the maximum" 
        + "\n\t//value it becomes locked.")]
    [KSPFieldDebug("CanDescendAfterMax?")]
    public bool CanDescendAfterMax = true;

    #endregion

    #region Persistent

    /// <summary>
    /// When TRUE, the animation is complete, CanDescendAfterMax == false, 
    /// and animation is locked at 1.0f;
    /// </summary>
    [KSPFieldDebug("IsMaxLocked?")]
    public bool IsMaxLocked = false;

    #endregion

    #endregion

    #region Fields

    /// <summary>
    /// _Denominator = this.MaxValue - this.MinValue;
    /// </summary>
    [KSPFieldDebug("_Denominator")]
    protected float _Denominator = -1;

    /// <summary>
    /// _AnimationRange = this.MaxFrame - this.MinFrame;
    /// </summary>
    [KSPFieldDebug("_AnimationRange")]
    protected float _AnimationRange = -1;

    #endregion

    #region KModuleAnimate

    protected override bool SetupModule()
    {
        if (MaxValue <= MinValue)
        {
            LogError("MinValue was greater than or equal to MaxValue!");
            return false;
        }

        _Denominator = MaxValue - MinValue;

        if(_Denominator == 0)
        {
            LogError("(MaxValue - MinValue == 0) Not even the Doctor can do that!");
            return false;
        }

        _AnimationRange = MaxFrame - MinFrame;

        if (_AnimationRange == 0.0)
        {
            LogError("(MaxFrame - MinFrame == 0) Not even the Doctor can do that!");
            return false;
        }

        return true;
    }

    protected override void UpdateModule()
    {
        if(_Denominator == 0)
        {
            LogError("(_Denominator == 0) Not even the Doctor can do that!");
        }

        if(_AnimationRange == 0)
        {
            LogError("(_AnimationRange == 0) Not even the Doctor can do that!");
        }

        if(!CanDescendAfterMax && !IsMaxLocked)
            IsMaxLocked = LastNormalTime >= MaxFrame;
        
        if (IsMaxLocked)
            LastNormalTime = MaxFrame;
    }

    #endregion

    #region Logging

    /// <summary>
    /// Logs a formatted error string to the Debug Console in KSP.
    /// </summary>
    /// <param name="message">The message to be formatted.</param>
    protected void LogError(string message)
    {
        Debug.LogError(
            string.Format(
            ERROR_FORMAT,
            MaxValue,
            MinValue,
            AnimationName,
            message));
    }

    #endregion
}
