# 🚀 Ambev Developer Evaluation Web API

Bem-vindo ao projeto **Ambev Developer Evaluation Web API**!  
Este repositório contém uma API robusta desenvolvida em C# 12 e .NET 8, projetada para avaliações técnicas, integração de sistemas e automação de processos.  
Aqui você encontrará uma arquitetura moderna, práticas recomendadas e integração com múltiplas tecnologias.

---

## 🛠️ Tecnologias Utilizadas

- **.NET 8** &nbsp;|&nbsp; C# 12
- **Entity Framework Core** (PostgreSQL)
- **MongoDB** (com suporte a serialização avançada)
- **RabbitMQ** (mensageria)
- **MediatR** (CQRS e pipeline behaviors)
- **AutoMapper** (mapeamento de objetos)
- **Swagger/OpenAPI** (documentação interativa)
- **JWT Authentication** (segurança)
- **Docker** (containerização)
- **Angular** (front-end sugerido para integração)

---

## 🏗️ Padrões e Boas Práticas

- **Injeção de Dependência** via IoC
- **Validação Centralizada** com pipeline MediatR
- **Middleware customizado** para tratamento de exceções
- **Configuração por ambiente** via `appsettings.json`
- **CORS** configurado para integração com aplicações Angular
- **Documentação automática** com Swagger

---

## 🚦 Como Executar Localmente

1. **Pré-requisitos**  
   - [.NET 8 SDK](https://dotnet.microsoft.com/download)
   - [Docker](https://www.docker.com/get-started)

2. **Clone o repositório**

3. **Suba os containers**

4. **Acesse a documentação da API**  
   - [https://localhost:5001/swagger](https://localhost:5001/swagger)

5. **Autenticação**  
   - Utilize o endpoint `/api/auth` para obter um token JWT e acessar rotas protegidas.

---

## 🧪 Testes e Banco de Dados

- Um arquivo de **backup do banco de dados** estará disponível na raiz do projeto (`src/backup-database.sql`) para facilitar a restauração e execução de testes locais.
- Para restaurar o banco, utilize o comando apropriado o SGBD (PostgreSQL).

---

## 💡 Dicas Rápidas

- O CORS está liberado para `http://localhost:4200` (padrão do Angular).
- As configurações de conexão estão em `src/Ambev.DeveloperEvaluation.WebApi/appsettings.json`.
- O projeto segue o padrão **Clean Architecture** para máxima manutenibilidade.

---

## 🤝 Contribuição

Contribuições são bem-vindas!  

---

Feito com 💙 por Lucas Holanda