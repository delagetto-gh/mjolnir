## Mjölnir :zap::hammer::zap:

Small application developed for the purposes of learning:

- [x] Api Gateway (Ocelot)
- [x] JWT authentication (Claims, Issuer, Key Signing, Refresh Tokens, Best Practices)
- [x] Entity Framework Core (Migrations)

### Overview

The aim of the game is to be able to wield Mjölnir. According to Norse mythology, Mjölnir is the hammer ofthe thunder god Thor. It is enchanted, meaning it can only be wielded by those who are deemed _"worthy"_.  

If you follow the Marvel Comic Universe, then you will know that - apart from Thor himself, only a select few characters were also deemed worthy and thus could wield Mjölnir. 

You will attempt to wield Mjolnir. You do this by first creating a hero. This hero must then enter the Bifrost to get to Ásgard, where they can attempt to wield Mjolnir.

When your hero tries to wield Mjolnir, if they are indeed worthy, you will recieve a `200 (Worthy)`, however if they are unworthy, you will recieve a `403 (Unworhy)`.

If your hero tries to sneak into Ásgard without having taken the Bifrost, then your hero shall be banished back to Midgard `401 (Banished)`.

### Steps:

1. Create your hero `POST {baseUrl}/heroes`  
2. Enter the Bifrost with your hero `POST {baseUrl}/bifrost`  
2.1 In response you will receive your Ásgardian pass.
3. Attempt to wield Mjolnir at `GET {baseUrl}/mjolnir` with your Ásgardian pass add `Authorization: Bearer <yourAsgardianPass>` to the request headers.

### Architecture:

#### Bifrost.Api:  
> _The burning rainbow bridge that reaches between Midgard (Earth) and Asgard, the realm of the gods._

As such, this seemed like a perfect fit as the API Gateway to our application (_...and Ásgard_). It will be responsible for: 
* Protecting our APIs - implements AuthN/AuthZ via JWT
* Routing the requests to the right APIs (Ocelot)

The API iself exposes a`POST /` endpoint to allow heros to enter their credentials. In response, you will recieve your Ásgard Pass (JWT) which shall allow you to attempt to wield Mjölnir.

As the service also acts as our API Gateway it exposes and maps the following routes:

| Upstream Endpoint     |   Downstream Endpoint |
| ------------- | ------------- |
| `POST /heroes`   | `POST heroes-api/heroes`  |
| `POST /bifrost`  | `POST bifrost-api/inscribe`  |
| `GET /wield`     | `GET mjolnir-api/mjolnir`   |

#### Heroes.Api:
In essence our 'users' service. Shall store (EF Core + SQLite) all the heroes that have come to try and wield Mjölnir. 

Exposes a single `POST /` endpoint to register a hero so they can enter the Bifrost and attempt to wield Mjölnir.

#### Mjölnir.Api:
The hammer that our heroes wish to yield. Exposes only one root `GET /` endpoint. 


### Notes:   
#### Acceptance Tests  

Solution was build using .NET Core in VSCode - as Specflow doesnt yet currently have an official extensinof or VSCode, the next best option is to use their [.NET Core Template](https://www.nuget.org/packages/SpecFlow.Templates.DotNet) and the Cucumber VsCode extension.