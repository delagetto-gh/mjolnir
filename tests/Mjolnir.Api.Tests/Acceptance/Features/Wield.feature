Feature: Wield Mjolnir

	Allows a hero to attempt to wield Mjolnir.

	Scenario Outline: Worthy hero attemps to wield Mjolnir
		Given the worthy hero <hero>
		And the hero has a Bifrost pass
		When the hero attempts to wield Mjolnir
		Then they should be successful
		Examples:
			| hero            |
			| Captain America |
			| Black Panther   |
			| Odin            |

	Scenario: Unworthy hero attemps to wield Mjolnir
		Given the hero is unworthy
		And the hero has a Bifrost pass
		When the hero attempts to wield Mjolnir
		Then they should be unsuccessful
		Examples:
			| hero            |
			| Captain America |
			| Black Panther   |
			| Odin            |

	Scenario: Hero attempts to wield Mjolnir without a Bifrost pass
		Given the hero does not have a Bifrost pass
		When the hero attempts to wield Mjolnir
		Then they should be banished from the Asgard
		Examples:
			| hero            |
			| Captain America |
			| Black Panther   |
			| Odin            |
