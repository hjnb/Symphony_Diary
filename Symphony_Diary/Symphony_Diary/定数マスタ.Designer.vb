﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class 定数マスタ
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnRegist = New System.Windows.Forms.Button()
        Me.dgvConstM = New Symphony_Diary.ConstMDataGridView(Me.components)
        CType(Me.dgvConstM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnRegist
        '
        Me.btnRegist.Location = New System.Drawing.Point(549, 227)
        Me.btnRegist.Name = "btnRegist"
        Me.btnRegist.Size = New System.Drawing.Size(75, 30)
        Me.btnRegist.TabIndex = 3
        Me.btnRegist.Text = "登録"
        Me.btnRegist.UseVisualStyleBackColor = True
        '
        'dgvConstM
        '
        Me.dgvConstM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvConstM.Location = New System.Drawing.Point(12, 30)
        Me.dgvConstM.Name = "dgvConstM"
        Me.dgvConstM.RowTemplate.Height = 21
        Me.dgvConstM.Size = New System.Drawing.Size(1029, 183)
        Me.dgvConstM.TabIndex = 2
        '
        '定数マスタ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1058, 269)
        Me.Controls.Add(Me.dgvConstM)
        Me.Controls.Add(Me.btnRegist)
        Me.Name = "定数マスタ"
        Me.Text = "Diary 定数マスタ"
        CType(Me.dgvConstM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnRegist As System.Windows.Forms.Button
    Friend WithEvents dgvConstM As Symphony_Diary.ConstMDataGridView
End Class
