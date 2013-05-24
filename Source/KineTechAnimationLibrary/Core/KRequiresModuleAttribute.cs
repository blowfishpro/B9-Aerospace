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

using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
public class KRequiresModuleAttribute : System.Attribute
{
    public readonly Type RequiredType;

    public KRequiresModuleAttribute(Type requires)
    {
        if (requires == null)
            throw new ArgumentNullException();

        this.RequiredType = requires;
    }

    public static bool RequirementsMet(KModuleAnimate animate)
    {
        if(animate == null)
            return false;

        object[] objs = animate.GetType().UnderlyingSystemType.GetCustomAttributes(typeof(KRequiresModuleAttribute), true);

        if(objs.Length != 1)
            return false;

        KRequiresModuleAttribute[] attr =  objs as KRequiresModuleAttribute[];

        if(attr == null)
            return false;

        foreach(KRequiresModuleAttribute cAttr in attr)
        {
            bool check = false;
            
            foreach(PartModule current in animate.part.Modules)
            {
                if(current.GetType().UnderlyingSystemType != cAttr.RequiredType)
                    continue;


                check = true;
                break;
            }

            if(!check)
                return false;
        }

        return true;
    }
}

