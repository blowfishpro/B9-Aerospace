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
using System.Text;
using UnityEngine;
using System.Reflection;

/// <summary>
/// Generates documentation for the PartModules 
/// contained within this assembly.
/// </summary>
public class KConfigDocumentationGenerator : MonoBehaviour
{
    //private static MethodInfo _RecurseFormat;
    //private static MethodInfo _PreFormatConfig;
    
    private static MethodInfo _WriteAllText;

    private void OnLevelWasLoaded(int level)
    {
        if (HighLogic.LoadedScene < GameScenes.MAINMENU)
            return;

        Generate();
        DestroyImmediate(this.gameObject);
    }

    public static void AddValueToNode(
        ref StringBuilder working,
        FieldInfo field,
        KPartModuleFieldConfigurationDocumentationAttribute attribute)
    {
        if(working == null)
            return;

        if(field == null)
            return;

        if(attribute == null)
            return;

        //Create description;
        working.Append("\t//");
        working.AppendLine(attribute.Description);

        //WriteValue
        working.Append("\t");
        working.Append(field.Name);
        working.Append(" = ");
        working.AppendLine(attribute.DefaultValue);
        working.AppendLine();
    }

    /// <summary>
    /// Adds the documentation for the current 
    /// module type to the referenced stringbuild.
    /// </summary>
    /// <returns></returns>
    public static bool AddModuleDocumentation(ref StringBuilder working, Type moduleType)
    {
        if(moduleType == null)
            return false;

        object[] attrs = moduleType
            .GetCustomAttributes(typeof(KPartModuleConfigurationDocumentationAttribute), true);

        if(attrs.Length == 0)
            return false;

        KPartModuleConfigurationDocumentationAttribute attribute = 
                attrs[0] as KPartModuleConfigurationDocumentationAttribute;

        if(attribute == null)
            return false;

        attrs = moduleType.GetCustomAttributes(typeof(KRequiresModuleAttribute), true);

        string moduleStringName = string.Concat(moduleType.UnderlyingSystemType.Name, ".cs");

        working.Append("//");
        working.Append('=', moduleStringName.Length);
        working.AppendLine();

        working.Append("//");
        working.AppendLine(moduleStringName);

        working.Append("//");
        working.Append('=', moduleStringName.Length);

        working.AppendLine(attribute.ModuleDocumentation);
        working.AppendLine("//");

        working.AppendLine("//Requires Modules:");
        if(attrs.Length > 0)
        {
            foreach(KRequiresModuleAttribute current in attrs)
            {
                working.Append("// > ");
                working.AppendLine(
                    FormatAssemblyString(current.RequiredType));
            }
        }
        else
        {
            working.Append("//=> None");
        }

        working.Append("//");
        working.AppendLine();

        return true;
    }

    private static string FormatAssemblyString(Type forType)
    {
        if(forType == null)
            return string.Empty;

        StringBuilder sb = new StringBuilder();
        sb.Append(forType.UnderlyingSystemType.Assembly.GetName().Name);
        sb.Append(".");
        if(!string.IsNullOrEmpty(forType.UnderlyingSystemType.Namespace))
        {
            sb.Append(forType.UnderlyingSystemType.Namespace);
            sb.Append(".");
        }
        sb.Append(forType.UnderlyingSystemType.Name);

        return sb.ToString();
    }

    /// <summary>
    /// Adds all fields within the current moduleType 
    /// to the referenced StrinBuilder.
    /// </summary>
    /// <returns>
    /// When TRUE, the fields were successfully added 
    /// to the module; otherwise FALSE.
    /// </returns>
    public static bool AddModuleFields(ref StringBuilder working, Type moduleType)
    {
        if(working == null)
            return false;

        if(moduleType == null)
            return false;

        if(!typeof(PartModule).IsAssignableFrom(moduleType))
            return false;

        bool hasDocumentation = false;

        working.AppendLine("MODULE");
        working.AppendLine("{");
        working.Append("\tname = ");
        working.AppendLine(moduleType.Name);
        working.AppendLine();
        foreach(FieldInfo current in
            moduleType.UnderlyingSystemType.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            object[] objs = current.GetCustomAttributes(
                typeof(KPartModuleFieldConfigurationDocumentationAttribute),
                true);

            if(objs == null || objs.Length != 1)
                continue;

            KPartModuleFieldConfigurationDocumentationAttribute attr = 
                objs[0] as KPartModuleFieldConfigurationDocumentationAttribute;

            if(attr == null)
                continue;

            AddValueToNode(ref working, current, attr);
            hasDocumentation = true;
        }
        working.Remove(working.Length - 1, 1);
        working.AppendLine("}");

        return hasDocumentation;
    }

