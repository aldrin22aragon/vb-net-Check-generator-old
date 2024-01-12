﻿Imports System.Drawing.Printing

Public Class DefaultChequeFormat
   Implements IChequeFormat

   Private MaxPayeeWidth As Integer = 475
   Private FullWidth As Integer = 700

   Private Payee1Location As Point
   Private Payee2Location As Point
   Private AmountLocation As Point
   Private AmountInWordsLocation As Point
   Private ChequeDateLocation As Point

   Private Font As Font

   Private Brush As Brush

   Public Sub New(payee1Location As Point, payee2Location As Point, amountLocation As Point, amountInWordsLocation As Point, chequeDateLocation As Point, font As Font)
      Me.Payee1Location = payee1Location
      Me.Payee2Location = payee2Location
      Me.AmountLocation = amountLocation
      Me.AmountInWordsLocation = amountInWordsLocation
      Me.ChequeDateLocation = chequeDateLocation
      Me.Font = font

      Brush = Brushes.Black
   End Sub

   Public Sub Print(payee1 As String, payee2 As String, chequeDate As Date, amount As Double, printer As Printing.PrintPageEventArgs, crossed As Boolean, Optional xAdjustment As Integer = 0, Optional yAdjustment As Integer = 0) Implements IChequeFormat.Print

      Dim amountInWords As String = ChequeModelExtension.AmountInWords(amount)

      Dim payee1Font As Font = Font
      Dim payee2Font As Font = Font

      ChequeModelExtension.GuagePayeeFontSize(payee1, payee1Font, MaxPayeeWidth, printer, True)


      Dim payee1Args As String() = payee1.Split(vbNewLine)
      If payee1Args.Length > 1 Then
         payee2 = String.Format("{0} {1}", payee1Args(1), payee2)
         payee2Font = payee1Font
      End If

      ChequeModelExtension.GuagePayeeFontSize(payee2, payee2Font, MaxPayeeWidth, printer, False)



      If Not chequeDate = Nothing Then
         printer.Graphics.DrawString(chequeDate.ToString("MMMM dd, yyyy"), Font, Brush, ChequeDateLocation)
      End If
      If payee2.Length > 0 Then
         Payee1Location.X = (FullWidth - ChequeModelExtension.GetTextWidth(payee1Args(0).Trim, payee1Font, printer)) / 2
         printer.Graphics.DrawString(payee1Args(0).Trim, payee1Font, Brush, Payee1Location)

         Payee2Location.X = (FullWidth - ChequeModelExtension.GetTextWidth(payee2.Trim, payee2Font, printer)) / 2
         printer.Graphics.DrawString(payee2.Trim, payee2Font, Brush, Payee2Location)
      Else
         Payee1Location.X = (FullWidth - ChequeModelExtension.GetTextWidth(payee1Args(0).Trim, payee1Font, printer)) / 2
         printer.Graphics.DrawString(payee1Args(0).Trim, payee1Font, Brush, Payee1Location)
      End If
      If amount > 0 Then
         printer.Graphics.DrawString(String.Format("**{0:N2}**", amount), Font, Brush, AmountLocation)
         printer.Graphics.DrawString(amountInWords, Font, Brush, AmountInWordsLocation)
      End If
      If crossed Then
         printer.Graphics.DrawLine(Pens.Black, 100, 570 - 280, 200, 520 - 280)
         printer.Graphics.DrawLine(Pens.Black, 115, 570 - 280, 215, 520 - 280)
      End If
   End Sub






End Class