﻿/***************************************************************************\
Project:      Daily Rewards
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using System;
using System.Threading;

namespace NiobiumStudios
{
    /**
    * The class representation of the Reward
    **/
    [Serializable]
    public class Reward
    {
        public string unit;
        public int reward;
        public Sprite sprite;
    }
}