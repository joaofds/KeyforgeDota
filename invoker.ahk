#NoEnv
#UseHook
#InstallKeybdHook
#SingleInstance, force
#Persistent

#IfWinActive Dota 2

;=====================================================================
; Suas teclas aqui
;=====================================================================
QUAS_KEY := "q"
WEX_KEY := "w"
EXORT_KEY := "e"
INVOKE_KEY := "r"
FIRST_SPELL_KEY := "d"
SECOND_SPELL_KEY := "f"

;=====================================================================
; Variaveis globais
;=====================================================================
DELAY := 1.00
ShouldResetAfterCasting := True
SCRIPT__TEMPORARILY_SUSPENDED := False

;=====================================================================
; Atalhos
;=====================================================================

;	+---------------------+--------------------------+-----------------------------+
;	| Space+Q: Cold Snap  | Space+3: Chaos Meteor    | Space+B: Fast Ghost Walk    |
;	+---------------------+--------------------------+-----------------------------+
;	| Space+W: EMP        | Space+4: Deafening Blast | Space+1: Alacrity           |
;	+---------------------+--------------------------+-----------------------------+
;	| Space+E: Sun Strike | Space+V: Ice Wall        | Space+F: Forge Spirits      |
;	+---------------------+--------------------------+-----------------------------+
;	| Space+T: Tornado    | Space+R: Ghost Walk      | Pause Break: Pause Script   |
;	+---------------------+--------------------------+-----------------------------+

;=====================================================================
; Base functions
;=====================================================================

All__Quas(){
	Gosub, CastQuas
	Gosub, CastQuas
	Gosub, CastQuas
}

All__Wex(){
	Gosub, CastWex
	Gosub, CastWex
	Gosub, CastWex
}

All__Exort(){
	Gosub, CastExort
	Gosub, CastExort
	Gosub, CastExort
}

RESET__AfterCasting(){
	If (ShouldResetAfterCasting){
		All__Wex()
	}
}

; Quas spells
; Cold Snap
Cast__ColdSnap(){
	All__Quas()
	Gosub, CastInvoke
}
; Ghost Walk
Cast__Ghostwalk(){
	Gosub, CastQuas
	Gosub, CastQuas
	Gosub, CastWex
	Gosub, CastInvoke
}

;Ice wall
Cast__IceWall(){
	Gosub, CastQuas
	Gosub, CastQuas
	Gosub, CastExort
	Gosub, CastInvoke
}

; Wex spells
; EMP
Cast__EMP(){
	All__Wex()
	Gosub, CastInvoke
}
; Tornado
Cast__Tornado(){
	Gosub, CastWex
	Gosub, CastWex
	Gosub, CastQuas
	Gosub, CastInvoke
}
; Alacrity
Cast__Alacrity(){
	Gosub, CastWex
	Gosub, CastWex
	Gosub, CastExort
	Gosub, CastInvoke
}

; Exort spells
; Sun Strike
Cast__SunStrike(){
	All__Exort()
	Gosub, CastInvoke
}
; Forge Spirit
Cast__ForgeSpirit(){
	Gosub, CastExort
	Gosub, CastExort
	Gosub, CastQuas
	Gosub, CastInvoke
}
; Chaos Meteor
Cast__ChaosMeteor(){
	Gosub, CastExort
	Gosub, CastExort
	Gosub, CastWex
	Gosub, CastInvoke
}

; DefeaningBlast
Cast__DefeaningBlast(){
	Gosub, CastQuas
	Gosub, CastWex
	Gosub, CastExort
	Gosub, CastInvoke
}

;=====================================================================
; Binds
;=====================================================================

Space & q::
	Cast__ColdSnap()
	All__Wex()
Return

; EMP
Space & w::
	Cast__EMP()
	All__Wex()
Return

; SunStrike
Space & e::
	Cast__SunStrike()
	All__Wex()
Return

; Tornado
Space & t::
	Cast__Tornado()
	All__Wex()
Return

; ChaosMeteor
Space & 3::
	Cast__ChaosMeteor()
	All__Wex()
Return


; DeafeningBlast
Space & 4::
	Cast__DefeaningBlast()
	All__Wex()
Return

; Cast__IceWall
Space & v::
	Cast__IceWall()
	All__Wex()
Return

; GhostWalk
Space & r::
	Cast__Ghostwalk()
	All__Wex()
Return

; Panic GhostWalk
Space & b::
	Cast__Ghostwalk()
	All__Wex()
	Gosub, CastFirstSpell
Return

; Alacrity
Space & 1::
	Cast__Alacrity()
	All__Wex()
Return

; ForgeSpirit
; GhostWalk
Space & f::
	Cast__ForgeSpirit()
	All__Wex()
Return


;=====================================================================
; Skills e casts
;=====================================================================
XButton1::
	Gosub, CastFirstSpell
Return

XButton2::
	Gosub, CastSecondSpell
Return

CastQuas:
	Send {%QUAS_KEY%}
	Sleep, Round(10 * DELAY)
Return

CastWex:
	Send {%WEX_KEY%}
	Sleep, Round(10 * DELAY)
Return

CastExort:
	Send {%EXORT_KEY%}
	Sleep, Round(10 * DELAY)
Return

CastFirstSpell:
	Sleep, Round(10 * DELAY)
	Send {%FIRST_SPELL_KEY%}
Return

CastSecondSpell:
	Sleep, Round(10 * DELAY)
	Send {%Second_SPELL_KEY%}
Return

CastInvoke:
	Send {%INVOKE_KEY%}
	Sleep, Round(10 * DELAY)
Return


;=====================================================================
; Pausando coisas
;=====================================================================

SUSPEND__SCRIPT(){
	Suspend, On
	SCRIPT__TEMPORARILY_SUSPENDED := True
}

UNSUSPEND__SCRIPT(){
	Suspend, Permit
	Suspend, Off
	SCRIPT__TEMPORARILY_SUSPENDED := False
}

RELOAD__SCRIPT(){
	Suspend, Permit
	Reload
}

; Pausa script ao entrar no chat durante uma partida
+~Enter::
~Enter::
Suspend, Permit
	If (A_IsSuspended = 0)
	{
		SUSPEND__SCRIPT()
	}
	Else
	{
		UNSUSPEND__SCRIPT()
	}
Return

; Esc continua execução ao sair do chat.
~Escape::
	Suspend, Permit
	If (A_IsSuspended = 0)
	{
		UNSUSPEND__SCRIPT()
	}
Return

; Recarrega script. (Ctrl+Alt+R)
^!r::
	Suspend, Permit
	RELOAD__SCRIPT()
Return

; Pausa o script (Pause Break)
Pause::Suspend