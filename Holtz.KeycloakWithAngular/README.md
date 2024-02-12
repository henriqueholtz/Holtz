# Holtz.KeycloakWithAngular

## Setup environment

1. Run keycloak as a docker container:
   ```
   docker run -dit --name holtz_keycloak -p 8080:8080 -e KEYCLOAK_USER=holtz_keycloak -e KEYCLOAK_PASSWORD=holtz_keycloak_pass jboss/keycloak
   ```
2. Go to http://localhost:8080/ and create a client:
   - Client ID: `Holtz-Keycloak`
   - Client Protocol: `openid-connect`
3. Update the following client' settings
   - Enable Implicit Flow
   - Redirect URL: `*`
