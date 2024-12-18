using System.Diagnostics;
using System.Windows;
using Startwizard.Class;
using System;
using System.IO;

namespace Startwizard
{
    public partial class MainWindow : Window
    {
        pProcess processRunner = new pProcess();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            // 初期画面を非表示にして次の画面を表示
            Step1.Visibility = Visibility.Collapsed;
            Step2.Visibility = Visibility.Visible;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // 戻るボタン: 前の画面に戻る
            Step2.Visibility = Visibility.Collapsed;
            Step1.Visibility = Visibility.Visible;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // ユーザー名とパスワードを取得
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("ユーザー名とパスワードを入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                string batFolderPath = Path.Combine(projectRoot, "bat");
                // bat フォルダが存在しない場合は作成
                if (!Directory.Exists(batFolderPath))
                {
                    Directory.CreateDirectory(batFolderPath);
                }
                string batchFilePath = Path.Combine(batFolderPath, "createuser.bat");
                // バッチファイルの内容を作成
                string batchContent = $@"
                @echo off
                net user {username} {password} /add
                net localgroup Administrators {username} /add
                echo 新しい管理者ユーザー {username} が正常に作成されました。
                pause
                ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

    //インストーラーを終えた際に管理者ユーザーを設定するか
    //設定するならアカウントを作成する
    //パスワードとユーザー名を設定して新しく作ったアカウントを管理者ユーザーにする
    //cmd net user