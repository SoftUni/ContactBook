# ContactBook C# App + RESTful API

Summary: ContactBook allows users to manage their own contact book! Create, edit, and delete contacts into your contact book. Fill in names, email, phone number and info and build your very own contact list!

- Target platform: .NET 6
- Seeded database with one user and three contacts
- Default user credentials: `guest@mail.com` / `guest`

## ContactBook Web App

The ASP.NET Core app "ContactBook" is an app for creating contacts.

- Technologies: C#, ASP.NET Core, Entity Framework Core, ASP.NET Core Identity, NUnit
- The app supports the following operations:
  - Home page (view contacts count + menu): `/`
  - View contacts: `/Contacts/All`
  - Create a new contact (name + email + phone number + comments): `/Contacts/Create`
  - Edit contact: `/Contacts/Edit/:id`
  - Delete contact: `/Contacts/Delete/:id`

## ContactBook RESTful API

The following endpoints are supported:

- `GET /api` - list all API endpoints
- `GET /api/contacts` - list all contacts
- `GET /api/contacts/count` - returns contacts count
- `GET /api/contacts/:id` - returns a contact by given `id`
- `GET /api/contacts/search/:keyword` - returns contacts by given `keyword`
- `POST /api/contacts/create` - create a new event (send a JSON object in the request body, e.g. `{ "first name": "Johnny", "last name": "Depp", "email": "j.depp@mail.com", "phone number": "+12298015369", "comments": "An American actor, producer, and musician. Best in 'Pirates of the Caribbean'" })
- `PUT /api/contacts/:id` - edit event by `id` (send a JSON object in the request body, holding all fields, e.g. `{ "first name": "Johnny", "last name": "Depp", "email": "j.depp@mail.com", "phone number": "+12298015369", "comments": "An American actor, producer, and musician. Best in 'Pirates of the Caribbean'" }`)
- `PATCH /api/contacts/:id` - partially edit event by `id` (send a JSON object in the request body, holding the fields to modify, e.g. `{ "email": "johnny.depp@mail.com", "phone number": "+12298015369" }`)
- `DELETE /api/contacts/:id` - delete event by `id`
- `GET /api/users` - list all users
- `POST /api/users/login` - logs in an existing user (send a JSON object in the request body, holding all fields, e.g. `{"email": "guest@mail.com", "password": "guest"}`)
- `POST /api/users/register` - registers a new user (send a JSON object in the request body, holding all fields, e.g. `{ "email": "someUsername@mail.bg", "firstName": "someName", "lastName": "someLastName", "phoneNumber": "+192088877744", "password": "somePassword", "confirmPassword": "somePassword" }`)

## Screenshots

![home-page](https://user-images.githubusercontent.com/72888249/207355016-1054a001-e68f-4536-93b5-13ebd68c35e3.png)
![home-page-logged-in](https://user-images.githubusercontent.com/72888249/207356520-32f2bae3-4c02-4445-b6c1-83f66f3a211a.png)
![all-contacts](https://user-images.githubusercontent.com/72888249/207357086-fca28bea-e434-4896-9716-40bb5514889b.png)
![create-contact](https://user-images.githubusercontent.com/72888249/207357475-54646dba-2be6-4d76-81b8-3e05043a1e61.png)
![edit-contact](https://user-images.githubusercontent.com/72888249/207357743-bfa34884-fadf-4794-9fcf-076e7b24baeb.png)
![search-contacts](https://user-images.githubusercontent.com/72888249/207357570-039509a1-ea86-4e79-9e2c-9b5aabae440a.png)
