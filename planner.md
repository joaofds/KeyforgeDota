---

## OVERVIEW DO APP

### O que é?
Aplicativo desktop para automatizar combos do Invoker no Dota 2, com configuração dinâmica de teclas e combos. Interface moderna, botão flutuante para atalhos, configuração fácil e feedback visual.

### Como funciona?
- Interface principal simples: define janela alvo e mostra status.
- Botão flutuante abre janela de configuração de atalhos.
- Janela de configuração lista habilidades e permite definir até dois atalhos por habilidade, usando captura real de hotkeys.
- Alterações são salvas em tempo real no config.json.
- Toda lógica de combos e teclas é baseada no arquivo de configuração externo.

### Estrutura dos arquivos principais
- MainWindow.axaml/cs: interface principal, botão flutuante, status.
- ShortcutConfigWindow.axaml/cs: janela de configuração dinâmica de atalhos.
- HotkeyCaptureBox.axaml/cs: captura de hotkeys.
- AppConfig.cs: leitura/escrita do config.json.
- ComboRunner.cs: lógica de combos e envio de teclas.
- KeyMapper.cs: mapeamento nome → VK code (reescrito corretamente).
- config.json: configuração persistente.

### Fluxo do usuário
1. Abre o app, define a janela do Dota 2.
2. Usa o botão flutuante para configurar atalhos.
3. Define/ajusta atalhos para cada habilidade.
4. Alterações são salvas e aplicadas automaticamente.
5. Executa combos conforme configuração.

---

## LISTA DE CORREÇÕES

### 🔴 Crítico
- [x] **1.** `AppConfig.Load()` — substituir `catch {}` vazio por log + aviso ao usuário
- [x] **2.** `MainWindow.AttachControls` — remover handlers `LostFocus` duplicados de WexKeyBox, ExortKeyBox e InvokeKeyBox
- [x] **3.** `KeyboardHookWin` — verificar retorno de `SetWindowsHookEx` e notificar falha

### 🟠 Alto
- [x] **4.** Deletar `KeySender.cs` (dead code)
- [x] **5.** Deletar `KeyMapper.cs` (dead code) e reescrever com mapeamento nome → VK correto
- [x] **6.** Corrigir scancode errado de 'R' em `KeyMapper` — resolvido na reescrita do item 5
- [x] **7.** Remover bloco WinAPI duplicado de `MainWindow` (`SendKeyToWindow`, `KeyToVirtualKey`, `PostMessage`, constantes)
- [x] **8.** `ComboRunner.KeyToVirtualKey` — suportar teclas especiais (`space`, `f1`–`f12`, etc.) via `KeyMapper`
- [x] **9.** Remover `AtualizarListaAtalhosSalvos` e sua chamada
- [x] **10.** Extrair lista de habilidades para constante estática em `AppConfig` (duplicada nas linhas 52 e 68)

### 🟡 Médio
- [x] **11.** `MainWindow.OpenShortcutConfigBtn_Click` — parar de recriar o hook inteiro no save; apenas reconstruir o mapa de combos
- [x] **12.** ~~`Thread.Sleep` em método `Task` no `KeySender`~~ — resolvido com deleção do arquivo (item 4)
- [x] **13.** Remover `RunComboAsync` ou ao menos o `await Task.Delay(500)` vestigial
- [ ] **14.** Remover `Avalonia.Controls.DataGrid` do `.csproj`

### 🟢 Baixo
- [ ] **15.** `ShortcutConfigWindow` — exibir nomes legíveis das habilidades (ex: "Panic Ghost Walk")
- [ ] **16.** `ShortcutConfigWindow.axaml` — envolver `ShortcutStack` em `ScrollViewer`
- [ ] **17.** `MainWindow.axaml` — renomear labels "Skill 1–6" para "Quas", "Wex", "Exort", "Invoke", "Spell 1", "Spell 2"
- [ ] **18.** README — regravar como UTF-8 sem BOM

---

## PROGRESSO

Concluídos: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13
Pendentes: 10, 11, 13, 14, 15, 16, 17, 18

---
