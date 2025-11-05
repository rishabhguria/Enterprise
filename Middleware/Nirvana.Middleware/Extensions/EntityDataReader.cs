using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Data.Common;
using System.Data.Objects.DataClasses;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Linq.Expressions;

namespace System.Data.SqlClient
{

    /// <summary>
    /// Entity Data Reader
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks></remarks>
    public sealed class EntityDataReader<T> : DbDataReader, IDataReader
    {
        /// <summary>
        /// 
        /// </summary>
        readonly IEnumerator<T> enumerator;
        /// <summary>
        /// 
        /// </summary>
        readonly EntityDataReaderOptions options;
        /// <summary>
        /// 
        /// </summary>
        T current;
        /// <summary>
        /// 
        /// </summary>
        bool closed = false;
        /// <summary>
        /// 
        /// </summary>
        static List<Attribute> scalarAttributes;
        /// <summary>
        /// 
        /// </summary>
        static List<Attribute> scalarAttributesPlusRelatedObjectScalarAttributes;
        /// <summary>
        /// 
        /// </summary>
        static List<Attribute> scalarAttributesPlusRelatedObjectKeyAttributes;
        /// <summary>
        /// 
        /// </summary>
        readonly List<Attribute> attributes;

        #region Attribute inner type

        /// <summary>
        /// Attributes
        /// </summary>
        /// <remarks></remarks>
        private class Attribute
        {
            //PropertyInfo propertyInfo;          
            /// <summary>
            /// 
            /// </summary>
            public readonly Type Type;
            /// <summary>
            /// 
            /// </summary>
            public readonly string FullName;
            /// <summary>
            /// 
            /// </summary>
            public readonly string Name;
            /// <summary>
            /// 
            /// </summary>
            public readonly bool IsRelatedAttribute;
            /// <summary>
            /// 
            /// </summary>
            readonly Func<T, object> ValueAccessor;

            /// <summary>
            /// Uses Lamda expressions to create a Func T <!--<object>--> that invokes the given property getter.
            /// The property value will be extracted and cast to type TProperty
            /// </summary>
            /// <typeparam name="TObject">The type of the object declaring the property.</typeparam>
            /// <typeparam name="TProperty">The type to cast the property value to</typeparam>
            /// <param name="pi">PropertyInfo pointing to the property to wrap</param>
            /// <returns></returns>
            public static Func<TObject, TProperty> MakePropertyAccessor<TObject, TProperty>(PropertyInfo pi)
            {
                ParameterExpression objParam = Expression.Parameter(typeof(TObject), "obj");
                MemberExpression typedAccessor = Expression.PropertyOrField(objParam, pi.Name);
                UnaryExpression castToObject = Expression.Convert(typedAccessor, typeof(object));
                LambdaExpression lambdaExpr = Expression.Lambda<Func<TObject, TProperty>>(castToObject, objParam);

                return (Func<TObject, TProperty>)lambdaExpr.Compile();
            }


