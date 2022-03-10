## Mjölnir :zap::hammer::zap:

> Whosoever holds this hammer, if he be worthy, shall possess the power of Thor.  
> 
> --<cite>Mjölnir's enchantment</cite>  

Small application developed for the purposes of learning:

- [x] API Gateway (Nginx)
- [x] JWT Authentication (Claims, Signing, Secrets, Best Practices)
- [x] Entity Framework Core (Migrations, SQLite)

### Overview

The aim of the game is to be able to try and wield Mjölnir. According to Norse mythology, Mjölnir is the hammer of the thunder god Thor. It is enchanted, meaning it can only be wielded by those who are deemed _"worthy"_.  

If you follow the Marvel Comic Universe, then you will know that - apart from Thor himself, only a select few characters were also deemed worthy and thus could wield Mjölnir. 

Your hero will attempt to wield Mjolnir. You do this by first creating a hero. The hero must then call upon Heimdall, whom shall validate your hero's credentials and issue you your hero's Ásgard Pass (AP) as well as grant safe passage on the Bifrost to Ásgard - where then, your hero can attempt to wield Mjölnir.

When your hero tries to lift Mjölnir, if they are indeed worthy, you shall recieve a `200 (Worthy)`, and your hero thus becomes the beweilder to Mjölnir. However if they are unworthy, you shall recieve a `403 (Unworhy)`, and your doth not beweild.

If your hero tries to lift Mjölnir without their Ásgard Pass, then your hero will be banished to Midgard `401 (Banished)`. 

#### Steps:

1. Create your hero `POST /create-hero`  
2. Summon Heimdall to issue your hero's Ásgard pass (AP) `POST /summon-heimdall`  
3. Attempt to wield Mjolnir at `GET /weild-mjolnir` with your AP _(add `Authorization: AP <yourAsgardPass>` to the request headers)_.

### Design & Development:

#### The test list

> What should you test? Before you begin, write a list of all the tests you know you will have to write.
>
> --<cite>Kent Beck, "Test-Driven Development By Example"</cite>


| UC.1: Weild Mjölnir |                                            |                     |                                     |
| ------------------- | ------------------------------------------ | ------------------- | ----------------------------------- |
| #ID                 | Scenario                                   | Outcome             | User Story                          |
| UC.1.S1             | Hero has AP and is worthy                  | `return success`    | [See user story](./docs/uc-1-s1.md) |
| UC.1.S2             | Hero has AP but isn't worthy               | `return failure`    | [See user story](./docs/uc-1-s2.md) |
| UC.1.S3             | Hero does not have AP but is worthy        | `banish to Midgard` | [See user story](./docs/uc-1-s3.md) |
| UC.1.S4             | Hero does not have AP and is not is worthy | `banish to Midgard` | [See user story](here)              |


| UC-2: Summon Heimdall |                                |                   |                        |
| --------------------- | ------------------------------ | ----------------- | ---------------------- |
| #ID                   | Scenario                       | Outcome           | User Story             |
| UC-2.1                | Heimdall verifies hero         | `issue AP`        | [See user story](here) |
| UC-2.1                | Heimdall unable to verify hero | `do not issue AP` | [See user story](here) |

| UC-3: Create Hero |                                    |                                      |                        |
| ----------------- | ---------------------------------- | ------------------------------------ | ---------------------- |
| #ID               | Scenario                           | Outcome                              | User Story             |
| UC-3.1            | Hero with same name already exists | `error "hero name is already taken"` | [See user story](here) |

### Project Structure

##### Bifrost

> The rainbow bridge that connects Asgard, the world of the Aesir tribe of gods.

As such, this seemed like a perfect fit as the API Gateway to our application (_...and Ásgard_). It is responsible for: 
* Routing the requests to the right APIs (Nginx)

As the service also acts as our API Gateway it exposes and maps the following routes:

| Upstream Endpoint       | Downstream Endpoint    |
| ----------------------- | ---------------------- |
| `POST /create-hero`     | `POST multiverse-api/` |
| `POST /summon-heimdall` | `POST heimdall-api/`   |
| `GET /wield-mjolnir`    | `GET mjolnir-api/`     |

##### Heimdall 

> Heimdall is the sentry of the Bifrost, guard to Asgard.

It is responsible for: 
* Protecting our APIs - implementing AuthN/AuthZ & issuing JWTs. 

The API iself exposes a `POST /` endpoint to allow heroes to enter their credentials. In response, you will recieve your Ásgard Pass (AP), allowing you attempt to wield Mjölnir.

##### Multiverse
In essence our _'users'_ microservice. It shall store (EF Core + SQLite) all the heroes that have been created in the system. 

Exposes a `POST /` endpoint to create a hero.

##### Mjölnir
The hammer that our heroes wish to wield.   
Exposes a single `GET /` endpoint.   
If your hero is worthy, you will recieve a `200 (Worthy)`, however if they are unworthy, you will recieve a `403 (Unworhy)`.

### Notes
#### Acceptance Tests

The app was built using .NET Core in VSCode - as Specflow doesnt yet currently have officiall extension support for this IDE, the next best option is to use their [.NET Core Template](https://www.nuget.org/packages/SpecFlow.Templates.DotNet) and the Cucumber VSCode extension.