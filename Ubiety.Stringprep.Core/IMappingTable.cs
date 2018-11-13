namespace Ubiety.Stringprep.Core
{
    /// <summary>
    ///     Mapping table interface
    /// </summary>
    public interface IMappingTable
    {
        /// <summary>
        ///     Does the value have a replacement
        /// </summary>
        /// <param name="value">Value to replace</param>
        /// <returns>A value indicating whether there is a replacement or not</returns>
        bool HasReplacement(int value);

        /// <summary>
        ///     Gets the value replacement
        /// </summary>
        /// <param name="value">Value to replace</param>
        /// <returns>Replacement value</returns>
        int[] GetReplacement(int value);
    }
}