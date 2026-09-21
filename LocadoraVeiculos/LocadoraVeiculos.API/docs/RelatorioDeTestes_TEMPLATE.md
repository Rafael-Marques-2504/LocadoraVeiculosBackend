# Relatório de Testes — API Locadora de Veículos

> **IMPORTANTE:** este arquivo é um **template**. Ele foi feito para você preencher com os **prints de tela**
> reais do Swagger rodando no seu ambiente (Codespaces), conforme exige o item 3.3 do enunciado:
> *"Os testes devem ser evidenciados em um relatório de testes contendo a chamada de cada método das
> APIs e também o retorno obtido. Utilize prints de tela para tal."*
>
> Para cada endpoint abaixo:
> 1. Execute a chamada no Swagger ("Try it out" → "Execute").
> 2. Tire um print mostrando a requisição (parâmetros/body) e a resposta (status code + JSON retornado).
> 3. Cole o print no lugar indicado (`[COLAR PRINT AQUI]`), usando `![descrição](caminho/da/imagem.png)`
>    se estiver versionando as imagens junto com o relatório, ou inserindo a imagem diretamente
>    se estiver escrevendo este relatório em Word/Google Docs.
>
> Sugestão de ordem de execução (para os dados baterem entre si): Fabricantes → Veículos → Clientes →
> Aluguéis → Devolução → Reservas → Filtros.

---

## 1. Fabricantes

### 1.1 POST /api/fabricantes — Cadastrar fabricante
- Body enviado: `[DESCREVER OU COLAR O JSON ENVIADO]`
- Retorno obtido:

[COLAR PRINT AQUI]

### 1.2 GET /api/fabricantes — Listar fabricantes
[COLAR PRINT AQUI]

### 1.3 GET /api/fabricantes/{id} — Buscar por Id
[COLAR PRINT AQUI]

### 1.4 PUT /api/fabricantes/{id} — Atualizar fabricante
[COLAR PRINT AQUI]

### 1.5 DELETE /api/fabricantes/{id} — Remover fabricante
[COLAR PRINT AQUI]

---

## 2. Veículos

### 2.1 POST /api/veiculos — Cadastrar veículo
[COLAR PRINT AQUI]

### 2.2 GET /api/veiculos — Listar veículos
[COLAR PRINT AQUI]

### 2.3 GET /api/veiculos/{id} — Buscar por Id
[COLAR PRINT AQUI]

### 2.4 PUT /api/veiculos/{id} — Atualizar veículo
[COLAR PRINT AQUI]

### 2.5 DELETE /api/veiculos/{id} — Remover veículo
[COLAR PRINT AQUI]

### 2.6 GET /api/veiculos/por-fabricante/{fabricanteId} — Filtro 1 (INNER JOIN)
[COLAR PRINT AQUI]

### 2.7 GET /api/veiculos/disponiveis — Filtro 2 (LEFT JOIN)
[COLAR PRINT AQUI]

### 2.8 GET /api/veiculos/mais-alugados — Filtro 3 (LEFT JOIN + agregação)
[COLAR PRINT AQUI]

---

## 3. Clientes

### 3.1 POST /api/clientes — Cadastrar cliente
[COLAR PRINT AQUI]

### 3.2 GET /api/clientes — Listar clientes
[COLAR PRINT AQUI]

### 3.3 GET /api/clientes/{id} — Buscar por Id
[COLAR PRINT AQUI]

### 3.4 PUT /api/clientes/{id} — Atualizar cliente
[COLAR PRINT AQUI]

### 3.5 DELETE /api/clientes/{id} — Remover cliente
[COLAR PRINT AQUI]

---

## 4. Aluguéis

### 4.1 POST /api/alugueis — Registrar aluguel
[COLAR PRINT AQUI]

### 4.2 GET /api/alugueis — Listar aluguéis
[COLAR PRINT AQUI]

### 4.3 GET /api/alugueis/{id} — Buscar por Id
[COLAR PRINT AQUI]

### 4.4 PUT /api/alugueis/{id}/devolucao — Registrar devolução
[COLAR PRINT AQUI]

### 4.5 DELETE /api/alugueis/{id} — Remover aluguel
[COLAR PRINT AQUI]

### 4.6 GET /api/alugueis/por-cliente/{clienteId} — Filtro 4 (INNER JOIN)
[COLAR PRINT AQUI]

### 4.7 GET /api/alugueis/periodo?inicio=&fim= — Filtro 5 (INNER JOIN + datas)
[COLAR PRINT AQUI]

---

## 5. Reservas

### 5.1 POST /api/reservas — Criar reserva
[COLAR PRINT AQUI]

### 5.2 GET /api/reservas — Listar reservas
[COLAR PRINT AQUI]

### 5.3 GET /api/reservas/{id} — Buscar por Id
[COLAR PRINT AQUI]

### 5.4 PUT /api/reservas/{id} — Atualizar reserva
[COLAR PRINT AQUI]

### 5.5 DELETE /api/reservas/{id} — Remover reserva
[COLAR PRINT AQUI]

---

## 6. Testes de validação e tratamento de erros

### 6.1 Cadastro de cliente com e-mail inválido (deve retornar 400)
[COLAR PRINT AQUI]

### 6.2 Tentativa de alugar um veículo já ocupado (deve retornar 400)
[COLAR PRINT AQUI]

### 6.3 Busca por Id inexistente (deve retornar 404)
[COLAR PRINT AQUI]

### 6.4 Tentativa de excluir fabricante com veículos vinculados (deve retornar 400)
[COLAR PRINT AQUI]

---

## Conclusão

[Escreva aqui um parágrafo curto confirmando que todos os endpoints foram testados e funcionaram
conforme o esperado, mencionando eventuais observações relevantes.]
