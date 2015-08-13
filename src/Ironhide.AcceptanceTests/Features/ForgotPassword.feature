Feature: ForgotPassword
	In order to reset my password and regain access to my account
	As a user who has forgotten his password
	I want to receive an email with a link to reset my password

Scenario: Start Reset Password Process
	Given a website visitor
	And the forgot password page
	When enter my email address
	And click the submit button
	Then I should see a thank you screen
	And I should receive an email with a link to reset my password

Scenario: Reset Password
	Given a website visitor
	And I have requested to reset my password
	And I have clicked the link in the email to reset my password
	When I enter my new password twice
	And I click the reset password button
	Then I should see confirmation that I have successfully reset my password
	And I should see a prompt to log in with my new password

Scenario: Login After Reset Password
	Given a website visitor
	And I am on the login page
	When I enter my username and new password
	Then I should be taken to the home page
