Imports System.DirectoryServices.AccountManagement
Imports System.Management
Imports Microsoft.WindowsAPICodePack.Shell

Class Helpers
    Public Shared Function getFileName(path As String)
        Return System.IO.Path.GetFileName(path)
    End Function

    Public Shared Function getPath(path As String)
        Return System.IO.Path.GetDirectoryName(path)
    End Function
End Class
