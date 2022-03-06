## Mjölnir

Small application developed for the purposes of learning:

- [x] Api Gateway (Ocelot)
- [x] JWT authentication (Claims, Issuer, Key Signing, Refresh Tokens, Best Practices)
- [x] Entity Framework Core (Migrations)

> ### Overview
- The aim of the game is to be able to lift Mjölnir. Mjölnir is the hammer of the thunder god Thor in Norse mythology. It is enchanted, meaning it can only be wielded by those who are deemed "worthy".

- If you follow the MCU, then you will know that - apart from Thor himself, only a select few characters were also deemed "worthy" and thus could wield Mjölnir.

You will attempt to wield Mjolnir. You do this by first registering yourself as a superhero, then logging in.
After then, you may try to "lift" Mjölnir by calling `GET /mjolnir` - if indeed you are worthy, you will recieve a `200 (WORTHY)`, otherwise, if you are unworthy, you will recieve a `401 (UNWORTHY)`.

> #### Steps:

1. Create your superhero at `POST /heroes`
2. Inscribe (Login) your superhero at `POST /inscriptions`
3. Attempt to wield Mjolnir at `GET /mjolnir`

> #### Application Architecture:

Bifrost.Api - theburning rainbow bridge that reaches between Midgard (Earth) and Asgard, the realm of the gods. This is the API Gateway to out application. As an API Gateway it will be responsible for 
1) Protecting our API's - implements authentication
2) Orchestrating the requests to the right services

Exposes `POST /` - enter your hero the book of records of challengers to wield Mjölnir. Upon inscription, you will recieve your Bifrost Pass (JWT) which shall allow you enter Asgard to attempt to wield Mjölnir.


Heroes.Api - In essence our 'users' service. Shall store (EF Core + SQLite) all the heroes  that have come to try and wield Mjölnir. 
             Exposes root `POST /` endpoint to register a superhero so they can enter the Bifrost and attempt to wield Mjölnir.

Mjölnir.Api - The hammer that our heroes wish to yield. Exposes only one root `GET /` endpoint. 