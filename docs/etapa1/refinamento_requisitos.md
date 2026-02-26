# Refinamento de Requisitos Ambíguos com LLM
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

**Versão:** 1.0 | **Data:** 24 de Fevereiro de 2026 | **Projeto:** LI4 2025/2026

---

## Introdução

Durante o processo de elicitação de requisitos, vários requisitos apresentaram-se com **ambiguidade** — formulações vagas, critérios de aceitação indefinidos ou conflito entre necessidades de diferentes stakeholders. Para resolver estas ambiguidades, foi utilizado um **LLM (Gemini)** como ferramenta de apoio, através de prompts estruturados que permitiram:

1. Identificar a fonte de ambiguidade em cada requisito
2. Reformular o requisito com linguagem mais precisa e verificável
3. Definir critérios de aceitação mensuráveis

O presente documento regista o processo de refinamento para os **8 requisitos ambíguos** identificados.

---

## Processo de Refinamento

Para cada requisito ambíguo, o processo seguido foi:

```
1. Identificação da ambiguidade (pela equipa)
2. Formulação do prompt para o LLM
3. Resposta do LLM com alternativas de refinamento
4. Seleção e validação pela equipa
5. Versão final aprovada
```

---

## Refinamentos Realizados

---

### REF-01 – Tempo de Resposta do Sistema (RNF01)

**Versão original (ambígua):**
> *"O sistema deve ser rápido."*

**Problema identificado:** "Rápido" não é um critério mensurável. Não especifica que operações, em que condições, nem o limiar aceitável.

**Prompt enviado ao LLM:**
> *"Estou a escrever um SRS para um sistema de gestão de lojas em Blazor. Tenho o requisito 'o sistema deve ser rápido'. Isto é demasiado vago. Podes ajudar-me a reformulá-lo de forma mensurável, segundo a norma ISO/IEC 25010 (atributo de eficiência de desempenho)? O sistema terá até 50 utilizadores simultâneos em rede local."*

**Resposta do LLM (resumo):**
> *"Sugiro dividir em dois requisitos distintos e mensuráveis: (1) Tempo de resposta: as operações de consulta devem completar em menos de 2 segundos sob carga normal (até 50 utilizadores). Operações críticas (registo de venda) em menos de 1 segundo. (2) Carga concorrente: o sistema deve suportar 50 utilizadores simultâneos sem degradação superior a 20% no tempo de resposta."*

**Versão final (RNF01 aprovado):**
> *"As páginas e operações de consulta devem responder em menos de 2 segundos em condições normais de carga (até 50 utilizadores simultâneos). Operações de registo de venda (POS) devem completar em menos de 1 segundo."*

---

### REF-02 – Consolidação "ao Fim do Dia" (RF33)

**Versão original (ambígua):**
> *"O sistema deve consolidar os dados das lojas ao fim de cada dia."*

**Problema identificado:** "Ao fim do dia" é ambíguo — pode ser meia-noite, fim do expediente (22h), ou configurável. Não especifica o que acontece em caso de falha.

**Prompt enviado ao LLM:**
> *"Tenho o requisito 'o sistema deve consolidar os dados das lojas ao fim de cada dia'. O meu sistema tem um processo automático (background job) em .NET que corre à meia-noite. O gestor quer que a hora seja configurável. O que acontece se a consolidação falhar? Pode reformular este requisito de forma precisa e acrescentar casos de falha?"*

**Resposta do LLM (resumo):**
> *"Sugiro: 'O sistema deve executar automaticamente a consolidação dos dados de cada loja num horário configurável (default: 23:59). Em caso de falha, o sistema deve registar o erro e tentar novamente após 30 minutos, até um máximo de 3 tentativas. Após 3 falhas consecutivas, o Gestor da Cadeia deve ser notificado."*

**Versão final (RF33 + RF36 aprovados):**
> *"RF33: O sistema deve executar automaticamente a consolidação dos dados de cada loja no servidor central ao fim de cada dia, numa hora configurável (default: 23:59)."*
> *"RF36: Em caso de falha de consolidação, o sistema deve tentar novamente de 30 em 30 minutos e notificar o Gestor da Cadeia após 3 falhas consecutivas."*

---

### REF-03 – Ajuste de Stock com "Motivo" (RF15)

**Versão original (ambígua):**
> *"O Gerente de Loja deve poder registar ajustes de stock com motivo."*

**Problema identificado:** Não especifica se o motivo é texto livre ou lista pré-definida, nem se é obrigatório ou opcional. Não menciona auditoria.

**Prompt enviado ao LLM:**
> *"O requisito 'ajuste de stock com motivo' é ambíguo. No contexto de uma loja de conveniência, o motivo deve ser texto livre ou uma lista de categorias (quebra, inventário, devolução, roubo...)? E deve ficar registado em auditoria? O Gerente precisa de justificar perante o Gestor da Cadeia."*

