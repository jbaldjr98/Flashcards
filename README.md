# Flashcards

A flashcard study app built with a **React** frontend and an **ASP.NET Core Web API** backend backed by SQL Server. Users create subjects and chapters to organise their flashcards, then study them with a flip-card interface that tracks correct/incorrect answers per card.

---

## Features

- Create **subjects**, **chapters**, and **flashcards**
- Edit existing decks — update front/back text, change chapters, add or delete cards inline
- **Study mode** — flip cards, mark correct/incorrect, flag cards for revisit, see a results summary at the end
- Delete subjects (cascades to all chapters and flashcards)

---

## Architecture

The solution is split into two top-level parts: a React SPA (`client/`) and a .NET backend (`Flashcards/` + supporting projects).

```
client/                  → React frontend (Vite)
Flashcards/              → ASP.NET Core Web API
Model/                   → Domain entities + EF Core DbContext
ServiceInterface/        → Service interfaces
Service/                 → Service implementations
DomainInterface/         → Repository interfaces
Domain/                  → Repository implementations (EF Core)
```

### Backend layer responsibilities

| Layer | Project | Responsibility |
|---|---|---|
| API | `Flashcards` | REST controllers, CORS, DI wiring |
| Application | `Service` / `ServiceInterface` | Business logic, orchestration |
| Data access | `Domain` / `DomainInterface` | EF Core repositories |
| Domain model | `Model` | Entities, `ApplicationDbContext` |

### Frontend structure

```
client/src/
  api/          → Fetch wrappers (subjects.js, chapters.js, flashcards.js)
  components/   → Shared components (Navbar, Modal)
  pages/        → One file per route
  App.jsx       → Router setup
  index.css     → Global dark-theme styles
```

### Request flow

```
React (localhost:5173)
  └── Vite proxy /api/* → .NET API (localhost:51234)
        └── Controller → Service → Repository → SQL Server
```

---

## API Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/subjects` | Get all subjects |
| POST | `/api/subjects` | Create a subject |
| DELETE | `/api/subjects/{id}` | Delete a subject (cascades) |
| GET | `/api/chapters?subjectId=X` | Get chapters for a subject |
| POST | `/api/chapters` | Create a chapter |
| GET | `/api/flashcards?subjectId=X` | Get flashcards for a subject |
| POST | `/api/flashcards/batch` | Create multiple flashcards |
| PUT | `/api/flashcards/{id}` | Update a flashcard |
| DELETE | `/api/flashcards/{id}` | Delete a flashcard |
| PATCH | `/api/flashcards/{id}/mark` | Record correct/incorrect/revisit |

---

## Data Model

```
Subject
 ├── Id, Name, Description
 ├── Chapters  (one-to-many → cascade delete)
 └── Flashcards (one-to-many → NoAction)

Chapter
 ├── Id, Name, Description, SubjectId
 └── Flashcards (one-to-many → cascade delete)

Flashcard
 ├── Id, Front, Back
 ├── SubjectId, ChapterId
 ├── numSuccess, numFailure
 └── IsRevisit
```

**Cascade behaviour:** deleting a Subject cascades to its Chapters, which cascades to their Flashcards. The direct Subject → Flashcard FK is `NoAction` to avoid a multiple-cascade-path conflict in SQL Server — flashcards are cleaned up via the Chapter path.

---

## Pages

| Route | Component | Description |
|---|---|---|
| `/` | `Index` | Subjects table with Study / Update / Delete per row |
| `/create-subject` | `CreateSubject` | Form to create a subject |
| `/create-chapter` | `CreateChapter` | Form to create a chapter (accepts `?subjectId=X`) |
| `/create-flashcards` | `CreateFlashcards` | Subject + chapter picker, dynamic card table |
| `/update-deck/:subjectId` | `UpdateDeck` | Edit/add/delete cards, add chapters via modal |
| `/study` | `Study` | Flip-card study session with progress + results |

---

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server
- Node.js 18+ / npm

### 1. Configure the database

Set the connection string in `Flashcards/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=Flashcards;Trusted_Connection=True;"
}
```

The app calls `db.Database.Migrate()` on startup so the schema is applied automatically.

### 2. Start the .NET API

Open the solution in Visual Studio and press **F5**, or:

```
dotnet run --project Flashcards
```

The API runs on `http://localhost:51234`.

### 3. Start the React app

```
cd client
npm install   # first time only
npm run dev
```

Open **http://localhost:5173** in your browser.

The Vite dev server proxies all `/api/*` requests to the .NET API automatically, so no CORS configuration is needed in the browser.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | React 18, React Router 6, Vite |
| Backend | ASP.NET Core 9 Web API |
| ORM | Entity Framework Core 9 (SQL Server) |
| Styling | Custom CSS, dark theme (`#111827` base, `#6366f1` accent) |
