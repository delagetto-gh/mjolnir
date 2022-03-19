
Feature: Register Hero

	As a user
	I want be able to register a hero
	So that my hero can try to wield Mjolnir

	Scenario: Hero with same name already exists
		Given a hero exists with the name Bruce Wayne
		And I create a POST request to /heroes
		And I add the following hero registration details to the request body:
			| heroName    | password     |
			| Bruce Wayne | 1amTh3Bvtmvn |
		When I make the request
# Then the response status code should be 409
# And the response body should be
