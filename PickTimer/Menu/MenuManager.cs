using System;
using System.Collections;
using PickTimer.Menu.Impl;
using PickTimer.Util;
using UnboundLib;
using UnityEngine;

namespace PickTimer.Menu
{
    public static class MenuManager
    {
        static MenuManager()
        {
            Unbound.RegisterMenu("Pick Timer", () => { }, GeneralSettings.Menu, null, false);
        }

        internal static void Initialize()
        {

        }
    }
}
