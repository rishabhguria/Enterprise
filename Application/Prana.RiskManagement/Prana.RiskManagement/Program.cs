using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using Nirvana.RuleEngine.Core;

namespace Nirvana.RiskManagement
{
    class Program
    {
        

        static void Main(string[] args)
        {
            RiskRuleEngine obj = RiskRuleEngine.Instance;

            XmlDocument rulesDocument = new XmlDocument();
            string rulesDocumentPath = "../../Rules.xml";
            RiskRules myRules;
            Facts myFacts;
            
            RuleEventArgs myRuleEventArgs = new RuleEventArgs();
            

            myRules = (RiskRules) obj.LoadRules(rulesDocumentPath);
            myFacts = obj.LoadFacts(rulesDocumentPath);
             
            double a = 100.10;

            foreach (RiskRule rule in myRules)
            {
                Console.WriteLine(rule.Name + " :: " + rule.Subject);
            }
            
            //foreach (KeyValuePair<string,string> fact in myFacts)
            //{
            //    Console.WriteLine("\n ***************************************************************** \n");
            //    Console.WriteLine("Fact Name :: " + fact.Key);
            //    myFacts[fact.Key] = a.ToString();
            //    Console.WriteLine("Fact Value :: " + fact.Value);
            //    Console.WriteLine("\n ***************************************************************** \n");
            //    a += a;
            //}
            ////////myFacts["CalculatedCompanyExposureLimit"] = "500";

            ////////myFacts["CalculatedNegativePnlLimit"] = "600";

            ////////myFacts["CalculatedPositivePnlLimit"] = "700";

            ////////myFacts["CompanyExposureLimit"] = "600";

            ////////myFacts["NegativePnlLimit"] = "700";

            ////////myFacts["PositivePnlLimit"] = "800";

            obj.FillFactsInRuleCondition(myRules, myFacts);
            foreach(Rule myRule in myRules)
                obj.Evaluate(myRule);

            foreach (Rule myRule in myRules)
            { //obj.Evaluate(myRule);

                myRuleEventArgs.Rule = myRule;
                //myRuleEventArgs.myRule.Condition = "5 > 10";
                obj.Evaluate(obj, myRuleEventArgs);
            }

            
            Console.ReadLine();
        }

    }

    //enum FactMapper
    //{
    //    [StringValue("CompanyExposureLimit")] CompanyExposureLimit,
    //    [StringValue("PositivePnlLimit")] PositivePnlLimit,
    //    [StringValue("NegativePnlLimit")] NegativePnlLimit,
    //    [StringValue("CalculatedCompanyExposureLimit")] CalculatedCompanyExposureLimit,
    //    [StringValue("CalculatedPositivePnlLimit")] CalculatedPositivePnlLimit,
    //    [StringValue("CalculatedNegativePnlLimit")] CalculatedNegativePnlLimit
    //}
}
