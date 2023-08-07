# Pasatu - AD Password Changer

## What's that name?
It comes from "PASAhitza aldaTU": "Change password" in basque.

## Features
Allows to change a user's password in an Active Directory domain. Just that.

The reason for this is that I needed to implement local admin users for the staff, in order to remove admin permissions for them. My main problem was to include this password in the GPO rotation policies, as I didn't want the users to have to log in as the local admin just to change the password. Didn't find anything free or that didn't imply installing an IIS server to allow this, so I decided to program it myself.

## How?
The computer must be connected to a domain network. It must have network access to the domain controllers.

The password can only be changed in the domain in which the computer is connected. Does not allow to select the domain.

It respects password complexity rules established via GPO. Although I'm not an expert in AD security, the way it changes the password seems to respect this (see Credits below) and my own testings show this. Please feel free to show me if I'm mistaken, I DO care for security.

The application searches for all the domain controllers and starts trying to change the password:
 - If a 'you-can-try-the-next-dc error' arises, like network connectivity problems, it tries the next one
 - If an error like password complexity compliance error is received, it does not keep trying.

## Credits
https://stackoverflow.com/a/21252845 for the solution.
https://stackoverflow.com/users/4686882/binarypatrick for pointing out the reasons not to use UserPrincipal in that same post.

## Possible To Do's
[#3 multilanguage](https://github.com/gva-mgutierrez/pasatu/issues/1)

Not sure if allowing to choose the domain is a good idea. Maybe for an organization with a forest or linked domains, but never worked with that.

Logo change. For the moment, I keep advertising my company :P
