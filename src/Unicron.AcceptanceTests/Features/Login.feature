Feature: User can log in
	As a website visitor
	I want to log into the application
	In order to start using it

Background: 
	Given the feature is available in "development"

Scenario: Successful log in
	Given I have navigated to the login page
	When I have entered "test@test.com" into the email address field
	And I have entered "password" into the password field
	And I have clicked the login button
	Then I should see the home screen

Scenario: Unsuccessful log in
	Given I have navigated to the login page
	When I have entered "bob@invalid.com" into the email address field
	And I have entered "invalid" into the password field
	And I have clicked the login button
	Then I should see the error "We're sorry, that is not a valid login, please try again."
