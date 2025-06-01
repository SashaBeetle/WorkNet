# WorkNet

## Необхідні умови

Перед початком переконайтеся, що у вас встановлено наступне програмне забезпечення:

* **Node.js та npm:** Для фронтенд-частини (Angular). Рекомендується остання LTS-версія. [Завантажити Node.js](https://nodejs.org/) (Фронтенд використовує Angular CLI 18.2.4, що передбачає сучасну версію Node.js).
* **.NET SDK:** Для бекенд-частини (ASP.NET Core Web API). Рекомендується .NET 6.0 або новіша версія (на основі структури файлу `Program.cs`). [Завантажити .NET SDK](https://dotnet.microsoft.com/download)
* **Git:** Для клонування репозиторію. [Завантажити Git](https://git-scm.com/)
* **PostgreSQL:** Бекенд використовує PostgreSQL як базу даних. Переконайтеся, що у вас є запущений екземпляр PostgreSQL.
* **IDE/Редактор коду:** Наприклад, Visual Studio Code, Visual Studio, JetBrains Rider.

## Встановлення

1.  **Клонуйте репозиторій:**
    ```bash
    git clone [https://github.com/SashaBeetle/WorkNet.git](https://github.com/SashaBeetle/WorkNet.git)
    ```
2.  **Перейдіть до директорії проекту:**
    ```bash
    cd WorkNet
    ```

## Запуск Фронтенду (Angular)

Фронтенд частина проекту розроблена на Angular. Файл README для фронтенду знаходиться у `worknet-frontend/README.md`.

1.  **Перейдіть до директорії фронтенду:**
    ```bash
    cd worknet-frontend
    ```
    (Якщо ви знаходитесь в кореневій папці проекту)

2.  **Встановіть залежності:**
    ```bash
    npm install
    ```
    (Або `yarn install`, якщо ви використовуєте Yarn)

3.  **Запустіть сервер для розробки:**
    ```bash
    ng serve
    ```
   
    Ця команда запустить сервер розробки.

4.  **Відкрийте додаток у браузері:**
    Перейдіть за адресою `http://localhost:4200/`. Додаток автоматично перезавантажуватиметься при зміні вихідних файлів.

### Інші команди для Фронтенду

* **Збірка проекту для продакшену:**
    ```bash
    ng build
    ```
    Артефакти збірки будуть збережені в директорії `dist/worknet-frontend/`.
* **Запуск юніт-тестів:**
    ```bash
    ng test
    ```
    (виконується через Karma)
* **Запуск end-to-end тестів:**
    ```bash
    ng e2e
    ```
    (потребує попереднього додавання пакету для e2e тестування)
* **Довідка Angular CLI:**
    ```bash
    ng help
    ```

## Запуск Бекенду (.NET Web API)

Бекенд частина проекту розроблена на ASP.NET Core Web API. Основний проект знаходиться в `worknet-backend/Worknet.API/`.

1.  **Налаштування User Secrets:**
    Бекенд використовує User Secrets для зберігання конфігураційної інформації, такої як рядки підключення до бази даних та налаштування JWT і Google Drive. **Ці секрети потрібно налаштувати перед першим запуском.**

    * Перейдіть до директорії API проекту:
        ```bash
        cd worknet-backend/Worknet.API
        ```
    * Ініціалізуйте user secrets (якщо ще не зроблено):
        ```bash
        dotnet user-secrets init
        ```
    * Встановіть необхідні секрети. Приклади (замініть значення на ваші):
        ```bash
        dotnet user-secrets set "ConnectionStrings:WorknetDatabase" "Ваш_рядок_підключення_до_PostgreSQL"
        dotnet user-secrets set "Jwt:Key" "Ваш_дуже_довгий_і_складний_секретний_ключ_мінімум_32_символи"
        dotnet user-secrets set "Jwt:Issuer" "Ваш_Issuer"
        dotnet user-secrets set "Jwt:Audience" "Ваш_Audience"
        dotnet user-secrets set "GoogleDrive:FolderId" "Ваш_Google_Drive_Folder_Id"
        dotnet user-secrets set "GoogleDrive:JsonName" "ім'я_вашого_json_файлу_сервісного_акаунту.json" 
        ```
        **Примітка:** Файл JSON сервісного акаунту Google (`ім'я_вашого_json_файлу_сервісного_акаунту.json`) повинен знаходитися в директорії `worknet-backend/Worknet.API/App_Data/`.

2.  **Застосування Міграцій Бази Даних (Entity Framework Core):**
    Бекенд використовує Entity Framework Core для роботи з базою даних PostgreSQL.

    * Переконайтеся, що у вас встановлено `dotnet ef tools`. Якщо ні, встановіть глобально:
        ```bash
        dotnet tool install --global dotnet-ef
        ```
    * Перебуваючи в директорії `worknet-backend/Worknet.API/` (або вказуючи проект запуску та проект міграцій):
        ```bash
        dotnet ef database update --startup-project ./Worknet.API.csproj --project ../Worknet.Database/Worknet.Database.csproj
        ```
        Це створить схему бази даних на основі наявних міграцій.

3.  **Запуск API:**
    * Перебуваючи в директорії `worknet-backend/Worknet.API/`:
        ```bash
        dotnet run
        ```
    * API буде доступне за адресами, вказаними в `Properties/launchSettings.json`, зазвичай:
        * `http://localhost:5103`
        * `https://localhost:7047`
    * Фронтенд очікує бекенд на `https://localhost:7047/api`.
