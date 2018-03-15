using System.Collections.Generic;

namespace YouTrackClientVS.Contracts.Models.YouTrackClientModels
{
    public class YouTrackProject
    {

        /// <summary>
        /// Name of the project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Short name of the project.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Description of the project.
        /// </summary>
        /// <remarks>
        /// Only available when verbose project information is retrieved.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Versions defined for the project.
        /// </summary>
        /// <remarks>
        /// Only available when verbose project information is retrieved.
        /// </remarks>
        public ICollection<string> Versions { get; set; }

    }
}
