# 📘 WorkNet

> Веб-портал для професійного нетворкінгу, пошуку роботи та взаємодії між фахівцями та роботодавцями.

---

## 👤 Автор

* **ПІБ**: Жук Олександр Сергійович
* **Група**: ФеП-41
* **Керівник**: проф. О.М. Крупич
* **Дата виконання**: 02.06.2025

---

## 📌 Загальна інформація

* **Тип проєкту**: Веб-портал (Single Page Application + REST API)
* **Backend**:
    * **Мова**: C#
    * **Платформа / Фреймворки**: .NET Core, ASP.NET Core, Entity Framework Core
* **Frontend**:
    * **Мова**: TypeScript
    * **Фреймворк / Бібліотеки**: Angular, NgRx, Tailwind CSS
* **База даних**: PostgreSQL

---

## 🧠 Опис функціоналу

* 🔐 **Автентифікація**: Надійна система реєстрації та входу користувачів з використанням JWT.
* 🧑‍💼 **Профілі фахівців**: Створення, редагування та перегляд розширених профілів користувачів з інформацією про навички та досвід.
* 📁 **Інтеграція з Google Drive**: Завантаження та зберігання файлів (резюме, портфоліо) у хмарному сховищі.
* 📄 **Вакансії**: Базовий функціонал для перегляду списку вакансій.
* 💬 **Обмін повідомленнями**: Основа для майбутньої системи комунікації між користувачами.
* 🌐 **REST API**: Чітко структурований API для взаємодії між frontend та backend.

---

## 🧱 Опис основних класів / файлів

| Клас / Файл                                                      | Призначення                                                                  |
| ---------------------------------------------------------------- | ---------------------------------------------------------------------------- |
| **Backend (.NET)** |                                                                              |
| `worknet-backend/Worknet.API/Program.cs`                         | Точка входу, конфігурація сервісів та middleware.                            |
| `worknet-backend/Worknet.API/Controllers/AuthController.cs`      | Контролер для обробки запитів реєстрації та входу.                            |
| `worknet-backend/Worknet.API/Controllers/ProfileController.cs`   | REST API маршрути для операцій з профілями користувачів.                      |
| `worknet-backend/Worknet.Database/WorknetDbContext.cs`           | Контекст бази даних Entity Framework Core для роботи з PostgreSQL.         |
| `worknet-backend/Worknet.Core/Entities/Profile.cs`               | Сутність, що описує модель профілю користувача.                              |
| `worknet-backend/Worknet.API/Util/JwtUtil.cs`                    | Сервіс для генерації та валідації JWT.                                       |
| **Frontend (Angular)** |                                                                              |
| `worknet-frontend/src/main.ts`                                   | Точка входу frontend-застосунку.                                             |
| `worknet-frontend/src/app/core/services/auth.service.ts`         | Сервіс для взаємодії з API автентифікації.                                   |
| `worknet-frontend/src/app/features/pages/profile/profile.component.ts`| Компонент для відображення та редагування профілю.                            |
| `worknet-frontend/src/app/store/app.state.ts`                    | Визначення глобального стану (NgRx) для управління даними.                  |
| `worknet-frontend/src/app/core/interceptors/auth.interceptor.ts` | Перехоплювач HTTP-запитів для додавання JWT.                                 |

---

## ▶️ Як запустити проєкт "з нуля"

### 1. Встановлення інструментів

* .NET 8 SDK або новіше
* Node.js v20.x або новіше + npm
* PostgreSQL (локально або через Docker)
* Angular CLI: `npm install -g @angular/cli`

### 2. Клонування репозиторію

```bash
git clone [https://github.com/SashaBeetle/WorkNet.git](https://github.com/SashaBeetle/WorkNet.git)
cd WorkNet
```

### 3. Встановлення залежностей

```bash
# Backend
cd worknet-backend
dotnet restore

# Frontend
cd ../worknet-frontend
npm install
```

### 4. Налаштування `appsettings.json` (Backend)

Відредагуйте файл `worknet-backend/Worknet.API/appsettings.Development.json`, вказавши правильні налаштування:

```json
{
  "ConnectionStrings": {
    "WorknetDatabase": "Host=localhost;Port=5432;Database=worknetdb;Username=postgres;Password=your_password"
  },
  "Jwt": {
    "Key": "your_super_secret_key_that_is_long_enough",
    "Issuer": "WorkNetAPI",
    "Audience": "WorkNetClient"
  },
  "GoogleDrive": {
    "FolderId": "your_google_drive_folder_id",
    "JsonName": "your_google_credentials_filename.json"
  }
}
```

