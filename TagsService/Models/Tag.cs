namespace TagsService.Models
{
    /// <summary>
    /// Represents a tag with its name, count, and calculated percentage share.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of times the tag has been used or mentioned.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The percentage of the total count represented by this tag.
        /// </summary>
        public double Percentage { get; set; }
    }
}
