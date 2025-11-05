using System;
using System.Collections.Generic;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class SearchOptionType
    /// </summary>
    [Serializable]
    public class SearchOptions
    {
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public List<SearchCategory> Category;
        /// <summary>
        /// Gets or sets the type of the product.
        /// </summary>
        /// <value>The type of the product.</value>
        public ProductType ProductType;
        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <value>The type of the field.</value>
        public FieldType FieldType;
        /// <summary>
        /// Initializes the categories.
        /// </summary>
        internal void InitializeCategories()
        {
            Category = new List<SearchCategory>();
            foreach (SearchCategory value in Enum.GetValues(typeof(SearchCategory)))
                Category.Add(value);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchOptions"></see> class.
        /// </summary>
        public SearchOptions()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchOptions"></see> class.
        /// </summary>
        /// <param name="productType">Type of the product.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="values">The values.</param>
        public SearchOptions(ProductType productType, FieldType fieldType, params SearchCategory[] values)
        {
            this.ProductType = productType;
            this.FieldType = fieldType;
            if (values != null)
            {
                Category = new List<SearchCategory>();
                Category.AddRange(values);
            }
            else
                InitializeCategories();
        }
    }
}