    /// <summary>
    /// Generates the Documentation to the module's 
    /// ...\PluginData\plugindllname\ directory.
    /// </summary>
    public static void Generate()
    {
#if DEBUG
        Type fileType = Type.GetType(string.Concat("System.I", "O.File"));
       _WriteAllText =
            fileType.GetMethod(
                "WriteAllText",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new Type[] { typeof(string), typeof(string), typeof(Encoding) },
                null);

       //_RecurseFormat = typeof(ConfigNode).GetMethod(
       //    "RecurseFormat",
       //    BindingFlags.NonPublic | BindingFlags.Static,
       //    null,
       //    new Type[] { typeof(List<string[]>) },
       //    null);

       //_PreFormatConfig = typeof(ConfigNode).GetMethod(
       //    "PreFormatConfig",
       //    BindingFlags.NonPublic | BindingFlags.Static,
       //    null,
       //    new Type[] { typeof(string[]) },
       //    null);
#endif



        foreach(Assembly current in AppDomain.CurrentDomain.GetAssemblies())
        {
            GenerateAssemblyDocumentation(current);
        }

    }


    public static void GenerateAssemblyDocumentation(Assembly assembly)
    {
#if DOCS_TO_REPOSITORY
        string root = @"Z:\Users\KineMorto\Desktop\KineTech\KineTechAnimation\KineTechAnimationLibrary";
#else
        //Get the filepath of our data sandbox
        string searchsString = "SEARCHSTRING.txt";
        string root = KSP.IO.IOUtils.GetFilePathFor(typeof(KineTechAnimationModuleLoader), searchsString);
        root = root.Replace(searchsString, string.Empty);
#endif

        //Iterate over types in assembly generating documentation
        //for those which have the correct attributes.
        foreach(Type current in assembly.GetTypes())
        {
            if(current.GetCustomAttributes(typeof(KPartModuleConfigurationDocumentationAttribute), false).Length == 0)
                continue;

            if(current.IsAbstract)
                continue;

            StringBuilder working = new StringBuilder();

            AddModuleDocumentation(ref working, current);

            if(AddModuleFields(ref working, current))
            {
                string fileName;
                if(GetFileNameFor(root, assembly, current, out fileName))
#if !DEBUG
                    KSP.IO.File.WriteAllText<KineTechAnimationModuleLoader>(
                        working.ToString(), 
                        string.Concat(fileName));   
#else
                    WriteConfig(ref working, ref fileName);
#endif


            }
        }
    }

    private static bool GetFileNameFor(string root, Assembly assembly, Type moduleType, out string result)
    {
        result = string.Empty;

        if(string.IsNullOrEmpty(root))
            return false;

        if(assembly == null)
            return false;

        if(moduleType == null)
            return false;


        result = string.Concat(
            root,
#if !DEBUG
            "/",
#else
            "/Documentation/",
#endif
            moduleType.Name,
            ".cfg");

        return true;
    }

    private static void WriteConfig(ref StringBuilder working, ref string filePath)
    {
#if DEBUG
        if(working == null)
            return;

        if(string.IsNullOrEmpty(filePath))
            return;

        _WriteAllText.Invoke(
            null,
            new object[]
                {
                    filePath,
                    working.ToString(),
                    Encoding.UTF8
                });
#endif


        //string[] preFormatSplit = 
        //    working.ToString().Split(
        //        new string[] { Environment.NewLine }, 
        //        StringSplitOptions.None);

        //List<string[]> preFormat = 
        //    _PreFormatConfig.Invoke(
        //        null, 
        //        new object[] { preFormatSplit }) as List<string[]>;

        //if(preFormat == null)
        //    return;

        //ConfigNode result = 
        //    _RecurseFormat.Invoke(
        //        null, 
        //        new object[] { preFormat }) as ConfigNode;

        //if(result == null)
        //    return;
        //result.Save(filePath);
    }
}
