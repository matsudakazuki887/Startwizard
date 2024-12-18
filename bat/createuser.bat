
@echo off
:: ユーザー名とパスワードの入力を促す
set /p username=新しいユーザー名を入力してください: 
set /p password=新しいパスワードを入力してください: 

:: 新しいユーザーを作成
net user %username% %password% /add
if %errorlevel% neq 0 (
    echo ユーザー作成中にエラーが発生しました。
    pause
    exit /b 1
)

:: 作成したユーザーを管理者グループに追加
net localgroup Administrators %username% /add
if %errorlevel% neq 0 (
    echo 管理者グループに追加中にエラーが発生しました。
    pause
    exit /b 1
)

echo 新しい管理者ユーザー %username% が正常に作成されました。
pause
