# 🎵 AuraPlay - Music Streaming API

## 📌 1. Explicación del Proyecto

Este sistema es una **API RESTful** desarrollada con **.NET 9** diseñada para la gestión integral de un servicio de *streaming* de música.

El proyecto resuelve la necesidad de una administración centralizada de bibliotecas musicales, permitiendo a los usuarios crear bibliotecas personalizadas y a los administradores gestionar el catálogo global. Su objetivo principal es ofrecer una interfaz segura, rápida y escalable, garantizando la integridad de las **relaciones complejas** entre usuarios, canciones, playlists y estadísticas.

### Arquitectura
El sistema sigue una **Arquitectura por Capas (Layered Architecture)** utilizando el patrón **Repository**, lo que asegura un código limpio, mantenible y desacoplado:

* **Controllers:** Manejan las peticiones HTTP y la validación de entrada.
* **Services:** Contienen la lógica de negocio, validaciones de propiedad (ej: *solo el dueño edita su playlist*) y orquestación.
* **Repositories:** Se encargan del acceso directo a datos mediante **Entity Framework Core**.
* **Data/Models:** Definición de entidades, DTOs y contexto de base de datos.

---

## 🚀 2. Características y Tecnologías

| Categoría | Característica | Detalle |
| :--- | :--- | :--- |
| **Tecnologías** | **Framework** | .NET 9.0 |
| | **Base de Datos** | PostgreSQL (Gestionado por Entity Framework Core) |
| | **Contenerización** | Docker y `docker-compose` |
| | **Despliegue** | Railway (Soporte nativo para `DATABASE_URL`) |
| **Seguridad** | **Autenticación** | JWT (JSON Web Tokens) con Refresh Token Rotation |
| | **Roles** | **Admin** (Gestión de catálogo) y **User** (Gestión de playlists) |
| | **Protección** | Endpoints protegidos con `[Authorize]` y políticas personalizadas |
| **Funcionalidades** | **Gestión CRUD** | Canciones (`Song`), Playlists y Usuarios. |
| | **Lógica Compleja** | Relación M:N entre Canciones y Playlists (`PlaylistSong`). |
| | **Estadísticas** | Relación 1:1 para métricas de canciones (`Statistics`). |
| | **Defensa** | **Rate Limiting (TimeGate)** para proteger contra ataques de fuerza bruta. |

---

## 🏛️ 3. Diagrama ER (Entidad-Relación)

El modelo de datos utiliza una base de datos relacional **PostgreSQL**. A continuación, se describen las entidades y sus relaciones obligatorias:

### Entidades y Atributos

| Tabla | Atributos Principales | Descripción |
| :--- | :--- | :--- |
| **Users** | `Id`, `Username`, `Email`, `Role` | Usuarios del sistema con roles y credenciales encriptadas. |
| **Songs** | `Id`, `Name`, `Duration`, `Description` | Catálogo de canciones disponibles. |
| **Playlists** | `Id`, `Name`, `UserId` | Listas de reproducción creadas por usuarios. |
| **Statistics** | `Id`, `Reproductions`, `Likes`, `SongId` | Métricas en tiempo real asociadas a una canción. |
| **PlaylistSongs** | `PlaylistId`, `SongId`, `AddedAt` | Tabla intermedia para la relación muchos a muchos. |

### 🔗 Relaciones del Modelo

1.  **1 a 1 (Song ↔ Statistics):**
    * Cada canción tiene un único registro de estadísticas (reproducciones, likes).
    * *Implementación:* `Statistics` tiene la clave foránea `SongId` y eliminación en cascada.

2.  **1 a Muchos (User ↔ Playlist):**
    * Un usuario puede crear múltiples playlists, pero una playlist pertenece a un solo creador.
    * *Implementación:* `Playlist` tiene la clave foránea `UserId`.

3.  **Muchos a Muchos (Song ↔ Playlist):**
    * Una canción puede estar en muchas playlists y una playlist contiene muchas canciones.
    * *Implementación:* Se utiliza la tabla intermedia **`PlaylistSong`**.

---

## 🔐 4. Autenticación, Autorización y Roles

El sistema utiliza **JWT** para asegurar las comunicaciones.

* **Autenticación:** Endpoint `/api/auth/login` devuelve `AccessToken` y `RefreshToken`.
* **Autorización:** El token debe enviarse en el header HTTP.

### Roles del Sistema
* **Admin:** Puede crear, editar y eliminar Canciones y ver todos los usuarios.
* **User:** Puede crear Playlists propias, agregar canciones a ellas y ver el catálogo.

### Uso del Token
Cabecera requerida para endpoints protegidos:
```http
Authorization: Bearer <tu_token_jwt_aqui>


## 🌐 5. Documentación de Endpoints

### 🔐 Auth (Autenticación)

| Método | Endpoint | Permiso | Descripción |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/v1/Auth/register` | Público | Registrar un nuevo usuario. |
| `POST` | `/api/v1/Auth/login` | Público | Iniciar sesión y obtener tokens. |
| `POST` | `/api/v1/Auth/refresh` | Público | Renovar el Access Token. |

### 🎵 Song (Canciones)

