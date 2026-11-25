Sistema de Gestión de Facturación y Recordatorios

Aplicación desarrollada con .NET, usando Clean Architecture, MongoDB para la base de datos y React para gestionar el estado de cobro de facturas y automatizar el envío de notificaciones por correo electrónico.

Requisitos para la ejecución:

- .NET 8 SDK
- Node.js (v18 o superior)
- Docker Desktop

Para su instalación y uso local:
1. Clonar el repositorio

git clone https://github.com/laurajunco-7/prueba-facturas

2. Levantar la base de datos y el servidor de correo 

docker compose up -d

3. Levantar el backend
   
dotnet run --project 
backend/src/facturacion.api

La API queda disponible en => http://localhost:5027

Swagger => http://localhost:5027/swagger

4. Levantar el frontend

cd frontend

npm install

npm run dev

La aplicación web queda disponible en => http://localhost:5173

Y la bandeja de correos queda disponible en => http://localhost:8025
