# UsuariosAPI
API de mantenimiento de usuarios con model controller. El proyecto cuenta con archivos de configuración, models, services, controllers, DTOs, DAO.

## Requistos
- MySQL
- .Net Core SDK 8

## Usar el proyecto
Se usarán los siguintes pasos:
- Descarga del proyecto o clonación
- Ejecutar el script del recurso Base de datos para obtener la base
- Abrir el proyecto UsuariosAPI y ejecutar
- Las pruebas se pueden realizar en Swagger o usar Postman o aplicaciíon de preferencia para realizar las peticiones
- Se ha configurado Swagger para versionamiento y autenticación
- Usar la etiqueta de autorización para poder acceder a los recursos web (de momento se puede crear usuario con la API si desean acceder a las demás rutas autenticando)
- login y GetUsers accesibles sin requerir inicio de sesión de usuario
- login genera token de autentcación si el usuario se autenticó
- Junto con el token del login probar los demás recursos
- Las peticiones se pueden realizar con la esructura solicitada

## Estructura usada

