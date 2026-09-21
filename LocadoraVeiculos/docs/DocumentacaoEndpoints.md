# Documentação dos Endpoints — API Locadora de Veículos

Todos os endpoints estão documentados de forma interativa via **Swagger**, disponível na raiz da aplicação
(`http://localhost:5000/` ou na URL pública gerada pelo Codespaces). Abaixo está a documentação detalhada
de cada rota, para consulta e para compor o relatório acadêmico.

Todas as respostas de erro seguem o formato:
```json
{ "mensagem": "Descrição do erro" }
```
Erros internos não tratados são capturados pelo middleware global e retornam HTTP 500 com o formato:
```json
{ "sucesso": false, "mensagem": "Ocorreu um erro interno ao processar a requisição.", "detalhe": "..." }
```

---

## Fabricantes — `/api/fabricantes`

| Método | Rota | Descrição | Body | Respostas |
|---|---|---|---|---|
| GET | `/api/fabricantes` | Lista todos os fabricantes | — | 200 OK |
| GET | `/api/fabricantes/{id}` | Busca fabricante por Id | — | 200 OK / 404 Not Found |
| POST | `/api/fabricantes` | Cadastra um fabricante | `{ "nome": "string", "paisOrigem": "string" }` | 201 Created / 400 Bad Request |
| PUT | `/api/fabricantes/{id}` | Atualiza um fabricante | `{ "nome": "string", "paisOrigem": "string" }` | 204 No Content / 400 / 404 |
| DELETE | `/api/fabricantes/{id}` | Remove um fabricante | — | 204 No Content / 400 (se possuir veículos) / 404 |

## Veículos — `/api/veiculos`

| Método | Rota | Descrição | Body | Respostas |
|---|---|---|---|---|
| GET | `/api/veiculos` | Lista todos os veículos (com fabricante) | — | 200 OK |
| GET | `/api/veiculos/{id}` | Busca veículo por Id | — | 200 OK / 404 |
| POST | `/api/veiculos` | Cadastra um veículo | `{ "modelo", "placa", "anoFabricacao", "quilometragem", "fabricanteId" }` | 201 / 400 |
| PUT | `/api/veiculos/{id}` | Atualiza um veículo | idem acima | 204 / 400 / 404 |
| DELETE | `/api/veiculos/{id}` | Remove um veículo | — | 204 / 400 (se possuir aluguéis) / 404 |
| GET | `/api/veiculos/por-fabricante/{fabricanteId}` | **Filtro 1** — Veículos de um fabricante (INNER JOIN Veiculo × Fabricante) | — | 200 OK |
| GET | `/api/veiculos/disponiveis` | **Filtro 2** — Veículos sem aluguel em aberto (LEFT JOIN Veiculo × Aluguel) | — | 200 OK |
| GET | `/api/veiculos/mais-alugados` | **Filtro 3** — Ranking de veículos por nº de aluguéis (LEFT JOIN + agregação) | — | 200 OK |

## Clientes — `/api/clientes`

| Método | Rota | Descrição | Body | Respostas |
|---|---|---|---|---|
| GET | `/api/clientes` | Lista todos os clientes | — | 200 OK |
| GET | `/api/clientes/{id}` | Busca cliente por Id | — | 200 OK / 404 |
| POST | `/api/clientes` | Cadastra um cliente | `{ "nome", "cpf", "email", "telefone" }` | 201 / 400 (CPF/e-mail duplicado) |
| PUT | `/api/clientes/{id}` | Atualiza um cliente | idem acima | 204 / 400 / 404 |
| DELETE | `/api/clientes/{id}` | Remove um cliente | — | 204 / 400 (se possuir aluguéis/reservas) / 404 |

## Aluguéis — `/api/alugueis`

| Método | Rota | Descrição | Body | Respostas |
|---|---|---|---|---|
| GET | `/api/alugueis` | Lista todos os aluguéis (com cliente e veículo) | — | 200 OK |
| GET | `/api/alugueis/{id}` | Busca aluguel por Id | — | 200 OK / 404 |
| POST | `/api/alugueis` | Registra um novo aluguel | `{ "clienteId", "veiculoId", "dataInicio", "dataFimPrevista", "quilometragemInicial", "valorDiaria" }` | 201 / 400 (veículo ocupado, datas inválidas) |
| PUT | `/api/alugueis/{id}/devolucao` | Registra a devolução do veículo e calcula o valor total | `{ "dataDevolucao", "quilometragemFinal" }` | 200 OK / 400 / 404 |
| DELETE | `/api/alugueis/{id}` | Remove um registro de aluguel | — | 204 / 404 |
| GET | `/api/alugueis/por-cliente/{clienteId}` | **Filtro 4** — Histórico de aluguéis de um cliente (INNER JOIN Aluguel × Cliente × Veiculo) | — | 200 OK |
| GET | `/api/alugueis/periodo?inicio=&fim=` | **Filtro 5** — Aluguéis iniciados em um período (INNER JOIN + filtro de datas) | — | 200 OK / 400 |

## Reservas — `/api/reservas`

| Método | Rota | Descrição | Body | Respostas |
|---|---|---|---|---|
| GET | `/api/reservas` | Lista todas as reservas (com cliente e veículo) | — | 200 OK |
| GET | `/api/reservas/{id}` | Busca reserva por Id | — | 200 OK / 404 |
| POST | `/api/reservas` | Cria uma reserva | `{ "clienteId", "veiculoId", "dataInicioPrevista", "dataFimPrevista", "status" }` | 201 / 400 |
| PUT | `/api/reservas/{id}` | Atualiza uma reserva | idem acima | 204 / 400 / 404 |
| DELETE | `/api/reservas/{id}` | Cancela/remove uma reserva | — | 204 / 404 |

---

## Resumo dos 5 filtros exigidos (Etapa 2.5)

| # | Rota | Tipo de Join |
|---|---|---|
| 1 | `GET /api/veiculos/por-fabricante/{id}` | INNER JOIN |
| 2 | `GET /api/veiculos/disponiveis` | LEFT JOIN (GroupJoin + DefaultIfEmpty) |
| 3 | `GET /api/veiculos/mais-alugados` | LEFT JOIN + agregação (GROUP BY / COUNT) |
| 4 | `GET /api/alugueis/por-cliente/{clienteId}` | INNER JOIN (3 tabelas) |
| 5 | `GET /api/alugueis/periodo?inicio=&fim=` | INNER JOIN + filtro por intervalo de datas |

Os filtros utilizam, portanto, **dois tipos diferentes de join**: `INNER JOIN` (filtros 1, 4 e 5) e `LEFT JOIN` via `GroupJoin`/`DefaultIfEmpty` (filtros 2 e 3), conforme exigido no item 2.5 do enunciado.
