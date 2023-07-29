using Microsoft.Extensions.Localization;

namespace Plum.Localization.Json
{
    /// <summary>
    /// JsonLocalizationOptions
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Localization.LocalizationOptions" />
    public class JsonLocalizationOptions : LocalizationOptions
    {
        /// <summary>
        /// Gets or sets the type of the resources.
        /// </summary>
        /// <value>
        /// The type of the resources.
        /// </value>
        public ResourcesType ResourcesType { get; set; } = ResourcesType.TypeBased;
    }
}