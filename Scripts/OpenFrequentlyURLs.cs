//====================================================================================================
// OpenURLs         v.2.0.0
//
// Copyright (C) 2022 ayaha401
// Twitter : @ayaha__401
//
// This software is released under the MIT License.
// https://github.com/ayaha401/OpenURLs/blob/main/LICENSE
//====================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace AyahaTools.OpenURLs
{
    public class OpenFrequentlyURLs : EditorWindow
    {
        private string _version = "2.0.0";

        private MakeURLAsset _makeURLAsset= null;
        [SerializeField] private OpenFrequentlyURLsSaveData _data = null;
        [SerializeField] private List<URL_SObj> _URLs = new List<URL_SObj>();

        private SerializedObject so;
        private SerializedProperty _URLsProp;

        private string _filter = string.Empty;
        private enum FilterMode
        {
            AssetName,
            TitleName,
            All,
        }
        private FilterMode _filterMode = FilterMode.AssetName;
        private bool _filterFlag;
        private int _filterModeIndex = 2;
        private string[] _filterModeNames = {"AssetName", "TitleName", "All"};

        private Vector2 _scrollPosition = Vector2.zero;
        private Vector2 _deleteButtonSize = new Vector2(18.0f,18.0f);
        private Vector2 _startWindowSize = new Vector2(430.0f,530.0f);


        [MenuItem("AyahaTools/OpenURLs")]
        private static void OpenWindow()
        {
            EditorWindow.GetWindow<OpenFrequentlyURLs>();
        }

        void OnEnable()
        {
            so = new SerializedObject(this);
            _URLsProp = so.FindProperty("_URLs");

            Rect currentPosition = position;
            currentPosition.size = _startWindowSize;
            position = currentPosition;
        }

        void SoInit()
        {
            so = new SerializedObject(this);
            _URLsProp = so.FindProperty("_URLs");
        }

        private void GUIPartition()
        {
            GUI.color = Color.gray;
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            GUI.color = Color.white;
        }
        
        private void Information()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Version");
                    EditorGUILayout.LabelField("Version " + _version);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("How to use (Japanese)");
                    if(GUILayout.Button("How to use (Japanese)"))
                    {
                        System.Diagnostics.Process.Start("https://github.com/ayaha401/OpenURLs");
                    }
                }
            }
        }

        private void URLLabel(int indexNum)
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(_URLs[indexNum].URLHeaderName);
                    EditorGUILayout.Space();
                    if(GUILayout.Button("X", GUILayout.Width(_deleteButtonSize.x), GUILayout.Height(_deleteButtonSize.y)))
                    {
                        _URLsProp.DeleteArrayElementAtIndex(indexNum);
                    }
                }
                
                if(GUILayout.Button("OpenURL"))
                {
                    System.Diagnostics.Process.Start(_URLs[indexNum].URL);
                }

                GUIPartition();
            }
        }

        private void URLsLabel()
        {
            for(int i=0;i<_URLsProp.arraySize;i++)
            {
                if(_URLs[i] != null)
                {
                    switch (_filterMode)
                    {
                        case FilterMode.AssetName:
                            _filterFlag = _URLs[i].name.Contains(_filter);
                            break;
                        case FilterMode.TitleName:
                            _filterFlag = _URLs[i].URLHeaderName.Contains(_filter);
                            break;
                        case FilterMode.All:
                            _filterFlag = _URLs[i].name.Contains(_filter) || _URLs[i].URLHeaderName.Contains(_filter);
                            break;
                    }

                    if(_filterFlag)
                    {
                        URLLabel(i);
                    }
                }
            }
        }

        void OnGUI()
        {
            // EditorGUILayout.LabelField($"大きさ{position.size}");
            Information();

            GUIPartition();

            if(GUILayout.Button("URLアセットを作成"))
            {
                _makeURLAsset = GetWindow<MakeURLAsset>();
            }

            GUIPartition();

            using (new EditorGUILayout.VerticalScope("box"))
            {
                if(_data == null) _data = LoadData();
                _URLs = _data._URLs;

                EditorGUI.BeginChangeCheck();

                SoInit();
                EditorGUILayout.PropertyField(_URLsProp,true);

                if(EditorGUI.EndChangeCheck())
                {
                    _data.UpdateData(_URLs);
                    EditorUtility.SetDirty(_data);
                }

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

                GUIPartition();

                using (new EditorGUILayout.VerticalScope())
                {
                    _filterModeIndex = GUILayout.Toolbar(_filterModeIndex, _filterModeNames);
                    _filterMode = (FilterMode)Enum.ToObject(typeof(FilterMode), _filterModeIndex);
                    

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("Filter",GUILayout.Width(45));
                        _filter = EditorGUILayout.TextField(_filter);
                        if(GUILayout.Button("X", GUILayout.Width(_deleteButtonSize.x), GUILayout.Height(_deleteButtonSize.y)))
                        {
                            _filter = string.Empty;
                        }
                    }
                }
                
                GUIPartition();

                URLsLabel();

                EditorGUILayout.EndScrollView();
            }
            so.ApplyModifiedProperties();
        }

        void OnDestroy()
        {
            if(_makeURLAsset == null) return; 
            if(_makeURLAsset.isOpen == true)
            {
                _makeURLAsset.CloseWindow();
            }
        }

        private OpenFrequentlyURLsSaveData LoadData()
        {
            return (OpenFrequentlyURLsSaveData)AssetDatabase.FindAssets("t:ScriptableObject")
                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                    .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(OpenFrequentlyURLsSaveData)))
                    .Where(obj => obj != null)
                    .FirstOrDefault();
        }
    }
}
