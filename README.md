# Sample Microsoft Foundry Project

## 1. Overview
This repository contains a .NET console application that connects to an Azure AI Foundry project and uses a deployed model to generate responses through the Azure AI Projects SDK.

The sample demonstrates how to:
- authenticate to Azure using `DefaultAzureCredential`
- connect to an Azure AI Foundry endpoint
- create a project-based OpenAI responses client
- send prompts and print model responses in a simple command-line loop

## 2. Description
This project is a minimal sample for working with Microsoft Azure AI Foundry in a .NET application. It uses the `Azure.AI.Projects` and `Azure.Identity` libraries to authenticate and interact with a project-backed model endpoint.

The app reads user input from the terminal, sends it to the configured model, and prints the generated response. It is intended as a starting point for developers building AI-powered .NET applications on Azure AI Foundry.

The key configuration values are defined in `SampleMicrosoftFoundryProject/Program.cs`:
- `endpoint`: Azure AI Foundry project endpoint
- `model`: deployed model name
- `instructions`: system instructions for the assistant

### SDKs Used
This project uses the Microsoft Foundry SDK (Azure.AI.Projects) — the Azure/Azure AI Foundry SDK for .NET — as the primary SDK. It is not using the raw OpenAI SDK directly.

It also uses `Azure.Identity` for authentication. The code imports some OpenAI-related types via `Azure.AI.Extensions.OpenAI` and `OpenAI.Responses`, but those are consumed through the Azure Foundry client surface (ProjectResponsesClient) rather than the standalone OpenAI SDK.

Relevant project lines:

From the project file (`SampleMicrosoftFoundryProject/SampleMicrosoftFoundryProject.csproj`):

```xml
<ItemGroup>
  <PackageReference Include="Azure.AI.Projects" Version="2.1.0-beta.4" />
  <PackageReference Include="Azure.Identity" Version="1.21.0" />
</ItemGroup>
```

From `SampleMicrosoftFoundryProject/Program.cs` (client construction):

```csharp
// Create a project client with the endpoint and credential.
AIProjectClient projectClient = new(
    endpoint: new Uri(endpoint),
    tokenProvider: credential);

// Get a responses client for the model.
ProjectResponsesClient responseClient =
    projectClient.ProjectOpenAIClient.GetProjectResponsesClientForModel(model);
```

The configured endpoint in the sample points to an Azure AI Foundry "services.ai.azure.com" endpoint, which confirms the Foundry integration.

## 3. Pre-requisites
Before running the project, make sure you have:
- .NET 10 SDK installed
- An Azure subscription
- An Azure AI Foundry project created in Azure
- A deployed model in your Foundry project
- Access to the project with a valid Azure identity

You should also sign in to Azure before running the app, for example:

```bash
az login
```

If you are using Visual Studio, Visual Studio Code, or another supported Azure credential source, those will also work with `DefaultAzureCredential`.

## 4. Build and Run
1. Clone the repository:

```bash
git clone https://github.com/atishagarwaal/SampleMicrosoftFoundryProject.git
cd SampleMicrosoftFoundryProject
```

2. Update the Azure AI Foundry configuration in `SampleMicrosoftFoundryProject/Program.cs`:
- set the correct `endpoint`
- set the correct `model`
- confirm the `instructions` match your scenario

3. Restore dependencies and build the project:

```bash
dotnet restore
dotnet build
```

4. Run the application:

```bash
dotnet run --project SampleMicrosoftFoundryProject/SampleMicrosoftFoundryProject.csproj
```

5. Enter a prompt in the terminal. Type `quit` to exit the app.

Example:

```text
Assistant: Enter a prompt (or type 'quit' to exit)

You: Hello!

Assistant: Hello! How can I help you today?
```

## Notes
This sample is intentionally simple and is designed to help you understand the basic flow for integrating Azure AI Foundry with a .NET application.
