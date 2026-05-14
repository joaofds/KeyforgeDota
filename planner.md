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
- ComboRunner.cs, KeyMapper.cs, KeySender.cs: lógica de combos e envio de teclas.
- config.json: configuração persistente.

### Fluxo do usuário
1. Abre o app, define a janela do Dota 2.
2. Usa o botão flutuante para configurar atalhos.
3. Define/ajusta atalhos para cada habilidade.
4. Alterações são salvas e aplicadas automaticamente.
5. Executa combos conforme configuração.

---
