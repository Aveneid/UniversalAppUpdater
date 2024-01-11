Imports System.Net.Mail
Imports Updater.Helpers

Public Module SMTPHelper

    Dim hostname = "hostname"
    Dim port = 587
    Dim emailFrom = New MailAddress("username")
    Dim emailFromPassword = ""

    Dim smtp = New SmtpClient()
    Sub prepareServer()
        smtp = New SmtpClient()
        smtp.Host = hostname
        smtp.Port = port
        If emailFromPassword <> "" Then
            smtp.UseDefaultCredentials = True
            emailFrom = New MailAddress(Environment.UserName & "@" & hostname)
        Else
            smtp.UseDefaultCredentials = False
        End If
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network
    End Sub

    Public Sub sendEmail(ByVal recipient As String, topic As String, text As String)
        Try
            prepareServer()
            Dim msg = New MailMessage()

            msg.From = emailFrom

            msg.To.Add(recipient)
            msg.Subject = topic
            msg.IsBodyHtml = True
            msg.Body = text
            msg.Priority = MailPriority.High

            smtp.Send(msg)
        Catch ex As Exception
        End Try
    End Sub
End Module
