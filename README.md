# Event Sourcing & Idempotency Demo - CodeStock 2018

This is the code demo from my talk, Deliver at Warp Speed. You'll need to fill in your own AWS credentials to run it.

## Overview

This API represents a simple notion of a Rebate Submission. It can be created, decided, and released. We use the Command side of CQRS and event sourcing to persist events to a DynamoDb table, but we never persist the actual entity - we reconstruct it from events.

## Setup
Go to `Framework/CommandBroker.cs` and at the bottom, in `GetDynamoClient()`, fill in your AWS API key credentials.

Also create a DynamoDB table called `sandbox_codestock_commandstore` with the following configuration:
- A hash key called `entity_id` of type `String`
- A range/sort key called `command_number` of type `Number`

## Accompanying Slides

https://www.slideshare.net/rexamorgan/codestock-2018-deliver-at-warp-speed