**Resposta do LLM (resumo):**
> *"Recomendo uma lista de categorias pré-definidas (quebra, inventário, devolução, roubo, erro de contagem, outro) com campo de texto livre adicional quando 'outro' é selecionado. O registo deve ser obrigatório e guardado no log de auditoria com: utilizador, data/hora, quantidade ajustada, motivo e observações."*

**Versão final (RF15 aprovado):**
> *"O Gerente de Loja deve poder registar ajustes manuais de stock (quebras, devoluções, inventário) com motivo obrigatório selecionado de lista pré-definida (Quebra, Inventário, Devolução, Roubo, Erro de Contagem, Outro) e campo de observação livre. O ajuste é registado no log de auditoria com utilizador, data/hora e dados completos."*

---

### REF-04 – Preço "Configurável por Loja" (RF08)

**Versão original (ambígua):**
> *"O preço de venda pode ser diferente por loja."*

**Problema identificado:** Não especifica quem pode alterar, se o preço de loja substitui sempre o base, se há limites de desconto, e o que acontece quando o preço base central é atualizado.

**Prompt enviado ao LLM:**
> *"Tenho um sistema com produtos que têm preço base definido centralmente, mas cada loja pode ter preços diferentes. Estou a escrever o requisito. Quem deve poder alterar o preço por loja? O que acontece quando o gestor central atualiza o preço base — o preço da loja é afetado? Pode haver um limite de desconto máximo?"*

**Resposta do LLM (resumo):**
> *"Sugiro: o Gerente de Loja pode definir um preço de venda local que substitui o preço base apenas para a sua loja. Se o preço local não existir, usa-se o preço base. Se o Gestor atualizar o preço base, o preço local da loja mantém-se inalterado (não é sobrescrito automaticamente). O Gestor da Cadeia pode definir um limite máximo de desconto por produto (% abaixo do preço base)."*

**Versão final (RF08 aprovado):**
> *"O Gerente de Loja pode definir um preço de venda local para qualquer produto, sobrepondo o preço base central apenas para a sua loja. Na ausência de preço local, aplica-se o preço base. Atualizações ao preço base pelo Gestor da Cadeia não afetam automaticamente os preços locais definidos."*

---

### REF-05 – Dashboard "em Tempo Real" (RF37)

**Versão original (ambígua):**
> *"O Gestor da Cadeia deve ver um dashboard com os dados das lojas em tempo real."*

**Problema identificado:** "Tempo real" é ambíguo num sistema com consolidação diária. Os dados só ficam consolidados às 23:59. O dashboard mostra dados do dia atual ou do dia consolidado anterior?

**Prompt enviado ao LLM:**
> *"O meu sistema consolida os dados das lojas uma vez por dia. O gestor quer ver dados 'em tempo real' no dashboard. Isto é uma contradição — os dados só ficam consolidados à meia-noite. Como posso resolver esta ambiguidade e definir claramente o que o dashboard deve mostrar?"*

**Resposta do LLM (resumo):**
> *"Sugiro distinguir dois tipos de dados no dashboard: (1) Dados do dia atual (em curso) — atualizados em tempo quase real (polling a cada X minutos ou via SignalR): total de vendas do dia, stock atual, encomendas pendentes. (2) Dados históricos consolidados — do dia anterior e anteriores, apenas após consolidação. O utilizador deve ver claramente qual dos dois períodos está a visualizar."*

**Versão final (RF37 aprovado):**
> *"O Gestor da Cadeia deve visualizar um dashboard central com: (a) KPIs do dia em curso (atualizados a cada 5 minutos): total de vendas por loja, alertas de stock e encomendas pendentes; (b) dados históricos consolidados de dias anteriores. O dashboard deve indicar claramente a hora da última atualização."*

---

### REF-06 – Cancelamento vs. Devolução de Venda (RF21)

**Versão original (ambígua):**
> *"O sistema deve permitir cancelar vendas e processar devoluções."*

**Problema identificado:** Cancelamento e devolução são operações diferentes. Não é claro se são o mesmo fluxo, quem pode fazer cada uma, nem o prazo para cancelamento.

**Prompt enviado ao LLM:**
> *"No meu sistema de POS para lojas de conveniência, qual é a diferença entre cancelar uma venda e processar uma devolução? Devem ser fluxos separados? Quem deve ter permissão para cada um?"*

**Resposta do LLM (resumo):**
> *"São operações distintas: Cancelamento — anula a venda completa, imediatamente (mesmo turno/dia); idealmente só o Gerente de Loja. Devolução — o cliente devolve itens específicos de uma venda já concluída (pode ser dias depois); deve gerar uma transação de devolução e repor o stock. Ambas requerem motivo obrigatório e registo em auditoria."*

**Versão final (RF21 aprovado):**
> *"O Gerente de Loja deve poder: (a) Cancelar uma venda completa no mesmo dia com motivo obrigatório e reversão total do stock; (b) Processar a devolução de itens individuais de vendas anteriores, com motivo obrigatório, especificando produtos e quantidades devolvidas. O stock é reposto automaticamente em ambos os casos. Ambas as operações ficam registadas em auditoria."*

