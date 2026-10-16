// dotnet add package Azure.AI.Projects --version 2.1.0-beta.4
// dotnet add package Azure.Identity --version 1.21.0
using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using Azure.Identity;
using OpenAI.Responses;

#pragma warning disable OPENAI001

const string endpoint = "https://aatishfoundry2026.services.ai.azure.com/api/projects/proj-default";
const string model = "gpt-6-sol";
const string instructions = "You are a helpful AI assistant that answers questions clearly and concisely.";

DefaultAzureCredential credential = new(new DefaultAzureCredentialOptions
{
    ExcludeManagedIdentityCredential = true,
    ExcludeWorkloadIdentityCredential = true,
});

// Create a project client with the endpoint and credential.
// Authentication and endpoint go through AIProjectClient
AIProjectClient projectClient = new(
    endpoint: new Uri(endpoint),
    tokenProvider: credential);

// Get a responses client for the model.
ProjectResponsesClient responseClient =
    projectClient.ProjectOpenAIClient.GetProjectResponsesClientForModel(model);

string? lastResponseId = null;

Console.WriteLine("Assistant: Enter a prompt (or type 'quit' to exit)");
while (true)
{
    // Loop until the user quits.
    Console.Write("\nYou: ");
    string? inputText = Console.ReadLine();
    if (inputText is null || inputText.Equals("quit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Assistant: Goodbye!");
        break;
    }

    // Send message to the model and create response options.
    CreateResponseOptions options = new()
    {
        Model = model,
        Instructions = instructions,
        PreviousResponseId = lastResponseId,
        InputItems = { ResponseItem.CreateUserMessageItem(inputText) },
    };

    // Create response.
    ResponseResult response = responseClient.CreateResponse(options);

    // Print the response.
    Console.WriteLine($"\nAssistant: {response.GetOutputText()}");

    // Set the last response id.
    lastResponseId = response.Id;
}
