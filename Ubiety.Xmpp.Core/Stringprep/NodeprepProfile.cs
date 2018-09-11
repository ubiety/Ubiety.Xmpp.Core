// Copyright 2018 Dieter Lunn
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using StringPrep;

namespace Ubiety.Xmpp.Core.Stringprep
{
    /// <summary>
    ///     XMPP Nodeprep Stringprep profile
    /// </summary>
    public static class NodeprepProfile
    {
        /// <summary>
        ///     Nodeprep prohibited code points
        /// </summary>
        public static readonly int[] NodeprepProhibited = new int[]
        {
            0x0022, 0x0022,
            0x0026, 0x0026,
            0x0027, 0x0027,
            0x002F, 0x002F,
            0x003A, 0x003A,
            0x003C, 0x003C,
            0x003E, 0x003E,
            0x0040, 0x0040,
        };

        /// <summary>
        ///     Create Nodeprep profile
        /// </summary>
        /// <returns>Nodeprep process</returns>
        public static IPreparationProcess Create()
        {
            return PreparationProcess.Build()
                .WithMappingStep(MappingTable.Build()
                    .WithMappingTable(Mapping.B_1)
                    .WithMappingTable(Mapping.B_2)
                    .Compile())
                .WithNormalizationStep()
                .WithProhibitedValueStep(ValueRangeTable.Create(
                    Prohibited.C_1_1,
                    Prohibited.C_1_2,
                    Prohibited.C_2_1,
                    Prohibited.C_2_2,
                    Prohibited.C_3,
                    Prohibited.C_4,
                    Prohibited.C_5,
                    Prohibited.C_6,
                    Prohibited.C_7,
                    Prohibited.C_8,
                    Prohibited.C_9,
                    NodeprepProhibited))
                .WithBidirectionalStep()
                .WithProhibitedValueStep(ValueRangeTable.Create(
                    Unassigned.A_1))
                .Compile();
        }
    }
}