            /// <summary>
            /// Makes the related property accessor.
            /// </summary>
            /// <typeparam name="TObject">The type of the object.</typeparam>
            /// <typeparam name="TProperty">The type of the property.</typeparam>
            /// <param name="pi">The pi.</param>
            /// <param name="pi2">The pi2.</param>
            /// <returns></returns>
            /// <remarks></remarks>
            public static Func<TObject, TProperty> MakeRelatedPropertyAccessor<TObject, TProperty>(PropertyInfo pi, PropertyInfo pi2)
            {

                Func<TObject, object> getRelatedObject;
                {
                    // expression like:
                    //    return (object)t.SomeProp;
                    ParameterExpression typedParam = Expression.Parameter(typeof(T), "t");
                    MemberExpression typedAccessor = Expression.PropertyOrField(typedParam, pi.Name);
                    UnaryExpression castToObject = Expression.Convert(typedAccessor, typeof(object));
                    LambdaExpression lambdaExpr = Expression.Lambda<Func<TObject, object>>(castToObject, typedParam);
                    getRelatedObject = (Func<TObject, object>)lambdaExpr.Compile();
                }


                Func<object, TProperty> getRelatedObjectProperty;
                {

                    // expression like:
                    //    return (object)((PropType)o).RelatedProperty;
                    ParameterExpression objParam = Expression.Parameter(typeof(object), "o");
                    UnaryExpression typedParam = Expression.Convert(objParam, pi.PropertyType);
                    MemberExpression typedAccessor = Expression.PropertyOrField(typedParam, pi2.Name);
                    UnaryExpression castToObject = Expression.Convert(typedAccessor, typeof(TProperty));
                    LambdaExpression lambdaExpr = Expression.Lambda<Func<object, TProperty>>(castToObject, objParam);
                    getRelatedObjectProperty = (Func<object, TProperty>)lambdaExpr.Compile();
                }

                Func<TObject, TProperty> f = (TObject t) =>
                {
                    object o = getRelatedObject(t);
                    if (o == null) return default(TProperty);
                    return getRelatedObjectProperty(o);
                };

                return f;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="EntityDataReader&lt;T&gt;.Attribute"/> class.
            /// </summary>
            /// <param name="pi">The pi.</param>
            /// <remarks></remarks>
            public Attribute(PropertyInfo pi)
            {
                this.FullName = String.Format("{0}_{1}", pi.DeclaringType.Name, pi.Name);
                this.Name = pi.Name;
                Type = pi.PropertyType;
                IsRelatedAttribute = false;

                ValueAccessor = MakePropertyAccessor<T, object>(pi);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="EntityDataReader&lt;T&gt;.Attribute"/> class.
            /// </summary>
            /// <param name="fullName">The full name.</param>
            /// <param name="name">The name.</param>
            /// <param name="type">The type.</param>
            /// <param name="getValue">The get value.</param>
            /// <param name="isRelatedAttribute">if set to <c>true</c> [is related attribute].</param>
            /// <remarks></remarks>
            public Attribute(string fullName, string name, Type type, Func<T, object> getValue, bool isRelatedAttribute)
            {
                this.FullName = fullName;
                this.Name = name;
                this.Type = type;
                this.ValueAccessor = getValue;
                this.IsRelatedAttribute = isRelatedAttribute;
            }

            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <returns></returns>
            /// <remarks></remarks>
            public object GetValue(T target)
            {
                return ValueAccessor(target);
            }
        }
        #endregion

        #region "Scalar Types"

        static bool IsScalarType(Type t)
        {
            return scalarTypes.Contains(t);
        }
        static readonly HashSet<Type> scalarTypes = LoadScalarTypes();
        static HashSet<Type> LoadScalarTypes()
        {
            HashSet<Type> set = new HashSet<Type>() 
                              { 
                                //reference types
                                typeof(String),
                                typeof(Byte[]),
                                //value types
                                typeof(Byte),
                                typeof(Int16),
                                typeof(Int32),
                                typeof(Int64),
                                typeof(Single),
                                typeof(Double),
                                typeof(Decimal),
                                typeof(DateTime),
                                typeof(Guid),
                                typeof(Boolean),
                                typeof(TimeSpan),
                                //nullable value types
                                typeof(Byte?),
                                typeof(Int16?),
                                typeof(Int32?),
                                typeof(Int64?),
                                typeof(Single?),
                                typeof(Double?),
                                typeof(Decimal?),
                                typeof(DateTime?),
                                typeof(Guid?),
                                typeof(Boolean?),
                                typeof(TimeSpan?)
                              };


            return set;

        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDataReader&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <remarks></remarks>
        public EntityDataReader(IEnumerable<T> col)
            : this(col, EntityDataReaderOptions.Default, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDataReader&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <param name="options">The options.</param>
        /// <remarks></remarks>
        public EntityDataReader(IEnumerable<T> col, EntityDataReaderOptions options)
            : this(col, options, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDataReader&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <param name="options">The options.</param>
        /// <param name="objectContext">The object context.</param>
        /// <remarks></remarks>
        public EntityDataReader(IEnumerable<T> col, EntityDataReaderOptions options, ObjectContext objectContext)
        {
            this.enumerator = col.GetEnumerator();
            this.options = options;

            if (options.RecreateForeignKeysForEntityFrameworkEntities && objectContext == null)
            {
                throw new ArgumentException("If RecreateForeignKeysForEntityFrameworkEntities=true then objectContext is required");
            }

            //done without a lock, so we risk running twice
            if (scalarAttributes == null)
            {
                scalarAttributes = DiscoverScalarAttributes(typeof(T));
            }
            if (options.FlattenRelatedObjects && scalarAttributesPlusRelatedObjectScalarAttributes == null)
            {
                var atts = DiscoverRelatedObjectScalarAttributes(typeof(T));
                scalarAttributesPlusRelatedObjectScalarAttributes = atts.Concat(scalarAttributes).ToList();
            }
            if (options.RecreateForeignKeysForEntityFrameworkEntities && scalarAttributesPlusRelatedObjectKeyAttributes == null)
            {
                var atts = DiscoverRelatedObjectKeyAttributes(typeof(T), objectContext);
                scalarAttributesPlusRelatedObjectKeyAttributes = atts.Concat(scalarAttributes).ToList();
            }

            if (options.FlattenRelatedObjects)
            {
                attributes = scalarAttributesPlusRelatedObjectScalarAttributes;
            }
            else if (objectContext != null)
            {
                attributes = scalarAttributesPlusRelatedObjectKeyAttributes;
            }
            else
            {
                attributes = scalarAttributes;
            }


        }


        /// <summary>
        /// Discovers the scalar attributes.
        /// </summary>
        /// <param name="thisType">Type of the this.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        static List<Attribute> DiscoverScalarAttributes(Type thisType)
        {

            //Not a collection of entities, just an IEnumerable<String> or other scalar type.
            //So add just a single Attribute that returns the object itself
            if (IsScalarType(thisType))
            {
                return new List<Attribute> { new Attribute("Value", "Value", thisType, t => t, false) };
            }


            //find all the scalar properties
            var allProperties = (from p in thisType.GetProperties()
                                 where IsScalarType(p.PropertyType)
                                 select p).ToList();

            //Look for a constructor with arguments that match the properties on name and type
            //(name modulo case, which varies between constructor args and properties in coding convention)
            //If such an "ordering constructor" exists, return the properties ordered by the corresponding
            //constructor args ordinal position.  
            //An important instance of an ordering constructor, is that C# anonymous types all have one.  So
            //this enables a simple convention to specify the order of columns projected by the EntityDataReader
            //by simply building the EntityDataReader from an anonymous type projection.
            //If such a constructor is found, replace allProperties with a collection of properties sorted by constructor order.
            foreach (var completeConstructor in from ci in thisType.GetConstructors()
                                                where ci.GetParameters().Count() == allProperties.Count()
                                                select ci)
            {
                var q = (from cp in completeConstructor.GetParameters()
                         join p in allProperties
                           on new { n = cp.Name.ToLower(), t = cp.ParameterType } equals new { n = p.Name.ToLower(), t = p.PropertyType }
                         select new { cp, p }).ToList();

                if (q.Count() == allProperties.Count()) //all constructor parameters matched by name and type to properties
                {
                    //sort all properties by constructor ordinal position
                    allProperties = (from o in q
                                     orderby o.cp.Position
                                     select o.p).ToList();
                    break; //stop looking for an ordering consturctor
                }


            }

            return allProperties.Select(p => new Attribute(p)).ToList();

        }
        /// <summary>
        /// Discovers the related object key attributes.
        /// </summary>
        /// <param name="thisType">Type of the this.</param>
        /// <param name="objectContext">The object context.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        static List<Attribute> DiscoverRelatedObjectKeyAttributes(Type thisType, ObjectContext objectContext)
        {

            var attributeList = new SortedList<string, Attribute>();


            //recreate foreign key column values
            //by adding Attributes for any key values of referenced entities 
            //that aren't already exposed as scalar properties
            var mw = objectContext.MetadataWorkspace;
            var entityTypesByName = mw.GetItems<EntityType>(DataSpace.OSpace).ToLookup(e => e.FullName);

            //find the EntityType metadata for T 
            EntityType thisEntity = entityTypesByName[thisType.FullName].First();
            var thisEntityKeys = thisEntity.KeyMembers.ToDictionary(k => k.Name);

            //TODOx use the NavigationProperties instead of the ENtityRelations -- too complicated
            //TODO fix the attribute naming.  Probably requires marking each attribtue as direct or related.


            var erProps = thisType.GetProperties()
                                  .Where(p => typeof(EntityReference)
                                  .IsAssignableFrom(p.PropertyType)).ToList();


            //For each EntityRelation property add add the keys of the related Entity
            foreach (var pi in erProps)
            {
                //Find the name of the CLR Type at the other end of the reference because we need to get its key attributes.
                //the property type is EntityReference<T>, we need T.
                string relatedEntityCLRTypeName = pi.PropertyType.GetGenericArguments().First().FullName;

                //Find the EntityType at the other end of the relationship because we need to get its key attributes.
                EntityType relatedEntityEFType = entityTypesByName[relatedEntityCLRTypeName].FirstOrDefault();
                if (relatedEntityEFType == null)
                {
                    throw new InvalidOperationException("Cannot find EntityType for EntityReference Property " + pi.Name);
                }

                //Add attributes for each key value of the related entity.  These are the properties that
                //would probably appear in the storage object.  The names will be the same as they are on the 
                //related entity, except prefixed with the related entity name, 
                //and with a check to make sure that we're not introducing a duplicate.
                // so if you have 
                //  if OrderItem.OrderID -> Order.ID   then the column will be Order_ID
                //  if OrderItem.OrderID -> Order.OrderID   then the column will be Order_OrderID
                foreach (var key in relatedEntityEFType.KeyMembers)
                {
                    string targetKeyAttributeName = key.Name;

                    //TODO it would be better to get the NavigationProperty and find the ToEndMember name
                    //but the NavigationProperty doesn't have good way to get the EntityReference 
                    //or the related entity key.
                    string referenceName;
                    if (pi.Name.EndsWith("Reference", StringComparison.Ordinal))
                    {
                        referenceName = pi.Name.Substring(0, pi.Name.Length - "Reference".Length);
                    }
                    else  //there's no rule that the EntityReference named like this so, if not just use the target type
                    {
                        referenceName = pi.PropertyType.Name;

                        //if there are multiple relations to the same target type, just uniqify them with an index
                        int ix = erProps.Where(p => p.PropertyType == pi.PropertyType).ToList().IndexOf(pi);
                        if (ix > 0)
                        {
                            referenceName = referenceName + ix.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        }

                    }
                    string fullName = referenceName + "_" + key.Name;


                    //bind out local variables for the valueAccessor closure.
                    Type kType = Type.GetType(key.TypeUsage.EdmType.FullName);
                    PropertyInfo entityReferenceProperty = pi;

                    Func<T, object> valueAccessor = o =>
                    {
                        EntityReference er = (EntityReference)entityReferenceProperty.GetValue(o, null);

                        //for nullable foregn keys, just return null
                        if (er.EntityKey == null)
                        {
                            return null;
                        }
                        object val = er.EntityKey.EntityKeyValues.First(k => k.Key == targetKeyAttributeName).Value;
                        return val;
                    };
                    string name = key.Name;

                    attributeList.Add(name, new Attribute(fullName, name, kType, valueAccessor, true));
                }


            }

            return attributeList.Values.ToList();

        }
        /// <summary>
        /// Discovers the related object scalar attributes.
        /// </summary>
        /// <param name="thisType">Type of the this.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        static List<Attribute> DiscoverRelatedObjectScalarAttributes(Type thisType)
        {

            var atts = new List<Attribute>();

            //get the related objects which aren't scalars, not EntityReference objects and not collections
            var relatedObjectProperties =
                              (from p in thisType.GetProperties()
                               where !IsScalarType(p.PropertyType)
                                  && !typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType)
                                  && !typeof(EntityReference).IsAssignableFrom(p.PropertyType)
                                  && !typeof(EntityKey).IsAssignableFrom(p.PropertyType)
                               select p).ToList();

            foreach (var rop in relatedObjectProperties)
            {
                var type = rop.PropertyType;
                //get the scalar properties for the related type
                var scalars = type.GetProperties().Where(p => IsScalarType(p.PropertyType)).ToList();

                foreach (var sp in scalars)
                {
                    string attName = rop.Name + "_" + sp.Name;
                    //create a value accessor which takes an instance of T, and returns the related object scalar
                    var valueAccessor = Attribute.MakeRelatedPropertyAccessor<T, object>(rop, sp);
                    string name = attName;
                    Attribute att = new Attribute(rop.Name, attName, sp.PropertyType, valueAccessor, true);
                    atts.Add(att);
                }

            }
            return atts;

        }



        #endregion

        #region Utility Methods
        static Type nullable_T = typeof(System.Nullable<int>).GetGenericTypeDefinition();
        /// <summary>
        /// Determines whether the specified t is nullable.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns><c>true</c> if the specified t is nullable; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        static bool IsNullable(Type t)
        {
            return (t.IsGenericType
                && t.GetGenericTypeDefinition() == nullable_T);
        }
        /// <summary>
        /// Strips the type of the nullable.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        static Type StripNullableType(Type t)
        {
            return t.GetGenericArguments()[0];
        }
        #endregion

        #region GetSchemaTable


        const string shemaTableSchema = @"<?xml version=""1.0"" standalone=""yes""?>
<xs:schema id=""NewDataSet"" xmlns="""" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
  <xs:element name=""NewDataSet"" msdata:IsDataSet=""true"" msdata:MainDataTable=""SchemaTable"" msdata:Locale="""">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""SchemaTable"" msdata:Locale="""" msdata:MinimumCapacity=""1"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""ColumnName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""ColumnOrdinal"" msdata:ReadOnly=""true"" type=""xs:int"" default=""0"" minOccurs=""0"" />
              <xs:element name=""ColumnSize"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""NumericPrecision"" msdata:ReadOnly=""true"" type=""xs:short"" minOccurs=""0"" />
              <xs:element name=""NumericScale"" msdata:ReadOnly=""true"" type=""xs:short"" minOccurs=""0"" />
              <xs:element name=""IsUnique"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsKey"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""BaseServerName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseCatalogName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseColumnName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseSchemaName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseTableName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DataType"" msdata:DataType=""System.Type, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""AllowDBNull"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""ProviderType"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""IsAliased"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsExpression"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsIdentity"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsAutoIncrement"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsRowVersion"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsHidden"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsLong"" msdata:ReadOnly=""true"" type=""xs:boolean"" default=""false"" minOccurs=""0"" />
              <xs:element name=""IsReadOnly"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""ProviderSpecificDataType"" msdata:DataType=""System.Type, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DataTypeName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionDatabase"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionOwningSchema"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""UdtAssemblyQualifiedName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""NonVersionedProviderType"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        /// <summary>
        /// Returns a <see cref="T:System.Data.DataTable"/> that describes the column metadata of the <see cref="T:System.Data.Common.DbDataReader"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.Data.DataTable"/> that describes the column metadata.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Data.SqlClient.SqlDataReader"/> is closed. </exception>
        /// <remarks></remarks>
        public override DataTable GetSchemaTable()
        {
            DataSet s = new DataSet();
            s.Locale = System.Globalization.CultureInfo.CurrentCulture;
            s.ReadXmlSchema(new System.IO.StringReader(shemaTableSchema));
            DataTable t = s.Tables[0];
            for (int i = 0; i < this.FieldCount; i++)
            {
                DataRow row = t.NewRow();
                row["ColumnName"] = this.GetName(i);
                row["ColumnOrdinal"] = i;

                Type type = this.GetFieldType(i);
                if (type.IsGenericType
                  && type.GetGenericTypeDefinition() == typeof(System.Nullable<int>).GetGenericTypeDefinition())
                {
                    type = type.GetGenericArguments()[0];
                }
                row["DataType"] = this.GetFieldType(i);
                row["DataTypeName"] = this.GetDataTypeName(i);
                row["ColumnSize"] = -1;
                t.Rows.Add(row);
            }
            return t;

        }
        #endregion

        #region IDataReader Members

        /// <summary>
        /// Closes the <see cref="T:System.Data.Common.DbDataReader"/> object.
        /// </summary>
        /// <remarks></remarks>
        public override void Close()
        {
            closed = true;
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        /// <returns>The depth of nesting for the current row.</returns>
        /// <remarks></remarks>
        public override int Depth
        {
            get { return 1; }
        }


        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Data.Common.DbDataReader"/> is closed.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Data.Common.DbDataReader"/> is closed; otherwise false.</returns>
        ///   
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Data.SqlClient.SqlDataReader"/> is closed. </exception>
        /// <remarks></remarks>
        public override bool IsClosed
        {
            get { return closed; }
        }

        /// <summary>
        /// Advances the reader to the next result when reading the results of a batch of statements.
        /// </summary>
        /// <returns>true if there are more result sets; otherwise false.</returns>
        /// <remarks></remarks>
        public override bool NextResult()
        {
            return false;
        }

        int entitiesRead = 0;
        /// <summary>
        /// Advances the reader to the next record in a result set.
        /// </summary>
        /// <returns>true if there are more rows; otherwise false.</returns>
        /// <remarks></remarks>
        public override bool Read()
        {
            bool rv = enumerator.MoveNext();
            if (rv)
            {
                current = enumerator.Current;
                entitiesRead += 1;
            }
            return rv;
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        /// <returns>The number of rows changed, inserted, or deleted. -1 for SELECT statements; 0 if no rows were affected or the statement failed.</returns>
        /// <remarks></remarks>
        public override int RecordsAffected
        {
            get { return -1; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Releases the managed resources used by the <see cref="T:System.Data.Common.DbDataReader"/> and optionally releases the unmanaged resources.
        /// </summary>
        /// <param name="disposing">true to release managed and unmanaged resources; false to release only unmanaged resources.</param>
        /// <remarks></remarks>
        protected override void Dispose(bool disposing)
        {
            Close();
            base.Dispose(disposing);
        }

        #endregion

        #region IDataRecord Members

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <returns>The number of columns in the current row.</returns>
        ///   
        /// <exception cref="T:System.NotSupportedException">There is no current connection to an instance of SQL Server. </exception>
        /// <remarks></remarks>
        public override int FieldCount
        {
            get
            {
                return attributes.Count;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        TField GetValue<TField>(int i)
        {
            TField val = (TField)attributes[i].GetValue(current);
            return val;
        }
        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override bool GetBoolean(int i)
        {
            return GetValue<bool>(i);
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The 8-bit unsigned integer value of the specified column.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override byte GetByte(int i)
        {
            return GetValue<byte>(i);
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for <paramref name="buffer"/> to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {

            var buf = GetValue<byte[]>(i);
            int bytes = Math.Min(length, buf.Length - (int)fieldOffset);
            Buffer.BlockCopy(buf, (int)fieldOffset, buffer, bufferoffset, bytes);
            return bytes;

        }

        /// <summary>
        /// Gets the character value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The character value of the specified column.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override char GetChar(int i)
        {
            return GetValue<char>(i);
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldoffset">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for <paramref name="buffer"/> to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of characters read.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            //throw new NotImplementedException();
            string s = GetValue<string>(i);
            int chars = Math.Min(length, s.Length - (int)fieldoffset);
            s.CopyTo((int)fieldoffset, buffer, bufferoffset, chars);

            return chars;
        }

        //public override DbDataReader GetData(int i)
        //{
        //  throw new NotImplementedException();
        //}

        /// <summary>
        /// Gets the data type information for the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The data type information for the specified field.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override string GetDataTypeName(int i)
        {
            return attributes[i].Type.Name;
        }

        /// <summary>
        /// Gets the date and time data value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The date and time data value of the specified field.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override DateTime GetDateTime(int i)
        {
            return GetValue<DateTime>(i);
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The fixed-position numeric value of the specified field.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override decimal GetDecimal(int i)
        {
            return GetValue<decimal>(i);
        }

        /// <summary>
        /// Gets the double-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The double-precision floating point number of the specified field.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override double GetDouble(int i)
        {
            return GetValue<double>(i);
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type"/> information corresponding to the type of <see cref="T:System.Object"/> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)"/>.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Type"/> information corresponding to the type of <see cref="T:System.Object"/> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)"/>.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override Type GetFieldType(int i)
        {
            Type t = attributes[i].Type;
            if (!options.ExposeNullableTypes && IsNullable(t))
            {
                return StripNullableType(t);
            }
            return t;
        }

        /// <summary>
        /// Gets the single-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The single-precision floating point number of the specified field.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override float GetFloat(int i)
        {
            return GetValue<float>(i);
        }

        /// <summary>
        /// Returns the GUID value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The GUID value of the specified field.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override Guid GetGuid(int i)
        {
            return GetValue<Guid>(i);
        }

        /// <summary>
        /// Gets the 16-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 16-bit signed integer value of the specified field.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override short GetInt16(int i)
        {
            return GetValue<short>(i);
        }

        /// <summary>
        /// Gets the 32-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 32-bit signed integer value of the specified field.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override int GetInt32(int i)
        {
            return GetValue<int>(i);
        }

        /// <summary>
        /// Gets the 64-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 64-bit signed integer value of the specified field.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override long GetInt64(int i)
        {
            return GetValue<long>(i);
        }

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override string GetName(int i)
        {
            Attribute a = attributes[i];
            if (a.IsRelatedAttribute && options.PrefixRelatedObjectColumns)
            {
                return a.FullName;
            }
            return a.Name;
        }

        /// <summary>
        /// Gets the column ordinal given the name of the column.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>The zero-based column ordinal.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The name specified is not a valid column name.</exception>
        /// <remarks></remarks>
        public override int GetOrdinal(string name)
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                var a = attributes[i];

                if (!a.IsRelatedAttribute && a.Name == name)
                {
                    return i;
                }

                if (options.PrefixRelatedObjectColumns && a.IsRelatedAttribute && a.FullName == name)
                {
                    return i;
                }

                if (!options.PrefixRelatedObjectColumns && a.IsRelatedAttribute && a.Name == name)
                {
                    return i;
                }


            }
            return -1;
        }

        /// <summary>
        /// Gets the string value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The string value of the specified field.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override string GetString(int i)
        {
            return GetValue<string>(i);
        }



        /// <summary>
        /// Populates an array of objects with the column values of the current row.
        /// </summary>
        /// <param name="values">An array of <see cref="T:System.Object"/> into which to copy the attribute columns.</param>
        /// <returns>The number of instances of <see cref="T:System.Object"/> in the array.</returns>
        /// <remarks></remarks>
        public override int GetValues(object[] values)
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                values[i] = GetValue(i);
            }
            return attributes.Count;
        }



        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Object"/> which will contain the field value upon return.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override object GetValue(int i)
        {
            object o = GetValue<object>(i);
            if (!options.ExposeNullableTypes && o == null)
            {
                return DBNull.Value;
            }
            return o;
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>true if the specified field is set to null; otherwise, false.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception>
        /// <remarks></remarks>
        public override bool IsDBNull(int i)
        {
            object o = GetValue<object>(i);
            return (o == null);
        }

        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>The value of the specified column.</returns>
        ///   
        /// <exception cref="T:System.IndexOutOfRangeException">No column with the specified name was found. </exception>
        /// <remarks></remarks>
        public override object this[string name]
        {
            get { return GetValue(GetOrdinal(name)); }
        }

        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>The value of the specified column.</returns>
        ///   
        /// <exception cref="T:System.IndexOutOfRangeException">No column with the specified name was found. </exception>
        /// <remarks></remarks>
        public override object this[int i]
        {
            get { return GetValue(i); }
        }

        #endregion

        #region DbDataReader Members



        /// <summary>
        /// Returns an <see cref="T:System.Collections.IEnumerator"/> that can be used to iterate through the rows in the data reader.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> that can be used to iterate through the rows in the data reader.</returns>
        /// <remarks></remarks>
        public override System.Collections.IEnumerator GetEnumerator()
        {
            return this.enumerator;
        }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="T:System.Data.Common.DbDataReader"/> contains one or more rows.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Data.Common.DbDataReader"/> contains one or more rows; otherwise false.</returns>
        /// <remarks></remarks>
        public override bool HasRows
        {
            get { throw new NotSupportedException(); }
        }
        #endregion

    }

    /// <summary>
    /// Entity Data Reader Options Class
    /// </summary>
    /// <remarks></remarks>
    public class EntityDataReaderOptions
    {
        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <remarks></remarks>
        public static EntityDataReaderOptions Default
        {
            get { return new EntityDataReaderOptions(true, false, true, false); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDataReaderOptions"/> class.
        /// </summary>
        /// <param name="exposeNullableTypes">if set to <c>true</c> [expose nullable types].</param>
        /// <param name="flattenRelatedObjects">if set to <c>true</c> [flatten related objects].</param>
        /// <param name="prefixRelatedObjectColumns">if set to <c>true</c> [prefix related object columns].</param>
        /// <param name="recreateForeignKeysForEntityFrameworkEntities">if set to <c>true</c> [recreate foreign keys for entity framework entities].</param>
        /// <remarks></remarks>
        public EntityDataReaderOptions(
          bool exposeNullableTypes,
          bool flattenRelatedObjects,
          bool prefixRelatedObjectColumns,
          bool recreateForeignKeysForEntityFrameworkEntities)
        {
            this.ExposeNullableTypes = exposeNullableTypes;
            this.FlattenRelatedObjects = flattenRelatedObjects;
            this.PrefixRelatedObjectColumns = prefixRelatedObjectColumns;
            this.RecreateForeignKeysForEntityFrameworkEntities = recreateForeignKeysForEntityFrameworkEntities;
        }

        /// <summary>
        /// If true nullable value types are returned directly by the DataReader.
        /// If false, the DataReader will expose non-nullable value types and return DbNull.Value
        /// for null values.
        /// When loading a DataTable this option must be set to True, since DataTable does not support
        /// nullable types.
        /// </summary>
        /// <value><c>true</c> if [expose nullable types]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool ExposeNullableTypes { get; set; }

        /// <summary>
        /// If True then the DataReader will project scalar properties from related objects in addition
        /// to scalar properties from the main object.  This is especially useful for custom projecttions like
        /// var q = from od in db.SalesOrderDetail
        /// select new
        /// {
        /// od,
        /// ProductID=od.Product.ProductID,
        /// ProductName=od.Product.Name
        /// };
        /// Related objects assignable to EntityKey, EntityRelation, and IEnumerable are excluded.
        /// If False, then only scalar properties from teh main object will be projected.
        /// </summary>
        /// <value><c>true</c> if [flatten related objects]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool FlattenRelatedObjects { get; set; }

        /// <summary>
        /// If True columns projected from related objects will have column names prefixed by the
        /// name of the relating property.  This appies to either from setting FlattenRelatedObjects to True,
        /// or RecreateForeignKeysForEntityFrameworkEntities to True.
        /// If False columns will be created for related properties that are not prefixed.  This can lead
        /// to column name collision.
        /// </summary>
        /// <value><c>true</c> if [prefix related object columns]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool PrefixRelatedObjectColumns { get; set; }

        /// <summary>
        /// If True the DataReader will create columns for the key properties of related Entities.
        /// You must pass an ObjectContext and have retrieved the entity with change tracking for this to work.
        /// </summary>
        /// <value><c>true</c> if [recreate foreign keys for entity framework entities]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool RecreateForeignKeysForEntityFrameworkEntities { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public static class EntityDataReaderExtensions
    {

        /// <summary>
        /// Wraps the IEnumerable in a DbDataReader, having one column for each "scalar" property of the type T.
        /// The collection will be enumerated as the client calls IDataReader.Read().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection)
        {

            //For anonymous type projections default to flattening related objects and not prefixing columns
            //The reason being that if the programmer has taken control of the projection, the default should
            //be to expose everying in the projection and not mess with the names.
            if (typeof(T).IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false))
            {
                var options = EntityDataReaderOptions.Default;
                options.FlattenRelatedObjects = true;
                options.PrefixRelatedObjectColumns = false;
                return new EntityDataReader<T>(collection, options);
            }
            return new EntityDataReader<T>(collection);
        }

        /// <summary>
        /// Wraps the IEnumerable in a DbDataReader, having one column for each "scalar" property of the type T.
        /// The collection will be enumerated as the client calls IDataReader.Read().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="exposeNullableColumns">if set to <c>true</c> [expose nullable columns].</param>
        /// <param name="flattenRelatedObjects">if set to <c>true</c> [flatten related objects].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection, bool exposeNullableColumns, bool flattenRelatedObjects)
        {
            EntityDataReaderOptions options = new EntityDataReaderOptions(exposeNullableColumns, flattenRelatedObjects, true, false);

            return new EntityDataReader<T>(collection, options, null);
        }


        /// <summary>
        /// Enumerates the collection and copies the data into a DataTable.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="collection">The collection to copy to a DataTable</param>
        /// <returns>A DataTable containing the scalar projection of the collection.</returns>
        /// <remarks></remarks>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable t = new DataTable();
            t.Locale = System.Globalization.CultureInfo.CurrentCulture;
            t.TableName = typeof(T).Name;
            EntityDataReaderOptions options = EntityDataReaderOptions.Default;
            options.ExposeNullableTypes = false;
            EntityDataReader<T> dr = new EntityDataReader<T>(collection, options);
            t.Load(dr);
            return t;
        }

        /// <summary>
        /// Wraps the collection in a DataReader, but also includes columns for the key attributes of related Entities.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="collection">A collection to wrap in a DataReader</param>
        /// <param name="context">The context.</param>
        /// <returns>A DbDataReader wrapping the collection.</returns>
        /// <remarks></remarks>
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection, ObjectContext context) where T : EntityObject
        {
            EntityDataReaderOptions options = EntityDataReaderOptions.Default;
            options.RecreateForeignKeysForEntityFrameworkEntities = true;
            return new EntityDataReader<T>(collection, options, context);
        }

        /// <summary>
        /// Wraps the collection in a DataReader, but also includes columns for the key attributes of related Entities.
        /// </summary>
        /// <typeparam name="T">The element type of the collectin.</typeparam>
        /// <param name="collection">A collection to wrap in a DataReader</param>
        /// <param name="context">The context.</param>
        /// <param name="detachObjects">Option to detach each object in the collection from the ObjectContext.  This can reduce memory usage for queries returning large numbers of objects.</param>
        /// <param name="prefixRelatedObjectColumns">if set to <c>true</c> [prefix related object columns].</param>
        /// <returns>A DbDataReader wrapping the collection.</returns>
        /// <remarks></remarks>     
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection, ObjectContext context, bool detachObjects, bool prefixRelatedObjectColumns) where T : EntityObject
        {
            EntityDataReaderOptions options = EntityDataReaderOptions.Default;
            options.RecreateForeignKeysForEntityFrameworkEntities = true;
            options.PrefixRelatedObjectColumns = prefixRelatedObjectColumns;

            if (detachObjects)
            {
                return new EntityDataReader<T>(collection.DetachAllFrom(context), options, context);
            }
            return new EntityDataReader<T>(collection, options, context);
        }

        /// <summary>
        /// Detaches all from.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col">The col.</param>
        /// <param name="cx">The cx.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        static IEnumerable<T> DetachAllFrom<T>(this IEnumerable<T> col, ObjectContext cx)
        {
            foreach (var t in col)
            {
                cx.Detach(t);
                yield return t;
            }
        }

        /// <summary>
        /// Enumerates the collection and copies the data into a DataTable, but also includes columns for the key attributes of related Entities.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="collection">The collection to copy to a DataTable</param>
        /// <param name="context">The context.</param>
        /// <returns>A DataTable containing the scalar projection of the collection.</returns>
        /// <remarks></remarks>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection, ObjectContext context) where T : EntityObject
        {
            DataTable t = new DataTable();
            t.Locale = System.Globalization.CultureInfo.CurrentCulture;
            t.TableName = typeof(T).Name;

            EntityDataReaderOptions options = EntityDataReaderOptions.Default;
            options.RecreateForeignKeysForEntityFrameworkEntities = true;

            EntityDataReader<T> dr = new EntityDataReader<T>(collection, options, context);
            t.Load(dr);
            return t;
        }




    }
}
