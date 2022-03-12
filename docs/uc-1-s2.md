```gherkin
    Use Case: Wield Mjolnir
    
    As a user
    I want my hero to try and wield Mjolnir
    So that I can see if they are worthy

    Scenario: Unworthy hero attempts to wield Mjolnir
        Given I have created the hero <unworthyHero> 
        And my hero has been issued an Asgard pass
        When my hero attempts to wield Mjolnir
        Then my hero should be unsuccessful and be deemed unworthy
        Examples:
 			| unworthyHero       |
			| Captain Underpants |
			| One Punch Man      |
			| Gambit             |
			| Spiderman          |
			| Cyclops            |
```


