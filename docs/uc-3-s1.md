```gherkin
    Use Case: Register Hero
    
    As a user
    I want be able to register my hero
    So that my hero can try to wield Mjolnir

    Scenario: Hero with same name already exists
        Given a hero exists with the name <heroName>
        When I try to register a hero with the same name
        Then I should recieve an error indicating that my hero's name is already taken
        Examples:
 			| heroName           |
			| Thor               |
			| Captain America    |
			| Black Panther      |
			| Loki               |
			| Vision             |
			| Superman           |
			| Captain Underpants |
			| One Punch Man      |
			| Gambit             |
			| Spiderman          |
			| Cyclops            |
```


