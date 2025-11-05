using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace Prana.ExposurePnlCache
{
    public static class PropertyAccessorUpdater
    {


        static Type _typeToWorkOn = null;
        static List<PropertyInfo> _dynamicColumnPropertyList;
        static List<PropertyAccessor> _listOfAccessors = null;

        public static List<PropertyInfo> DynamicColumnPropertyList
        {
            get { return _dynamicColumnPropertyList; }
            set
            {
                _dynamicColumnPropertyList = value;
                if (_listOfAccessors == null)
                {
                    _listOfAccessors = new List<PropertyAccessor>();
                }
                else
                {
                    _listOfAccessors.Clear();
                }
                foreach (PropertyInfo existingProp in _dynamicColumnPropertyList)
                {
                    if (_typeToWorkOn == null)
                    {
                        _typeToWorkOn = typeof(ExposurePnlCacheItem);
                    }
                    _listOfAccessors.Add(new PropertyAccessor(_typeToWorkOn, existingProp.Name));
                }
            }
        }


        static int _valueInt;
        static double _valueDouble;

        public static void SetTargetFromSource(object source, object target)
        {


            for (int j = 0; j < _listOfAccessors.Count; j++)
            {
                switch (_listOfAccessors[j].PropertyType.ToString())
                {
                    case "System.String":

                        _listOfAccessors[j].Set(target, _listOfAccessors[j].Get(source).ToString());
                        break;


                    case "System.Int32":
                        //for (int i = 0; i < sourcelistOfItems.Count; i++)
                        //{
                        _valueInt = (int)_listOfAccessors[j].Get(source);

                        _listOfAccessors[j].Set(target, _valueInt);
                        //}

                        break;


                    case "System.Double":
                        //for (int i = 0; i < sourcelistOfItems.Count; i++)
                        //{
                        //    if (_listOfAccessors[j].Get(sourcelistOfItems[i]) != null)
                        //    {
                        _valueDouble = (double)_listOfAccessors[j].Get(source);

                        _listOfAccessors[j].Set(target, _valueDouble);
                        //    }
                        //}
                        break;
                    default:
                        break;
                }

            }



        }
    }
}
