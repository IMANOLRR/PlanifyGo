# Deploy de PlanifyGo en Render

## Estado del proyecto

El proyecto queda preparado para correr como servicio Docker en Render. La app:

- Lee la base de datos desde `ConnectionStrings__DefaultConnection`.
- Escucha en `0.0.0.0` usando la variable `PORT` de Render.
- Ejecuta migraciones de Entity Framework al arrancar.
- Aisla las tareas por usuario en dashboard, listado, edición, borrado y completado.

## 1. Base de datos MySQL

PlanifyGo usa MySQL con Pomelo Entity Framework. Para producción necesitas una base MySQL accesible desde Render. Tienes dos caminos:

- Usar un proveedor externo de MySQL, por ejemplo Aiven, Railway, PlanetScale u otro equivalente.
- Crear MySQL en Render como servicio privado con disco persistente, siguiendo la guía oficial: https://render.com/docs/deploy-mysql

Cuando tengas la base, guarda estos datos:

- Host
- Puerto
- Nombre de base de datos
- Usuario
- Password
- Requisito SSL del proveedor

La cadena tendrá esta forma:

```text
server=HOST;port=3306;database=DB_NAME;user=DB_USER;password=DB_PASSWORD;SslMode=Required;AllowPublicKeyRetrieval=True;
```

Si tu proveedor no exige SSL, usa `SslMode=None`, pero para producción es mejor usar SSL cuando esté disponible.

## 2. Subir el repositorio

1. Sube este repo a GitHub.
2. Verifica que `render.yaml` esté en la raíz del repo.
3. Verifica que no subas archivos `.env` reales ni contraseñas.

## 3. Crear el servicio en Render

1. En Render, crea un nuevo Blueprint desde tu repositorio, o crea un Web Service manual.
2. Si usas Blueprint, Render leerá `render.yaml`.
3. Si lo haces manual:
   - Runtime: Docker
   - Dockerfile path: `./Gestor de Tareas/Dockerfile`
   - Docker context: `./Gestor de Tareas`
   - Environment: `Production`

Render necesita que el servicio escuche en `0.0.0.0` y use `PORT`; el Dockerfile ya lo hace.

## 4. Variables de entorno

En Environment Variables de Render agrega:

```text
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=server=HOST;port=3306;database=DB_NAME;user=DB_USER;password=DB_PASSWORD;SslMode=Required;AllowPublicKeyRetrieval=True;
```

No pongas la cadena de conexión en `appsettings.json`.

## 5. Primer deploy

1. Ejecuta Deploy en Render.
2. Revisa los logs.
3. En el primer arranque la app aplicará las migraciones y creará las tablas.
4. Abre la URL `https://TU-SERVICIO.onrender.com`.
5. Registra un usuario nuevo y crea una tarea de prueba.

Si el deploy falla, revisa primero:

- La cadena `ConnectionStrings__DefaultConnection`.
- Que el host MySQL acepte conexiones desde Render.
- Que el usuario tenga permisos para crear tablas.
- Que el proveedor MySQL permita SSL si usaste `SslMode=Required`.

## 6. Conectar dominio

1. En Render, abre tu servicio.
2. Ve a Settings > Custom Domains.
3. Agrega tu dominio, por ejemplo `planifygo.com` o `www.planifygo.com`.
4. En tu proveedor DNS, crea los registros que Render te indique.
5. Quita registros `AAAA` si Render lo pide.
6. Vuelve a Render y pulsa Verify.

Render genera y renueva automáticamente el certificado TLS. Guía oficial: https://render.com/docs/custom-domains

## 7. Prueba final antes de compartir

Haz esta prueba antes de enviarlo al público:

1. Registra dos usuarios distintos.
2. Crea tareas con ambos.
3. Confirma que cada usuario solo ve sus tareas.
4. Edita, borra y completa tareas.
5. Cierra sesión y confirma que no puedes entrar al dashboard sin login.
6. Revisa logs de Render buscando errores 500.
