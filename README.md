# DevFreela API - Projeto de Estudos

## 🚨 Observação Importante
Atualmente, estou enfrentando problemas de conectividade entre o SQL Server no Azure e a integração com minha solution para testes locais. Preciso resolver essa questão antes de avançar com a aplicação.

---

## 📌 Sobre o Projeto
Este projeto é uma API RESTful desenvolvida com **.NET 8**, focada em aprendizado e aplicação de conceitos modernos de desenvolvimento backend. No futuro, pretendo integrar essa API com um frontend em **Angular**.

### ✅ Tecnologias e Conceitos Aplicados:
- **ASP.NET Core**
- **Entity Framework** (atual foco de estudo)
- **SQL Server no Azure**

### 📌 Conceitos que ainda serão implementados:
- 🛠 **Clean Code**
- 📚 **Padrão CQRS**
- 🏛 **Padrão Repository**
- ✅ **Validação de API**
- 🧪 **Testes Unitários**
- 🔐 **Autenticação e Autorização**

---

## 🔧 Comandos Úteis

### 📌 Criar a primeira migration
```sh
dotnet ef migrations add PrimeiraMigration -o Persistence/Migrations
```
*(O parâmetro `-o` define o diretório onde as migrations serão salvas na primeira execução.)*

### 📌 Criar banco de dados e atualizar tabelas
```sh
dotnet ef database update
```

### 📌 Outros comandos úteis

🔹 **Criar uma nova migration**
```sh
dotnet ef migrations add NomeDaMigration
```

🔹 **Remover última migration** (caso tenha sido criada por engano)
```sh
dotnet ef migrations remove
```

🔹 **Ver o script SQL gerado pelas migrations**
```sh
dotnet ef migrations script
```

🔹 **Listar migrations existentes**
```sh
dotnet ef migrations list
```

---

## 🚀 Próximos Passos
1. Resolver a conexão com o SQL Server no Azure.
2. Refatorar a API aplicando Clean Code e boas práticas.
3. Implementar autenticação e autorização.
4. Criar testes unitários.
5. Desenvolver um frontend em Angular para consumir essa API.

---

📌 **Status do projeto:** Em desenvolvimento 🏗️

📌 **Objetivo:** Criar um sistema completo aplicando boas práticas e conceitos avançados de desenvolvimento backend.

