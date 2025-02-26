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
    /// Represents the SASLprep string preparation profile.
    /// SASLprep is used to prepare Unicode strings for comparison during
    /// authentication processes by applying specific normalization, mapping,
    /// and prohibitions defined in the SASLprep specification.
    /// </summary>
    public static class SaslprepProfile
    {
        /// <summary>
        /// Creates a new SASLprep profile for preparing Unicode strings as per SASLprep specifications.
        /// The profile applies the necessary normalization, mapping, and prohibitions
        /// for use in authentication processes.
        /// </summary>
        /// <returns>An implementation of <see cref="IPreparationProcess"/> used to execute SASLprep string preparation.</returns>
        public static IPreparationProcess Create()
        {
            return PreparationProcess.Build()
                .WithMappingStep(MappingTable.Build()
                    .WithValueRangeTable(Prohibited.ASCIISpaceCharacters, ' ')
                    .WithMappingTable(Mapping.MappedToNothing)
                    .Compile())
                .WithNormalizationStep()
                .WithProhibitedValueStep(ValueRangeTable.Create(
                    Prohibited.NonASCIISpaceCharacters,
                    Prohibited.ASCIIControlCharacters,
                    Prohibited.NonASCIIControlCharacters,
                    Prohibited.PrivateUseCharacters,
                    Prohibited.NonCharacterCodePoints,
                    Prohibited.SurrogateCodePoints,
                    Prohibited.InappropriateForPlainText,
                    Prohibited.InappropriateForCanonicalRepresentation,
                    Prohibited.TaggingCharacters))
                .WithBidirectionalStep()
                .WithProhibitedValueStep(ValueRangeTable.Create(
                    Unassigned.UnassignedCodePoints))
                .Compile();
        }
    }
}
