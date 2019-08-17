using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SauceNao10.Core.Models
{
    public struct Result
    {
        /// <summary>
        /// Gets or sets the title of the artwork.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the author of the artwork.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string Author { get; set; }
        /// <summary>
        /// Gets or sets the database of the artwork.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string DB { get; set; }
        /// <summary>
        /// Gets or sets the thumbnail URL of the artwork.
        /// </summary>
        /// <value>
        /// The thumbnail.
        /// </value>
        public string Thumbnail { get; set; }
        /// <summary>
        /// Gets or sets the similarity of the artwork.
        /// </summary>
        /// <value>
        /// The similarity.
        /// </value>
        public double Similarity { get; set; }
        /// <summary>
        /// Gets or sets the sources of the artwork.
        /// </summary>
        /// <value>
        /// The sources.
        /// </value>
        public IList<string> Sources { get; set; }
        /// <summary>
        /// Gets or sets the raw data of the artwork.
        /// </summary>
        /// <value>
        /// The raw data.
        /// </value>
        public JToken RawData { get; set; }

        /// <summary>
        /// Converts to string with the following format: "<see cref="Title"/> by <see cref="Author"/> on <see cref="DB"/>".
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var rn = "";
            if (!string.IsNullOrWhiteSpace(Title)) rn += $"{Title} ";
            if (!string.IsNullOrWhiteSpace(Author)) rn += $"by {Author} ";
            if (!string.IsNullOrWhiteSpace(DB)) rn += $"on {DB}";
            return rn;
        }
    }
}
