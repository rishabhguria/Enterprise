using System;
using System.Configuration;

namespace Prana.APIAdapter
{

    public class HttpRequestConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("HttpRequestConfigs", IsDefaultCollection = false)]
        public HttpRequestConfigCollection HttpRequestConfigs
        {
            get
            {
                HttpRequestConfigCollection httpRequestCollection = (HttpRequestConfigCollection)base["HttpRequestConfigs"];
                return httpRequestCollection;
            }
        }

        [ConfigurationProperty("General", IsDefaultCollection = false)]
        public GeneralConfigsElement GeneralConfigs
        {
            get
            {
                GeneralConfigsElement httpRequestCollection = (GeneralConfigsElement)base["General"];
                return httpRequestCollection;
            }
        }
    }

    public class GeneralConfigsElement : ConfigurationElement
    {
        [ConfigurationProperty("Config", IsDefaultCollection = true, IsRequired = false)]
        public GeneralConfigCollection GeneralConfig
        {
            get { return (GeneralConfigCollection)base["Config"]; }
        }
    }
    public class GeneralConfigCollection : ConfigurationElementCollection
    {
        public new GeneralConfigElement this[string name]
        {
            get
            {
                if (IndexOf(name) < 0) return null;
                return (GeneralConfigElement)BaseGet(name);
            }
        }

        public GeneralConfigElement this[int index]
        {
            get { return (GeneralConfigElement)BaseGet(index); }
        }

        public int IndexOf(string name)
        {
            name = name.ToLower();

            for (int idx = 0; idx < base.Count; idx++)
            {
                if (this[idx].Key.ToLower() == name)
                    return idx;
            }
            return -1;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new GeneralConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((GeneralConfigElement)element).Key;
        }

        protected override string ElementName
        {
            get { return "Config"; }
        }
    }

    public class HttpRequestConfigCollection : ConfigurationElementCollection
    {

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new HttpRequestConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((HttpRequestConfigElement)element).Name;
        }

        public HttpRequestConfigElement this[int index]
        {
            get
            {
                return (HttpRequestConfigElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public HttpRequestConfigElement this[string name]
        {
            get
            {
                return (HttpRequestConfigElement)BaseGet(name);
            }
        }

        public int IndexOf(HttpRequestConfigElement details)
        {
            return BaseIndexOf(details);
        }

        public void Add(HttpRequestConfigElement details)
        {
            BaseAdd(details);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(HttpRequestConfigElement details)
        {
            if (BaseIndexOf(details) >= 0)
                BaseRemove(details.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override string ElementName
        {
            get { return "HttpRequestConfig"; }
        }
    }

    public class HttpRequestConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("URL", IsRequired = true)]
        public string URL
        {
            get { return (string)this["URL"]; }
            set { this["URL"] = value; }
        }

        [ConfigurationProperty("Timeout", IsRequired = false, DefaultValue = 30000)]
        public int Timeout
        {
            get { return (int)this["Timeout"]; }
            set { this["Timeout"] = value; }
        }

        [ConfigurationProperty("HttpMethod", IsRequired = true)]
        public string HttpMethod
        {
            get { return (string)this["HttpMethod"]; }
            set { this["HttpMethod"] = value; }
        }

        [ConfigurationProperty("HttpContentType", IsRequired = false)]
        public string HttpContentType
        {
            get { return (string)this["HttpContentType"]; }
            set { this["HttpContentType"] = value; }
        }


        [ConfigurationProperty("HttpHeaders", IsDefaultCollection = false, IsRequired = false)]
        public HttpHeaderCollection HttpHeaders
        {
            get { return (HttpHeaderCollection)base["HttpHeaders"]; }
        }

        [ConfigurationProperty("HttpContents", IsDefaultCollection = false, IsRequired = false)]
        public HttpContentsCollection HttpContents
        {
            get { return (HttpContentsCollection)base["HttpContents"]; }
        }
    }

    public class HttpContentsCollection : ConfigurationElementCollection
    {
        public new HttpContentConfigElement this[string name]
        {
            get
            {
                if (IndexOf(name) < 0) return null;
                return (HttpContentConfigElement)BaseGet(name);
            }
        }

        public HttpContentConfigElement this[int index]
        {
            get { return (HttpContentConfigElement)BaseGet(index); }
        }

        public int IndexOf(string name)
        {
            name = name.ToLower();

            for (int idx = 0; idx < base.Count; idx++)
            {
                if (this[idx].Key.ToLower() == name)
                    return idx;
            }
            return -1;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new HttpContentConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HttpContentConfigElement)element).Key;
        }

        protected override string ElementName
        {
            get { return "HttpContent"; }
        }
    }

    public class HttpHeaderCollection : ConfigurationElementCollection
    {
        public new HttpHeaderConfigElement this[string name]
        {
            get
            {
                if (IndexOf(name) < 0) return null;
                return (HttpHeaderConfigElement)BaseGet(name);
            }
        }

        public HttpHeaderConfigElement this[int index]
        {
            get { return (HttpHeaderConfigElement)BaseGet(index); }
        }

        public int IndexOf(string name)
        {
            name = name.ToLower();

            for (int idx = 0; idx < base.Count; idx++)
            {
                if (this[idx].Key.ToLower() == name)
                    return idx;
            }
            return -1;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new HttpHeaderConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HttpHeaderConfigElement)element).Key;
        }

        protected override string ElementName
        {
            get { return "HttpHeader"; }
        }
    }

    public class HttpHeaderConfigElement : ConfigurationElement
    {
        public HttpHeaderConfigElement()
        {
        }

        public HttpHeaderConfigElement(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        [ConfigurationProperty("key", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true, DefaultValue = "")]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

    }

    public class HttpContentConfigElement : ConfigurationElement
    {
        public HttpContentConfigElement()
        {
        }

        public HttpContentConfigElement(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        [ConfigurationProperty("key", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true, DefaultValue = "")]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }


    }


    public class GeneralConfigElement : ConfigurationElement
    {
        public GeneralConfigElement()
        {
        }

        public GeneralConfigElement(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        [ConfigurationProperty("key", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true, DefaultValue = "")]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }


    }


}
