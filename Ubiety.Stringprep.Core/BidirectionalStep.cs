// This is free and unencumbered software released into the public domain.
//
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
//
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
// For more information, please refer to <http://unlicense.org/>

namespace Ubiety.Stringprep.Core
{
    /// <summary>
    ///     Bidirectional stringprep step
    /// </summary>
    internal class BidirectionalStep : IPreparationProcess
    {
        private readonly IValueRangeTable _lTable;
        private readonly IValueRangeTable _prohibitedTable;
        private readonly IValueRangeTable _ralTable;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BidirectionalStep" /> class
        /// </summary>
        /// <param name="prohibitedTable">Prohibited character table</param>
        /// <param name="ralTable">RandAL character table</param>
        /// <param name="lTable">L character table</param>
        public BidirectionalStep(IValueRangeTable prohibitedTable, IValueRangeTable ralTable, IValueRangeTable lTable)
        {
            _prohibitedTable = prohibitedTable;
            _ralTable = ralTable;
            _lTable = lTable;
        }

        /// <summary>
        ///     Run the stringprep step
        /// </summary>
        /// <param name="input">Input to run the step on</param>
        /// <returns>String parsed for unicode characters</returns>
        public string Run(string input)
        {
            var ralProhibited = false;
            var ralFound = false;
            var ral = false;
            var l = false;
            var first = true;

            foreach (var c in input)
            {
                if (_prohibitedTable.Contains(c))
                {
                    throw new ProhibitedValueException(c);
                }

                ralFound = _ralTable.Contains(c);
                ral = ral || ralFound;
                if (ral && ralProhibited)
                {
                    throw new BidirectionalFormatException(
                        "A character from the RandAL set was found without the string beginning with an RandAL character");
                }

                l = l || _lTable.Contains(c);
                if (ral && l)
                {
                    throw new BidirectionalFormatException("Characters from both the RandAL and L sets were found");
                }

                if (first)
                {
                    first = false;
                    ralProhibited = !ral;
                }
            }

            if (ral && !ralFound)
            {
                throw new BidirectionalFormatException(
                    "A character from the RandAL set must be the last character in an RandAL string");
            }

            return input;
        }
    }
}