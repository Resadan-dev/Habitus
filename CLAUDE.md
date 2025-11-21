# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Habitus** (codenamed Valoron) is a .NET 10.0 application that transforms real-life daily actions into immersive RPG mechanics. Every task completed IRL (cleaning, sports, nutrition, social interactions, administrative tasks, personal projects) becomes an **activity** that impacts an avatar in a fantasy world. The player progresses, unlocks skills, gains loot, and participates in dynamic storytelling by accomplishing their own objectives.

**Current Status:** Early MVP phase - foundational domain model implemented, application and infrastructure layers awaiting implementation.

The goal: make personal progression **motivational, playful, and coherent** through a unified business system based on daily activities.

## Domain Vision

### The Four Pillars of Real Life

The domain models real-life activities across eight categories that map to four conceptual pillars:

#### 💠 1. Environment (Cleanup / Home / Organization)
- **Category Code:** `ENV`
- **Activities:** Cleaning, organizing, home maintenance, paperwork related to housing
- **RPG Impact:** Order, stability, Chaos reduction

#### 💠 2. Body (Sports / Health / Nutrition / Hygiene)
Three distinct categories:
- **Body** (`BODY`): Gym sessions, walking, cycling, structured routines → Strength/endurance
- **Nutrition** (`NUTR`): Meals, hydration, nutritional goals → Vitality/buffs
- **Hygiene** (`HYG`): Shower, grooming, dental care → Maintenance buffs

#### 💠 3. Mind (Social / Administrative / Learning / Projects)
Four distinct categories:
- **Social** (`SOC`): Messages, calls, outings, relationship maintenance → Charisma
- **Administrative** (`ADM`): Bills, documents, taxes, important emails → Intelligence
- **Learning** (`LRN`): Studies, training, personal creation → Wisdom
- **Project** (`PROJ`): Personal projects, creative work → Specialized progression

#### 💠 4. Life Goals (Long Quests / Seasons)
- Monthly/annual objectives, complex projects, milestones
- **RPG Impact:** Narrative arcs, major bosses, thematic seasons

### Core RPG Mechanics

1. **Activities → RPG Actions**: Each IRL activity has difficulty (1-10) → XP, loot, narrative events
2. **Classes & Psychology**: Each class represents a productivity style (Chronomancer, Warrior of Order, Archivist, Alchemist, Urban Ranger)
3. **IRL Combats & Buffs**: Sports sessions → strength buffs, balanced meals → restoration, cleaning → Chaos reduction
4. **Chaos Meter**: Increases with procrastination, unlocks mini-bosses and corrupted zones

## Current Implementation Status

### ✅ Implemented

#### BuildingBlocks (`src/BuildingBlocks/Valoron.BuildingBlocks/`)
- **`Entity.cs`**: Abstract base class for all domain entities
  - GUID-based identity (`Id` property)
  - `IsTransient()` method to detect unsaved entities
  - Proper equality comparison based on ID and type
  - Overloaded equality operators
  - Type-safe `GetHashCode()` implementation

- **`ValueObject.cs`**: Abstract base class for value objects
  - Component-based equality via `GetEqualityComponents()`
  - Value semantics (equality by content, not identity)
  - Overloaded equality operators

#### Activities Domain (`src/Modules/Activities/Valoron.Activities.Domain/`)

**`Activity.cs`** - Aggregate Root
```csharp
public class Activity : Entity
{
    public string Title { get; private set; }
    public ActivityCategory Category { get; private set; }
    public Difficulty Difficulty { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public bool IsCompleted => CompletedAt.HasValue;
}
```

**Invariants enforced:**
- Title cannot be null or whitespace
- Cannot change difficulty of completed activities
- Auto-timestamps creation and completion
- Idempotent completion (calling `Complete()` multiple times is safe)

**Methods:**
- `Complete()`: Marks activity as completed with timestamp (TODO: domain event)
- `UpdateDifficulty(Difficulty)`: Changes difficulty if not yet completed

---

**`ActivityCategory.cs`** - Value Object

The 8 predefined categories with static factory properties:

| Category | Code | Name | Pillar |
|----------|------|------|--------|
| `Environment` | ENV | Environment | Environment |
| `Body` | BODY | Body | Body |
| `Nutrition` | NUTR | Nutrition | Body |
| `Hygiene` | HYG | Hygiene | Body |
| `Social` | SOC | Social | Mind |
| `Admin` | ADM | Administrative | Mind |
| `Learning` | LRN | Learning | Mind |
| `Project` | PROJ | Project | Mind |

**Usage:**
```csharp
var category = ActivityCategory.Environment;
// or specific: ActivityCategory.Body, .Nutrition, .Social, etc.
```

**Equality:** Based on `Code` property only

---

**`Difficulty.cs`** - Value Object

Difficulty scale from 1 (easiest) to 10 (hardest).

