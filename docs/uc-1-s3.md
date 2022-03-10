```gherkin
    Use Case: Wield Mjolnir
    
    As a user
    I want my hero to try and wield Mjolnir
    So that I can see if they are worthy

    Scenario: Hero attempts to wield Mjolnir without an Asgard pass
        Given I have created the hero <hero> 
        When my hero attempts to wield Mjolnir
		Then my hero should be unsuccessful and be banished from Asgard
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