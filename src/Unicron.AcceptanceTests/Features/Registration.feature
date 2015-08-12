Feature: User can register
	As a website visitor
	I want to register a new user account
	In order to log in and start using the application

Background: 
	Given the feature is available in "development"

Scenario: Create User
	Given I have navigated to the registration page
	When I have entered a random email address into the email address field
	And I have filled in the rest of the required fields
	And I have clicked the register button
	Then I should see the confirmation screen
