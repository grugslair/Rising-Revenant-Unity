using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetsManager
{
    public enum OutpostColorOption
    {
        OWN_OUTPOST,
        ENEMY_OUTPOST,
        SELECTED_OUTPOST,
        DEAD_OUTPOST,
        ATTACKED_OUTPOST,
        GREEN_OUTPOST,
    }

    // Static method to get the string representation of ColorOptions
    public static string ToTextureOutputString(OutpostColorOption color)
    {
        switch (color)
        {
            case OutpostColorOption.OWN_OUTPOST:
                return "Outposts_Icons/Blue_Outpost";
            case OutpostColorOption.ENEMY_OUTPOST:
                return "Outposts_Icons/Purple_Outpost";
            case OutpostColorOption.SELECTED_OUTPOST:
                return "Outposts_Icons/White_Outpost";
            case OutpostColorOption.DEAD_OUTPOST:
                return "Outposts_Icons/Grey_Outpost";
            case OutpostColorOption.GREEN_OUTPOST:
                return "Outposts_Icons/Green_Outpost";
            case OutpostColorOption.ATTACKED_OUTPOST:
                return "Outposts_Icons/Red_Outpost";
            default:
                throw new ArgumentOutOfRangeException(nameof(color), color, null);
        }
    }

    public static string ToMaterialOutputString(OutpostColorOption color)
    {
        switch (color)
        {
            case OutpostColorOption.OWN_OUTPOST:
                return "Outposts_Icons/Materials/Blue_Outpost";
            case OutpostColorOption.ENEMY_OUTPOST:
                return "Outposts_Icons/Materials/Purple_Outpost";
            case OutpostColorOption.SELECTED_OUTPOST:
                return "Outposts_Icons/Materials/White_Outpost";
            case OutpostColorOption.DEAD_OUTPOST:
                return "Outposts_Icons/Materials/Grey_Outpost";
            case OutpostColorOption.GREEN_OUTPOST:
                return "Outposts_Icons/Materials/Green_Outpost";
            case OutpostColorOption.ATTACKED_OUTPOST:
                return "Outposts_Icons/Materials/Red_Outpost";
            default:
                throw new ArgumentOutOfRangeException(nameof(color), color, null);
        }
    }

}
