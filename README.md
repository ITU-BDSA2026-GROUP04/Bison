# Bison

## How to run the program

### 1: Start the webserver database
Change the directory to Bison.CSVDBService

```bash
dotnet run
```

### 2: Use the client

#### Read current observations
```bash
dotnet run read
```

#### Make an observation
`<Message>` is the message you want to write in your observation
`<Location>` is the location where you saw the observation, the location has to be in one word
```bash
dotnet run observe <Message> <Location>
```

#### Read current comments
`<ObservationId>` is the ID of the observation where you want to see the comments

```bash
dotnet run discussion <ObservationId>
```

#### Make a comment
`<Message>` is the message you want to write in your comment, it has to have " before and after.
`<ObservationId>` is the ID of the observation you want to comment on

```bash
dotnet run comment "<Message>" <ObservationId>
```

#### Read observations with a specific location
`<Location>` is the location where you want to see observations from

```bash
dotnet run location <Location>
```

