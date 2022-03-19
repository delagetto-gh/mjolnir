## Mjölnir :zap::hammer::zap:

> Whosoever holds this hammer, if he be worthy, shall possess the power of Thor.  
> 
> --<cite>Mjölnir's enchantment</cite>  

Small application developed for the purposes of learning:

- [x] API Gateway & Load Balancer (Nginx)
- [x] JWT Auth[Z|N]
  - [x] Issuing (IdentityServer4)
  - [x] Claims, Signing, Secrets
- [x] Entity Framework Core (Migrations, SQLite)

### Overview

The aim of the game is to be able to try and wield Mjölnir. According to Norse mythology, Mjölnir is the hammer of the thunder god Thor. It is enchanted, meaning it can only be wielded by those who are deemed _"worthy"_.  

If you follow the Marvel Comic Universe, then you will know that - apart from Thor himself, only a select few characters were also deemed worthy and thus could wield Mjölnir. 

Your hero will attempt to wield Mjolnir. You do this by first registering a hero. The hero must then call upon Heimdall, whom shall verify your hero and issue their Ásgard Pass (AP) - this will grant safe passage Ásgard - where then, your hero can attempt to wield Mjölnir.

When your hero attempts to lift Mjölnir, if they are indeed worthy, you shall recieve a `200 (Worthy)`, and your hero thus becomes the beweilder to Mjölnir. However if they are unworthy, you shall recieve a `403 (Unworhy)`, and your hero doth not beweild.

If your hero tries to lift Mjölnir without presenting their Ásgard Pass, then your hero will be banished to Midgard `401 (Banished)`. 

#### Steps:

1. Register your hero `POST /heroes`  
2. Summon Heimdall in order to obtain your hero's Ásgard Pass (AP) `POST /passes`  
3. Attempt to wield Mjolnir at `GET /mjolnir` with your AP _(add `Authorization: Bearer <yourAsgardPass>` to the request headers)_.

### Design & Development:

#### The test list

> What should you test? Before you begin, write a list of all the tests you know you will have to write.
>
> --<cite>Kent Beck, "Test-Driven Development By Example"</cite>


| UC.1: Weild Mjölnir |                                                  |                      |                                     |
| ------------------- | ------------------------------------------------ | -------------------- | ----------------------------------- |
| #ID                 | Scenario                                         | Outcome              | User Story                          |
| UC.1.S1             | Hero has AP and is worthy                        | `return success`     | [See user story](./docs/uc-1-s1.md) |
| UC.1.S2             | Hero has AP but isn't worthy                     | `return failure`     | [See user story](./docs/uc-1-s2.md) |
| UC.1.S3             | Hero (regardless of worthiness) does not have AP | `banish from Asgard` | [See user story](./docs/uc-1-s3.md) |


| UC-2: Obtain Asgard Pass (AP) |                                     |                    |                                     |
| ----------------------------- | ----------------------------------- | ------------------ | ----------------------------------- |
| #ID                           | Scenario                            | Outcome            | User Story                          |
| UC.2.S1                       | Correct hero credentials supplied   | `AP is issued`     | [See user story](./docs/uc-2-s1.md) |
| UC.2.S2                       | Incorrect hero credentials supplied | `AP is not issued` | [See user story](./docs/uc-2-s2.md) |

| UC-3: Register Hero |                                    |                                      |                                     |
| ----------------- | ---------------------------------- | ------------------------------------ | ----------------------------------- |
| #ID               | Scenario                           | Outcome                              | User Story                          |
| UC.3.S1           | Hero with same name already exists | `error "hero name is already taken"` | [See user story](./docs/uc-3-s1.md) |

### Project Structure

##### Bifrost

> The rainbow bridge that connects Asgard, the world of the Aesir tribe of gods.

As such, this seemed like a perfect fit as the API Gateway to our application. It is responsible for: 
* Load-Balancing requests to our downstream APIs (we shall be running _multiple instances_ of each microservice)
* Routing the requests to the right APIs (Nginx), specifically:

| Upstream Endpoint       | Downstream Endpoint        |
| ----------------------- | -------------------------- |
| `POST {baseUrl}/heroes` | `POST heimdall-api/heroes` |
| `POST {baseUrl}/passes` | `POST heimdall-api/passes` |
| `GET {baseUrl}/mjolnir` | `GET asgard-api-/mjolnir`  |

##### Heimdall 

> Heimdall is the ever-vigilant guardian of the gods’ stronghold, Asgard.

It is responsible for: 
* In essence our authentication server. It shall store (EF Core + SQLite) all the heroes that have been registered in the system. 
* Protecting our APIs - implementing AuthN/AuthZ & issuing JWTs (IdentityServer4) 

Exposes a `POST /heroes` endpoint to register a hero.

Exposes a `POST /passes` endpoint to obtain your Ásgard Pass (AP). You must enter your hero's correct credentials in order to recieve your AP. Your AP permits your hero to enter Asgard to attempt to wield Mjölnir.

##### Asgard

> Located in the sky, Asgard is one of the Nine Worlds of Norse mythology and the home and fortress of the Aesir, one of the tribes of gods.

This service is responsible for housing Mjölnir - the hammer (resource) that our heroes wish to wield.   

Exposes a single `GET /mjolnir` endpoint.   

If your hero is worthy, you will recieve a `200 (Worthy)`, however if they are unworthy, you will recieve a `403 (Unworhy)`.

### Notes
#### Acceptance Tests

The app was built using .NET Core in VSCode - as Specflow doesnt yet currently have officiall extension support for this IDE, the next best option is to use their [.NET Core Template](https://www.nuget.org/packages/SpecFlow.Templates.DotNet) and the Cucumber VSCode extension.