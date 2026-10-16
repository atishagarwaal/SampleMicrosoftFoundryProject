// dotnet add package Azure.AI.Projects --version 2.1.0-beta.4
// dotnet add package Azure.Identity --version 1.21.0
using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using Azure.Identity;
using OpenAI;
using OpenAI.Responses;

#pragma warning disable OPENAI001

const string endpoint = "https://YOUR-FOUNDRY-RESOURCE.services.ai.azure.com/api/projects/YOUR-PROJECT";
const string agentName = "your-agent-name";
const string userMessage = "Hi";

// Skip managed identity on this machine; that probe blocks for about two minutes
// and then fails the whole chain before Visual Studio or Azure CLI can sign in.
DefaultAzureCredential credential = new(new DefaultAzureCredentialOptions
{
    ExcludeManagedIdentityCredential = true,
    ExcludeWorkloadIdentityCredential = true,
});

// AIProjectClient is created with the Foundry project endpoint.
AIProjectClient projectClient = new(
    endpoint: new Uri(endpoint),
    tokenProvider: credential);

// Get an agent-scoped Responses client from the project's OpenAI client.
// Chat/responses go through the OpenAI-style client obtained from that project client.
ProjectResponsesClient agentClient =
    projectClient.ProjectOpenAIClient.GetProjectResponsesClientForAgentEndpoint(agentName);

// Reuse the agent-scoped pipeline for conversations.
ProjectConversationsClient conversationClient = new(
    agentClient.Pipeline,
    new OpenAIClientOptions { Endpoint = agentClient.Endpoint });

ProjectConversation conversation = conversationClient.CreateProjectConversation();

// Create a response on the conversation for this agent.
ProjectResponsesClient responseClient =
    projectClient.ProjectOpenAIClient.GetProjectResponsesClientForAgentEndpoint(
        agentName,
        defaultConversationId: conversation.Id);

ResponseResult response = responseClient.CreateResponse(userMessage);

Console.WriteLine(response.GetOutputText());
