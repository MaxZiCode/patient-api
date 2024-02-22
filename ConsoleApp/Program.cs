using PatientApi.Actions.Create;
using PatientApi.Constants;

using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

using System.Net.Http.Json;
using System.Text.Json;

var boolRandomizer = RandomizerFactory.GetRandomizer(new FieldOptionsBoolean());
var textRandomizer = RandomizerFactory.GetRandomizer(new FieldOptionsTextWords() { Max = 1 });
var dateTimeRandomizer = RandomizerFactory.GetRandomizer(new FieldOptionsDateTime() { From = DateTime.Now.AddYears(-1), To = DateTime.Now.AddYears(1) });
var firstNameRandomizer = RandomizerFactory.GetRandomizer(new FieldOptionsFirstName());
var lastNameRandomizer = RandomizerFactory.GetRandomizer(new FieldOptionsLastName());
var genderRandomizer = RandomizerFactory.GetRandomizer(new FieldOptionsInteger() { Min = 0, Max = 3 });

using HttpClient httpClient = new HttpClient();
for (int i = 0; i < 100; i++)
{
    using HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5010/patient");
    CreatePatientDto createPatientDto = new CreatePatientDto()
    {
        Name = new CreatePatientNameDto()
        {
            Use = textRandomizer.Generate(),
            Family = lastNameRandomizer.Generate()!,
            Given = new List<string>() { firstNameRandomizer.Generate()!, firstNameRandomizer.Generate()! }
        },
        Gender = (Gender)genderRandomizer.Generate()!,
        BirthDate = dateTimeRandomizer.Generate()!.Value,
        Active = boolRandomizer.Generate()!.Value
    };
    message.Content = JsonContent.Create(createPatientDto);

    HttpResponseMessage response;
    try
    {
        response = httpClient.Send(message);
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine(ex.Message);
        continue;
    }

    Stream responseContentStream = response.Content.ReadAsStream();
    using TextReader responseContentReader = new StreamReader(responseContentStream);
    string responseContent = responseContentReader.ReadToEnd();

    var jsonContent = JsonSerializer.Deserialize<JsonElement>(responseContent);
    string prettyResponseContent = JsonSerializer.Serialize(jsonContent, new JsonSerializerOptions()
    {
        WriteIndented = true
    });

    Console.WriteLine($"StatusCode: {(int)response.StatusCode}\n{prettyResponseContent}");
}

