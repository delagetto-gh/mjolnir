Feature: Wield Mjolnir
	As a superhero
	I want to try and wield Mjolnir
	So that I can prove my worthiness

	Scenario Outline: Worthy hero attemps to wield Mjolnir
		Given the hero <worthyHero> has been created
		And the hero has travelled via the Bifrost and obtained an Asgard pass
		When the hero attempts to wield Mjolnir
		Then they should be successful and deemed worthy
		Examples:
			| worthyHero      |
			| Thor            |
			| Captain America |
			| Black Panther   |
			| Loki            |
			| Vision          |
			| Superman        |


	Scenario: Unworthy hero attemps to wield Mjolnir
		Given the hero <hero> has been created
		And the hero has travelled via the Bifrost and obtained an Asgard pass
		When the hero attempts to wield Mjolnir
		Then they should be unsuccessful and deemed unworthy
		Examples:
			| hero               |
			| Captain Underpants |
			| One Punch Man      |
			| Gambit             |
			| Spiderman          |
			| Cyclops            |

	Scenario: A hero attempts to wield Mjolnir without an Asgard pass
		Given the hero <hero> has been created
		And the hero did not travel via the Bifrost
		When the hero attempts to wield Mjolnir
		Then they should be unsuccessful and banished from Asgard
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
