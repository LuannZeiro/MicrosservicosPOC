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