[Техническое задание](https://docs.google.com/document/d/15Wb-wEG29As5XE8sqJFEPq3lYchkC8f5YwHzDl7TL3A/edit?tab=t.0)

# Market

## Описание

**Market** — это веб-приложение для маркетплейса, реализованное на ASP.NET Core 8 с использованием архитектуры слоёв (Application, Domain, Infrastructure, MVC). В проекте используются PostgreSQL для хранения данных и Minio для хранения файлов (например, обложек товаров).

## Быстрый старт

### 1. Запуск инфраструктуры (PostgreSQL и Minio) через Docker

Создайте файл `docker-compose.yml` в корне проекта со следующим содержимым:

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:15
    container_name: market_postgres
    restart: always
    environment:
      POSTGRES_DB: market_db
      POSTGRES_USER: user
      POSTGRES_PASSWORD: password
    ports:
      - "5435:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data

  minio:
    image: minio/minio:latest
    container_name: market_minio
    environment:
      MINIO_ROOT_USER: minioadmin
      MINIO_ROOT_PASSWORD: minioadmin
    ports:
      - "9000:9000"
      - "9001:9001"
    command: server --console-address ":9001" /data
    volumes:
      - minio_data:/data

volumes:
  pgdata:
  minio_data:
```

Запустите контейнеры командой:

```bash
docker-compose up -d
```

### 2. Настройка переменных окружения

Проверьте, что в `Market/Market.MVC/appsettings.json` указаны следующие параметры подключения:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5435;Username=user;Password=password;Database=market_db"
},
"Minio": {
  "Endpoint": "localhost:9000",
  "AccessKey": "minioadmin",
  "SecretKey": "minioadmin",
  "BucketName": "products",
  "UseSSL": false
}
```

### 3. Применение миграций

Перейдите в папку с проектом инфраструктуры и примените миграции:

```bash
cd Market/Infrastructure/Market.Persistence
dotnet ef database update --project Market.Persistence.csproj --startup-project ../../Market.MVC/Market.MVC.csproj
```

> Убедитесь, что у вас установлен пакет `dotnet-ef` (`dotnet tool install --global dotnet-ef`).

### 4. Запуск приложения

Перейдите в папку с MVC-проектом и запустите приложение:

```bash
cd ../../Market.MVC
dotnet run
```

Приложение будет доступно по адресу http://localhost:5000 (или другому, указанному в настройках).

---

## Основные возможности

- Регистрация и аутентификация пользователей и авторов
- Добавление, просмотр и покупка товаров
- Лайки, корзина, покупки, статистика продаж
- Админ-панель для управления пользователями, товарами и заявками

## Стек технологий

- ASP.NET Core 8
- Entity Framework Core + PostgreSQL
- Minio (S3-совместимое хранилище)
- MediatR, Identity
- Docker (для инфраструктуры)