---

### REF-07 – "Gerir" Fornecedores (RF23)

**Versão original (ambígua):**
> *"O sistema deve permitir gerir fornecedores."*

**Problema identificado:** "Gerir" é excessivamente vago. Não especifica os atributos do fornecedor, quem pode criar/editar, o que acontece a fornecedores com encomendas ativas.

**Prompt enviado ao LLM:**
> *"O requisito 'gerir fornecedores' é demasiado vago. Quais os campos obrigatórios de um fornecedor num sistema de gestão para lojas de conveniência portuguesas? O que deve acontecer quando se tenta eliminar um fornecedor que tem encomendas em curso?"*

**Resposta do LLM (resumo):**
> *"Campos obrigatórios: Nome, NIF (9 dígitos, validado), Morada, Contacto telefónico, Email. Campos opcionais: Website, Observações, produtos associados. Não deve ser possível eliminar definitivamente — apenas desativar (soft delete). Se tiver encomendas ativas no estado Pendente ou Enviada, a desativação deve ser bloqueada ou alertada. Só o Gestor da Cadeia devia poder criar/editar/desativar fornecedores."*

**Versão final (RF23 aprovado):**
> *"O Gestor da Cadeia deve poder criar, editar e desativar (logicamente) fornecedores. Cada fornecedor tem os atributos: Nome (obrigatório), NIF (obrigatório, validado, 9 dígitos), Morada, Telefone, Email e Observações. A desativação de um fornecedor com encomendas nos estados Pendente ou Enviada é bloqueada com mensagem de alerta."*

---

### REF-08 – Exportação de Relatórios (RF42)

**Versão original (ambígua):**
> *"Os relatórios devem ser exportáveis."*

**Problema identificado:** Não especifica formatos, que relatórios são exportáveis, quem pode exportar, ou o conteúdo exato do export.

**Prompt enviado ao LLM:**
> *"O requisito 'relatórios exportáveis' é vago. Num sistema de gestão de retalho, que formatos fazem sentido para exportação de relatórios de vendas? Que dados devem constar no CSV vs. PDF? Deve haver restrições de acesso à exportação?"*

**Resposta do LLM (resumo):**
> *"PDF: documento formatado com logótipo da empresa, cabeçalho com filtros aplicados, tabelas de dados e gráficos visuais — ideal para apresentação e arquivo. CSV: dados tabulares sem formatação, todas as colunas disponíveis — ideal para análise posterior em Excel ou importação noutros sistemas. Tanto o Gestor da Cadeia como o Gerente de Loja devem poder exportar, mas o Gerente apenas os dados da sua loja."*

**Versão final (RF42 aprovado):**
> *"Os relatórios de vendas, stock e consolidação devem ser exportáveis em: (a) PDF — com formatação, logótipo, cabeçalho com filtros aplicados, tabelas e gráficos; (b) CSV — dados tabulares brutos para análise em ferramentas externas. O Gestor da Cadeia pode exportar dados de todas as lojas; o Gerente de Loja apenas os dados da sua loja."*

---

## Sumário de Refinamentos

| REF | Requisito | Ambiguidade Principal | Resultado |
|---|---|---|---|
| REF-01 | RNF01 – Desempenho | "Rápido" não mensurável | Critério de 2s / 1s definido |
| REF-02 | RF33+36 – Consolidação | "Fim do dia" indefinido | Hora configurável + retry |
| REF-03 | RF15 – Ajuste stock | Motivo livre vs. lista | Lista pré-definida + auditoria |
| REF-04 | RF08 – Preço por loja | Comportamento na atualização | Preço local independente |
| REF-05 | RF37 – Dashboard | "Tempo real" vs. consolidação | Dois tipos de dados distintos |
| REF-06 | RF21 – Cancelamento | Cancelar vs. devolver | Dois fluxos separados e claros |
| REF-07 | RF23 – Fornecedores | "Gerir" = vago | Campos + soft delete + bloqueio |
| REF-08 | RF42 – Exportação | Formatos indefinidos | PDF visual + CSV para análise |

---

## Avaliação Crítica do Uso de LLM

O LLM foi útil para:
- **Sugerir critérios de aceitação mensuráveis** (RNF01, RF33)
- **Identificar casos de borda** não levantados nas entrevistas (RF21, RF23)
- **Propor soluções de design** baseadas em boas práticas de sistemas similares (RF08, RF37)

Limitações observadas:
- O LLM por vezes sugere funcionalidades mais complexas do que o necessário (ex.: limite de desconto máximo em RF08 foi descartado por exceder o âmbito)
- As respostas foram sempre **validadas pela equipa** antes de incorporadas no SRS
- Requisitos com implicações legais (NIF, faturação) foram validados com referências externas independentes do LLM