### 5. Застосування міграцій до БД

```bash
cd worknet-backend
dotnet ef database update --startup-project ./Worknet.API/Worknet.API.csproj --project ./Worknet.Database/Worknet.Database.csproj
```

### 6. Запуск

```bash
# Запустити Backend (з папки worknet-backend/Worknet.API)
dotnet run

# Запустити Frontend (з папки worknet-frontend, в новому терміналі)
ng serve
```

Застосунок буде доступний за адресою `http://localhost:4200`.

---

## 🔌 API приклади

### 🔐 Автентифікація

**POST /api/auth/register**

```json
{
  "email": "newuser@example.com",
  "password": "StrongPassword123!",
  "userName": "IvanPetrenko"
}
```

**POST /api/auth/login**

```json
{
  "email": "newuser@example.com",
  "password": "StrongPassword123!"
}
```
**Response:**
```json
{
  "token": "jwt_token_here"
}
```

---

### 🧑‍💼 Профілі

**GET /api/profile/{userId}**

Отримати профіль користувача. (Потребує JWT в заголовку `Authorization: Bearer <token>`)

**PUT /api/profile/{userId}**

Оновити профіль. (Потребує JWT в заголовку)

```json
{
  "headline": "Junior .NET Developer",
  "about": "Початківець у розробці з великим бажанням вчитись.",
  "skills": ["C#", ".NET", "ASP.NET Core"]
}
```

---

## 🖱️ Інструкція для користувача

1.  **Головна сторінка**: Надає можливість `Увійти` для існуючих користувачів або `Зареєструватись` для нових.
2.  **Реєстрація**: Користувач вводить свої дані для створення облікового запису.
3.  **Вхід**: Після успішної автентифікації користувач перенаправляється на головну сторінку порталу.
4.  **Мій профіль**:
    * Користувач може переглядати свій профіль.
    * Кнопка `Редагувати профіль` дозволяє змінити особисті дані, навички та досвід.
    * У формі редагування є можливість завантажити резюме через інтеграцію з Google Drive.
5.  **Вакансії**: Сторінка для перегляду списку доступних вакансій.
6.  **Вихід**: Кнопка `Вийти` у профілі завершує сесію користувача та повертає на головну сторінку.

---

## 📷 Приклади / скриншоти
| Сторінка входу | Сторінка чату | Сторінка вакансій |
| :---: | :---: | :---: |
| ![Зображення сторінки входу](https://drive.google.com/thumbnail?id=13yvKrDLqJbsYnt2kbzx2biVB8x-O4JG2&sz=w800) | ![Зображення сторінки чату](https://drive.google.com/thumbnail?id=1HjIY4pLjrvK0U2ub3iz4tk0tncC7OfJV&sz=w800) | ![Зображення сторінки вакансій](https://drive.google.com/thumbnail?id=1sEf84HA5Tkg4S8_dHSeKcovy90QPYEgW&sz=w800) |

---

## 🧪 Проблеми і рішення

| Проблема                                    | Рішення                                                                                                                      |
| ------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| Помилка CORS при запитах з frontend         | Перевірити конфігурацію CORS middleware в `Startup.cs` на backend. Правило має дозволяти запити з `http://localhost:4200`.     |
| 401 Unauthorized                            | Перевірити, чи правильно надсилається JWT у заголовку `Authorization`. Перевірити термін дії токена та секретний ключ на сервері. |
| Не вдається підключитися до БД              | Перевірити рядок підключення `ConnectionString` в `appsettings.json`. Переконатись, що сервер PostgreSQL запущено і доступний. |
| Помилка при завантаженні файлів             | Перевірити `ClientId` та `ClientSecret` для Google Drive API. Перевірити, чи надано відповідні дозволи в Google Cloud Console.   |

---

## 🧾 Використані джерела / література

* [Офіційна документація .NET та ASP.NET Core](https://docs.microsoft.com/en-us/dotnet/)
* [Офіційна документація Angular](https://angular.io/docs)
* [PostgreSQL Documentation](https://www.postgresql.org/docs/)
* [Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)
* [JSON Web Tokens (JWT.io)](https://jwt.io/)
* [Google Drive API Documentation](https://developers.google.com/drive/api)
* Матеріали з форумів StackOverflow та спільнот розробників.
