# Roteiro do Vídeo — Pitch do Sistema de Locadora de Veículos

Duração alvo: **8 a 10 minutos** (dentro da faixa de 6 a 12 min exigida). Grave a tela (ex: OBS Studio)
com o Swagger aberto e vá seguindo o roteiro abaixo. Troque **[seu nome]** e **[nome da disciplina]**
pelos dados reais.

---

### 1. Abertura (30-40s)

"Olá, meu nome é **[seu nome]** e este é o pitch do meu projeto da disciplina de **[nome da disciplina]**:
um sistema de aluguel de veículos, desenvolvido em C# com ASP.NET Core, Entity Framework e SQL Server,
com toda a documentação e testes das APIs feitos através do Swagger.

Vou apresentar a arquitetura do sistema, o modelo de dados, e uma demonstração prática das
funcionalidades, incluindo cadastro, aluguel, devolução e consultas com filtros."

### 2. Arquitetura (1min)

"O backend foi desenvolvido em C# com ASP.NET Core, seguindo o padrão de APIs RESTful. A persistência
dos dados é feita através do Entity Framework Core, que mapeia minhas classes em C# para tabelas no
SQL Server. Toda a documentação e os testes das rotas são feitos pelo Swagger, que já abre
automaticamente na raiz da aplicação."

*(Mostre rapidamente a estrutura de pastas: Models, Data, Controllers, Dtos.)*

### 3. Modelo de dados (1min-1min30)

"O sistema tem 5 entidades principais: **Fabricante**, **Veículo** — que sempre pertence a um
fabricante e guarda modelo, ano e quilometragem —, **Cliente** — com CPF e e-mail únicos —,
**Aluguel** — que conecta um cliente a um veículo em um período, registrando quilometragem inicial e
final, valor da diária e valor total — e **Reserva**, que permite reservar um veículo antes de
efetivar o aluguel."

*(Mostre o diagrama do modelo conceitual, em docs/ModeloConceitual.md.)*

### 4. Demonstração — Cadastro (CRUD) (2min)

"Vou cadastrar um fabricante..." *(POST /api/fabricantes)*
"Agora um veículo vinculado a esse fabricante..." *(POST /api/veiculos)*
"Um cliente..." *(POST /api/clientes)*
"E agora eu registro um aluguel, vinculando esse cliente a esse veículo..." *(POST /api/alugueis)*

"Percebam que cada operação retorna o código de status correto — 201 Created na criação — e o objeto
criado, com seu Id gerado pelo banco."

*(Mostre também um PUT de atualização e um DELETE.)*

### 5. Devolução do veículo (1min)

"Agora vou demonstrar a devolução do veículo, que é uma regra de negócio específica do sistema: ao
registrar a devolução, informando a data e a quilometragem final, o sistema calcula automaticamente o
valor total do aluguel, multiplicando o número de dias pelo valor da diária, e atualiza a quilometragem
do veículo." *(PUT /api/alugueis/{id}/devolucao)*

### 6. Filtros com Join (2min-2min30)

"Agora vou demonstrar as 5 rotas de filtro que desenvolvi, que utilizam junções entre tabelas.

O primeiro filtro lista os veículos de um fabricante específico, usando um inner join entre Veículo e
Fabricante." *(GET /api/veiculos/por-fabricante/{id})*

"O segundo mostra os veículos disponíveis — ou seja, sem nenhum aluguel em aberto — usando um left
join entre Veículo e Aluguel." *(GET /api/veiculos/disponiveis)*

"O terceiro gera um ranking dos veículos mais alugados, também com left join e agregação."
*(GET /api/veiculos/mais-alugados)*

"O quarto traz o histórico de aluguéis de um cliente específico, unindo três tabelas: Aluguel, Cliente
e Veículo." *(GET /api/alugueis/por-cliente/{clienteId})*

"E o quinto filtra os aluguéis realizados dentro de um período de datas." *(GET /api/alugueis/periodo)*

"Ao todo, essas rotas utilizam dois tipos diferentes de join: inner join e left join, conforme exigido
no trabalho."

### 7. Validações e tratamento de erros (30-40s)

"O sistema também trata erros. Por exemplo, se eu tentar alugar um veículo que já está ocupado, a API
retorna um erro 400 com uma mensagem clara." *(Demonstre um erro de validação.)*

### 8. Encerramento (30s)

"Com isso, encerro a demonstração do sistema de aluguel de veículos, cobrindo o cadastro, a locação, a
devolução e as consultas com filtros, tudo documentado e testado via Swagger. Obrigado pela atenção!"

---

### Checklist antes de gravar
- [ ] Ambiente rodando no Codespaces (API + SQL Server), sem erros no console.
- [ ] Já ter pelo menos 1 fabricante, 2 veículos, 2 clientes cadastrados antes de gravar (agiliza a
      demonstração dos filtros).
- [ ] Testar cada rota pelo menos uma vez antes de gravar, para não travar ao vivo.
- [ ] Gravar em 1080p, com áudio claro.
- [ ] Subir no YouTube como "não listado" e colar o link no relatório acadêmico.
