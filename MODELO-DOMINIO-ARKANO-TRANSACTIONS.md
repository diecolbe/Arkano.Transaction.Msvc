# 🏗️ Modelo de Dominio - Arkano Transactions System

## 📋 Tabla de Contenidos

1. [Visión General del Dominio](#visión-general)
2. [Agregados y Entidades](#agregados-entidades)
3. [Value Objects](#value-objects)
4. [Servicios de Dominio](#servicios-dominio)
5. [Eventos de Dominio](#eventos-dominio)
6. [Repositorios](#repositorios)
7. [Reglas de Negocio](#reglas-negocio)
8. [Diagrama de Clases UML](#diagrama-uml)

---

## 1. Visión General del Dominio {#visión-general}

### 🎯 **Contexto Acotado (Bounded Context)**
**Arkano Transactions** - Sistema de procesamiento de transacciones financieras con validación antifraude.

### 🏛️ **Subdominios**
- **Core Domain**: Procesamiento de Transacciones
- **Supporting Domain**: Validación Antifraude
- **Generic Domain**: Notificaciones y Auditoría

### 📐 **Principios de Diseño**
- **Domain-Driven Design (DDD)**
- **Aggregate Pattern** para consistencia transaccional
- **Event Sourcing** para auditabilidad
- **CQRS** para separación de responsabilidades

---

## 2. Agregados y Entidades {#agregados-entidades}

### 🔵 **Agregado: Transaction**

#### **Transaction (Aggregate Root)**
```csharp
public class Transaction
{
    // Identificadores
    public Guid TransactionExternalId { get; private set; } = Guid.NewGuid();
    public Guid SourceAccountId { get; private set; }
    public Guid TargetAccountId { get; set; }
    
    // Propiedades de Valor
    public decimal Value { get; set; }
    public TransactionStatus Status { get; private set; } = TransactionStatus.Pending;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    // Métodos de Dominio
    public void Approve() => Status = TransactionStatus.Approved;
    public void Reject() => Status = TransactionStatus.Rejected;
    
    // Invariantes de Dominio
    public bool SourceAccountIdIsEmpty() => SourceAccountId == Guid.Empty;
    public bool TargetAccountIdIsEmpty() => TargetAccountId == Guid.Empty;
    public bool ValueIsCeroOrLess() => Value <= 0;
}
```

**📋 Responsabilidades:**
- ✅ Mantener estado consistente de la transacción
- ✅ Aplicar reglas de negocio de validación
- ✅ Controlar transiciones de estado
- ✅ Generar eventos de dominio implícitos

**🔒 Invariantes:**
1. `TransactionExternalId` debe ser único y no vacío
2. `SourceAccountId` y `TargetAccountId` deben ser válidos
3. `Value` debe ser mayor que cero
4. Solo transiciones de estado válidas: `Pending → Approved|Rejected`

### 🟡 **Entidad: Account**

```csharp
public class Account
{
    public Guid AccountId { get; private set; }
    public string OwnerName { get; private set; }
    
    // Constructor
    public Account(Guid accountId, string ownerName)
    {
        AccountId = accountId;
        OwnerName = ownerName;
    }
}
```

**📋 Responsabilidades:**
- ✅ Identificar únicamente una cuenta
- ✅ Mantener información básica del propietario
- ✅ Servir como referencia para transacciones

---

## 3. Value Objects {#value-objects}

### 🟢 **TransactionStatus (Enum)**

```csharp
public enum TransactionStatus
{
    Pending = 1,    // Estado inicial tras creación
    Approved = 2,   // Aprobada tras validación antifraude
    Rejected = 3    // Rechazada por validación antifraude
}
```

**📋 Estados y Transiciones:**
```
Pending ──┐
          ├─→ Approved
          └─→ Rejected
```

### 🟢 **AntifraudOptions (Configuration Value Object)**

```csharp
public class AntifraudOptions
{
    public decimal MaxTransactionValue { get; set; } = 2000m;
    public decimal MaxDailyAccumulation { get; set; } = 20000m;
}
```

### 🟢 **ValidationDataDto (Value Object)**

```csharp
public record ValidationDataDto
{
    public Guid TransactionExternalId { get; set; }
    public bool IsValid { get; set; }
    public string ValidationReason { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
```

### 🟢 **AntifraudResultDto (Value Object)**

```csharp
public record AntifraudResultDto
{
    public Guid TransactionExternalId { get; set; }
    public decimal TransactionValue { get; set; }
    public bool IsValid { get; set; }
    public string ValidationReason { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
}
```

---

## 4. Servicios de Dominio {#servicios-dominio}

### ⚙️ **TransactionService**

```csharp
[DomainService]
public class TransactionService
{
    // Crear transacción y publicar evento
    public async Task<Guid> CreateAsync(Transaction transaction, CancellationToken cancellationToken = default);
    
    // Consultar por ID externo y fecha
    public async Task<Transaction?> GetByExternalIdAndCreateAtAsync(Guid externalId, DateTime createAt, CancellationToken cancellationToken = default);
    
    // Obtener estado de transacción
    public async Task<TransactionStatus> GetTransactionStatusByIdAsync(Guid externalId, CancellationToken cancellationToken = default);
}
```

**📋 Responsabilidades:**
- ✅ Orquestar creación de transacciones
- ✅ Calcular totales diarios para antifraude
- ✅ Publicar eventos de dominio
- ✅ Coordinar con repositorios

### ⚙️ **AntifraudValidationService**

```csharp
[DomainService]
public class AntifraudValidationService
{
    public AntifraudResultDto ValidateTransaction(TransactionCreatedDto transaction);
}
```

**🛡️ Reglas de Validación:**
1. **Límite por Transacción**: `Value ≤ MaxTransactionValue`
2. **Límite Diario**: `DailyTotal + Value ≤ MaxDailyAccumulation`
3. **Cuentas Válidas**: `SourceAccount ≠ TargetAccount`

### ⚙️ **TransactionStatusService**

```csharp
public class TransactionStatusService
{
    public async Task Update(Guid transactionExternalId, string status, CancellationToken cancellationToken);
}
```

---

## 5. Eventos de Dominio {#eventos-dominio}

### 📡 **TransactionCreatedEvent**

```csharp
public record TransactionCreatedEvent(TransactionCreatedDto Data) : IEvent
{
    public string Subject => $"transaction.created.{Data.TransactionExternalId}";
    public DateTime Timestamp => DateTime.UtcNow;
}
```

**📦 Payload:**
```csharp
public record TransactionCreatedDto(
    Guid TransactionExternalId,
    decimal Value,
    string Status,
    decimal TotalValueDaily
);
```

### 📡 **TransactionValidatedEvent**

```csharp
public record TransactionValidatedEvent(object Data) : IEvent
{
    public string Subject => "transaction.validated";
    public DateTime Timestamp => DateTime.UtcNow;
}
```

**📦 Payload:**
- `ValidationDataDto` serializado como objeto

---

## 6. Repositorios (Ports) {#repositorios}

### 🔌 **ITransactionRepository**

```csharp
public interface ITransactionRepository
{
    // Operaciones CRUD
    Task<Guid> CreateAsync(Transaction transaction, CancellationToken cancellationToken = default);
    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default);
    
    // Consultas especializadas
    Task<Transaction?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);
    Task<Transaction?> GetByExternalIdAndCreateAtAsync(Guid externalId, DateTime createdAt, CancellationToken cancellationToken = default);
}
```

### 🔌 **IDailyTotalRepository**

```csharp
public interface IDailyTotalRepository
{
    Task<decimal> GetDailyTotalAmountAsync(Guid accountExternalId, CancellationToken cancellationToken = default);
}
```

### 🔌 **IEventBus**

```csharp
public interface IEventBus
{
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class, IEvent;
}
```

---

## 7. Reglas de Negocio {#reglas-negocio}

### 📋 **Reglas de Creación de Transacciones**

#### **RN-001: Validación de Cuentas**
```csharp
// ❌ Casos inválidos
sourceAccountId == Guid.Empty         // Cuenta origen vacía
targetAccountId == Guid.Empty         // Cuenta destino vacía
sourceAccountId == targetAccountId    // Misma cuenta origen y destino
```

#### **RN-002: Validación de Montos**
```csharp
// ❌ Casos inválidos
value <= 0                           // Valor cero o negativo
value > MaxTransactionValue          // Excede límite por transacción
```

#### **RN-003: Estado Inicial**
```csharp
// ✅ Estado por defecto
Status = TransactionStatus.Pending   // Siempre inicia como Pending
CreatedAt = DateTime.UtcNow          // Fecha UTC de creación
```

### 📋 **Reglas de Validación Antifraude**

#### **RN-004: Límite por Transacción**
```csharp
if (transaction.Value > antifraudOptions.MaxTransactionValue)
{
    return new AntifraudResult
    {
        IsValid = false,
        ValidationReason = $"Valor {transaction.Value:C} excede límite de {MaxTransactionValue:C}"
    };
}
```

#### **RN-005: Límite Diario Acumulado**
```csharp
var projectedTotal = dailyTotal + transaction.Value;
if (projectedTotal > antifraudOptions.MaxDailyAccumulation)
{
    return new AntifraudResult
    {
        IsValid = false,
        ValidationReason = $"Acumulado diario {projectedTotal:C} excede límite de {MaxDailyAccumulation:C}"
    };
}
```

### 📋 **Reglas de Transiciones de Estado**

#### **RN-006: Transiciones Válidas**
```csharp
// ✅ Transiciones permitidas
Pending → Approved    // Tras validación exitosa
Pending → Rejected    // Tras validación fallida

// ❌ Transiciones prohibidas
Approved → Rejected   // No se puede cambiar de aprobada a rechazada
Rejected → Approved   // No se puede cambiar de rechazada a aprobada
Approved → Pending    // No se puede regresar a pendiente
Rejected → Pending    // No se puede regresar a pendiente
```

---

## 8. Diagrama de Clases UML {#diagrama-uml}

```mermaid
classDiagram
    %% Agregado Transaction
    class Transaction {
        <<AggregateRoot>>
        +Guid TransactionExternalId
        +Guid SourceAccountId
        +Guid TargetAccountId
        +decimal Value
        +TransactionStatus Status
        +DateTime CreatedAt
        +Approve() void
        +Reject() void
        +SourceAccountIdIsEmpty() bool
        +TargetAccountIdIsEmpty() bool
        +ValueIsCeroOrLess() bool
    }

    %% Entidad Account
    class Account {
        <<Entity>>
        +Guid AccountId
        +string OwnerName
    }

    %% Value Objects
    class TransactionStatus {
        <<Enumeration>>
        Pending
        Approved
        Rejected
    }

    class ValidationDataDto {
        <<ValueObject>>
        +Guid TransactionExternalId
        +bool IsValid
        +string ValidationReason
        +DateTime ProcessedAt
        +string Status
    }

    class AntifraudResultDto {
        <<ValueObject>>
        +Guid TransactionExternalId
        +decimal TransactionValue
        +bool IsValid
        +string ValidationReason
        +DateTime ProcessedAt
    }

    class AntifraudOptions {
        <<ValueObject>>
        +decimal MaxTransactionValue
        +decimal MaxDailyAccumulation
    }

    %% Servicios de Dominio
    class TransactionService {
        <<DomainService>>
        +CreateAsync(Transaction) Task~Guid~
        +GetByExternalIdAndCreateAtAsync(Guid, DateTime) Task~Transaction?~
        +GetTransactionStatusByIdAsync(Guid) Task~TransactionStatus~
    }

    class AntifraudValidationService {
        <<DomainService>>
        +ValidateTransaction(TransactionCreatedDto) AntifraudResultDto
    }

    class TransactionStatusService {
        <<DomainService>>
        +Update(Guid, string, CancellationToken) Task
    }

    %% Eventos
    class TransactionCreatedEvent {
        <<DomainEvent>>
        +TransactionCreatedDto Data
        +string Subject
        +DateTime Timestamp
    }

    class TransactionValidatedEvent {
        <<DomainEvent>>
        +object Data
        +string Subject
        +DateTime Timestamp
    }

    %% Interfaces (Ports)
    class ITransactionRepository {
        <<Interface>>
        +CreateAsync(Transaction) Task~Guid~
        +UpdateAsync(Transaction) Task
        +GetByExternalIdAsync(Guid) Task~Transaction?~
        +GetByExternalIdAndCreateAtAsync(Guid, DateTime) Task~Transaction?~
    }

    class IDailyTotalRepository {
        <<Interface>>
        +GetDailyTotalAmountAsync(Guid) Task~decimal~
    }

    class IEventBus {
        <<Interface>>
        +PublishAsync~T~(string, T, CancellationToken) Task
    }

    %% Relaciones
    Transaction ||--|| TransactionStatus : status
    Transaction }|--|| Account : sourceAccount
    Transaction }|--|| Account : targetAccount
    
    TransactionService --> ITransactionRepository : uses
    TransactionService --> IDailyTotalRepository : uses
    TransactionService --> IEventBus : uses
    TransactionService --> TransactionCreatedEvent : publishes
    
    AntifraudValidationService --> AntifraudOptions : uses
    AntifraudValidationService --> AntifraudResultDto : creates
    AntifraudValidationService --> ValidationDataDto : creates
    
    TransactionStatusService --> ITransactionRepository : uses
    
    TransactionCreatedEvent --> ValidationDataDto : contains
    TransactionValidatedEvent --> AntifraudResultDto : contains
```

---

## 📊 Resumen del Modelo

### 🎯 **Elementos Principales**

| Tipo | Cantidad | Ejemplos |
|------|----------|----------|
| **Agregados** | 1 | Transaction |
| **Entidades** | 2 | Transaction, Account |
| **Value Objects** | 5 | TransactionStatus, ValidationDataDto, AntifraudResultDto, AntifraudOptions |
| **Servicios de Dominio** | 3 | TransactionService, AntifraudValidationService, TransactionStatusService |
| **Eventos** | 2 | TransactionCreatedEvent, TransactionValidatedEvent |
| **Repositorios** | 3 | ITransactionRepository, IDailyTotalRepository, IEventBus |

### 🔄 **Flujos Principales**

#### **1. Creación de Transacción**
1. `Transaction` es creada con estado `Pending`
2. `TransactionService.CreateAsync()` persiste y publica evento
3. `TransactionCreatedEvent` es enviado al bus de eventos

#### **2. Validación Antifraude**
1. `AntifraudValidationService` recibe `TransactionCreatedDto`
2. Aplica reglas de negocio (límites por transacción y diarios)
3. Genera `AntifraudResultDto` con resultado de validación
4. Publica `TransactionValidatedEvent`

#### **3. Actualización de Estado**
1. `TransactionStatusService` recibe evento de validación
2. Actualiza estado de `Transaction` a `Approved` o `Rejected`
3. Persiste cambio en repositorio

### ✅ **Principios DDD Aplicados**

- **✅ Ubiquitous Language**: Terminología consistente del dominio financiero
- **✅ Bounded Context**: Contexto acotado de transacciones financieras
- **✅ Aggregate Pattern**: Transaction como agregado raíz
- **✅ Domain Services**: Lógica compleja en servicios especializados
- **✅ Repository Pattern**: Abstracción de persistencia
- **✅ Domain Events**: Comunicación desacoplada entre contextos
- **✅ Value Objects**: Inmutabilidad y comparación por valor
- **✅ Invariantes**: Reglas de consistencia en agregados

---

*Este modelo de dominio representa la base conceptual del sistema Arkano Transactions, siguiendo principios de Domain-Driven Design para asegurar que el código refleje fielmente el conocimiento del negocio financiero.*