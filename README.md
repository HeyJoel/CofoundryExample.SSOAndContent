# CofoundryExample.SSOAndContent

This sample shows how you can use an external auth provider and manage simple content using a **SimpleContent** custom entity containing a single Html property in the data model.

#### To get started:

1. Create a database named 'CofoundryExample.SSOAndContent' and check the Cofoundry connection string in appsettings.json file is correct for your sql server instance
2. Run the website and navigate to *"/admin"*, which will display the setup screen
3. Enter an application name and setup your user account. Submit the form to complete the site setup. 
4. Either log in and enter your content or try out the SSO sign in at */auth/signin/*. The homepage uses `AuthorizeUserAreaAttribute` so you should be redirected there anyway.

#### Editing Content

- Navigate to the admin panel at *"/admin"* and sign in
- You can edit your content on the *Content > Simple Content* node. 
- This example assumes you're not interesting in using features like *Pages*, so it has been removed from the admin GUI. 

#### Notes

- No actual auth is used here, but I've added TODO comments in `MemberSignInService` where you'd plug this in
- Alternatively to avoid having to do any of the Cofoundry user integration, you could go without installing the Cofoundry.Admin package altogether and host content administration in a separate website. This can also be a helpful scenario if you don't want the back-office running on your internet-facing website.

