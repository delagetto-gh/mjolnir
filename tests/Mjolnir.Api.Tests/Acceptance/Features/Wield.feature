Feature: Wield Mjolnir
	As a superhero
	I want to try and wield Mjolnir
	So that I can prove my worthiness

	Scenario Outline: Worthy hero attemps to wield Mjolnir
		Given the hero <heroName> has been created
		And the hero is <worthiness>
		And the hero has obtained their Asgard pass
		When the hero attempts to wield Mjolnir
		Then they should be successful and be deemed worthy
		Examples:
			| heroName        | worthiness |
			| Thor            | Worthy     |
			| Captain America | Worthy     |
			| Black Panther   | Worthy     |
			| Loki            | Worthy     |
			| Vision          | Worthy     |
			| Superman        | Worthy     |


	Scenario Outline: Unworthy hero attemps to wield Mjolnir
		Given the hero <heroName> has been created
		And the hero is <worthiness>
		And the hero has obtained their Asgard pass
		When the hero attempts to wield Mjolnir
		Then they should be unsuccessful and be deemed unworthy
		Examples:
			| heroName           | worthiness |
			| Captain Underpants | Unworthy   |
			| One Punch Man      | Unworthy   |
			| Gambit             | Unworthy   |
			| Spiderman          | Unworthy   |
			| Cyclops            | Unworthy   |

# Scenario Outline: A hero attempts to wield Mjolnir without an Asgard pass
# 	Given the hero <heroName> has been created
# 	And the hero is <worthiness>
# 	But the hero does not have an Asgard pass
# 	When the hero attempts to wield Mjolnir
# 	Then they should be unsuccessful and be banished from Asgard
# 	Examples:
# 		| heroName           | worthiness |
# 		| Thor               | Worthy     |
# 		| Captain America    | Worthy     |
# 		| Black Panther      | Worthy     |
# 		| Loki               | Worthy     |
# 		| Vision             | Worthy     |
# 		| Superman           | Worthy     |
# 		| Captain Underpants | Unworthy   |
# 		| One Punch Man      | Unworthy   |
# 		| Gambit             | Unworthy   |
# 		| Spiderman          | Unworthy   |
# 		| Cyclops            | Unworthy   |
