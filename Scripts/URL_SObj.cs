//====================================================================================================
// OpenURLs         v.1.1.1
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

namespace AyahaTools.OpenURLs
{
    public class URL_SObj : ScriptableObject
    {
        [Header("====見出し====")]
        public string URLHeaderName;

        [Header("====URL====")]
        public string URL;
    }
}

