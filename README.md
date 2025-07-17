# 🛠 Infonetica Workflow Engine

A minimal backend service built using **.NET 8 Minimal APIs** to define and manage configurable **state-machine workflows**.

---

## ✅ Features

- Define workflows (states + transitions)
- Start new instances from workflow definitions
- Execute valid state transitions (actions)
- Fully validated definitions and runtime rules
- In-memory persistence (no external DB)
- Modular, cleanly layered project structure

---

## 📁 Folder Structure

```
INFONETICAASSIGNMENT/
├── WorkflowEngine/                          ← Main project directory (contains .csproj, source code)
│   ├── WorkflowEngine.csproj                ← .NET project file
│   ├── Program.cs                           ← Minimal API entry point (app + endpoint wiring)
│
│   ├── Models/                              ← Domain models (core entities)
│   │   ├── State.cs                         ← Represents a state in the workflow
│   │   ├── ActionTransition.cs              ← Represents a state transition/action
│   │   ├── WorkflowDefinition.cs            ← Workflow definition (states + actions)
│   │   └── WorkflowInstance.cs              ← Runtime instance of a workflow
│
│   ├── Persistence/                         ← In-memory data store
│   │   ├── IRepository.cs                   ← Generic repository interface
│   │   └── InMemoryRepository.cs            ← In-memory implementation using dictionary
│
│   ├── Services/                            ← Business logic and validation
│   │   ├── DefinitionValidator.cs           ← Enforces definition rules
│   │   └── WorkflowService.cs               ← Manages instance creation, execution, transitions
│
│   ├── Extensions/                          ← DI and routing helpers
│   │   ├── ServiceCollectionExtensions.cs   ← Registers services in DI container
│   │   └── EndpointRouteBuilderExtensions.cs← Maps REST API endpoints
│
│   ├── WorkflowEngine.http                  ← (Optional) REST Client test file for VS Code
│   └── README.md                            ← This file
````

---

## 🚀 Getting Started

### 🔧 Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### ▶️ Run the project

```bash
dotnet run --project WorkflowEngine
```

Default address:

```
http://localhost:5222
```

---

## 📬 API Usage

### ✅ Health Check

```http
GET http://localhost:5222/
```

---

### 🔧 Create Workflow Definition

```http
POST http://localhost:5222/api/definitions
Content-Type: application/json
```

```json
{
  "id": "sample-workflow",
  "states": [
    { "id": "start", "name": "Start", "isInitial": true, "isFinal": false, "enabled": true },
    { "id": "end",   "name": "End",   "isInitial": false, "isFinal": true, "enabled": true }
  ],
  "actions": [
    {
      "id": "advance",
      "name": "Advance",
      "fromStates": ["start"],
      "toState": "end",
      "enabled": true
    }
  ]
}
```

---

### 🆕 Start Workflow Instance

```http
POST http://localhost:5222/api/definitions/sample-workflow/instances
```

---

### 🔁 Execute Action (Transition)

```http
POST http://localhost:5222/api/instances/{instanceId}/actions/advance
```

---

### 🔎 Get Instance Status & History

```http
GET http://localhost:5222/api/instances/{instanceId}
```

---

## 🛑 Validation Rules

| Rule              | Description                                                 |
| ----------------- | ----------------------------------------------------------- |
| One initial state | Must be exactly one `isInitial: true` state per definition  |
| Unique IDs        | State and action IDs must be unique                         |
| Valid transitions | Transitions must reference valid, enabled states            |
| Final state lock  | Cannot execute actions from a final state                   |
| Disabled elements | Disabled states/actions are ignored in validation/execution |

---

## 💾 Persistence

* Stores data in memory using a thread-safe dictionary
* Uses clean `IRepository<T>` abstraction for testability
* No files or databases used (per assignment spec)

---

## 🧪 Manual Testing

You can use:

* ✅ `curl`
* ✅ Postman
* ✅ VS Code + [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)

Example `.http` test file:

```http
### Health Check
GET http://localhost:5222/

### Create Workflow
POST http://localhost:5222/api/definitions
Content-Type: application/json

{
  "id": "sample-workflow",
  "states": [
    { "id": "start", "name": "Start", "isInitial": true, "isFinal": false, "enabled": true },
    { "id": "end",   "name": "End",   "isInitial": false, "isFinal": true, "enabled": true }
  ],
  "actions": [
    { "id": "advance", "name": "Advance", "fromStates": ["start"], "toState": "end", "enabled": true }
  ]
}

### Start Instance
POST http://localhost:5222/api/definitions/sample-workflow/instances

### Execute Action
POST http://localhost:5222/api/instances/{instanceId}/actions/advance

### Get Instance
GET http://localhost:5222/api/instances/{instanceId}
```

---

## 📌 Assumptions

* One initial state is mandatory per workflow
* Transitions are only valid from current enabled state to a valid target
* Final states are terminal; transitions from them are blocked
* No persistence beyond in-memory; restarts clear state
* No authentication or concurrency handling (out of scope)

---

## 📎 Submission Notes

* ⏱️ Time-boxed to \~2 hours as instructed
* ✅ All functional and validation requirements covered
* 📁 Modular and clean structure for easy maintenance
* 🔒 Fully in-memory; no external systems or dependencies
* 📄 This `README.md` documents all routes, structure, and rules

---

## 👤 Author

Submitted by **Aneesh Singh Rajoriya**
for the **Infonetica Software Engineer Intern Assignment**

```
