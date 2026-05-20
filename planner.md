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
- ComboRunner.cs: lógica de combos e envio de teclas (ativo).
- KeyMapper.cs: mapeamento scancode/VK — DEAD CODE (nunca usado).
- KeySender.cs: envio de teclas — DEAD CODE (nunca usado).
- config.json: configuração persistente.

### Fluxo do usuário
1. Abre o app, define a janela do Dota 2.
2. Usa o botão flutuante para configurar atalhos.
3. Define/ajusta atalhos para cada habilidade.
4. Alterações são salvas e aplicadas automaticamente.
5. Executa combos conforme configuração.

---

## BACKLOG DE MELHORIAS E CORREÇÕES

### 🔴 CRÍTICO — Bugs que causam perda de dados ou falha silenciosa

- [x] **`AppConfig.Load()` engole toda exceção com `catch {}`**
  - Arquivo: `AppConfig.cs:63`
  - Problema: qualquer JSON corrompido apaga a config do usuário sem aviso.
  - Correção: logar o erro e mostrar mensagem ao usuário antes de usar fallback.

- [x] **Handlers de evento duplicados em `AttachControls`**
  - Arquivo: `MainWindow.axaml.cs:174-181`
  - Problema: `WexKeyBox`, `ExortKeyBox` e `InvokeKeyBox` registram `LostFocus += SaveConfigFromUI` duas vezes cada, causando duplo salvamento.
  - Correção: remover as três linhas duplicadas (179, 180, 181).

- [x] **`KeyboardHookWin` não verifica se o hook foi instalado com sucesso**
  - Arquivo: `KeyboardHookWin.cs:16-18`
  - Problema: se `SetWindowsHookEx` falhar, `_hookID` fica `IntPtr.Zero` e nenhum hotkey funciona — sem alerta ao usuário.
  - Correção: verificar retorno de `SetWindowsHookEx` e lançar exceção ou disparar evento de erro.

---

### 🟠 ALTO — Dead code, duplicação e lógica incorreta

- [x] **`KeySender.cs` é dead code — deletar** — arquivo removido.
- [x] **`KeyMapper.cs` é dead code — deletar** — arquivo removido. Será reescrito corretamente ao resolver o item 8.
- [x] **Scancode errado para 'R' em `KeyMapper`** — resolvido junto com a remoção do arquivo.
- [x] **Código WinAPI duplicado entre `MainWindow` e `ComboRunner`**
  - Removidos `SendKeyToWindow`, `KeyToVirtualKey`, `PostMessage`, `WM_KEYDOWN/WM_KEYUP` de `MainWindow`.
  - Mantido apenas `FindWindow` / `FindWindowByTitle` (ainda necessários).

- [x] **`ComboRunner.KeyToVirtualKey` não suporta teclas especiais**
  - `KeyMapper.cs` reescrito com mapeamento correto nome → VK (letras, números, space, f1-f12, modificadores).
  - `ComboRunner.KeyToVirtualKey` delegado ao `KeyMapper.TryGetVirtualKey`.

- [x] **`AtualizarListaAtalhosSalvos` é método vazio ainda chamado**
  - Arquivo: `ShortcutConfigWindow.axaml.cs:85-88`
  - Remover o método e sua chamada em `BtnSalvar_Click`.

- [ ] **Lista de habilidades duplicada em `AppConfig.Load()`**
  - Arquivo: `AppConfig.cs:52-73`
  - O array de habilidades está copiado duas vezes (linha 52 e linha 68).
  - Correção: extrair para constante estática `string[] DefaultAbilities`.

---

### 🟡 MÉDIO — Qualidade e robustez

- [ ] **Hook recriado desnecessariamente a cada save**
  - Arquivo: `MainWindow.axaml.cs:45-48`
  - Ao salvar configuração, o hook é descartado e recriado inteiro — há uma janela sem hotkeys ativos.
  - Correção: apenas chamar `BuildComboToAbilityMap()` e recriar `ComboRunner`; manter o hook intacto.

- [ ] **`Thread.Sleep` em método que retorna `Task` no `KeySender`**
  - Arquivo: `KeySender.cs:14-18`
  - Anti-pattern: bloqueia thread de forma síncrona em método assíncrono.
  - Irrelevante se o arquivo for deletado; anotar para não reproduzir o padrão.

- [ ] **`RunComboAsync` tem delay vestigial de 500ms**
  - Arquivo: `MainWindow.axaml.cs:230`
  - `await Task.Delay(500)` de debug nunca removido. O método nem é mais chamado pelo fluxo atual.
  - Correção: remover o delay (ou o método inteiro, que é dead code).

- [ ] **`Avalonia.Controls.DataGrid` referenciado sem uso**
  - Arquivo: `KeyforgeDota.csproj:17`
  - Nenhum `DataGrid` é usado. Remover a dependência para reduzir tamanho do build.

---

### 🟢 BAIXO — UX e polish

- [ ] **Nomes internos exibidos na janela de atalhos**
  - Arquivo: `ShortcutConfigWindow.axaml.cs:37`
  - Exibe `"panicghostwalk"`, `"deafeningblast"` etc. em vez de nomes legíveis.
  - Correção: criar mapa `string → string` com nomes de exibição (ex: "panicghostwalk" → "Panic Ghost Walk").

- [ ] **Sem ScrollViewer na janela de configuração de atalhos**
  - Arquivo: `ShortcutConfigWindow.axaml`
  - `ShortcutStack` sem scroll — com 11 habilidades pode cortar conteúdo em telas menores.
  - Correção: envolver `ShortcutStack` em `ScrollViewer`.

- [ ] **Labels das teclas na MainWindow são genéricas**
  - Arquivo: `MainWindow.axaml:18-42`
  - "Skill 1–6" não informa o usuário. Renomear para "Quas", "Wex", "Exort", "Invoke", "Spell 1", "Spell 2".

- [ ] **README com encoding errado (UTF-16 LE com BOM)**
  - O arquivo aparece garbled no GitHub e em qualquer leitor UTF-8.
  - Correção: regravar como UTF-8 sem BOM.

---

## ORDEM SUGERIDA DE EXECUÇÃO

1. Bugs críticos (catch vazio, handlers duplicados, verificação do hook)
2. Limpeza de dead code (KeySender, KeyMapper, duplicatas em MainWindow)
3. Corrigir KeyMapper e integrar com ComboRunner para suporte a teclas especiais
4. Refatorar recriação desnecessária do hook no save
5. UX: nomes legíveis, ScrollViewer, labels
6. README encoding

---
