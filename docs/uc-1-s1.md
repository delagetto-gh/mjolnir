```gherkin
    Use Case: Wield Mjolnir
    
    As a user
    I want my hero to try and wield Mjolnir
    So that I can see if they are worthy

    Scenario: Worthy hero attempts to wield Mjolnir
        Given I have registered the hero <worthyHero> 
        And my hero has been issued an Asgard pass
        When my hero attempts to wield Mjolnir
        Then they should be successful and be deemed worthy
        Examples:
            | worthyHero      |
            | Thor            |
            | Captain America |
            | Black Panther   |
            | Loki            |
            | Vision          |
            | Superman        |
```


