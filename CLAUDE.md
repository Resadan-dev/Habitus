# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Habitus** (codenamed Valoron) is a .NET 10.0 application that transforms real-life daily actions into immersive RPG mechanics. Every task completed IRL (cleaning, sports, nutrition, social interactions, administrative tasks, personal projects) becomes an **activity** that impacts an avatar in a fantasy world. The player progresses, unlocks skills, gains loot, and participates in dynamic storytelling by accomplishing their own objectives.

The goal: make personal progression **motivational, playful, and coherent** through a unified business system based on daily activities.

## Domain Vision

### The Four Pillars of Real Life

#### 💠 1. Environment (Cleanup / Home / Organization)
- Cleaning, organizing, home maintenance, paperwork related to housing
- **RPG Impact**: order, stability, Chaos reduction

#### 💠 2. Body (Sports / Health / Nutrition / Hygiene)
- **Sports**: gym sessions, walking, cycling, structured routines
- **Nutrition**: meals, hydration, nutritional goals
- **Hygiene**: shower, grooming, dental care
- **RPG Impact**: strength, vitality, endurance, temporary buffs

#### 💠 3. Mind (Social / Administrative / Learning / Projects)
- **Social**: messages, calls, outings, relationship maintenance
- **Administrative**: bills, documents, taxes, important emails
- **Learning & Projects**: studies, training, personal creation, career goals
- **RPG Impact**: charisma, wisdom, control magic, narrative progression

#### 💠 4. Life Goals (Long Quests / Seasons)
- Monthly/annual objectives, complex projects, milestones
- **RPG Impact**: narrative arcs, major bosses, thematic seasons

### Core RPG Mechanics

1. **Activities → RPG Actions**: Each IRL activity has difficulty → XP, loot, narrative events
2. **Classes & Psychology**: Each class represents a productivity style (Chronomancer, Warrior of Order, Archivist, Alchemist, Urban Ranger)
3. **IRL Combats & Buffs**: Sports sessions → strength buffs, balanced meals → restoration, cleaning → Chaos reduction
4. **Chaos Meter**: Increases with procrastination, unlocks mini-bosses and corrupted zones

## Architecture Principles

### Modular Monolith with DDD
This application follows Domain-Driven Design with multiple Bounded Contexts:

#### 🔷 Bounded Context: **Activity** (Primary BC)
Manages all IRL activities classified into categories:
- `environment`, `body`, `nutrition`, `hygiene`, `social`, `admin`, `learning`, `project`

**Key Invariants:**
- An activity belongs to exactly one category
- An activity has a difficulty level
- Completion triggers XP + potential loot

**Domain Events:**
- `ActivityCompleted`
- `WorkoutCompleted`
- `MealLogged`
- `SocialInteractionCompleted`

#### 🔷 Bounded Context: **Body**
Manages physical activities, nutrition, and hygiene with specific business rules.

#### 🔷 Bounded Context: **Social**
Manages relationships and significant social interactions.

#### 🔷 Bounded Context: **RPG Core**
Manages stats, classes, leveling, skills, and effects.

**Activity → Stats mapping:**
- Sports → strength/endurance
- Nutrition → vitality/buffs
- Social → charisma
- Cleanup → Chaos reduction
- Administrative → intelligence

#### 🔷 Bounded Context: **Narrative**
Dynamically generates narrative arcs, thematic chapters, and bosses linked to chaos or procrastination.

#### 🔷 Bounded Context: **Inventory**
Items, rarities, equipment, crafting.

#### 🔷 Bounded Context: **Economy**
Currencies, shops, pricing.

#### 🔷 Bounded Context: **Achievements**
Badges, streaks, milestones (e.g., social streak, sport streak, Full Balanced Day, hydration goal).

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
- **Domain**: Core business logic and entities (depends on BuildingBlocks only)
- **Application**: Use cases and application services (depends on Domain)
- **Infrastructure**: External concerns like persistence (depends on Application)

The API project references Application and Infrastructure layers of modules.

### BuildingBlocks
Contains shared abstractions and base classes used across modules.

**Currently includes:**
- `Entity` base class with GUID-based identity, transient entity support, proper equality comparison

## Solution Structure

```
src/
├── Valoron.Api/                              # Web API entry point
├── BuildingBlocks/
│   └── Valoron.BuildingBlocks/               # Shared domain building blocks
└── Modules/
    └── Activities/                            # Activities module (Primary BC)
        ├── Valoron.Activities.Domain/        # Domain layer
        ├── Valoron.Activities.Application/   # Application layer
        └── Valoron.Activities.Infrastructure/ # Infrastructure layer

tests/
└── Valoron.Activities.Tests/                 # xUnit tests for Activities module
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

### Run Tests
```bash
# Run all tests
dotnet test

# Run tests for specific project
dotnet test tests/Valoron.Activities.Tests/Valoron.Activities.Tests.csproj

# Run specific test
dotnet test --filter "FullyQualifiedName~TestName"
```

### Restore Dependencies
```bash
dotnet restore
```

## Development Guidelines

### Adding New Modules (Bounded Contexts)
When creating a new module (e.g., "Body", "RPGCore"):
1. Create module folder structure under `src/Modules/NewModule/`
2. Create three projects: Domain, Application, Infrastructure
3. Domain should reference `Valoron.BuildingBlocks`
4. Application should reference Domain
5. Infrastructure should reference Application
6. API project should reference Application and Infrastructure layers
7. Add test project under `tests/` with xUnit

### Domain Modeling
- **Entities**: Inherit from `Valoron.BuildingBlocks.Entity` for domain entities with identity
- **Aggregates**: Define clear boundaries and enforce invariants
- **Domain Events**: Use events for cross-BC communication (e.g., `ActivityCompleted` triggers RPG progression)
- **Value Objects**: Use for concepts without identity (e.g., difficulty level, activity category)
- Use `protected` constructors and `protected set` for properties to maintain encapsulation
- Check `IsTransient()` before persisting entities to ensure they have valid IDs

### Business Rules & Invariants
When implementing features, always enforce domain invariants:
- Activity must have valid category and difficulty
- Stats must remain within valid ranges
- XP derivation exclusively from domain events
- No duplicate activities in same time slot
- Chaos meter increases with procrastination

### Domain Events Pattern
Use domain events for loose coupling between Bounded Contexts:
- Events are raised within aggregates
- Handlers in other BCs react to events
- Example: `ActivityCompleted` → triggers XP calculation in RPG Core

## Technology Stack

- **.NET 10.0**: Target framework
- **ASP.NET Core**: Web API framework
- **Microsoft Agent Framework** (`Microsoft.Agents.AI`): AI agent orchestration for the personal coach
- **xUnit**: Testing framework with coverlet for code coverage
- **C# Features**: Nullable reference types enabled, implicit usings enabled

## Next Steps (MVP)
1. Define the **Activity** aggregate precisely
2. Detail business rules (XP, difficulty, Chaos, buffs)
3. Establish initial Domain Events
4. Define first vertical slice (MVP)
5. Prepare .NET architecture around these Bounded Contexts
