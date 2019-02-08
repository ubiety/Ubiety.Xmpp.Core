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

using Ubiety.Stringprep.Core;

namespace Ubiety.Xmpp.Core.Stringprep
{
    /// <summary>
    ///     XMPP Nameprep Stringprep profile
    /// </summary>
    public static class NameprepProfile
    {
        /// <summary>
        ///     Create Nameprep profile
        /// </summary>
        /// <returns>Nameprep process</returns>
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
                    Prohibited.C_2_2,
                    Prohibited.C_3,
                    Prohibited.C_4,
                    Prohibited.C_5,
                    Prohibited.C_6,
                    Prohibited.C_7,
                    Prohibited.C_8,
                    Prohibited.C_9))
                .WithBidirectionalStep()
                .WithProhibitedValueStep(ValueRangeTable.Create(
                    Unassigned.A_1))
                .Compile();
        }
    }
}
