﻿using System.Collections.Generic;

namespace Plum.Mapping
{
    /// <summary>
    /// PropertyMapping
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        /// <summary>
        /// Gets the mapping dictionary.
        /// </summary>
        /// <value>
        /// The mapping dictionary.
        /// </value>
        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapping{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="mappingDictionary">The mapping dictionary.</param>
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            Check.NotNull(mappingDictionary);

            MappingDictionary = mappingDictionary;
        }
    }
}