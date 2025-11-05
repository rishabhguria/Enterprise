using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windale.Options;


namespace Nirvana.Middleware
{

    public class OptionPoco
    {
        public String Symbol { get; set; }
        public String PutOrCall { get; set; }
        public double UnderlyingPrice { get; set; }
        public double ExercisePrice { get; set; }
        public double Interest { get; set; }
        public double Dividend { get; set; }
        public double Time { get; set; }
        public double SymbolPrice { get; set; }
    }

   public  class Client
    {
        /// <summary>
        /// used to support dummy call to windale. This is an issue that we have to make a dummy call to avoid licensing problem 
        /// </summary>
        /// <returns></returns>
        public static double  Run()
        {
            OptionPoco option = new OptionPoco();
            option.Symbol = "MSFT";
            option.PutOrCall = "Put";
            option.Dividend = 1;
            option.ExercisePrice = 45.0;
            option.Interest = 5.0;
            option.SymbolPrice = 65.0;
            option.Time = 10;
            option.UnderlyingPrice = 56.0;
            
            OptionsNET OptionsNet1 = new OptionsNET();
            return OptionsNet1.BSImpliedVolatility(option.UnderlyingPrice, option.ExercisePrice, option.Interest, option.Time, option.Dividend, option.SymbolPrice, 2);
        }
    }
}
