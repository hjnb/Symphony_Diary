﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class 勤務画面
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
        Me.adBox = New ADBox.adBox()
        Me.btnDisplay = New System.Windows.Forms.Button()
        Me.btnRowInsert = New System.Windows.Forms.Button()
        Me.btnRowDelete = New System.Windows.Forms.Button()
        Me.btnRegist = New System.Windows.Forms.Button()
        Me.btnConv = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnPersonal = New System.Windows.Forms.Button()
        Me.labelPanel = New System.Windows.Forms.Panel()
        Me.dgvWork = New Symphony_Diary.WorkDataGridView(Me.components)
        CType(Me.dgvWork, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'adBox
        '
        Me.adBox.dateText = "14"
        Me.adBox.Location = New System.Drawing.Point(30, 3)
        Me.adBox.Mode = 3
        Me.adBox.monthText = "03"
        Me.adBox.Name = "adBox"
        Me.adBox.Size = New System.Drawing.Size(130, 35)
        Me.adBox.TabIndex = 0
        Me.adBox.yearText = "2020"
        '
        'btnDisplay
        '
        Me.btnDisplay.Location = New System.Drawing.Point(168, 8)
        Me.btnDisplay.Name = "btnDisplay"
        Me.btnDisplay.Size = New System.Drawing.Size(50, 25)
        Me.btnDisplay.TabIndex = 1
        Me.btnDisplay.Text = "表示"
        Me.btnDisplay.UseVisualStyleBackColor = True
        '
        'btnRowInsert
        '
        Me.btnRowInsert.Location = New System.Drawing.Point(393, 6)
        Me.btnRowInsert.Name = "btnRowInsert"
        Me.btnRowInsert.Size = New System.Drawing.Size(50, 25)
        Me.btnRowInsert.TabIndex = 3
        Me.btnRowInsert.Text = "行挿入"
        Me.btnRowInsert.UseVisualStyleBackColor = True
        '
        'btnRowDelete
        '
        Me.btnRowDelete.Location = New System.Drawing.Point(444, 6)
        Me.btnRowDelete.Name = "btnRowDelete"
        Me.btnRowDelete.Size = New System.Drawing.Size(50, 25)
        Me.btnRowDelete.TabIndex = 4
        Me.btnRowDelete.Text = "行削除"
        Me.btnRowDelete.UseVisualStyleBackColor = True
        '
        'btnRegist
        '
        Me.btnRegist.Location = New System.Drawing.Point(684, 3)
        Me.btnRegist.Name = "btnRegist"
        Me.btnRegist.Size = New System.Drawing.Size(69, 33)
        Me.btnRegist.TabIndex = 5
        Me.btnRegist.Text = "登録"
        Me.btnRegist.UseVisualStyleBackColor = True
        '
        'btnConv
        '
        Me.btnConv.Location = New System.Drawing.Point(752, 3)
        Me.btnConv.Name = "btnConv"
        Me.btnConv.Size = New System.Drawing.Size(69, 33)
        Me.btnConv.TabIndex = 6
        Me.btnConv.Text = "換算"
        Me.btnConv.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(888, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(69, 33)
        Me.btnPrint.TabIndex = 8
        Me.btnPrint.Text = "印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(820, 3)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(69, 33)
        Me.btnDelete.TabIndex = 7
        Me.btnDelete.Text = "削除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnPersonal
        '
        Me.btnPersonal.Location = New System.Drawing.Point(956, 3)
        Me.btnPersonal.Name = "btnPersonal"
        Me.btnPersonal.Size = New System.Drawing.Size(69, 33)
        Me.btnPersonal.TabIndex = 9
        Me.btnPersonal.Text = "個人別"
        Me.btnPersonal.UseVisualStyleBackColor = True
        '
        'labelPanel
        '
        Me.labelPanel.Location = New System.Drawing.Point(68, 669)
        Me.labelPanel.Name = "labelPanel"
        Me.labelPanel.Size = New System.Drawing.Size(957, 71)
        Me.labelPanel.TabIndex = 10
        '
        'dgvWork
        '
        Me.dgvWork.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWork.Location = New System.Drawing.Point(30, 38)
        Me.dgvWork.Name = "dgvWork"
        Me.dgvWork.RowTemplate.Height = 21
        Me.dgvWork.Size = New System.Drawing.Size(995, 631)
        Me.dgvWork.TabIndex = 2
        '
        '勤務画面
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1072, 749)
        Me.Controls.Add(Me.labelPanel)
        Me.Controls.Add(Me.btnPersonal)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnConv)
        Me.Controls.Add(Me.btnRegist)
        Me.Controls.Add(Me.btnRowDelete)
        Me.Controls.Add(Me.btnRowInsert)
        Me.Controls.Add(Me.dgvWork)
        Me.Controls.Add(Me.btnDisplay)
        Me.Controls.Add(Me.adBox)
        Me.Name = "勤務画面"
        Me.Text = "Diary "
        CType(Me.dgvWork, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents adBox As ADBox.adBox
    Friend WithEvents btnDisplay As System.Windows.Forms.Button
    Friend WithEvents dgvWork As Symphony_Diary.WorkDataGridView
    Friend WithEvents btnRowInsert As System.Windows.Forms.Button
    Friend WithEvents btnRowDelete As System.Windows.Forms.Button
    Friend WithEvents btnRegist As System.Windows.Forms.Button
    Friend WithEvents btnConv As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnPersonal As System.Windows.Forms.Button
    Friend WithEvents labelPanel As System.Windows.Forms.Panel
End Class
