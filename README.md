# Library API

## Building and running application - Docker

1. Open terminal in the root folder where docker-compose.yml file is located.
2. Run `docker-compose up` command
3. API is exposed at `5000` port, use `http://localhost:5000/swagger/index.html` url to open API Swagger. SQL server is exposed at `14333` port, use `localhost,14333` server name and `sa : p@ssword1` credentials to connect to the server.

Database is seeded with 5 books, reader user and librarian user.  
Librarian credentials: `librarian@gmail.com : password`  
Reader credentials: `reader@gmail.com : password`

It is possible to add new accounts using `account/register` endpoint.  
To log in use `account/login` endpoint, use the JWT token from the response to authorize.
