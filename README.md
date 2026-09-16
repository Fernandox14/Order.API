# E-commerce Order API

API REST para gerenciamento de reservas temporárias de produtos em um cenário de alta demanda.

## Tecnologias

* .NET 10
* C#
* ASP.NET Core Web API
* Entity Framework Core
* EF Core InMemory
* MediatR
* xUnit
* Moq
* Swagger / OpenAPI

---

## Arquitetura

A aplicação foi desenvolvida utilizando uma abordagem baseada em **Clean Architecture**, separando responsabilidades entre domínio, aplicação, infraestrutura e API.

```text
Order/
│
├── Order.Dominio
│   ├── Entities
│   │   ├── Customer.cs
│   │   ├── Product.cs
│   │   └── Reservation.cs
│   │
│   ├── Enums
│   │   ├── ProductStatus.cs
│   │   └── ReservationStatus.cs
│   │
│   └── Interfaces
│       └── Repository
│
├── Order.Aplicacao
│   ├── Handles
│   │   └── Reservation
│   │
│   ├── Queries
│   │
│   ├── DTOs
│   │
│   └── Worker
│
├── Order.InfraEstrutura
│   ├── Database
│   │   └── AppDbContext.cs
│   │
│   └── Repository
│       ├── GenericRepository.cs
│       ├── ProductRepository.cs
│       ├── CustomerRepository.cs
│       └── ReservationRepository.cs
│
├── Order.Ioc.Dependency
│   └── DependencyInjection.cs
│
├── Order.Api
│   ├── Controllers
│   │   ├── ProductsController.cs
│   │   └── CustomersController.cs
│   │
│   └── Program.cs
│
└── Order.Tests
    └── Controllers / Handlers
```

# Endpoints

## Listar produtos

```http
GET /products
```

Retorna todos os produtos e seus respectivos status.

Exemplo:

```json
[
  {
    "id": 1,
    "name": "Notebook Dell Inspiron",
    "price": 4500.00,
    "status": "Available"
  }
]
```

---

## Reservar produto

```http
POST /products/{id}/reserve?customerId={customerId}
```

Exemplo:

```http
POST /products/1/reserve?customerId=1
```

Reserva o produto para o cliente informado.

Em caso de sucesso:

```http
200 OK
```

Se o produto não estiver disponível:

```http
400 Bad Request
```

---

## Cancelar reserva

```http
DELETE /products/{id}/reserve
```

Exemplo:

```http
DELETE /products/1/reserve
```

Cancela uma reserva ativa e libera o produto novamente.

Em caso de sucesso:

```http
200 OK
```

---

## Consultar reservas do cliente

```http
GET /customer/{id_customer}/reservations
```

Exemplo:

```http
GET /customer/1/reservations
```

Retorna os produtos reservados pelo cliente.

---

# Status dos produtos

Os produtos possuem três estados:

| Status        | Descrição                        |
| ------------- | -------------------------------- |
| `Available`   | Produto disponível para reserva  |
| `Reserved`    | Produto possui uma reserva ativa |
| `Unavailable` | Produto indisponível             |

# Status das reservas

| Status      | Descrição                        |
| ----------- | -------------------------------- |
| `Active`    | Reserva atualmente ativa         |
| `Cancelled` | Reserva cancelada pelo cliente   |
| `Expired`   | Reserva que ultrapassou 72 horas |

---

# Pré-requisitos

Para executar o projeto é necessário ter instalado:

* .NET SDK 10
* Git

---

# Como executar

Clone o repositório:

```bash
git clone https://github.com/Fernandox14/Order.API.git
```

Entre na pasta do projeto:

```bash
cd Order
```

Restaure as dependências:

```bash
dotnet restore
```

Compile a solução:

```bash
dotnet build
```

Execute a API:

```bash
dotnet run --project Order.Api
```

Após iniciar a aplicação, acesse o Swagger pela URL apresentada no console.

Por exemplo:

```text
http://localhost:5000/swagger
```

A porta pode variar conforme a configuração do projeto.

---

# Estrutura do fluxo completo

```text
                    ┌────────────────────┐
                    │      Cliente       │
                    └─────────┬──────────┘
                              │
                              ▼
                    ┌────────────────────┐
                    │    Order.Api       │
                    │    Controllers     │
                    └─────────┬──────────┘
                              │
                              ▼
                    ┌────────────────────┐
                    │ Order.Aplicacao    │
                    │ Commands / Queries │
                    │     Handlers       │
                    └─────────┬──────────┘
                              │
                              ▼
                    ┌────────────────────┐
                    │  Order.Dominio     │
                    │ Entities / Rules   │
                    └─────────┬──────────┘
                              │
                              ▼
                    ┌────────────────────┐
                    │ Order.InfraEstrutura│
                    │   Repositories     │
                    │    EF Core         │
                    └─────────┬──────────┘
                              │
                              ▼
                    ┌────────────────────┐
                    │   EF InMemory      │
                    │     DbOrder        │
                    └────────────────────┘

                    ┌────────────────────┐
                    │ Background Worker  │
                    │ Expiração 72 horas │
                    └─────────┬──────────┘
                              │
                              ▼
                    Reservation → Expired
                    Product     → Available
```

# Autor

Desenvolvido como solução para o desafio técnico de desenvolvimento backend utilizando C# e .NET 10.
