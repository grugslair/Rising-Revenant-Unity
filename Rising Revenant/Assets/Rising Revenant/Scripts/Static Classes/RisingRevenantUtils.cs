using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using Dojo.Starknet;
using Vector2 = UnityEngine.Vector2;
using Random = System.Random;
using SimpleGraphQL;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public static class RisingRevenantUtils
{

    [Serializable]
    public struct Vec2
    {
        public UInt32 x;
        public UInt32 y;
    }

    public static float MAP_WIDHT = 10240;
    public static float MAP_HEIGHT = 5124;
    public static float SCALE = 2;

    public static string[] NamesArray = new string[] {
        "Mireth", "Vexara", "Zorion", "Caelix",
        "Elyndor", "Tharion", "Sylphren", "Aravax",
        "Vexil", "Lyrandar", "Nyxen", "Theralis",
        "Qyra", "Fenrix", "Atheris", "Lorvael",
        "Xyris", "Zephyron", "Calaer", "Drakos",
        "Velixar", "Syrana", "Morvran", "Elithran",
        "Kaelith", "Tyrven", "Ysara", "Vorenth",
        "Alarix", "Ethrios", "Nyrax", "Thrayce",
        "Vynora", "Kerith", "Jorvax", "Lysandor",
        "Eremon", "Xanthe", "Zanther", "Cindris",
        "Baelor", "Lyvar", "Eryth", "Zalvor",
        "Gormath", "Sylvanix", "Quorin", "Taryx",
        "Nyvar", "Oryth", "Valeran", "Myrthil",
        "Zorvath", "Kyrand", "Thalren", "Vexim",
        "Aelar", "Grendar", "Xylar", "Zorael",
        "Calyph", "Vyrak", "Thandor", "Lyrax",
        "Riven", "Drexel", "Yvaris", "Zenthil",
        "Aravorn", "Morthil", "Sylvar", "Quinix",
        "Tharix", "Valthorn", "Nythar", "Lorvax",
        "Exar", "Zilthar", "Cynthis", "Veldor",
        "Arix", "Thyras", "Mordran", "Elyx",
        "Kythor", "Rendal", "Xanor", "Yrthil",
        "Zarvix", "Caelum", "Lythor", "Qyron",
        "Thoran", "Vexor", "Nyxil", "Orith",
        "Valix", "Myrand", "Zorath", "Kaelor"
    };
    public static string[] SurnamesArray = new string[] {
        "Velindor", "Tharaxis", "Sylphara", "Aelvorn",
        "Morvath", "Elynara", "Xyreth", "Zephris",
        "Kaelyth", "Nyraen", "Lorvex", "Quorinax",
        "Dravys", "Aeryth", "Thundris", "Gryfora",
        "Luminaer", "Orythus", "Veximyr", "Zanthyr",
        "Caelarix", "Nythara", "Vaelorix", "Myrendar",
        "Zorvyn", "Ethrios", "Mordraen", "Xanthara",
        "Yrthalis", "Zarvixan", "Calarun", "Vyrakar",
        "Thandoril", "Lyraxin", "Drexis", "Yvarix",
        "Zenithar", "Aravor", "Morthal", "Sylvoran",
        "Quinixar", "Tharixan", "Valthornus", "Nytharion",
        "Lorvax", "Exarion", "Ziltharix", "Cynthara",
        "Veldoran", "Arxian", "Thyras", "Elyxis",
        "Kythoran", "Rendalar", "Xanorath", "Yrthilix",
        "Zarvixar", "Caelumeth", "Lythorix", "Qyronar",
        "Thoranis", "Vexorath", "Nyxilar", "Orithan",
        "Valixor", "Myrandar", "Zorathel", "Kaeloran",
        "Skyrindar", "Nighsearix", "Flamveilar", "Thornvalix",
        "Stormwieldor", "Emberwindar", "Ironwhisparia", "Ravenfrostix",
        "Shadowgleamar", "Frostechoar", "Moonriftar", "Starbinderix",
        "Voidshaperix", "Earthmeldar", "Sunweaverix", "Seablazix",
        "Wraithbloomar", "Windshardix", "Lightchasar", "Darkwhirlar",
        "Thornspiritix", "Stormglowar", "Firegazix", "Nightstreamar",
        "Duskwingar", "Frostrealmar", "Shadowsparkix", "Ironbloomar",
        "Ravenmistar", "Embermarkix", "Gloomveinar", "Moonshroudar"
    };

    public enum TradesStatus
    {
        NOT_CREATED = 0,
        SELLING,
        SOLD,
        REVOKED
    }

    public static int GetConsistentRandomNumber(int input, int seed, int min, int max)
    {
        int combinedSeed = seed + input;
        Random random = new Random(combinedSeed);
        return random.Next(min, max + 1);
    }

    public static int FieldElementToInt(FieldElement value) 
    {
        string hexString = value.Hex(); 
        hexString = hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? hexString[2..] : hexString;
        BigInteger number = BigInteger.Parse(hexString, System.Globalization.NumberStyles.HexNumber);

        return (int)number;
    }

    public static bool IsPointInsideCircle(Vector2 circleCenter, float radius, Vector2 point)
    {
        float distance = Vector2.Distance(circleCenter, point);
        return distance <= radius;
    }

    public static string GetFullRevenantName(RisingRevenantUtils.Vec2 id) {
        var randomNum = DojoEntitiesDataManager.outpostDictInstance[id].position.x * DojoEntitiesDataManager.outpostDictInstance[id].position.y;

        return $"{NamesArray[GetConsistentRandomNumber((int)randomNum, (int)DojoEntitiesDataManager.outpostDictInstance[id].position.x, 0,49)] } {SurnamesArray[GetConsistentRandomNumber((int)randomNum, (int)DojoEntitiesDataManager.outpostDictInstance[id].position.y, 0, 49)]}";
    }

    public static int GeneralHexToInt(string hexString)
    {
        if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            hexString = hexString.Substring(2);
        }

        int intValue = Convert.ToInt32(hexString, 16);

        return intValue;
    }

    /// <summary>
    /// do this when the string is a literal otherwise replace doesnt work
    /// </summary>
    /// <param name="input"></param>
    /// <param name="replacements"></param>
    /// <returns></returns>
    public static string ReplaceWords(string input, Dictionary<string, string> replacements)
    {
        foreach (var pair in replacements)
        {
            input = input.Replace(pair.Key, pair.Value);
        }
        return input;
    }

    public static int CalculateShields(uint lifes)
    {
        if (lifes >= 1 && lifes <= 2) return 0; 
        if (lifes >= 3 && lifes <= 5) return 1;
        if (lifes >= 6 && lifes <= 9) return 2; 
        if (lifes >= 10 && lifes <= 13) return 3; 
        if (lifes >= 14 && lifes <= 19) return 4; 
        if (lifes >= 20) return 5; 
        return 0;
    }


}
