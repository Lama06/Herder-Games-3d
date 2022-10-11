﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Util
{
    public static class IteratorUtil
    {
        public static IEnumerable WaitForSeconds(float seconds)
        {
            var start = Time.time;
            while (Time.time < start + seconds)
            {
                yield return null;
            }
        }
    }
}