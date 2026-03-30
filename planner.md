# Planner: Tornar Macro de Invoker Genérico

## Objetivo
Deixar o programa de macro para Invoker em Dota 2 o mais genérico possível, permitindo fácil adaptação para outros heróis ou jogos.

## Etapas

1. Analisar o código atual e identificar pontos de hardcode (teclas, combos, lógica específica do Invoker).
2. Definir formato de configuração externo (ex: JSON) para combos, teclas e habilidades.
3. Implementar leitura dinâmica do arquivo de configuração.
4. Abstrair o envio de comandos de teclado/mouse.
5. Adaptar a lógica principal para usar as configurações externas.
6. Permitir múltiplos perfis (heróis/jogos diferentes).
7. Documentar como criar/adaptar perfis.
8. (Opcional) Criar interface para edição de perfis.

## Progresso
- [x] 1. Análise do código
- [x] 2. Definição do formato de configuração
- [x] 3. Implementação da leitura dinâmica
- [ ] 4. Abstração de entrada
- [ ] 5. Adaptação da lógica principal
- [ ] 6. Suporte a múltiplos perfis
- [ ] 7. Documentação
- [ ] 8. Interface de edição (opcional)

## Observações
- Este arquivo será atualizado a cada etapa concluída ou alterada.
- Sugestões e decisões de design serão registradas aqui.

---

## NOVA TAREFA: Atalhos Dinâmicos para o Usuário

### Requisitos
1. Remover todos os botões de habilidades da tela inicial para deixar a interface mais limpa.
2. Adicionar um botão flutuante no canto inferior direito da janela principal. Este botão abre uma nova janela de configuração de atalhos.
3. A janela de configuração exibe uma tabela/lista, onde cada linha representa uma habilidade do Invoker e permite ao usuário escolher até dois atalhos para cada uma.
4. Para cada atalho, o usuário poderá gravar a combinação de teclas pressionando-as (não será uma simples caixa de texto, mas um capturador de hotkey).
5. As alterações feitas pelo usuário são salvas dinamicamente no config.json, refletindo imediatamente no funcionamento dos combos.

### Etapas de Implementação
- [x] Remover botões de habilidades da tela inicial (MainWindow.axaml).
- [x] Adicionar botão flutuante de "Configurar Atalhos" no canto inferior direito.
- [x] Criar nova janela (ShortcutConfigWindow) com tabela de habilidades e campos de captura de hotkey.
- [x] Implementar controle de captura de hotkey para cada campo de atalho (HotkeyCaptureBox).
- [x] Salvar alterações no config.json e atualizar o AppConfig em tempo real.
- [x] Garantir que a interface seja responsiva e intuitiva.

---

## Status: IMPLEMENTAÇÃO CONCLUÍDA

- Atalhos dinâmicos configuráveis pelo usuário.
- Botão flutuante para abrir janela de configuração.
- Captura de hotkeys sem caixa de texto.
- Alterações persistidas em config.json e refletidas no app.
