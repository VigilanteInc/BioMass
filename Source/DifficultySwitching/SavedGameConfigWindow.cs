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

using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Tac;

namespace KSPBioMass
{
    class SavedGameConfigWindow : Window<SavedGameConfigWindow>
    {
        private Settings globalSettings;
        private GUIStyle labelStyle;
        private GUIStyle editStyle;
        private GUIStyle headerStyle;
        private GUIStyle headerStyle2;
        private GUIStyle warningStyle;
        private GUIStyle buttonStyle;

        private DifficultySetting BioMassDifficulty;
        private bool ClassicSettings = false;
        private bool EasySettings = false;
        private bool NormalSettings = false;
        private bool HardSettings = false;

        private readonly string version;

		private string thePath;
		private string theDirectoryPath;

		public SavedGameConfigWindow(Settings globalSettings)
            : base("BioMass Difficulty", 400, 300)
        {
            base.Resizable = false;
            this.globalSettings = globalSettings;
            //this.saveGame = saveGame;
            this.changeSettings(globalSettings.DifficultyBioMass, true);
            version = Utilities.GetDllVersion(this);
        }

        protected override void ConfigureStyles()
        {
            base.ConfigureStyles();

            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.alignment = TextAnchor.MiddleLeft;
                labelStyle.fontStyle = FontStyle.Normal;
                labelStyle.normal.textColor = Color.white;
                labelStyle.wordWrap = false;

                editStyle = new GUIStyle(GUI.skin.textField);
                editStyle.alignment = TextAnchor.MiddleRight;

                headerStyle = new GUIStyle(labelStyle);
                headerStyle.fontStyle = FontStyle.Bold;

                headerStyle2 = new GUIStyle(headerStyle);
                headerStyle2.wordWrap = true;

                buttonStyle = new GUIStyle(GUI.skin.button);

                warningStyle = new GUIStyle(headerStyle2);
                warningStyle.normal.textColor = new Color(0.88f, 0.20f, 0.20f, 1.0f);
            }
        }

        protected override void DrawWindowContents(int windowId)
        {
            GUILayout.Label("Version: " + version, labelStyle);
            GUILayout.Space(3);
            DifficultySettings();
            GUILayout.Space(10);
			GUILayout.Label("BioMass difficulty switching requires Module Manager.", labelStyle);
			GUILayout.Label("To change difficulty in-game, press alt-F11 and click 'Reload Database'", labelStyle);
			//somthing could be put here. BioMass logo?

            if (GUI.changed)
            {
                SetSize(150, 20);
            }
        }

        private void DifficultySettings()
        {
            GUILayout.Label("Difficulty Settings", headerStyle);
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(ClassicSettings, "Classic", buttonStyle))
            {
                changeSettings(DifficultySetting.Classic,false);
            }
            if (GUILayout.Toggle(EasySettings, "Easy", buttonStyle))
            {
                changeSettings(DifficultySetting.Easy,false);
            }
            if (GUILayout.Toggle(NormalSettings, "Moderate", buttonStyle))
            {
                changeSettings(DifficultySetting.Normal,false);
            }
            if (GUILayout.Toggle(HardSettings, "Masochist", buttonStyle))
            {
                changeSettings(DifficultySetting.Hard,false);
            }
            GUILayout.EndHorizontal();
        }


        private void changeSettings(DifficultySetting diff,bool initialSet)
        {
            if (diff == BioMassDifficulty)
            {
                return;
            }

            this.Log_DebugOnly("Change Difficulty FROM " + BioMassDifficulty.ToString() + " TO "+diff);
            ClassicSettings = false;
            EasySettings = false;
            NormalSettings = false;
            HardSettings = false;

            BioMassDifficulty = diff;

            switch (BioMassDifficulty)
            {				
                case DifficultySetting.Classic:
                    ClassicSettings = true;
					thePath = IOUtils.GetFilePathFor (this.GetType (), "Classic.txt");
					this.Log (thePath);
					theDirectoryPath = System.IO.Path.GetDirectoryName (thePath);
					System.IO.File.Copy (thePath, System.IO.Path.Combine(theDirectoryPath, "BioMassDifficulty_MM.cfg"), true);
                    break;
                case DifficultySetting.Easy:
                    EasySettings = true;
					thePath = IOUtils.GetFilePathFor (this.GetType (), "Easy.txt");
					this.Log (thePath);
					theDirectoryPath = System.IO.Path.GetDirectoryName (thePath);
					System.IO.File.Copy (thePath, System.IO.Path.Combine(theDirectoryPath, "BioMassDifficulty_MM.cfg"), true);
                    break;
                case DifficultySetting.Normal:
                    NormalSettings = true;
					thePath = IOUtils.GetFilePathFor (this.GetType (), "Moderate.txt");
					this.Log (thePath);
					theDirectoryPath = System.IO.Path.GetDirectoryName (thePath);
					System.IO.File.Copy (thePath, System.IO.Path.Combine(theDirectoryPath, "BioMassDifficulty_MM.cfg"), true);
                    break;
				case DifficultySetting.Hard:
					HardSettings = true;
					thePath = IOUtils.GetFilePathFor (this.GetType (), "Hard.txt");
					this.Log (thePath);
					theDirectoryPath = System.IO.Path.GetDirectoryName (thePath);
					System.IO.File.Copy (thePath, System.IO.Path.Combine(theDirectoryPath, "BioMassDifficulty_MM.cfg"), true);
                    break;
                default:
                    break;
            }

            if (!initialSet)
              globalSettings.setDifficultySettings(BioMassDifficulty);

        }
    }

    public enum DifficultySetting
    {
        Classic,
        Easy,
        Normal,
		Hard
    }
}
