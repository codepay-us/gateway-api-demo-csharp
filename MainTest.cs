using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace APITestDemo
{
    class MainTest
    {
        static void Main(string[] args)
        {
            // 1. Global parameter settings
            string appRsaPrivateKeyPem = "MIIEowIBAAKCAQEAkkZX3ziS8Zq2ksiYcNHobuohWieC8nyMF6PQ5O5SBZbMR7YX15NjyvOkqWoK4C450r6Oi4w2UyKFpRDO2qRF7EpyVLLRJhfC0uiXw302lz7xeuw2U9uODEUjde0f05JeGyU79O9xDd/OHvO4DcoYtTdDfH2OfuZ8686nFsbl768IYk51jfVi5txg7e+R8OR41GULVCV97Dz0PfHlqA835xvu9mF2yDYQBS2ehbP59dWoC+NwYQ+/kTSoRzqiTdlIUbU79KWlaQVchh79adEZx1lEPn4iBIt+i1uCrjsfPLIT2A3hT/20uGFHKJroVsWO9zA+vvh69u3ur6T6l8lc8QIDAQABAoIBABNa+4WuFsOhlUcXBBTpsbf7gy5KzCkKEf+OzbV9U50ptx5GGiGMf0f7tW41efrwIvagAHy2sPmPN9//uV91HUuHDlnmz4Ya3szJuktD5lVdHtcFKqsuAj3daDPSjMOSOqGc67IdfPg5BS1TUqeAdoSEK4ntk013clBBqBp7dzGf48yrWop+XcmBLkET32w9UG52/jSPrJs5XLNw/xZF6Z9yA0mOgsfr0lI9lyZ5uoDIdDl2tARtLpINU0b1rTaoPGol+3zCOGMWQ75+i5984yyEHScNlDOM8kc1oUC1p0NbRFizKAki9+Ky7fNW4FCST+7kJgTAH1Xm0F7V3ZMHggECgYEA4W5Fu9lxEWUAHpA72d7R+T1r+CH7oSdysbVt1i/D1GwSKhKPULPDts1eulwK8KJZw8R6Xy6IKbypq6CJDEFmccVB5InY9WyrhEsnm0AVzOwTajVC3cpUfR8WzuOIWe6zAcAtbwfDxoCpHaMG80pyOtKigvwDYg7vfhFClagT9SUCgYEAphw06TzBeeUUPKGnbH7NoyOBrNUtm0QkPW6bITQh07xZCfrpZpk2D5exVHS75ok93o3vwQvDe2cVsWzORlV/gAr2ezuif6bmZVCgLVIZr8Ri2bTXRhVW85pSn0twaO907KmahUjYmfn/dsxIv+2L/iTnzXJ7HeyJHkR1ZKx9DN0CgYAvqiJCetJmbCWfUL3m7i8VdQA8QeszguTEYGkt7YGJi6Q3kx8MYEYUg83wt390q88xDn0VXQBbWtHBQTtZBQcFLUEmcmMWWXjWixF/yQgTASOFxGc0ABDnN8iZzBBLe6YLy7ePj2O8t/2KD6tri6UlfN3xthl9BU1sKXgbrqMxpQKBgF9YvX5Lo3rGZVFQMPvz5TGJRfvg+aav/GVOwbjTZb9V034JCkQcGY3lsqIZx2lSybKfokka6YBB8Y/ANr7kgKUMpeKinmnLcWAiYW7iO2BKx7rTNOZDhX+ay2YoNUPmfyUBTXLIF5x6hYq5Q+D3B24/3NZuqgDJLwOyo/e/kp/5AoGBAN3US6TPfTU513gJGwtV6SFAgu+BDvWkstWcbo0oLw3h8jRVisDafKRinGrN6ls0ByzFeKUh5GDuw9/fqZg3HXAwqRmTk4J1TTTklgE1zRYLyPAMX5CdJvJDRkk+K8Ihu8hdPGmakhI8nlnj+1xk+ieUxAdUAUge8/yhashOS+ZI";
            string gatewayRsaPublicKeyPem = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAl838XBDyDxsR3ChhYPKTHo8pevqDb9TpjjnaeNpSOWvCsIABH3VWxYYErWI167oA1C/QukoBWKxK8TR4bz5doJQm0BKJ99fPK3Zx/H7f++UBjd97rG87bPFq1Q3pJs9BRqiHJwdZOegWwwfq3OPjD2m+sntyOcz2p6AhQ38l5q0bOPYjMWZ2k61e5x+CbJdfKxFCnICLzqe8A8oz1h1wGTdUNCZ/nyFZ2KwTUVwarPdgj6dJSV029dfjvcY9o3280nq+gKh5ZeAhpJSL0r0qk9mph7a9xY1oOi1jv3OGANIQLBOoN0GwibxUhmOJ3VnNv+9+e2D2ovO9i5TTw7TwiQIDAQAB";
            string gatewayUrl = "https://gw.codepay.us/api/entry";
            string appId = "wz6012822ca2f1as78";

            // 2. Set parameters
            Dictionary<string, string> parameters = new Dictionary<string, string>();
                // Common parameters
            parameters.Add("app_id", appId);
            parameters.Add("charset", "UTF-8");
            parameters.Add("format", "JSON");
            parameters.Add("sign_type", "RSA2");
            parameters.Add("version", "1.0");
            parameters.Add("timestamp", "" + DateTimeOffset.Now.ToUnixTimeMilliseconds());
            parameters.Add("method", "order.query");
                // API owned parameters
            parameters.Add("merchant_no", "312300000969");
            parameters.Add("merchant_order_no", "mapp1701763963536");

            // 3. Build a string to be signed
            string stringToBeSigned = buildToBeSignString(parameters);
            Console.WriteLine("StringToBeSigned : {0} \n", stringToBeSigned);

            // 4. Calculate signature
            string sign = GenerateSign(stringToBeSigned, appRsaPrivateKeyPem);
            parameters.Add("sign", sign);

            // 5. Send HTTP request
            string jsonString = JsonConvert.SerializeObject(parameters);
            
            Console.WriteLine("Request to gateway[" + gatewayUrl + "] send data  -->> " + jsonString + "\n");

            var responseStr = "";
            try{
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "CodePay Gateway API C# client");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = httpClient.PostAsync(gatewayUrl, new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode){
                    responseStr = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("Response from gateway[" + gatewayUrl + "] receive data <<-- " + responseStr + "\n");
                }
                else{
                    Console.WriteLine("Request to gateway[" + gatewayUrl + "] failed: " + response);
                    return;
                }
            }catch (Exception ex){
                Console.WriteLine("Request exception: " + ex.Message);
            }

            // 6. Verify the signature of the response message
            Dictionary<string, string> respParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseStr);
            string respStringToBeSigned = buildToBeSignString(respParameters);
            Console.WriteLine("RespStringToBeSigned : {0} \n", respStringToBeSigned);
            string respSignature = respParameters["sign"];
            bool verified = VerifySignature(respStringToBeSigned, respSignature, gatewayRsaPublicKeyPem);
            Console.WriteLine("SignVerifyResult : {0}", verified);

        }

        //Calculate signature
        private static string GenerateSign(string content, string privateKeyPem)
        {
            try
            {
                using (RSACryptoServiceProvider rsaService = BuildRSAServiceProvider(Convert.FromBase64String(privateKeyPem)))
                {
                    byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(content);
                    byte[] sign = rsaService.SignData(data, "SHA256");
                    return Convert.ToBase64String(sign);
                }
            }
            catch (Exception e)
            {
                string errorMessage = "Signature encountered an exception. Please check if the private key format is correct.content=" + content + " privateKeySize=" + privateKeyPem.Length + " reason=" + e.Message;
                throw new Exception(errorMessage);
            }
        }

        // verify signature
        public static bool VerifySignature(string content, string sign, string publicKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsaService = new RSACryptoServiceProvider())
                {
                    rsaService.PersistKeyInCsp = false;
                    rsaService.ImportParameters(ConvertFromPemPublicKey(publicKey));
                    return rsaService.VerifyData(Encoding.GetEncoding("UTF-8").GetBytes(content),
                        "SHA256", Convert.FromBase64String(sign));
                }
            }
            catch (Exception e)
            {
                string errorMessage = "The signature verification encountered an exception. Please check if the public key format or signature is correct. content=" + content + " sign=" + sign +
                                      " publicKey=" + publicKey + " reason=" + e.Message;
                throw new Exception(errorMessage);
            }
        }

        // Generate public key objects for c # from public keys in PEM format
        private static RSAParameters ConvertFromPemPublicKey(string pemPublicKey)
        {
            if (string.IsNullOrEmpty(pemPublicKey))
            {
                throw new Exception("The PEM format public key cannot be empty.");
            }

            // Remove interfering text
            pemPublicKey = pemPublicKey.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\n", "").Replace("\r", "");

            byte[] keyData = Convert.FromBase64String(pemPublicKey);
            bool keySize2048 = (keyData.Length == 294);
            if (!keySize2048)
            {
                throw new Exception("The public key length only supports 2048.");
            }
            byte[] pemModulus = new byte[256];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 33, pemModulus, 0, 256);
            Array.Copy(keyData, 291, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters
            {
                Modulus = pemModulus,
                Exponent = pemPublicExponent
            };
            return para;
        }

        // Building an RSA Signature Provider Object
        private static RSACryptoServiceProvider BuildRSAServiceProvider(byte[] privateKey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;

            //set up stream to decode the asn.1 encoded RSA private key
            //wrap Memory Stream with BinaryReader for easy reading
            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(privateKey)))
            {
                twobytes = binaryReader.ReadUInt16();
                //data read as little endian order (actual data order for Sequence is 30 81)
                if (twobytes == 0x8130)
                {
                    //advance 1 byte
                    binaryReader.ReadByte();
                }
                else if (twobytes == 0x8230)
                {
                    //advance 2 bytes
                    binaryReader.ReadInt16();
                }
                else
                {
                    return null;
                }

                twobytes = binaryReader.ReadUInt16();
                //version number
                if (twobytes != 0x0102)
                {
                    return null;
                }
                bt = binaryReader.ReadByte();
                if (bt != 0x00)
                {
                    return null;
                }

                //all private key components are Integer sequences
                elems = GetIntegerSize(binaryReader);
                MODULUS = binaryReader.ReadBytes(elems);

                elems = GetIntegerSize(binaryReader);
                E = binaryReader.ReadBytes(elems);

                elems = GetIntegerSize(binaryReader);
                D = binaryReader.ReadBytes(elems);

                elems = GetIntegerSize(binaryReader);
                P = binaryReader.ReadBytes(elems);

                elems = GetIntegerSize(binaryReader);
                Q = binaryReader.ReadBytes(elems);

                elems = GetIntegerSize(binaryReader);
                DP = binaryReader.ReadBytes(elems);

                elems = GetIntegerSize(binaryReader);
                DQ = binaryReader.ReadBytes(elems);

                elems = GetIntegerSize(binaryReader);
                IQ = binaryReader.ReadBytes(elems);

                //create RSACryptoServiceProvider instance and initialize with public key
                RSACryptoServiceProvider rsaService = new RSACryptoServiceProvider();
                RSAParameters rsaParams = new RSAParameters
                {
                    Modulus = MODULUS,
                    Exponent = E,
                    D = D,
                    P = P,
                    Q = Q,
                    DP = DP,
                    DQ = DQ,
                    InverseQ = IQ
                };
                rsaService.ImportParameters(rsaParams);
                return rsaService;
            }
        }

        private static int GetIntegerSize(BinaryReader binaryReader)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;

            bt = binaryReader.ReadByte();

            //expect integer
            if (bt != 0x02)
            {
                return 0;
            }
            bt = binaryReader.ReadByte();

            if (bt == 0x81)
            {
                //data size in next byte
                count = binaryReader.ReadByte();
            }
            else if (bt == 0x82)
            {
                //data size in next 2 bytes
                highbyte = binaryReader.ReadByte();
                lowbyte = binaryReader.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                //we already have the data size
                count = bt;
            }
            while (binaryReader.ReadByte() == 0x00)
            {   //remove high order zeros in data
                count -= 1;
            }
            //last ReadByte wasn't a removed zero, so back up a byte
            binaryReader.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        // Build a string to be signed
        private static string buildToBeSignString(Dictionary<string, string> parameters)
        {
            IEnumerator<KeyValuePair<string, string>> enumerator = ((IEnumerable<KeyValuePair<string, string>>)new SortedDictionary<string, string>(parameters, StringComparer.Ordinal)).GetEnumerator();
            StringBuilder stringBuilder = new StringBuilder();
            while (enumerator.MoveNext())
            {
                string key = enumerator.Current.Key;
                string value = enumerator.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && key != "sign")
                {
                    stringBuilder.Append(key).Append("=").Append(value).Append("&");
                }
            }
            return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
        }
    }
}
