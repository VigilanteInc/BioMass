/*Based on TAC-Lifesupport GlobalSettings.cs*/

/**
 * Thunder Aerospace Corporation's Life Support for Kerbal Space Program.
 * Written by Taranis Elsu.
 * 
 * (C) Copyright 2013, Taranis Elsu
 * 
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 * 
 * This code is licensed under the Attribution-NonCommercial-ShareAlike 3.0 (CC BY-NC-SA 3.0)
 * creative commons license. See <http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode>
 * for full details.
 * 
 * Attribution — You are free to modify this code, so long as you mention that the resulting
 * work is based upon or adapted from this code.
 * 
 * Non-commercial - You may not use this work for commercial purposes.
 * 
 * Share Alike — If you alter, transform, or build upon this work, you may distribute the
 * resulting work only under the same or similar license to the CC BY-NC-SA 3.0 license.
 * 
 * Note that Thunder Aerospace Corporation is a ficticious entity created for entertainment
 * purposes. It is in no way meant to represent a real entity. Any similarity to a real entity
 * is purely coincidental.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using UnityEngine;
using Tac;
using KSP.IO;

namespace KSPBioMass
{
    public class Settings
    {
        //Static
        public static String AddonName = "BioMass";
        public static String PathKSP;        
        public static String PathPlugin;
        public static String PathPluginData;
        public static String PathTextures;
        public static String GlobalConfigFile;
        
        public ConfigNode globalSettingsNode = new ConfigNode();
        public List<Component> controller = new List<Component>();


        private const string configNodeName = "GlobalSettings";
        public DifficultySetting DifficultyBioMass;



        private string GetKSPPath(string strSource)
        {
            int Start, End;
            if (strSource.Contains(AddonName))
            {
                Start = 0;
                End = strSource.IndexOf(AddonName, 0) - 1;
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public Settings()
        {
            this.Log_DebugOnly("Constructor Settings");
            
            //string filepath = IOUtils.GetFilePathFor(this.GetType(), "XYZ");
            //PathKSP = GetKSPPath(filepath);
            PathPlugin = string.Format("{0}",AddonName);
            PathPluginData = string.Format("{0}/PluginData", PathPlugin);
            PathTextures = string.Format("{0}/Textures", PathPlugin);
            GlobalConfigFile = IOUtils.GetFilePathFor(this.GetType(), "BioMass.cfg");

            this.Log_DebugOnly("ConfigFile: " + GlobalConfigFile);
            
            this.globalSettingsNode = new ConfigNode();

            DifficultyBioMass = DifficultySetting.Normal;
        }
        
        public Boolean FileExists(string FileName)
        {
            return (System.IO.File.Exists(FileName));
        }

        public Boolean Load()
        {
            return this.Load(GlobalConfigFile);
        }       

        public Boolean Load(String fileFullName)
        {
            Boolean blnReturn = false;
            try
            {
                if (FileExists(fileFullName))
                {
                    //Load the file into a config node
                    globalSettingsNode = ConfigNode.Load(fileFullName);
                    this.LoadFromNode(globalSettingsNode);
                    foreach (ISavable s in controller.Where(c => c is ISavable))
                    {
                        s.Load(globalSettingsNode);
                    }
                    this.LogWarning("Load GlobalSettings");
                    blnReturn = true;
                }
                else
                {
                    this.Log_DebugOnly(String.Format("File could not be found to load({0})", fileFullName));
                    blnReturn = false;
                }
            }
            catch (Exception ex)
            {
                this.LogError(String.Format("Failed to Load Configfile({0})-Error:{1}", fileFullName, ex.Message));
                blnReturn = false;
            }
            return blnReturn;
        }

        private void LoadFromNode(ConfigNode node)
        {
            if (node.HasNode(configNodeName))
            {
                ConfigNode settingsNode = node.GetNode(configNodeName);
                DifficultyBioMass = Utilities.GetValue(settingsNode, "DifficultySettings",DifficultyBioMass);
                this.LogWarning("Difficulty read "+DifficultyBioMass.ToString());
            }
        }

        public Boolean Save()
        {
            return this.Save(GlobalConfigFile);
        }

        public Boolean Save(String fileFullName)
        {
            Boolean blnReturn = false;
            try
            {
                //Encode the current object
                SaveToNode(globalSettingsNode);
                foreach (ISavable s in controller.Where(c => c is ISavable))
                {
                    s.Save(globalSettingsNode);
                }
                globalSettingsNode.Save(GlobalConfigFile);
                this.LogWarning("Save GlobalSettings");
                blnReturn = true;
            }
            catch (Exception ex)
            {
                this.LogError(String.Format("Failed to Save ConfigNode to file({0})-Error:{1}", fileFullName, ex.Message));
                blnReturn = false;
            }
            return blnReturn;
        }

        private void SaveToNode(ConfigNode node)
        {
            ConfigNode settingsNode;
            if (node.HasNode(configNodeName))
            {
                settingsNode = node.GetNode(configNodeName);
                settingsNode.ClearData();
            }
            else
            {
                settingsNode = node.AddNode(configNodeName);
            }

            settingsNode.AddValue("DifficultySettings", DifficultyBioMass);
        }

        public void setDifficultySettings(DifficultySetting difficulty)
        {           
            if (difficulty == DifficultyBioMass)
            {
                return;
            }

        }
    }
}
