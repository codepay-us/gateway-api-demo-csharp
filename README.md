# C# sample usage instructions

The sample is suitable for C# programming language and can call all OpenAPI interfaces. Sample encapsulates the process of executing integration, including adding and verifying signatures that call the payment gateway API. This is just a example, and we do not guarantee that the code will be applicable to any programming environment. It only demonstrates how to call our API. You can refer to this code to write your own program.

## Condition

Suitable for various Microsoft frameworks that comply with the. Net Standard 2.0 specification (such as. Net Framework>=4.6.1,. Net Core>=2.0, etc.).

## Download address

-   <a href="https://github.com/codepay-us/gateway-api-demo-csharp" target="_blank">
        GitHub project home page
    </a>

## Usage steps

### 1. Add dependencies and use tools such as NuGet to download the following package

```xml
<ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
  </ItemGroup>
```

Newtonsoft.Json parses and processes JSON packets.

### 2. Refer to the GitHub example code and write your program. The following is the main code

```C#
    // 1. Global parameter settings
    string appRsaPrivateKeyPem = "<YOUR APP RSA PRIVATE KEY>";
    string gatewayRsaPublicKeyPem = "<YOUR GATEWAY RSA PUBLIC KEY>";
    string gatewayUrl = "<YOUR GATEWAY URL>";
    string appId = "<YOUR APP ID>";

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
    parameters.Add("merchant_no", "312100000164");
    parameters.Add("merchant_order_no", "TEST_1685946062143");

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
```
