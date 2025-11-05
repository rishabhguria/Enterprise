using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Nirvana.RuleEngine.Core;

namespace Nirvana.RiskManagement
{
    class RiskRuleEngine : Nirvana.RuleEngine.Core.RuleEngine
    {

        static RiskRuleEngine _instance = null;
        static readonly object padlock = new object();

        public static RiskRuleEngine Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        return new RiskRuleEngine();
                    }
                    return _instance;
                }
            }
        }
        public override Rules LoadRules(String rulesDocumentPath)
        {
            XmlDocument rulesDocument = new XmlDocument();
            RiskRules rulesCollection = new RiskRules();

            try
            {
                rulesDocument.Load(rulesDocumentPath);

                XmlNodeList rulesNodeList = rulesDocument.SelectNodes("RuleEngine/Rules//Rule");
                XmlNodeList actionsList;

                foreach (XmlNode ruleNode in rulesNodeList)
                {
                    RiskRule ruleObject = new RiskRule();
                    Actions actionsCollection = new Actions();

                    ruleObject.Name = ruleNode.Attributes["name"].Value;
                    //ruleObject.Subject = ruleNode.Attributes["type"].Value;
                    ruleObject.Description = ruleNode.Attributes["description"].Value;
                    ruleObject.Condition = ruleNode["Condition"].InnerText;

                    Console.WriteLine("Rule Name :: " + ruleNode.Attributes["name"].Value);

                    Console.WriteLine("Rule Description :: " + ruleNode.Attributes["description"].Value);

                    //Console.WriteLine("Rule Type :: " + ruleNode.Attributes["type"].Value);

                    Console.WriteLine("Condition :: " + ruleNode["Condition"].InnerText);

                    //Get all the actions from ActionList and add them in actionsCollection
                    actionsList = ruleNode.SelectNodes("Actions//Action");
                    Console.WriteLine("Action list");
                    foreach (XmlNode actionNode in actionsList)
                    {
                        Action actionObject = new Action();
                        actionObject.Name = actionNode.Attributes["name"].Value;
                        actionObject.Message = actionNode.Attributes["message"].Value;
                        Console.WriteLine("Current Action :: " + actionNode.Attributes["name"].Value);
                        Console.WriteLine("Current Action Message :: " + actionNode.Attributes["message"].Value);
                        actionsCollection.Add(actionObject);
                    }

                    ruleObject.Actions = actionsCollection;

                    //Add rule to Rules Collection
                    rulesCollection.Add(ruleObject);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return rulesCollection;
        }
     	
    }
}
