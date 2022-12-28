# discord-bot-ai-voice
so the drake voice on uberduck.ai is kinda funny so it would be nice to get recordings thru the terminal, or even a discord bot. uberduck.ai's api outputs a url to the voice model on their amazon s3 bucket. its easy enough to query their api and do stuff with it.

## usage: (TODO)
### discord
```--discord --perist=-1```

### command line
```--commandline --download 'c:/users/ben/downloads' --filename 'drake.mp3'

### gui
-gui

## todo
- credential storage model (one file  for uberduck.ai and discord auth token/keys/secrets/etc)
- rate-limiting service - to take into account ubderduck.ai's rate limiting
- command-line args
- refine discord bot
- logging (serilog)
- gui app (wpf?)
- account for using other voice models other than drake lol
