🎵 AuraPlay - Music Streaming API
📋 Descripción del Proyecto
AuraPlay es una API REST completa para un servicio de streaming de música desarrollada en .NET 9.0 con arquitectura por capas, autenticación JWT y despliegue en Railway.
🚀 Características
🔐 Autenticación y Autorización
- JWT Token-based authentication
- Roles: Admin y User
- Endpoints protegidos con [Authorize]
- Políticas de autorización personalizadas
🎵 Gestión de Música
- Canciones con metadatos completos
- Playlists personalizadas
- Estadísticas de reproducción en tiempo real
- Relaciones 1:1, 1:N y M:N entre entidades
🛠️ Tecnologías
- .NET 9.0 - Framework principal
- Entity Framework Core - ORM y gestión de base de datos
- PostgreSQL - Base de datos principal
- JWT - Autenticación
- Docker - Contenerización
- Railway - Plataforma de despliegue
- Swagger - Documentación de API
📊 Estructura de la Base de Datos
🗃️ Entidades y Relaciones
Usuario (User)
├── Id (Guid)
├── Username (string)
├── Email (string)
└── Playlists (ICollection<Playlist>) 1:N

Playlist
├── Id (Guid)
├── Name (string)
├── Description (string)
├── UserId (Guid) FK
└── Songs (ICollection<Song>) M:N ↔ PlaylistSong

Song (Cancion)
├── Id (Guid)
├── Name (string)
├── Description (string)
├── Duration (float)
├── Statistics (Statistics) 1:1
└── Playlists (ICollection<Playlist>) M:N ↔ PlaylistSong

Statistics (Estadísticas)
├── Id (Guid)
├── Reproductions (int)
├── Likes (int)
├── Downloads (int)
├── Rating (double)
└── SongId (Guid) FK

PlaylistSong (Tabla intermedia M:N)
├── PlaylistId (Guid) FK
├── SongId (Guid) FK
└── AddedAt (DateTime)
📁 Arquitectura del Proyecto
AuraPlay-TecWeb-I/
├── Controllers/          # Controladores API
│   ├── AuthController.cs
│   ├── SongController.cs
│   └── PlaylistController.cs
├── Data/
│   └── AppDbContext.cs   # Contexto de base de datos
├── Models/
│   ├── Entities/         # Entidades principales
│   │   ├── User.cs
│   │   ├── Song.cs
│   │   ├── Playlist.cs
│   │   └── Statistics.cs
│   └── DTOs/            # Objetos de transferencia
│       ├── AuthDtos.cs
│       ├── SongDtos.cs
│       └── PlaylistDtos.cs
├── Repositories/         # Patrón Repository
│   ├── IUserRepository.cs
│   ├── ISongRepository.cs
│   ├── IPlaylistRepository.cs
│   └── Implementaciones
├── Services/            # Lógica de negocio
│   ├── IAuthService.cs
│   ├── ISongService.cs
│   ├── IPlaylistService.cs
│   └── Implementaciones
└── Properties/
    └── launchSettings.json
🔌 Endpoints de la API
🎵 Canciones (Song)
Metodo    |	   Endpoint       |	      Descripcion           |   Autenticación
GET       |   /api/song	      | Obtener todas las canciones	|  ✅
GET	      |   /api/song/{id}  |	Obtener canción específica  |  ✅
POST      |	  /api/song	      | Crear nueva canción	        |  ✅ Admin
PUT	      |   /api/song/{id}  |	Actualizar canción	        |  ✅
DELETE    |	  /api/song/{id}  |	Eliminar canción	        |  ✅ Admin
📝 Playlists
Método	|  Endpoint	                             |      Descripción	            |  Autenticación
GET	    |  /api/playlist	                     |  Obtener todas las playlists	|  ✅
GET	    |  /api/playlist/{id}	                 |  Obtener playlist específica	|  ✅
POST    |  /api/playlist	                     |  Crear nueva playlist	    |  ✅
PUT	    |  /api/playlist/{id}	                 |  Actualizar playlist	        |  ✅
DELETE	|  /api/playlist/{id}	                 |  Eliminar playlist	        |  ✅
POST	|  /api/playlist/{id}/songs	             |  Agregar canción a playlist	|  ✅
DELETE	|  /api/playlist/{id}/songs/{songId}     |	Remover canción de playlist	|  ✅
🔐 Autenticación
Método	  |      Endpoint	      |      Descripción
POST	  |  /api/auth/register   |	 Registrar nuevo usuario
POST	  |  /api/auth/login	  |  Iniciar sesión

🚀 Instalación y Configuración
Prerrequisitos
- .NET 9.0 SDK
- PostgreSQL
📊 Diagrama de Relaciones
┌───────────┐   1:N   ┌───────────┐   M:N   ┌───────────┐
│   User    │───────→│ Playlist  │←──────→│   Song    │
└───────────┘         └───────────┘         └───────────┘
                                    │         │
                                    │        1:1
                                    │         │
                                    ↓         ↓
                              ┌───────────┐ ┌───────────┐
                              │PlaylistSong││Statistics│
                              └───────────┘ └───────────┘
👥 Equipo de Desarrollo
Rol	          |  Desarrollador	|  Responsabilidades
Backend Lead	|  [Victor]	      |  Playlists, Relaciones M:N, Integración
API Developer	|  [Maria]	      |  Estadísticas, Relaciones 1:1
API Developer	|  [Adrian]	      |  Canciones, Entidades base

¡Disfruta de la música con AuraPlay! 🎧✨