| Método | Endpoint | Permiso | Descripción |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/Song` | Público | Obtener catálogo de todas las canciones. |
| `GET` | `/api/Song/{id}` | Auth | Ver detalles de una canción específica. |
| `POST` | `/api/Song` | **Admin** | Subir una nueva canción al sistema. |
| `PUT` | `/api/Song/{id}` | Auth | Actualizar datos básicos de la canción. |
| `PUT` | `/api/Song/{id}/stats` | Auth | Actualizar estadísticas (likes, reproducciones). |
| `DELETE` | `/api/Song/{id}` | **Admin** | Eliminar una canción del catálogo. |

### 📝 Playlist (Listas de Reproducción)

| Método | Endpoint | Permiso | Descripción |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/Playlist` | Auth | Ver todas las playlists. |
| `GET` | `/api/Playlist/{id}` | Auth | Ver detalle de una playlist. |
| `POST` | `/api/Playlist` | Auth | Crear una nueva playlist vacía. |
| `PUT` | `/api/Playlist/{id}` | Auth | Renombrar o cambiar descripción. |
| `DELETE` | `/api/Playlist/{id}` | Auth | Eliminar una playlist. |

### 🔗 PlaylistSong (Gestión de Canciones en Listas)

| Método | Endpoint | Permiso | Descripción |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/Playlist/{id}/songs` | Auth | Agregar una canción a la playlist. |
| `DELETE` | `/api/Playlist/{playlistId}/songs/{songId}`| Auth | Remover una canción de la playlist. |

## 📎 6. Swagger y Postman Collection

El proyecto incluye documentación interactiva automática generada con Swagger.

  * **URL de Acceso:** `http://localhost:8080/swagger` (cuando se ejecuta localmente o en Docker).
  * **Uso:** Permite probar todos los endpoints directamente desde el navegador. Incluye un botón **"Authorize"** arriba a la derecha para pegar el token JWT y probar las rutas protegidas.

## 📮 Colección de Postman

El repositorio incluye una colección de Postman completa (`Music API.postman_collection.json`) para facilitar las pruebas de todos los endpoints sin necesidad de configurarlos manualmente.

### 📥 Pasos para Importar

1.  **Ubicar el Archivo:** El archivo `Music API.postman_collection.json` se encuentra en la raíz del repositorio.
2.  **Abrir Postman:** Haz clic en el botón **"Import"** (esquina superior izquierda).
3.  **Cargar:** Arrastra el archivo JSON o selecciónalo desde tu explorador de archivos.

### ⚙️ Configuración del Entorno

La colección utiliza variables para facilitar el cambio entre entornos (Local, Docker, Railway).

1.  **Configurar `baseUrl`:**
    * Crea un nuevo entorno en Postman o edita la colección.
    * Crea una variable llamada `baseUrl`.
    * Establece su valor en: `http://localhost:8080` (para Docker local).

2.  **Autenticación (JWT):**
    * Ejecuta la petición **`POST /api/v1/Auth/login`**.
    * Copia el `accessToken` de la respuesta JSON.
    * En Postman, ve a la pestaña **Authorization** de la colección (o de la petición individual).
    * Selecciona **Type:** `Bearer Token`.
    * Pega el token en el campo **Token**.

¡Listo! Ahora puedes ejecutar cualquier petición sin reescribir las URLs.
-----

## ⏱️ 7. TimeGate (Rate Limiting)

El sistema implementa un **TimeGate** (Rate Limiter) configurado en el `Program.cs` para proteger la API contra el abuso y ataques de denegación de servicio.

  * **Configuración:** Ventana fija (`FixedWindow`).
  * **Límite:** Máximo **10 peticiones cada 10 segundos** por cliente.
  * **Respuesta al exceder:** El servidor responderá con un código `429 Too Many Requests`.

-----

## 🛠️ 8. Instalación y Configuración

### Requisitos Previos

  * .NET 9.0 SDK instalado.
  * Docker Desktop instalado y corriendo.
  * Postman (opcional, para pruebas).

### Pasos de Instalación

1.  **Clonar el repositorio:**

    ```bash
    git clone [https://github.com/mjlozada2003/AuraPlay-TecWeb-I.git]
    cd AuraPlay-TecWeb-I
    ```

2.  **Configurar Variables de Entorno:**
    Crea un archivo `.env` en la raíz con el siguiente contenido (basado en `docker-compose.yml`):

    ```properties
    POSTGRES_DB=musicdb
    POSTGRES_USER=musicuser
    POSTGRES_PASSWORD=upersecretpassword123
    JWT_KEY=EstaEsUnaClaveSuperSecretaYLoSuficientementeLargaParaHmacSha256!!
    JWT_ISSUER=MusicApi
    JWT_AUDIENCE=MusicClient
    ```

3.  **Levantar Infraestructura (Docker):**
    Esto iniciará la base de datos PostgreSQL automáticamente.

    ```bash
    docker-compose up -d
    ```

4.  **Ejecutar Migraciones (Crear Tablas):**

    ```bash
    dotnet tool install --global dotnet-ef
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

5.  **Ejecutar la API:**

    ```bash
    dotnet run
    ```

    La API estará disponible en `http://localhost:8080` (o el puerto indicado en la consola).

-----

## 📦 9. Datos de Prueba

Para probar el sistema, utiliza las siguientes credenciales de ejemplo:

| Cuenta | Email | Password | Role |
| :--- | :--- | :--- | :--- |
| **Administrador** | `auraplay@f1.com` | `Admin123!` | Admin |
| **Usuario** | `user@auraplay.com` | `User123!` | User |

-----