**Factory method:**
```csharp
Difficulty.Create(5); // Custom difficulty
```

**Presets:**
```csharp
Difficulty.Easy;   // Value = 1
Difficulty.Medium; // Value = 5
Difficulty.Hard;   // Value = 8
```

**Invariant:** Value must be between 1 and 10 (throws `ArgumentException` otherwise)

### 🚧 Empty Placeholders (Awaiting Implementation)

- `Valoron.Activities.Application/Class1.cs` - Application layer (use cases, DTOs, handlers)
- `Valoron.Activities.Infrastructure/Class1.cs` - Infrastructure layer (repositories, EF Core, events)
- `Valoron.Activities.Tests/UnitTest1.cs` - Unit tests
- `Valoron.Api/Program.cs` - Only has "Hello World" endpoint

### 📋 Planned Bounded Contexts (Not Yet Created)

1. **Body** - Physical activities with workout tracking
2. **Social** - Relationship management
3. **RPG Core** - Stats, classes, leveling, skills, effects
4. **Narrative** - Dynamic story generation based on behavior
5. **Inventory** - Items, equipment, crafting
6. **Economy** - Currency, shops, pricing
7. **Achievements** - Badges, streaks, milestones

## Architecture

### Modular Monolith with DDD

This application follows Domain-Driven Design with multiple Bounded Contexts organized as a modular monolith.

#### 🔷 Bounded Context: **Activity** (Primary BC - Implemented)
Manages all IRL activities classified into 8 categories.

**Current Domain Model:**
- Aggregate Root: `Activity`
- Value Objects: `ActivityCategory`, `Difficulty`

**Planned Domain Events:**
- `ActivityCompleted` (TODO in code)
- `ActivityCreated`
- `DifficultyUpdated`

**Activity → Stats Mapping (for RPG Core integration):**
- Environment → Chaos reduction
- Body → Strength/endurance
- Nutrition → Vitality/buffs
- Hygiene → Maintenance buffs
- Social → Charisma
- Administrative → Intelligence
- Learning → Wisdom
- Project → Specialized skills

### AI Personal Coach (Microsoft Agent Framework)

Habitus includes an **AI Personal Coach** orchestrated by **Microsoft Agent Framework**, which acts as an intelligent layer that observes player behavior, analyzes patterns, and provides personalized guidance.

#### Framework Overview
- **Microsoft Agent Framework**: Open-source SDK for creating AI agents and multi-agent workflows (compatible with .NET and Python)
- Brings together ideas from Semantic Kernel and AutoGen projects
- Supports graph-based workflows for complex multi-agent orchestration
- Package: `Microsoft.Agents.AI`

#### Coach Responsibilities
The Personal Coach leverages LLMs to:
- **Analyze activity patterns**: Detects trends, streaks, gaps in the four pillars
- **Provide personalized suggestions**: Recommends next activities based on current state (Chaos level, stats, time of day)
- **Generate motivational content**: Creates contextual encouragement aligned with narrative themes
- **Suggest custom quests**: Proposes short-term challenges adapted to player's habits and goals
- **Adapt difficulty**: Adjusts activity recommendations based on past performance and current buffs/debuffs
- **Detect procrastination**: Identifies when Chaos is rising and suggests preventive actions

#### Integration with Bounded Contexts
The coach consumes domain events from all BCs:
- `ActivityCompleted` → Updates player model, identifies patterns
- `WorkoutCompleted` → Suggests recovery or progressive overload
- `MealLogged` → Provides nutritional feedback
- `ChaosMeterIncreased` → Triggers urgent recommendations
- `AchievementUnlocked` → Celebrates and suggests next milestones

The coach can invoke actions through Application layer services but never modifies domain state directly—it acts as a read-model consumer and suggestion generator.

#### Multi-Agent Workflow Architecture
The framework's graph-based workflows enable:
- **Concurrent analysis**: Multiple specialized agents analyze different aspects (nutrition agent, fitness agent, productivity agent)
- **Sequential reasoning**: Agents can hand-off to specialists for deeper analysis
- **Human-in-the-loop**: Coach can request user clarification before making suggestions
- **Checkpointing**: Long-running analysis can be paused and resumed

### Clean Architecture Layers

Each module follows Clean Architecture with three layers:

1. **Domain** (`*.Domain.csproj`)
   - Pure business logic, entities, value objects, aggregates
   - Depends only on `Valoron.BuildingBlocks`
   - No dependencies on infrastructure concerns
   - Currently: Activity aggregate fully modeled

2. **Application** (`*.Application.csproj`)
   - Use cases, application services, DTOs, interfaces
   - Depends on Domain layer
   - Defines repository interfaces (implemented in Infrastructure)
   - Currently: Empty placeholder

