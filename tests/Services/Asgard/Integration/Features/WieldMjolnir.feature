Feature: Wield Mjolnir
	As a user
	I want my hero to try and wield Mjolnir
	So that I see if they are worthy

	Scenario Outline: Worthy hero attempts to wield Mjolnir
		Given I have the hero <heroName>
		And I have my hero's AP
		And I create a GET request to /mjolnir
		And I add in the authorisation header Bearer <myHeroAP>
		When I make the request
		Then the response status code should be 200
		And the response reason phrase should be Worthy
		Examples:
			| heroName        |
			| Thor            |
			| Captain America |
			| Black Panther   |
			| Loki            |
			| Vision          |
			| Superman        |

	Scenario Outline: Unworthy hero attempts to wield Mjolnir
		Given I have the hero <heroName>
		And I have my hero's AP
		And I create a GET request to /mjolnir
		And I add in the authorisation header Bearer <myHeroAP>
		When I make the request
		Then the response status code should be 403
		And the response reason phrase should be Unworthy
		Examples:
			| heroName           |
			| Captain Underpants |
			| One Punch Man      |
			| Gambit             |
			| Spiderman          |
			| Cyclops            |

	Scenario Outline: Hero attempts to wield Mjolnir without an AP
		Given I have the hero <heroName>
		And I create a GET request to /mjolnir
		When I make the request
		Then the response status code should be 401
		And the response reason phrase should be Banished
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


