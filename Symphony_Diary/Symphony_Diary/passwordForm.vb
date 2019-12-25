Imports System.Runtime.InteropServices

Public Class passwordForm

    Private iniFilePass As String = ""

    Private passCount As Integer = 0

    Private passDic As Dictionary(Of String, String)

    'テキストボックスのマウスダウンイベント制御用
    Private mdFlag As Boolean = False

    Private Const DEFAULT_VALUE As String = ""

    Private Const PASS_CHECK_ERROR As String = "パスワードが合いません"

    Private Const PASS_CONFIRM_ERROR As String = "変更確認パスワードが合いません"

    <DllImport("KERNEL32.DLL", CharSet:=CharSet.Auto)>
    Public Shared Function GetPrivateProfileString(
        ByVal lpAppName As String,
        ByVal lpKeyName As String, ByVal lpDefault As String,
        ByVal lpReturnedString As System.Text.StringBuilder, ByVal nSize As Integer,
        ByVal lpFileName As String) As Integer
    End Function

    <DllImport("KERNEL32.DLL", CharSet:=CharSet.Auto)>
    Public Shared Function WritePrivateProfileString(
        ByVal lpApplicationName As String,
        ByVal lpKeyName As String,
        ByVal lpString As String,
        ByVal lpFileName As String) As Long
    End Function

    Public Sub New(iniFilePass As String, passCount As Integer)

        InitializeComponent()

        Me.iniFilePass = iniFilePass
        Me.passCount = passCount

        'フォーム設定
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.FormBorderStyle = FormBorderStyle.FixedSingle

    End Sub

    Private Sub passwordForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If e.Control = False Then
                Me.SelectNextControl(Me.ActiveControl, Not e.Shift, True, True, True)
            End If
        End If
    End Sub

    Private Sub passwordForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'テキストボックス設定
        settingTextBox()

        'iniファイルからパスワード読み込み
        readPassword()
    End Sub

    Public Function getIniString(ByVal lpSection As String, ByVal lpKeyName As String, ByVal lpFileName As String) As String
        Dim strValue As System.Text.StringBuilder = New System.Text.StringBuilder(1024)

        Dim sLen = GetPrivateProfileString(lpSection, lpKeyName, DEFAULT_VALUE, strValue, 1024, lpFileName)
        Dim str As String = strValue.ToString()

        Return str
    End Function

    Public Function putIniString(ByVal lpSection As String, lpKeyName As String, ByVal lpValue As String, ByVal lpFileName As String) As Boolean
        Dim result As Long = WritePrivateProfileString(lpSection, lpKeyName, lpValue, lpFileName)
        Return result <> 0
    End Function

    Private Sub readPassword()
        If passCount <= 0 Then
            Return
        ElseIf passCount = 1 Then
            Dim pass As String = getIniString("System", "Pwd", iniFilePass)
            passDic = New Dictionary(Of String, String)
            passDic.Add("Pwd", pass)
        Else
            Dim pass As String
            passDic = New Dictionary(Of String, String)
            For i As Integer = 1 To passCount
                pass = getIniString("System", "Pwd" & i, iniFilePass)
                passDic.Add("Pwd" & i, pass)
            Next
        End If
    End Sub

    Private Sub settingTextBox()
        passBox.ImeMode = Windows.Forms.ImeMode.Disable
        newPassBox.ImeMode = Windows.Forms.ImeMode.Disable
        confirmPassBox.ImeMode = Windows.Forms.ImeMode.Disable

        passBox.PasswordChar = "*"c
        newPassBox.PasswordChar = "*"c
        confirmPassBox.PasswordChar = "*"c
    End Sub

    Private Function checkPassword(inputPass As String) As Boolean
        For Each pass As String In passDic.Values
            If inputPass = pass Then
                Return True
            End If
        Next

        Return False
    End Function

    Private Function changePassword(oldPass As String, newPass As String) As Boolean
        '以前のパスワードのkeyを取得
        Dim key As String = ""
        For Each kv As KeyValuePair(Of String, String) In passDic
            If kv.Value = oldPass Then
                key = kv.Key
            End If
        Next

        '該当のkeyの値を新パスワードに変更
        Dim result As Boolean = putIniString("System", key, newPass, iniFilePass)
        Return result
    End Function

    Private Sub btnOk_Click(sender As System.Object, e As System.EventArgs) Handles btnOk.Click
        Dim inputPass As String = passBox.Text
        Dim inputNewPass As String = newPassBox.Text
        Dim inputConfirmPass As String = confirmPassBox.Text

        If inputNewPass = "" AndAlso inputConfirmPass = "" Then
            '入力パスワード確認のみの場合
            If checkPassword(inputPass) = True Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                errorLabel.Text = PASS_CHECK_ERROR
                errorLabel.Visible = True
                passBox.Focus()
            End If
        Else
            '入力パスワード確認、パスワード変更ありの場合
            If checkPassword(inputPass) = True Then
                If inputNewPass = inputConfirmPass Then
                    'パスワード変更処理
                    If changePassword(inputPass, inputNewPass) = False Then
                        MsgBox("パスワードの変更に失敗しました。.iniファイルの書き込み権限を確認して下さい。")
                        Return
                    End If

                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                Else
                    errorLabel.Text = PASS_CONFIRM_ERROR
                    errorLabel.Visible = True
                    confirmPassBox.Focus()
                End If
            Else
                errorLabel.Text = PASS_CHECK_ERROR
                errorLabel.Visible = True
                passBox.Focus()
            End If
        End If

    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub textBox_Enter(sender As Object, e As System.EventArgs) Handles passBox.Enter, newPassBox.Enter, confirmPassBox.Enter
        Dim tb As TextBox = CType(sender, TextBox)
        tb.SelectAll()
        mdFlag = True
    End Sub

    Private Sub textBox_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles passBox.MouseDown, newPassBox.MouseDown, confirmPassBox.MouseDown
        If mdFlag = True Then
            Dim tb As TextBox = CType(sender, TextBox)
            tb.SelectAll()
            mdFlag = False
        End If
    End Sub
End Class