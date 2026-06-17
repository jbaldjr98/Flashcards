# Flashcards

A web-based flashcard study app built with ASP.NET Core Razor Pages and SQL Server. Users create subjects and chapters to organise their flashcards, then study them with a flip-card interface that tracks correct/incorrect answers.

---

## Features

- Create **subjects**, **chapters**, and **flashcards**
- Edit existing flashcard decks — update front/back text, change chapters, add or delete cards
- **Study mode** — flip cards, mark correct/incorrect, flag cards for revisit, see a results summary at the end
- Delete subjects (cascades to chapters and flashcards)

---

## Architecture

The solution follows a layered architecture split across six projects:

```
Flashcards/          → ASP.NET Core Razor Pages web app (UI + page models)
Model/               → Domain entities + EF Core DbContext
ServiceInterface/    → Service interfaces (IGenericService, ISubjectService, etc.)
Service/             → Service implementations
DomainInterface/     → Repository interfaces (IGenericRepository, etc.)
Domain/              → Repository implementations (EF Core)
```

### Layer responsibilities

| Layer | Project | Responsibility |
|---|---|---|
| Presentation | `Flashcards` | Razor Pages, page models, HTML/CSS/JS |
| Application | `Service` / `ServiceInterface` | Business logic, orchestration |
| Data access | `Domain` / `DomainInterface` | EF Core repositories |
| Domain model | `Model` | Entities, `ApplicationDbContext` |

### Dependency flow

```
Flashcards (Web)
    └── ServiceInterface (ISubjectService, IChapterService, IFlashcardService)
            └── DomainInterface (IGenericRepository, IChapterRepository, etc.)
                    └── Model (Subject, Chapter, Flashcard, ApplicationDbContext)
```

Concrete implementations (`Service`, `Domain`) are registered in `Program.cs` via dependency injection and are never referenced directly by the web layer.

---

## Data Model

```
Subject
 ├── Id, Name, Description
 ├── Chapters  (one-to-many)
 └── Flashcards (one-to-many)

Chapter
 ├── Id, Name, Description, SubjectId
 └── Flashcards (one-to-many)

Flashcard
 ├── Id, Front, Back
 ├── SubjectId, ChapterId
 ├── SuccessRate, numSuccess, numFailure
 └── IsRevisit
```

**Cascade behaviour:** deleting a Subject cascades to its Chapters, which in turn cascades to their Flashcards. The direct Subject → Flashcard foreign key is set to `NoAction` to avoid a multiple-cascade-path conflict in SQL Server.

---

## Pages

| Page | Route | Description |
|---|---|---|
| Index | `/` | Lists all subjects with Study / Update / Delete actions |
| Create Subject | `/CreateSubject` | Form to create a new subject |
| Create Chapter | `/CreateChapter` | Form to create a chapter under a subject |
| Create Flashcards | `/CreateFlashcards` | Select subject + chapter, add multiple flashcards in a table |
| Update Deck | `/UpdateDeck?subjectId=X` | Edit existing flashcards, add new ones, add chapters |
| Study | `/Study` | Flip-card study session with progress tracking |

---

## Key Patterns

### Generic repository + service
`GenericRepository<T>` and `GenericService<T>` provide `GetAllAsync`, `GetByIdAsync`, `AddAsync`, `AddRangeAsync`, `UpdateAsync`, and `DeleteAsync` for any entity. Entity-specific services (e.g. `ChapterService`) extend these with custom queries like `GetChaptersBySubjectId`.

### Razor Pages handlers
Each page uses named handlers for distinct operations:

- `OnGetAsync` — load page data
- `OnPostAsync` — primary form submit
- `OnPostDeleteSubjectAsync`, `OnPostDeleteFlashcardAsync` — targeted deletes
- `OnPostAddChapterAsync` — modal form submit
- `OnGetChaptersBySubject` — AJAX endpoint returning JSON for dynamic dropdowns

### Model binding
`[BindProperty]` is used on page model properties. Navigation properties (`Subject`, `Chapter`) are decorated with `[ValidateNever]` to prevent model validation from failing when those objects are not present in a form POST.

---

## Tech Stack

- **ASP.NET Core 9** — Razor Pages
- **Entity Framework Core** — SQL Server provider, code-first
- **Bootstrap 5** — layout utilities (navbar, grid)
- **Custom CSS** — dark-mode UI with a consistent indigo (`#6366f1`) accent colour

---

## Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server (local or remote)

### Setup

1. Clone the repository.
2. Set the connection string in `Flashcards/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=...;Database=Flashcards;..."
   }
   ```
3. The app calls `db.Database.Migrate()` on startup, so the schema is created automatically on first run. If you are using EF migrations via the Package Manager Console:
   ```
   Add-Migration InitialCreate -StartupProject Flashcards
   Update-Database -StartupProject Flashcards
   ```
4. Run the `Flashcards` project.
