#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.

;레지스트리에서 서든 폴더 찾기
RegRead, sa_path, HKLM, SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\SuddenAttack, InstallLocation

;맵 효과음 삭제
FileDelete, %sa_path%\game\sa_worlds\snd\*.wav

sa_path = %sa_path%\game\sa_characters\customvoice

;카운트용 변수
del_done_count := 0
del_error_count := 0

;캐릭터 숨소리 관련 삭제
Loop, Files, %sa_path%\*, D
{
    FileDelete, %A_LoopFileFullPath%\breath\*.ogg
    if(ErrorLevel == 0)
    {
        del_done_count++
    }
    else{
        del_error_count++
    }
}

result:=del_done_count+del_error_count

MsgBox, 정상: %del_done_count% `n에러: %del_error_count% `n총 캐릭터: %result%