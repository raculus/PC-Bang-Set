#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
#SingleInstance, Force

if not A_IsAdmin 
{ 
    Run *RunAs "%A_ScriptFullPath%" 
    ExitApp 
}

; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.
Gui, Add, ListBox, vMyList,
Gui, Add, Progress, vMyProgress
Gui, +AlwaysOnTop
Gui, Show, , AutoInstall

Gui, Submit, NoHide

Run, nvidia_driver.exe
ProgressAdd("파일 실행", 20)

WinWait, ahk_exe nvidia_driver.exe
ControlClick, Button2, ahk_exe nvidia_driver.exe
ProgressAdd("압축 해제", 20)

nvidiaInstall := "NVIDIA 설치 프로그램"
WinWait, %nvidiaInstall%
Sleep, 1000
ControlWait("동의 및 계속(&A)", nvidiaInstall)
ProgressAdd("동의 및 계속", 20)
Sleep, 500
ControlWait("다음(&N)", nvidiaInstall)
ProgressAdd("설치 시작", 20)
Sleep, 5000
ControlWait("닫기(&C)", nvidiaInstall)
ProgressAdd("완료", 20)

countdown := 3
Loop, countdown
{
    ProgressAdd(countdown+"초 후 닫습니다.", 0)
    countdown-=1
    Sleep, 1000
}
ExitApp

ProgressAdd(jobLabel, num){
    Gui, Submit, NoHide
    GuiControl, , MyList, %jobLabel%
    GuiControl, , MyProgress, +%num%
}

ControlWait(controlText, winTitle){
    ControlGet, available, Enabled, , %controlText%, %winTitle%, several

    While (available = False)
        ControlGet, available, Enabled, , %controlText%, %winTitle%, several

    ControlClick, %controlText%, %winTitle%
}

GuiClose:
    ExitApp