using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml.Linq;

namespace Prana.ConfigMerger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = args[0];
            string toFilePath = path + "\\" + args[1];
            string fromFilePath = path + "\\" + args[2];

            if (File.Exists(toFilePath) && File.Exists(fromFilePath) && Path.GetExtension(toFilePath).Equals(Path.GetExtension(fromFilePath)))
            {
                Console.WriteLine("Configuration file merging started. File names: " + args[1] + ", " + args[2]);

                if (Path.GetExtension(toFilePath).Equals(".config"))
                {
                    try
                    {
                        XDocument toConfig = XDocument.Load(toFilePath);
                        XDocument fromConfig = XDocument.Load(fromFilePath);

                        MergeConfigurations(toConfig, fromConfig);

                        toConfig.Save(toFilePath);

                        Console.WriteLine("Configuration file merged successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error during merge: " + ex.Message);
                        Environment.Exit(1);
                    }
                }
                else if (Path.GetExtension(toFilePath).Equals(".json"))
                {
                    try
                    {
                        MergeJsonFiles(toFilePath, fromFilePath);

                        Console.WriteLine("Configuration file merged successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error during merge: " + ex.Message);
                        Environment.Exit(1);
                    }
                }
            }
            else
            {
                Console.WriteLine("Provided configuration file(s) not found. File names: " + args[1] + ", " + args[2]);
                Environment.Exit(1);
            }
        }

        static void MergeConfigurations(XDocument toConfig, XDocument fromConfig)
        {
            foreach (var fromElement in fromConfig.Root.Elements())
            {
                // Find or add the element
                var toElement = toConfig.Root.Element(fromElement.Name);
                if (toElement == null)
                {
                    // Clone element from config2 to config1 if not found
                    toConfig.Root.Add(new XElement(fromElement));
                }
                else
                {
                    // Merge child elements if the element already exists
                    MergeChildElements(toElement, fromElement);
                }
            }
        }

        static void MergeChildElements(XElement toElement, XElement fromElement)
        {
            foreach (var fromChild in fromElement.Elements())
            {
                var toChild = FindMatchingChild(toElement, fromChild);

                if (toChild == null)
                {
                    // If no matching child is found, add the entire child element
                    toElement.Add(new XElement(fromChild));
                }
                else
                {
                    // Check if 'key' or all attributes are the same but the values differ
                    if (AreElementsIdentical(toChild, fromChild))
                    {
                        // If the elements are identical, skip merging to avoid duplication
                        continue;
                    }
                }
            }
        }

        // This method compares elements based on their attributes and values
        static bool AreElementsIdentical(XElement toChild, XElement fromChild)
        {
            // Compare element names
            if (toChild.Name != fromChild.Name) return false;

            // Compare attributes
            foreach (var attr in toChild.Attributes())
            {
                var correspondingAttr = fromChild.Attribute(attr.Name);
                if (correspondingAttr == null || attr.Value != correspondingAttr.Value)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return true; // All attributes and names are the same
        }

        static XElement FindMatchingChild(XElement toElement, XElement fromChild)
        {
            foreach (var existingChild in toElement.Elements())
            {
                if (AreElementsIdentical(existingChild, fromChild))
                {
                    return existingChild; // Return if identical element found
                }
            }
            return null; // No matching element found
        }

        static void MergeJsonFiles(string toFilePath, string fromFilePath)
        {
            // Load both JSON files
            JObject toJson = JObject.Parse(File.ReadAllText(toFilePath));
            JObject fromJson = JObject.Parse(File.ReadAllText(fromFilePath));

            // Iterate over each key in the source JSON
            foreach (var fromProperty in fromJson.Properties())
            {
                // Only add the property to the target JSON if it doesn't already exist
                if (!toJson.ContainsKey(fromProperty.Name))
                {
                    toJson.Add(fromProperty.Name, fromProperty.Value);
                }
            }

            // Save the merged result to the output file
            File.WriteAllText(toFilePath, toJson.ToString());
        }
    }
}