3. **Infrastructure** (`*.Infrastructure.csproj`)
   - Repository implementations, EF Core, external services
   - Depends on Application layer
   - Implements interfaces defined in Application
   - Currently: Empty placeholder

The **API project** (`Valoron.Api`) references both Application and Infrastructure layers of each module.

## Solution Structure

```
Habitus/
├── Valoron.sln
├── CLAUDE.md
│
├── src/
│   ├── Valoron.Api/                                    # ASP.NET Core Web API
│   │   ├── Program.cs                                  # Minimal API (Hello World)
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   └── Properties/launchSettings.json
│   │
│   ├── BuildingBlocks/
│   │   └── Valoron.BuildingBlocks/
│   │       ├── Entity.cs                               # Base entity with GUID identity
│   │       ├── ValueObject.cs                          # Base value object
│   │       └── Valoron.BuildingBlocks.csproj
│   │
│   └── Modules/
│       └── Activities/                                  # Primary Bounded Context
│           ├── Valoron.Activities.Domain/
│           │   ├── Activity.cs                         # Aggregate root ✅
│           │   ├── ActivityCategory.cs                 # Value object (8 categories) ✅
│           │   ├── Difficulty.cs                       # Value object (1-10) ✅
│           │   └── Valoron.Activities.Domain.csproj
│           │
│           ├── Valoron.Activities.Application/
│           │   ├── Class1.cs                           # Empty placeholder 🚧
│           │   └── Valoron.Activities.Application.csproj
│           │
│           └── Valoron.Activities.Infrastructure/
│               ├── Class1.cs                           # Empty placeholder 🚧
│               └── Valoron.Activities.Infrastructure.csproj
│
└── tests/
    └── Valoron.Activities.Tests/
        ├── UnitTest1.cs                                # Empty placeholder 🚧
        └── Valoron.Activities.Tests.csproj
```

## Common Commands

### Build
```bash
dotnet build Valoron.sln
```

### Run API
```bash
dotnet run --project src/Valoron.Api/Valoron.Api.csproj
```

**API Endpoints:**
- HTTP: `http://localhost:5183`
- HTTPS: `https://localhost:7117`

**Current endpoints:**
- `GET /` → "Hello World!"

### Run Tests
```bash
# Run all tests
dotnet test

# Run tests for specific project
dotnet test tests/Valoron.Activities.Tests/Valoron.Activities.Tests.csproj

# Run specific test by name
dotnet test --filter "FullyQualifiedName~TestName"

# Run with coverage
dotnet test /p:CollectCoverage=true
```

### Restore Dependencies
```bash
dotnet restore
```

### Watch Mode (Auto-reload)
```bash
dotnet watch --project src/Valoron.Api/Valoron.Api.csproj
```

## Development Guidelines

### Adding New Modules (Bounded Contexts)
When creating a new module (e.g., "Body", "RPGCore", "Social"):

1. Create module folder: `src/Modules/NewModule/`
2. Create three projects with naming convention:
   - `Valoron.NewModule.Domain` (class library)
   - `Valoron.NewModule.Application` (class library)
   - `Valoron.NewModule.Infrastructure` (class library)
3. Set up project references:
   - Domain → references `Valoron.BuildingBlocks` only
   - Application → references Domain
   - Infrastructure → references Application
4. Update API project to reference Application and Infrastructure
5. Add test project: `tests/Valoron.NewModule.Tests/`
6. Update solution file to include all projects in proper folders

### Domain Modeling Best Practices

#### Entities
- Inherit from `Valoron.BuildingBlocks.Entity`
- Use `protected set` for properties to enforce encapsulation
- Validate invariants in constructors
- Use private parameterless constructor for EF Core
- Methods should enforce business rules
- Check `IsTransient()` before operations requiring persistence

**Example:**
```csharp
public class MyEntity : Entity
{
    public string Name { get; private set; }

    private MyEntity() { } // EF Core

    public MyEntity(Guid id, string name) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name required");
        Name = name;
    }
}
```

#### Value Objects
- Inherit from `Valoron.BuildingBlocks.ValueObject`
- Implement `GetEqualityComponents()`
- Make properties immutable (`get` only or `private set`)
- Use static factory methods or properties for common instances
- Validate constraints in constructor

**Example:**
```csharp
public class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        if (!IsValid(value))
            throw new ArgumentException("Invalid email");
        Value = value;
    }

    public static Email Create(string value) => new Email(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
```

#### Aggregates
- Define clear boundaries - one aggregate root per consistency boundary
- Root entity manages all invariants within the aggregate
- External objects can only reference the root
- Child entities cannot be modified without going through the root

#### Domain Events
- Raise events for significant business occurrences
- Events are facts that have happened (past tense naming: `ActivityCompleted`)
- Other Bounded Contexts subscribe to events for loose coupling
- TODO: Implement event infrastructure in BuildingBlocks

