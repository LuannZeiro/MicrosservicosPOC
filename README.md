# Microsserviços POC - Projeto .NET

## Descrição do Projeto
Este projeto é uma prova de conceito (POC) de microsserviços em .NET, utilizando **arquitetura limpa (Clean Architecture)** e princípios **SOLID**.  
O sistema consiste em:

- 2 WebAPIs
  - **ApiOracle**: Conexão com banco Oracle usando EF Core
  - **ApiPublica**: Consome uma API pública REST com retry/log (resilience)
- 1 aplicação **MVC**: WebMvc, que consome ambas as APIs
- 3 Libraries
  - **Domain**: Contém entidades e regras de negócio
  - **Application**: Contém serviços, interfaces e casos de uso
  - **Infrastructure**: Implementações de acesso a dados e configurações

---

## Estrutura do Projeto

MicrosservicosPOC/
├── ApiOracle/
├── ApiPublica/
├── WebMvc/
├── Domain/
├── Application/
└── Infrastructure/


- **Separação de responsabilidades:**  
  - Controllers → apenas lógica de apresentação  
  - Services → regras de negócio e integração entre camadas  
  - Repositories → acesso a dados (Oracle ou API pública)  

---

## Requisitos Técnicos

- **Swagger:** Habilitado em todas as WebAPIs  
  - `ApiOracle`: `http://localhost:5289/swagger`  
  - `ApiPublica`: `http://localhost:5XXX/swagger`  

- **Princípios SOLID aplicados:**
  1. **SRP (Single Responsibility Principle):** Cada controller e service tem apenas uma responsabilidade.  
  2. **DIP (Dependency Inversion Principle):** Controllers recebem dependências via interfaces (`IRepository`, `IHttpClientFactory`).  
  3. **OCP (Open/Closed Principle):** Serviços podem ser estendidos sem alterar a implementação existente, usando interfaces e herança.  

- **Clean Code:** Nomes claros, classes coesas, baixo acoplamento, separação de camadas.

---

## Pré-requisitos

- .NET 9 SDK ou superior
- Oracle Database (para `ApiOracle`)
- Visual Studio ou VS Code
- Postman ou navegador para testar endpoints

---

## System Design
# Visão Geral
flowchart LR
    subgraph Client["Usuário (Browser)"]
        UI[WebMvc (ASP.NET MVC)]
    end

    subgraph S1["Microsserviços"]
        A1[ApiOracle<br/>.NET WebAPI]
        A2[ApiPublica<br/>.NET WebAPI]
    end

    subgraph Data["Persistência"]
        DB[(Oracle Database)]
    end

    subgraph Ext["Serviços Externos"]
        PUB[API Pública REST]
    end

    UI -->|HTTP REST| A1
    UI -->|HTTP REST| A2

    A1 <--> |EF Core| DB
    A2 --> |HttpClient + Resilience| PUB

# Fluxo 1 — Listagem de Clientes (WebMvc → ApiOracle → Oracle)
sequenceDiagram
    participant B as Browser
    participant MVC as WebMvc (MVC)
    participant API as ApiOracle (WebAPI)
    participant DB as Oracle DB

    B->>MVC: GET /Clientes/Index
    MVC->>API: GET /clientes
    API->>DB: SELECT * FROM Clientes
    DB-->>API: ResultSet
    API-->>MVC: 200 OK (JSON clientes)
    MVC-->>B: HTML (lista renderizada)

# Fluxo 2 — Consumo de API Pública (WebMvc → ApiPublica → API Externa)
sequenceDiagram
    participant B as Browser
    participant MVC as WebMvc (MVC)
    participant PAPI as ApiPublica (WebAPI)
    participant EXT as API Pública

    B->>MVC: GET /Clientes/Index
    par Buscar clientes
        MVC->>ApiOracle: GET /clientes
    and Buscar info pública
        MVC->>PAPI: GET /externo/info
        PAPI->>EXT: GET /public-endpoint
        EXT-->>PAPI: 200 (payload)
        PAPI-->>MVC: 200 (dados tratados)
    end
    MVC-->>B: HTML com dados combinados

Implantação Local
flowchart TB
    subgraph Machine["Dev Machine (localhost)"]
        subgraph Proc1["Processo 1"]
            W[WebMvc :5000]
        end
        subgraph Proc2["Processo 2"]
            O[ApiOracle :5289]
        end
        subgraph Proc3["Processo 3"]
            P[ApiPublica :52xx]
        end
        DB[(Oracle)]
    end

    W --> O
    W --> P
    O --> DB


---
## Rodando o Projeto Localmente

1. Clone o repositório:

```bash
git clone https://github.com/luannzeiro/MicrosservicosPOC.git
cd MicrosservicosPOC


## Rodar WebAPI com Oracle:

cd ApiOracle
dotnet run
# Swagger: http://localhost:5289/swagger

## Rodar WebAPI consumindo API pública:

cd ApiPublica
dotnet run
# Swagger: http://localhost:5XXX/swagger

## Rodar MVC WebMvc:

cd WebMvc
dotnet run
# Acesse: http://localhost:5000/Clientes/Index

