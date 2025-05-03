# Introduction

Core classes and interfaces that represent common objects in our system

Core Interfaces

```CSharp
ITeam
IMatch
IOverUnder
IPointSpread
IPrediction
IPredictionPattern
```

Extended Interfaces

## American Football

Because we specify a match by a week number, lets make this more specific in this example

```CSharp
IAmericanFootballMatch : IMatch
```
