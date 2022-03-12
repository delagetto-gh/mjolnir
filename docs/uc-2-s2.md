```gherkin
    Use Case: Obtain Asgard Pass (AP)
    
    As a user
    I want to obtain an Asgard Pass
    So that my hero can try to wield Mjolnir

    Scenario: Incorrect hero credentials supplied
        Given I have created the hero <hero> 
        When I request for an Asgard pass for my hero with incorrect credentials
        Then I should not be issued an Asgard Pass for my hero
        Examples:
 			| hero               |
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


