## Mjölnir :zap::hammer::zap:

Small application developed for the purposes of learning:

- [x] API Gateway (Ocelot)
- [x] JWT Authentication (Claims, Signing, Secrets, Best Practices)
- [x] Entity Framework Core (Migrations, SQLite)

### Overview

The aim of the game is to be able to try and wield Mjölnir. According to Norse mythology, Mjölnir is the hammer of the thunder god Thor. It is enchanted, meaning it can only be wielded by those who are deemed _"worthy"_.  

If you follow the Marvel Comic Universe, then you will know that - apart from Thor himself, only a select few characters were also deemed worthy and thus could wield Mjölnir. 

You will attempt to wield Mjolnir. You do this by first creating a hero. The hero must then summon Heimdall, who will validate your hero and issue your Ásgard Pass (AP), where then, your hero can attempt to wield Mjölnir.

When your hero tries to wield Mjölnir, if they are indeed worthy, you will recieve a `200 (Worthy)`, however if they are unworthy, you will recieve a `403 (Unworhy)`.

If you try to wield Mjölnir without an AP, then your hero will be banished to Midgard `401 (Banished)`.

#### Steps:

1. Create your hero `POST /heroes`  
2. Call upon Heimdall to issue your hero's Ásgard pass (AP) `POST /summon`  
3. Attempt to wield Mjolnir at `GET /wield` with your AP _(add `Authorization: Bearer <yourAsgardPass>` to the request headers)_.

### Design & Development:

#### The test list :ballot_box_with_check:  

> What should you test? Before you begin, write a list of all the tests you know you will have to write.
>
> --<cite>Kent Beck, "Test-Driven Development By Example"</cite>


| Lift Mjölnir                               |                     |
| ------------------------------------------ | ------------------- |
| Scenario                                   | Action              |
| Hero has AP and is worthy                  | `return success`    |
| Hero has AP but isn't worthy               | `return failure`    |
| Hero does not have AP but is worthy        | `banish to Midgard` |
| Hero does not have AP and is not is worthy | `banish to Midgard` |

| Summon Heimdall                |                   |
| ------------------------------ | ----------------- |
| Scenario                       | Action            |
| Heimdall verifies hero         | `issue AP`        |
| Heimdall unable to verify hero | `do not issue AP` |
|                                |                   |

| Create Hero                        |                                                |
| ---------------------------------- | ---------------------------------------------- |
| Scenario                           | Action                                         |
| Hero with same name already exists | `return error that hero name is already taken` |

#### Project Structure

##### Heimdall.Api:  

> Heimdall is the sentry of the Bifrost, guard to Asgard.

As such, this seemed like a perfect fit as the API Gateway to our application (_...and Ásgard_). It will be responsible for: 
* Protecting our APIs - implements AuthN/AuthZ via JWT
* Routing the requests to the right APIs (Ocelot)

The API iself exposes a `POST /summon` endpoint to allow heroes to enter their credentials. In response, you will recieve your Ásgard Pass (AP) which permits you to enter Ásgard so you can attempt to wield Mjölnir.

As the service also acts as our API Gateway it exposes and maps the following routes:

| Upstream Endpoint | Downstream Endpoint        |
| ----------------- | -------------------------- |
| `POST /heroes`    | `POST mcu-api/heroes`      |
| `POST /summon`    | `POST heimdall-api/summon` |
| `GET /wield`      | `GET mjolnir-api/wield`    |

##### Mcu.Api:
In essence our _'users'_ microservice. It shall store (EF Core + SQLite) all the heroes that have been created in the system. 

Exposes a single `POST /heroes` endpoint to create a hero.

##### Mjölnir.Api:
The hammer that our heroes wish to wield. Exposes a single `GET /wield` endpoint. If your hero is worthy, you will recieve a `200 (Worthy)`, however if they are unworthy, you will recieve a `403 (Unworhy)`.

#### Notes:   
#### Acceptance Tests notes:

The app was built using .NET Core in VSCode - as Specflow doesnt yet currently have officiall extension support for this IDE, the next best option is to use their [.NET Core Template](https://www.nuget.org/packages/SpecFlow.Templates.DotNet) and the Cucumber VSCode extension.