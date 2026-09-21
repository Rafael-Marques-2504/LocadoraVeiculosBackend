# Sistema de Locadora de Veículos — Trabalho Acadêmico

API RESTful para gerenciamento de um sistema de aluguel de veículos, desenvolvida em **C# (.NET 8)**
com **ASP.NET Core**, **Entity Framework Core** e **SQL Server**, documentada e testável via **Swagger**.

## Estrutura do projeto

```
LocadoraVeiculos/
├── LocadoraVeiculos.API/
│   ├── Controllers/       -> Endpoints da API (CRUD + filtros)
│   ├── Models/             -> Entidades (Fabricante, Veiculo, Cliente, Aluguel, Reserva)
│   ├── Dtos/                -> Objetos de entrada, com validação
│   ├── Data/                -> DbContext (mapeamento EF Core)
│   ├── Middleware/          -> Tratamento global de erros
│   └── Program.cs
├── docs/
│   ├── ModeloConceitual.md          -> Modelo ER e explicação (Etapa 1)
│   ├── DocumentacaoEndpoints.md     -> Documentação de cada rota (Etapa 3.2)
│   ├── RelatorioDeTestes_TEMPLATE.md-> Template para você colar os prints dos testes (Etapa 3.3)
│   └── RoteiroVideo.md              -> Roteiro para gravar o pitch (Etapa 4)
├── .devcontainer/          -> Configuração para abrir tudo pronto no Codespaces
└── LocadoraVeiculos.sln
```

## Como rodar no GitHub Codespaces (recomendado, já que você não tem o Visual Studio/VS Code local)

1. **Suba este projeto para um repositório no seu GitHub pessoal** (crie um repositório novo, dê
   `git init`, `git add .`, `git commit`, `git remote add origin ...`, `git push` — ou simplesmente
   faça upload dos arquivos pela interface do GitHub).
2. No repositório, clique em **Code → Codespaces → Create codespace on main**.
3. O Codespaces vai ler a pasta `.devcontainer/` e subir automaticamente **dois containers**:
   - `app`: com o SDK do .NET 8 instalado;
   - `db`: com o **SQL Server 2022** rodando (substitui o SQL Express, que é exclusivo do Windows —
     o SQL Server para Linux em container é a alternativa padrão para ambientes como o Codespaces,
     e usa a mesma engine e a mesma linguagem T-SQL).
4. Aguarde o `postCreateCommand` terminar (ele já roda `dotnet restore` e instala a ferramenta
   `dotnet-ef` automaticamente). Isso aparece no terminal integrado do Codespaces.

### Criando o banco de dados (migrations)

No terminal do Codespaces, dentro da pasta do projeto:

```bash
cd LocadoraVeiculos.API
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Isso vai criar as migrations do Entity Framework (traduzindo o modelo conceitual para o esquema físico
do banco) e aplicá-las no SQL Server que está rodando no container `db`.

> Se o comando `dotnet ef` não for reconhecido, rode antes:
> `export PATH="$PATH:$HOME/.dotnet/tools"`

### Rodando a API

Ainda dentro de `LocadoraVeiculos.API`:

```bash
dotnet run
```

O Codespaces vai detectar a porta (5000) e oferecer para abrir no navegador — ou você acessa pela aba
**Ports**. Como o Swagger foi configurado para abrir na rota raiz (`/`), a própria tela inicial já é a
documentação interativa da API, pronta para você clicar em **"Try it out"** em cada endpoint.

## Rodando localmente (caso tenha acesso a uma máquina com Visual Studio/SQL Server Express)

1. Instale o [.NET 8 SDK](https://dotnet.microsoft.com/download) e o SQL Server Express.
2. Ajuste a `ConnectionStrings:DefaultConnection` em `appsettings.json` com o nome da sua instância
   local (ex: `Server=localhost\\SQLEXPRESS;...`).
3. Rode `dotnet ef database update` e depois `dotnet run` dentro de `LocadoraVeiculos.API`.

## O que está implementado (mapeado para o enunciado)

- **Etapa 1 — Modelagem:** 5 entidades (`Fabricante`, `Veiculo`, `Cliente`, `Aluguel`, `Reserva`),
  chaves primárias/estrangeiras e restrições via Fluent API em `Data/LocadoraContext.cs`. Diagrama e
  explicação em `docs/ModeloConceitual.md`.
- **Etapa 2 — Backend:** CRUD completo para todas as entidades, validação de entrada com Data
  Annotations, tratamento de erros via middleware global, e **5 rotas de filtro** usando **2 tipos de
  join diferentes** (inner join e left join) — detalhadas em `docs/DocumentacaoEndpoints.md`.
- **Etapa 3 — Testes e documentação:** Swagger integrado (abre na raiz da aplicação). Documentação de
  cada endpoint em `docs/DocumentacaoEndpoints.md`. Template para o relatório de testes com prints em
  `docs/RelatorioDeTestes_TEMPLATE.md` — **você precisa preencher esse arquivo com os prints reais**,
  pois isso exige rodar o projeto e testar no seu ambiente.
- **Etapa 4 — Vídeo:** roteiro pronto em `docs/RoteiroVideo.md`.

## O que ainda depende de você

Por ser um trabalho individual e a IA não poder rodar SQL Server nem gravar sua tela, faltam apenas:

1. Subir o repositório no seu GitHub pessoal.
2. Rodar o projeto no Codespaces (passos acima) e conferir se tudo funciona.
3. Executar os testes pelo Swagger e **preencher o `docs/RelatorioDeTestes_TEMPLATE.md` com os
   prints reais** (Etapa 3.3 pede evidências suas).
4. Gravar o vídeo seguindo `docs/RoteiroVideo.md` e subir no YouTube (não listado).
5. Colocar o link do vídeo no relatório acadêmico final e entregar o `.zip` do repositório no Canvas.