### Business Rules & Invariants

**Currently Enforced:**
- Activity title cannot be empty (Activity.cs:12)
- Activity difficulty cannot be changed after completion (Activity.cs:31)
- Difficulty must be 1-10 (Difficulty.cs:9)
- Category codes are immutable and predefined (ActivityCategory.cs)

**To Be Enforced:**
- No duplicate activities in same time slot
- Stats must remain within valid ranges
- XP calculations follow difficulty curve
- Chaos meter increases with procrastination patterns

### Testing Strategy

Use xUnit with the following test categories:

1. **Unit Tests** - Domain logic in isolation
   - Test entity invariants
   - Test value object equality
   - Test business rule enforcement
   - Mock-free when possible

2. **Integration Tests** - Application + Infrastructure
   - Test use cases with real DB (in-memory or test container)
   - Test repository implementations
   - Test event handlers

3. **API Tests** - Full stack
   - Test endpoints
   - Test serialization
   - Test validation

### Domain Events Pattern (Planned)

1. Add `DomainEvent` base class to BuildingBlocks
2. Add `List<DomainEvent>` to Entity base class
3. Raise events in aggregate methods:
   ```csharp
   AddDomainEvent(new ActivityCompletedEvent(this));
   ```
4. Dispatch events after SaveChanges in Infrastructure
5. Handlers in other BCs react to events

## Technology Stack

- **.NET 10.0**: Target framework (latest)
- **C# 13**: Language version with nullable reference types enabled
- **ASP.NET Core**: Web API framework (minimal APIs)
- **Microsoft Agent Framework** (`Microsoft.Agents.AI`): AI agent orchestration for personal coach (planned)
- **Entity Framework Core**: ORM for data persistence (planned)
- **xUnit**: Testing framework (2.9.3)
- **Coverlet**: Code coverage collector (6.0.4)

**Compiler Settings (all projects):**
- `<Nullable>enable</Nullable>` - Null safety
- `<ImplicitUsings>enable</ImplicitUsings>` - Auto-import common namespaces
- `<TargetFramework>net10.0</TargetFramework>`

## Next Steps (Immediate Priorities)

### Phase 1: Complete Activity Bounded Context
1. **Application Layer**
   - [ ] Create `CreateActivityCommand` and handler
   - [ ] Create `CompleteActivityCommand` and handler
   - [ ] Create `UpdateActivityDifficultyCommand` and handler
   - [ ] Create `GetActivityQuery` and handler
   - [ ] Create DTOs (ActivityDto, CreateActivityDto, etc.)

2. **Infrastructure Layer**
   - [ ] Add EF Core packages
   - [ ] Create `ActivitiesDbContext` with Activity entity configuration
   - [ ] Implement `IActivityRepository`
   - [ ] Add migrations

3. **API Endpoints**
   - [ ] POST `/api/activities` - Create activity
   - [ ] GET `/api/activities/{id}` - Get activity
   - [ ] GET `/api/activities` - List activities (with filters)
   - [ ] PUT `/api/activities/{id}/complete` - Complete activity
   - [ ] PUT `/api/activities/{id}/difficulty` - Update difficulty
   - [ ] DELETE `/api/activities/{id}` - Delete activity

4. **Tests**
   - [ ] Unit tests for Activity aggregate
   - [ ] Unit tests for ActivityCategory
   - [ ] Unit tests for Difficulty
   - [ ] Integration tests for commands/queries
   - [ ] API endpoint tests

5. **Domain Events Infrastructure**
   - [ ] Add `IDomainEvent` interface to BuildingBlocks
   - [ ] Add domain events collection to Entity base class
   - [ ] Create `ActivityCompletedEvent`
   - [ ] Implement event dispatcher in Infrastructure

### Phase 2: Second Bounded Context
Choose one of:
- **RPG Core** (stats, leveling) - enables immediate gamification
- **Body** (workouts, nutrition tracking) - completes body pillar
- **Achievements** (streaks, badges) - adds motivation layer

### Phase 3: AI Coach Integration
1. Add `Microsoft.Agents.AI` package
2. Create Coach module
3. Set up event subscriptions
4. Implement suggestion generation
5. Create coach API endpoints

## Git Workflow

**Current Branch:** master
**Latest Commits:**
- `79f362d` - Add ValueObject abstraction
- `4b41796` - Add CLAUDE.md
- `45521b7` - Add Entity class
- `41bac43` - Add project files

**Recommended workflow:**
- Create feature branches: `feature/activity-commands`, `feature/ef-core-setup`
- Keep commits focused and atomic
- Use conventional commit messages: `feat:`, `fix:`, `refactor:`, `test:`

## References

- **Microsoft Agent Framework**: https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview
- **DDD Patterns**: Entity, Value Object, Aggregate, Domain Event, Repository
- **Clean Architecture**: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
