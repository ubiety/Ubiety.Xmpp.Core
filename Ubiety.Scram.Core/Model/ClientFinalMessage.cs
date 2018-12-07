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

using Ubiety.Scram.Core.Attributes;

namespace Ubiety.Scram.Core.Model
{
    public class ClientFinalMessage
    {
        public ClientFinalMessage(ClientFirstMessage clientFirstMessage, ServerFirstMessage serverFirstMessage)
        {
            Channel = new ChannelAttribute(clientFirstMessage.Gs2Header);
            Nonce = new NonceAttribute(serverFirstMessage.Nonce.Value);
        }

        public ChannelAttribute Channel { get; }

        public NonceAttribute Nonce { get; }

        public ClientProofAttribute Proof { get; private set; }

        public string MessageWithoutProof => $"{Channel},{Nonce}";

        public string Message => $"{MessageWithoutProof},{Proof}";

        public void SetProof(byte[] proof)
        {
            Proof = new ClientProofAttribute(proof);
        }
    }
}