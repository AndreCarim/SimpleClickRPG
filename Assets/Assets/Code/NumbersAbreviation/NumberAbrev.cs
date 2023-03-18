using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class NumberAbrev 
{
    /// <summary>
        /// The list containing all short scale values.
        /// </summary>
        public static readonly string[] ShortScaleReference = {"thousand","million","billion","trillion","quadrillion","quintillion",
        "sextillion","septillion","octillion","nonillion","decillion",
        "undecillion","duodecillion","tredecillion","quattuordecillion","quindecillion",
        "sexdecillion","septendecillion","octodecillion","novemdecillion","vigintillion",
        "unvigintillion", "duovigintillion","trevigintillion","quattuorvigintillion", // até aqui
        "quinvigitillion","sexvigintillion","septenvigitillion","octovigintillion","novemvigitillion",
        "trigintillion","untrigintillion","duotrigintillion","tretrigintillion","quattuortrigintillion",
        "quintrigintillion","sextrigintillion","septentrigintillion","octotrigintillion",
        "novemtrigintillion","quadragintillion","unquadragintillion","duoquadragintillion",
        "trequadragintillion","quattuorquadragintillion","quinquadragintillion","sexquadragintillion",
        "septenquadragintillion","octoquadragintillion","novemquadragintillion","quinquagintillion","unquinquagintillion",
        "duoquinquagintillion","trequinquagintillion","quattuorquinquagintillion","quinquinquagintillion",
        "sexquinquagintillion","septenquinquagintillion","octoquinquagintillion","novemquinquagintillion","sexagintillion",
        "unsexagintillion","duosexagintillion","tresexagintillion","quattuorsexagintillion","quinsexagintillion",
        "sexsexagintillion","septensexagintillion","octosexagintillion","novemsexagintillion","septuagintillion",
        "unseptuagintillion","duoseptuagintillion","treseptuagintillion","quattuorseptuagintillion","quinseptuagintillion",
        "sexseptuagintillion","septenseptuagintillion","octoseptuagintillion","novemseptuagintillion",
        "octogintillion","unoctogintillion","duooctogintillion","treoctogintillion","quattuoroctogintillion",
        "quinoctogintillion","sexoctogintillion","septenoctogintillion","octooctogintillion","novemoctogintillion","nonagintillion",
        "unnonagintillion","duononagintillion","trenonagintillion","quattuornonagintillion","quinnonagintillion",
        "sexnonagintillion","septennonagintillion","octononagintillion","novemnonagintillion","centillion"};

        ///<summary>
        /// List containing short scale values in symbol form. Can be further expanded in future updates.
        /// </summary>
        public static readonly string[] ShortScaleSymbolReference = { "K","M","B","T","Qa","Qi","Sx","Sp","Oc","No","Dc","Ud","Dd","Td","Qad","Qid","Sxd","Spd","Ocd","Nod","Vv","Uv","Dv","Tv","Qav","Qiv","Sxv","Spv","Ocv","Nov","TV","UV","DV","Tg","Qag","Qig","Sxg","Spg","Ocg","Nog","Ung","Dug","Ttg","Qadg","Qidg","SxDg","SpDg","OcDg","NoDg","Vg","Uvg","Dvg","Tvg","Qavg","Qivg","Sxvg","Spvg","Ocvg","Novg","TG","UvtG","DvtG","TvtG","QatG","QitG","SxtG","Cen"  };

        /// <summary>
        /// Parses the double value into short scale notation.
        /// </summary>
        /// <returns>The short scale string.</returns>
        /// <param name="value">The input value that will be parsed.</param>
        /// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
        /// Default value is 3.</param> 
        /// <param name="startShortScale">(Optinal) Set the value to begin parsing to short scale. Default value is 1 million.</param>
        /// <param name="useSymbol">
        /// (Optional) use the single symbol list for more shortened notation. currently supports only to Decillion. Default value is false.
        /// </param>
        public static string ParseDouble(double value, in int precision = 2, double startShortScale = 10000, bool useSymbol = true)
        {
            string symbol = "";
            string strVal = "";
            ParseDoubleInternal(value, ref strVal, ref symbol, precision, startShortScale, useSymbol);

            return strVal + symbol;
        }

        
        private static void ParseDoubleInternal(double value, ref string strVal, ref string symbol, in int precision, double startShortScale, bool useSymbol)
        {
            int index = -1;
            int isNegative = 1;
            string addPrecision = new string ('#', precision);
            double precisionValue = Mathf.Pow (10, precision);

            if (value < 0) 
            {
                isNegative = -1;
                value *= isNegative;
            } 
            else if (!(value > 0d)) 
            {
                strVal = "0";
                symbol = "";
                return;
            }

            if (value < 1000d || value < startShortScale)
            {
                strVal = (Math.Floor(value * isNegative * precisionValue) / precisionValue).ToString ("#,#." + addPrecision);
                symbol = "";
                return;
            }
            
            int maxIndex = useSymbol 
                ? ShortScaleSymbolReference.Length - 1 
                : ShortScaleReference.Length - 1;

            while (value >= 1000d && index < maxIndex) 
            {   
                value *= 0.001d;
                index++;
            }

            symbol = useSymbol 
                ? ShortScaleSymbolReference[index]
                : ShortScaleReference [index];
            
            strVal = (Math.Floor(value * isNegative * precisionValue) / precisionValue)
                .ToString("#,#." + addPrecision);
        }

}
