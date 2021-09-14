# Clockify NLP Bot

**Clockify NLP Bot** is a [Bot Framework](https://dev.botframework.com) bot developed to act as a timesheet assistant for the busy and the lazy among us.
It reminds proactively people to fill in their time when EOD approaches, and it accepts natural language for creating time entries directly from chat.

<img src="https://user-images.githubusercontent.com/11543564/133216495-aa6feb9d-c30e-45f4-aeed-0329e0f75585.png" width="700">

## How to use it
It's simple, just open a chat with the bot either on Teams or Telegram.

👉 [Chat on Telegram](http://t.me/clockify_telegram_bot)  
👉 [Chat on Teams](https://teams.microsoft.com/l/chat/0/0?users=28:d7dfef09-5ad7-4e1c-ac47-a3899bf1964c)


## Features:

**Proactive reminders**
It will assist you to complete your due diligence. If you have less than 6 hours 
reported on your Clocki (no matter how you put them in) it sends you a reminder.

**Adding time entries with ease**
Add your time flexibly, even indicating projects partially. For example, you can type
`time bot` instead of `time_survey_bot`. If it's not ambiguous, it'll figure it out.

**Smart task request**
Some projects benefit from tasks drill down, some don't. If you never use them on the project, 
it doesn't ask it in the first place.

## Architecture

This bot is hosted by [xtream](https://xtreamers.io/?utm_source=github&utm_medium=readme&utm_campaign=clockify_bot) on Azure Cloud.
This is what it looks like:

<img src="https://user-images.githubusercontent.com/11543564/133262156-140eaa1f-fbc0-4b60-a5c3-a17caf890406.png" width="1000">

## Support

This project was started as an internal tool for our employees, and then we decided to roll it out
to anyone who could benefit from it, with a few tweaks and adjustments. Hence, we'd do our best to work on any issue 
or bug but do consider we're maintaining it on our spare time.

If you'd like to see this project grow in features, richness and support, you can give a ⭐️ to let us know this is helpful. 
You can also buy us a coffee if you like.

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/R6R267IDP)

## Installing

If you want to host the bot yourself, you'll need to provide for the other components of the architecture on your own.  
The following are minimal instructions for installing and deploying the bot on App Service.

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) version 3.1

  ```bash
  # determine dotnet version
  dotnet --version
  ```

### To try this sample

- In a terminal, navigate to `Bot`

    ```bash
    # change into project folder
  cd Bot
    ```

- Run the bot from a terminal or from Visual Studio, choose option A or B.

  A) From a terminal

  ```bash
  # run the bot
  dotnet run
  ```

  B) Or from Visual Studio

  - Launch Visual Studio
  - File -> Open -> Project/Solution
  - Navigate to `Bot` folder
  - Select `Bot.csproj` file
  - Press `F5` to run the project

### Integration testing setup

- Create a Clockify account and generate an API key
- Create testing workspaces and a tag named `bot`
- Create a `test.runsettings` file with the following env variables

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
    <RunConfiguration>
        <EnvironmentVariables>
            <CLOCKIFY_API_KEY><!--API-KEY--></CLOCKIFY_API_KEY>
            <CLOCKIFY_WS_ID><!--ACTIVE-WORKSPACE-ID--></CLOCKIFY_WS_ID>
        </EnvironmentVariables>
    </RunConfiguration>
</RunSettings>
```
- Configure the Test Runner to use the custom settings

### Testing the bot using Bot Framework Emulator

[Bot Framework Emulator](https://github.com/microsoft/botframework-emulator) is a desktop application that allows bot developers to test and debug their bots on localhost or running remotely through a tunnel.

- Install the Bot Framework Emulator version 4.9.0 or greater from [here](https://github.com/Microsoft/BotFramework-Emulator/releases)

#### Connect to the bot using Bot Framework Emulator

- Launch Bot Framework Emulator
- File -> Open Bot
- Enter a Bot URL of `http://localhost:3978/api/messages`

### Deploy the bot to Azure

To learn more about deploying a bot to Azure, see [Deploy your bot to Azure](https://aka.ms/azuredeployment) for a complete list of deployment instructions.

## Further reading

- [Bot Framework Documentation](https://docs.botframework.com)
- [Bot Basics](https://docs.microsoft.com/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0)
- [Activity processing](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-activity-processing?view=azure-bot-service-4.0)
- [Azure Bot Service Introduction](https://docs.microsoft.com/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
- [Azure Bot Service Documentation](https://docs.microsoft.com/azure/bot-service/?view=azure-bot-service-4.0)
- [.NET Core CLI tools](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
- [Azure CLI](https://docs.microsoft.com/cli/azure/?view=azure-cli-latest)
- [Azure Portal](https://portal.azure.com)
- [Language Understanding using LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)

