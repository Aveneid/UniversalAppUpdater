Imports System.IO
Imports System.Security.Cryptography
Imports Updater.Helpers

Module Module1

    Sub Main(args() As String)

        'delete old updater
        If File.Exists(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "~Updater.exe")) And Not System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Contains("~") Then
            File.Delete(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "~Updater.exe"))
        End If

        checkSelfUpdate()
        checkForUpdate()

        Console.WriteLine(vbCrLf & "All done, exiting...")
        Threading.Thread.Sleep(1000)
    End Sub

    Function checkIfAppIsRunning(ByVal processName As String)
        Dim p() As Process
        p = Process.GetProcessesByName(processName)
        If p.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Sub checkSelfUpdate()

        If System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Contains("~") Then
            File.Delete(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "Updater.exe"))
            File.Copy(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "~Updater.exe"), System.IO.Path.Combine(My.Application.Info.DirectoryPath, "Updater.exe"))
            Shell(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "Updater.exe"))
            Environment.Exit(0)
        End If

        Try
            Dim selfPath = File.ReadAllLines(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "self.cfg"))

            If selfPath.Length > 1 Then
                Dim selfPathTarget = selfPath(0).Split("|")

                Dim hash = MD5.Create()
                Dim hashVals(2)() As Byte

                Dim fsLocalApp As FileStream = File.OpenRead(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "Updater.exe"))
                Dim fsRemoteApp As FileStream = File.OpenRead(selfPathTarget(0))

                fsLocalApp.Position = 0
                fsRemoteApp.Position = 0

                hashVals(0) = hash.ComputeHash(fsLocalApp)
                hashVals(1) = hash.ComputeHash(fsRemoteApp)

                fsLocalApp.Close()
                fsRemoteApp.Close()

                If Not hashVals(0).SequenceEqual(hashVals(1)) Then

                    'new updater version
                    My.Computer.FileSystem.CopyFile(selfPathTarget(0), System.IO.Path.Combine(My.Application.Info.DirectoryPath, "~Updater.exe"))
                    Shell(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "~Updater.exe"))
                    Environment.Exit(0)
                End If

            End If
        Catch ex As Exception

        End Try

    End Sub
    Sub checkForUpdate()
        Try
            Dim apps = File.ReadAllLines(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "apps.cfg"))
            'structure:
            ' local app path | remote local path

            For Each line As String In apps

                Dim localPath = line.Split("|")(0).Trim
                Dim remotePath = line.Split("|")(1).Trim

                If File.Exists(localPath) Then 'update local version
                    Console.WriteLine("Searching new version of " & Helpers.getFileName(localPath) & "...")
                    Dim hash = MD5.Create()
                    Dim hashVals(2)() As Byte

                    Dim fsLocalApp As FileStream = File.OpenRead(localPath)
                    Dim fsRemoteApp As FileStream = File.OpenRead(remotePath)

                    fsLocalApp.Position = 0
                    fsRemoteApp.Position = 0

                    hashVals(0) = hash.ComputeHash(fsLocalApp)
                    hashVals(1) = hash.ComputeHash(fsRemoteApp)

                    fsLocalApp.Close()
                    fsRemoteApp.Close()


                    If Not hashVals(0).SequenceEqual(hashVals(1)) Then

                        If checkIfAppIsRunning(Helpers.getFileName(localPath)) Then
                            For Each p As Process In Process.GetProcessesByName(Helpers.getFileName(remotePath))
                                p.Close()
                            Next
                        End If
                        Console.WriteLine("Found new version of " & Helpers.getFileName(remotePath) & ", downloading...")
                        Threading.Thread.Sleep(1000)
                        File.Copy(remotePath, localPath, True)
                    Else
                        Console.WriteLine(Helpers.getFileName(localPath) & ": no new version found.")
                    End If

                Else ' just download, no local version found
                    If checkIfAppIsRunning(Helpers.getFileName(localPath)) Then
                        For Each p As Process In Process.GetProcessesByName(Helpers.getFileName(remotePath))
                            p.Close()
                        Next
                    End If
                    Console.WriteLine("Downloading " & Helpers.getFileName(remotePath) & " for first time...")
                    If Not Directory.Exists(Helpers.getPath(localPath)) Then Directory.CreateDirectory(Helpers.getPath(localPath))
                    Threading.Thread.Sleep(1000)
                    File.Copy(remotePath, localPath, True)
                    If File.Exists(localPath) Then Console.WriteLine(Helpers.getFileName(localPath) & " downloaded.")
                End If

            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub


End Module

