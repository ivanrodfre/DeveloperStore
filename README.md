# Projeto de Avaliação Desenvolvedor Ambev

## Visão Geral

Este projeto foi desenvolvido como parte do processo de avaliação técnica. Segui rigorosamente os padrões modernos, incluindo Domain-Driven Design (DDD), Clean Code, Domínios Ricos e outros padrões modernos. A implementação demonstra boas práticas de validação, utilizando o **FluentValidation** juntamente com validações diretas no domínio que lançam exceções – uma abordagem simples e eficaz que não impacta significativamente o desempenho do sistema e se torna bem fexível maleável. 

Além disso, todas as ações realizadas no domínio principal podem disparar eventos utilizando a biblioteca [Rebus](https://github.com/rebus-org/Rebus). Estamos integrando o Rebus via service dentro do handler e salvando os eventos no banco de dados, na tabela **"Events"**, para garantir rastreabilidade e possibilitar o reprocessamento dos eventos, se necessário.

## Tecnologias Utilizadas

- **.NET (C#)**
- **PostgreSQL:** O projeto utiliza o PostgreSQL como banco de dados relacional. Todas as conexões e configurações estão definidas no arquivo `appsettings.json`.
- **Entity Framework Core:** Para acesso e manipulação dos dados no PostgreSQL.
- **MediatR**
- **AutoMapper**
- **FluentValidation**
- **Rebus:** Para disparo de eventos no domínio.
- **xUnit, NSubstitute, Bogus:** Para testes unitários e funcionais.

## Como Executar o Projeto

### Pré-requisitos

- **.NET SDK** (versão 6.0 ou superior)
- **PostgreSQL:** Certifique-se de ter o PostgreSQL instalado e configurado em sua máquina ou utilizar um servidor na nuvem.
- Configurações de ambiente conforme descritas nos arquivos de configuração (appsettings.json).

### Passos para Execução

1. **Clonar o Repositório**

   ```bash
   git clone https://github.com/ivanrodfre/DeveloperStore.git
   cd seu-repositorio


2. **Configurar as Conexões**

- Verifique o arquivo appsettings.json e ajuste a connection string para o PostgreSQL conforme seu ambiente.
- Exemplo de connection string:

  ```bash
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=AmbevEvalDb;Username=postgres;Password=suasenha"
  }


3. **Restaurar Dependências e Build**

    ```bash
    dotnet restore
    dotnet build


4. **Aplicar Migrations e Atualizar o Banco de Dados**
Caso você ainda não tenha criado as migrations, execute:

    ```bash
    dotnet ef migrations add InitialMigration -c DefaultContext --startup-project Ambev.DeveloperEvaluation.WebApi
    dotnet ef database update -c DefaultContext --startup-project Ambev.DeveloperEvaluation.WebApi

Isso criará as tabelas necessárias no PostgreSQL conforme as configurações do projeto.

5. **Executar o Projeto**


dotnet run --project Ambev.DeveloperEvaluation.WebApi

O projeto iniciará o servidor web (API) e utilizará as configurações definidas, conectando-se ao PostgreSQL.

3. **Executar o Projeto**

    ```bash
    dotnet run --project Ambev.DeveloperEvaluation.WebApi

O projeto iniciará o servidor web (API) e utilizará as configurações definidas, conectando-se ao PostgreSQL.


### Validações e Lançamento de Exceções

**FluentValidation**
Utilizamos o FluentValidation para garantir que as requisições atendam aos requisitos do sistema.

**Validações Diretas no Domínio:**
Para exemplificar boas práticas e simplificar a lógica, algumas validações são executadas diretamente nas entidades do domínio através do lançamento de exceções. Essa estratégia permite que as regras de negócio sejam aplicadas de forma consistente, sem impactar significativamente a performance.

### Eventos com Rebus


**Integração com Rebus:**
Todas as ações do domínio principal podem disparar eventos utilizando a biblioteca Rebus.

**Persistência de Eventos:**
Os eventos são armazenados no banco de dados na tabela "Events", permitindo auditoria, reprocessamento e facilitando o desacoplamento da lógica de negócio.

**Implementação via Service:**
O Rebus está configurado para ser utilizado via service dentro dos handlers, permitindo a disparada de eventos durante as operações do domínio.


### Estrutura do Projeto


**Domain:**
Contém as entidades do domínio (por exemplo, Sale, SaleItem) e toda a lógica de negócio, seguindo padrões de domínio rico.

**Application:**
Contém os comandos, consultas, handlers, DTOs e validadores.

**Infrastructure:**
Responsável pela persistência dos dados utilizando o Entity Framework Core com PostgreSQL, além da integração com o Rebus para eventos.

**WebApi:**
Contém os controllers, configuração do servidor e middleware para tratamento das requisições HTTP.

**Tests:**
Contém os testes unitários e funcionais utilizando xUnit, NSubstitute e Bogus.


### Considerações Finais


Embora o projeto não esteja 100% concluído, ele apresenta uma base sólida construída com padrões modernos e boas práticas de desenvolvimento, como DDD, Clean Code e outros. A utilização do PostgreSQL garante uma persistência robusta e escalável, enquanto a integração com o Rebus possibilita a emissão e persistência de eventos, promovendo uma arquitetura desacoplada e preparada para evoluções futuras.











