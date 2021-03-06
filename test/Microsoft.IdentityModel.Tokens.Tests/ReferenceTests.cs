//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    /// <summary>
    /// Tests for references in specs
    /// https://tools.ietf.org/html/rfc7518#appendix-A.3
    /// </summary>
    public class ReferenceTests
    {

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [Theory, MemberData("AuthenticatedEncryptionTheoryData")]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        public void AuthenticatedEncryptionReferenceTest(AuthenticationEncryptionTestParams testParams)
        {
            var providerForEncryption = CryptoProviderFactory.Default.CreateAuthenticatedEncryptionProvider(testParams.EncryptionKey, testParams.Algorithm);
            var providerForDecryption = CryptoProviderFactory.Default.CreateAuthenticatedEncryptionProvider(testParams.DecryptionKey, testParams.Algorithm);
            var encryptionResult = providerForEncryption.Encrypt(testParams.Plaintext, testParams.AuthenticationData, testParams.IV);
            var plaintext = providerForDecryption.Decrypt(encryptionResult.Ciphertext, testParams.AuthenticationData, encryptionResult.IV, encryptionResult.AuthenticationTag);

            Assert.True(Utility.AreEqual(encryptionResult.IV, testParams.IV), "Utility.AreEqual(encryptionResult.IV, testParams.IV)");
            Assert.True(Utility.AreEqual(encryptionResult.AuthenticationTag, testParams.AuthenticationTag), "Utility.AreEqual(encryptionResult.AuthenticationTag, testParams.AuthenticationTag)");
            Assert.True(Utility.AreEqual(encryptionResult.Ciphertext, testParams.Ciphertext), "Utility.AreEqual(encryptionResult.Ciphertext, testParams.Ciphertext)");
            Assert.True(Utility.AreEqual(plaintext, testParams.Plaintext), "Utility.AreEqual(plaintext, testParams.Plaintext)");
        }

        public static TheoryData<AuthenticationEncryptionTestParams> AuthenticatedEncryptionTheoryData
        {
            get
            {
                var theoryData = new TheoryData<AuthenticationEncryptionTestParams>();

                theoryData.Add(new AuthenticationEncryptionTestParams
                {
                    Algorithm = AES_128_CBC_HMAC_SHA_256.Algorithm,
                    AuthenticationData = AES_128_CBC_HMAC_SHA_256.A,
                    AuthenticationTag = AES_128_CBC_HMAC_SHA_256.T,
                    Ciphertext = AES_128_CBC_HMAC_SHA_256.E,
                    DecryptionKey = new SymmetricSecurityKey(AES_128_CBC_HMAC_SHA_256.K) { KeyId = "DecryptionKey.AES_128_CBC_HMAC_SHA_256.K" },
                    EncryptionKey = new SymmetricSecurityKey(AES_128_CBC_HMAC_SHA_256.K) { KeyId = "EncryptionKey.AES_128_CBC_HMAC_SHA_256.K" },
                    IV = AES_128_CBC_HMAC_SHA_256.IV,
                    Plaintext = AES_128_CBC_HMAC_SHA_256.P,
                    TestId = "AES_128_CBC_HMAC_SHA_256"
                });

                theoryData.Add(new AuthenticationEncryptionTestParams
                {
                    Algorithm = AES_256_CBC_HMAC_SHA_512.Algorithm,
                    AuthenticationData = AES_256_CBC_HMAC_SHA_512.A,
                    AuthenticationTag = AES_256_CBC_HMAC_SHA_512.T,
                    Ciphertext = AES_256_CBC_HMAC_SHA_512.E,
                    DecryptionKey = new SymmetricSecurityKey(AES_256_CBC_HMAC_SHA_512.K) { KeyId = "DecryptionKey.AES_256_CBC_HMAC_SHA_512.K" },
                    EncryptionKey = new SymmetricSecurityKey(AES_256_CBC_HMAC_SHA_512.K) { KeyId = "EncryptionKey.AES_256_CBC_HMAC_SHA_512.K" },
                    IV = AES_256_CBC_HMAC_SHA_512.IV,
                    Plaintext = AES_256_CBC_HMAC_SHA_512.P,
                    TestId = "AES_256_CBC_HMAC_SHA_512"
                });

                return theoryData;
            }
        }

        public class AuthenticationEncryptionTestParams
        {
            public string Algorithm { get; set; }
            public byte[] AuthenticationData { get; set; }
            public byte[] AuthenticationTag { get; set; }
            public byte[] Ciphertext { get; set; }
            public SecurityKey DecryptionKey { get; set; }
            public SecurityKey EncryptionKey { get; set; }
            public byte[] IV { get; set; }
            public byte[] Plaintext { get; set; }
            public string TestId { get; set; }

            public override string ToString()
            {
                return TestId + ", " + Algorithm + ", " + EncryptionKey.KeyId + ", " + DecryptionKey.KeyId;
            }
        }
    }
}
